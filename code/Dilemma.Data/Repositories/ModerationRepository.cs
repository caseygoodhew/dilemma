using System;
using System.Data.Entity;
using System.Linq;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    internal class ModerationRepository : IModerationRepository, IInternalModerationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<IInternalNotificationRepository> NotificationRepository = Locator.Lazy<IInternalNotificationRepository>();

        private static readonly Lazy<IInternalQuestionRepository> QuestionRepository = Locator.Lazy<IInternalQuestionRepository>();
        
        public void OnQuestionCreated(DilemmaContext context, Question question)
        {
            context.EnsureAttached(question, x => x.QuestionId);
            context.EnsureAttached(question.User, x => x.UserId);
                
            OnModerableCreated(
                context,
                new Moderation
                    {
                        ModerationFor = ModerationFor.Question,
                        Question = question,
                        ForUser = question.User
                    },
                question.Text);
        }

        public void OnAnswerCreated(DilemmaContext context, Answer answer)
        {
            context.EnsureAttached(answer, x => x.AnswerId);
            context.EnsureAttached(answer.User, x => x.UserId);
                
            OnModerableCreated(
                context,
                new Moderation
                    {
                        ModerationFor = ModerationFor.Answer,
                        Answer = answer,
                        ForUser = answer.User
                    },
                answer.Text);
            
        }

        public T GetNext<T>() where T : class
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations.Include(x => x.ForUser)
                        .Include(x => x.ModerationEntries)
                        .Select(x => new {
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

        public void Approve(int userId, int moderationId)
        {
            using (var context = new DilemmaContext())
            {
                UpdateModerationState(context, ModerationState.Approved, userId, moderationId, string.Empty);
            }
        }

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

        private void UpdateModerationState(DilemmaContext context, ModerationState state, int userId, int moderationId, string message)
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
            
            AddModerationEntry(context, moderationId, state, userId, message);

            if (state == ModerationState.Rejected)
            {
                NotificationRepository.Value.Raise(
                    context,
                    moderation.ForUser.UserId,
                    NotificationType.PostRejected,
                    moderation.ModerationId);
            }

            switch (moderation.ModerationFor)
            {
                case ModerationFor.Question:
                    QuestionRepository.Value.UpdateQuestionState(context, moderation.Question.QuestionId, state);
                    break;
                case ModerationFor.Answer:
                    QuestionRepository.Value.UpdateAnswerState(context, moderation.Answer.AnswerId, state);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void AddModerationEntry(
            DilemmaContext context,
            int moderationId,
            ModerationState state,
            int currentUserId,
            string message)
        {
            var moderation = context.EnsureAttached(new Moderation { ModerationId = moderationId }, x => x.ModerationId);
            var currentUser = context.EnsureAttached(new User { UserId = currentUserId }, x => x.UserId);

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
        }
    }
}
