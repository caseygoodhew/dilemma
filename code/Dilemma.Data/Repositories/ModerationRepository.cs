using System;
using System.Data.Entity;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Moderation repository implementation.
    /// </summary>
    internal class ModerationRepository : IInternalModerationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<IMessagePipe<ModerationState>> ModerationMessagePipe = Locator.Lazy<IMessagePipe<ModerationState>>();
        
        /// <summary>
        /// To be called when a question is created.
        /// </summary>
        /// <param name="messenger">The question <see cref="IMessenger{T}"/>.</param>
        public void OnQuestionCreated(IMessenger<QuestionDataAction> messenger)
        {
            var messageContext = messenger.GetContext<QuestionMessageContext>(QuestionDataAction.Created);

            if (messageContext.TestingConfigurationProperty(x => x.ManualModeration).Is(ActiveState.Active))
            {
                var dataContext = messageContext.DataContext;
                var question = messageContext.Question;

                dataContext.EnsureAttached(question, x => x.QuestionId);
                dataContext.EnsureAttached(question.User, x => x.UserId);

                question.QuestionState = QuestionState.ReadyForModeration;
                dataContext.Entry(question).State = EntityState.Modified;

                OnModerableCreated(
                    dataContext,
                    new Moderation
                        {
                            ModerationFor = ModerationFor.Question,
                            Question = question,
                            ForUser = question.User
                        },
                    question.Text);
            }

            messenger.Forward();
        }

        /// <summary>
        /// To be called when an answer is created.
        /// </summary>
        /// <param name="messenger">The answer <see cref="IMessenger{T}"/>.</param>
        public void OnAnswerCreated(IMessenger<AnswerDataAction> messenger)
        {
            var messageContext = messenger.GetContext<AnswerMessageContext>(AnswerDataAction.Created);

            if (messageContext.TestingConfigurationProperty(x => x.ManualModeration).Is(ActiveState.Active))
            {
                var dataContext = messageContext.DataContext;
                var answer = messageContext.Answer;

                dataContext.EnsureAttached(answer, x => x.AnswerId);
                dataContext.EnsureAttached(answer.User, x => x.UserId);

                answer.AnswerState = AnswerState.ReadyForModeration;
                dataContext.Entry(answer).State = EntityState.Modified;
                
                OnModerableCreated(
                    dataContext,
                    new Moderation
                        {
                            ModerationFor = ModerationFor.Answer,
                            Answer = answer,
                            ForUser = answer.User
                        },
                    answer.Text);
            }

            messenger.Forward();
        }

        /// <summary>
        /// Gets the next item for moderation.
        /// </summary>
        /// <typeparam name="T">The type to convert the output to.</typeparam>
        /// <returns>The converted <see cref="Moderation"/></returns>
        public T GetNext<T>() where T : class
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations.Include(x => x.ForUser)
                        .Include(x => x.ModerationEntries)
                        .Select(x => new 
                        {
                            MostRecentEntry = x.ModerationEntries.OrderByDescending(y => y.CreatedDateTime).FirstOrDefault(),
                            x.ModerationEntries,
                            x.ModerationFor,
                            x.ModerationId
                        })
                        .Where(x => x.MostRecentEntry.State == ModerationState.Queued)
                        .OrderBy(x => x.MostRecentEntry.CreatedDateTime)
                        .Take(1)
                        .ToList()
                        .Select(x => new Moderation
                                         {
                                             ModerationId = x.ModerationId,
                                             ModerationEntries = x.ModerationEntries.OrderByDescending(y => y.CreatedDateTime).ToList(),
                                             ModerationFor = x.ModerationFor
                                         })
                        .FirstOrDefault();

                return ConverterFactory.ConvertOne<Moderation, T>(moderation);
            }
        }

        /// <summary>
        /// Approves a moderation.
        /// </summary>
        /// <param name="userId">The id of the user approving the moderation.</param>
        /// <param name="moderationId">The id of the moderation to approve.</param>
        public void Approve(int userId, int moderationId)
        {
            using (var context = new DilemmaContext())
            {
                UpdateModerationState(context, ModerationState.Approved, userId, moderationId, string.Empty);
            }
        }

        /// <summary>
        /// Rejects a moderation.
        /// </summary>
        /// <param name="userId">The id of the user rejecting the moderation.</param>
        /// <param name="moderationId">The id of the moderation to reject.</param>
        /// <param name="message">A message as to why the moderation item is being rejected.</param>
        public void Reject(int userId, int moderationId, string message)
        {
            using (var context = new DilemmaContext())
            {
                UpdateModerationState(context, ModerationState.Rejected, userId, moderationId, message);
            }
        }

        private static void OnModerableCreated(DilemmaContext context, Moderation moderation, string message)
        {
            context.Moderations.Add(moderation);

            context.SaveChangesVerbose();

            AddModerationEntry(context, moderation.ModerationId, ModerationState.Queued, moderation.ForUser.UserId, message);
        }

        private static void UpdateModerationState(DilemmaContext context, ModerationState state, int userId, int moderationId, string message)
        {
            var moderation = context.Moderations
                    .Where(x => x.ModerationId == moderationId)
                    .Select(
                            x => new
                            {
                                x.ModerationId,
                                x.ModerationFor,
                                ForUserId = x.ForUser.UserId,
                                AnswerId = x.Answer == null ? -1 : x.Answer.AnswerId,
                                QuestionId = x.Question == null ? -1 : x.Question.QuestionId,
                            })
                     .ToList()
                     .Select(
                            x => new Moderation
                            {
                                ModerationId = x.ModerationId,
                                ModerationFor = x.ModerationFor,
                                ForUser = new User { UserId = x.ForUserId },
                                Answer = new Answer { AnswerId = x.AnswerId },
                                Question = new Question { QuestionId = x.QuestionId }
                            })
                      .Single();
            
            var entry = AddModerationEntry(context, moderationId, state, userId, message);

            var messageContext = new ModerationMessageContext(state, context, moderation, entry);
            ModerationMessagePipe.Value.Announce(messageContext);
        }

        private static ModerationEntry AddModerationEntry(
            DilemmaContext context,
            int moderationId,
            ModerationState state,
            int currentUserId,
            string message)
        {
            var moderation = context.GetOrAttachNew<Moderation, int>(moderationId, x => x.ModerationId);
            var currentUser = context.GetOrAttachNew<User, int>(currentUserId, x => x.UserId);

            var moderationEntry = new ModerationEntry
            {
                CreatedDateTime = TimeSource.Value.Now,
                Moderation = moderation,
                State = state,
                AddedByUser = currentUser,
                Message = message
            };

            context.ModerationEntries.Add(moderationEntry);

            context.SaveChangesVerbose();

            return moderationEntry;
        }
    }
}
