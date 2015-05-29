using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;
using Dilemma.Data.Models.Proxies;
using Dilemma.Logging;
using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;
using Disposable.MessagePipe;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Moderation repository implementation.
    /// </summary>
    internal class ManualModerationRepository : IInternalManualModerationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<ILogger> Logger = Locator.Lazy<ILogger>();

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
        /// To be called when an followup is created.
        /// </summary>
        /// <param name="messenger">The followup <see cref="IMessenger{T}"/>.</param>
        public void OnFollowupCreated(IMessenger<FollowupDataAction> messenger)
        {
            var messageContext = messenger.GetContext<FollowupMessageContext>(FollowupDataAction.Created);

            if (messageContext.TestingConfigurationProperty(x => x.ManualModeration).Is(ActiveState.Active))
            {
                var dataContext = messageContext.DataContext;
                var followup = messageContext.Followup;

                dataContext.EnsureAttached(followup, x => x.FollowupId);
                dataContext.EnsureAttached(followup.User, x => x.UserId);

                followup.FollowupState = FollowupState.ReadyForModeration;
                dataContext.Entry(followup).State = EntityState.Modified;

                OnModerableCreated(
                    dataContext,
                    new Moderation
                    {
                        ModerationFor = ModerationFor.Followup,
                        Followup = followup,
                        ForUser = followup.User
                    },
                    followup.Text);
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
            return GetNextForUser<T>(null);
        }

        public T GetNextForUser<T>(int userId) where T : class
        {
            return GetNextForUser<T>((int?)userId);
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

        public T GetQuestionHistory<T>(int userId, int questionId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var question = GetShortQuestion(context.Questions.Where(x => x.User.UserId == userId), questionId);

                if (question == null)
                {
                    return null;
                }

                var moderationEntries =
                    context.ModerationEntries.Where(x => x.Moderation.Question.QuestionId == questionId).ToList();

                return ConverterFactory.ConvertOne<QuestionHistoryProxy, T>(
                    new QuestionHistoryProxy
                        {
                            Question = question,
                            ModerationEntries = moderationEntries
                        });
            }
        }

        public T GetAnswerHistory<T>(int userId, int answerId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var answer = context.Answers.Where(x => x.AnswerId == answerId)
                    .Where(x => x.User.UserId == userId)
                    .Select(
                        x => new
                        {
                            x.Text,
                            x.CreatedDateTime,
                            x.Question.QuestionId
                        }).ToList().Select(
                                     x => new Answer
                                     {
                                         Text = x.Text,
                                         CreatedDateTime = x.CreatedDateTime,
                                         Question = new Question { QuestionId = x.QuestionId },
                                         User = new User { UserId = 0 }
                                     }).SingleOrDefault();

                if (answer == null)
                {
                    return null;
                }

                answer.Question = GetShortQuestion(context.Questions, answer.Question.QuestionId);

                var moderationEntries =
                    context.ModerationEntries.Where(x => x.Moderation.Answer.AnswerId == answerId).ToList();

                return ConverterFactory.ConvertOne<AnswerHistoryProxy, T>(
                    new AnswerHistoryProxy
                        {
                            Answer = answer,
                            Question = answer.Question,
                            ModerationEntries = moderationEntries
                        });
            }
        }

        public T GetFollowupHistory<T>(int userId, int followupId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var followup = context.Followups.Where(x => x.FollowupId == followupId)
                    .Where(x => x.User.UserId == userId)
                    .Select(
                        x => new
                        {
                            x.Text,
                            x.CreatedDateTime,
                            x.Question.QuestionId
                        }).ToList().Select(
                                     x => new Followup
                                     {
                                         Text = x.Text,
                                         CreatedDateTime = x.CreatedDateTime,
                                         Question = new Question { QuestionId = x.QuestionId },
                                         User = new User { UserId = 0 }
                                     }).SingleOrDefault();

                if (followup == null)
                {
                    return null;
                }

                followup.Question = GetShortQuestion(context.Questions, followup.Question.QuestionId);

                var moderationEntries =
                    context.ModerationEntries.Where(x => x.Moderation.Followup.FollowupId == followupId).ToList();

                return ConverterFactory.ConvertOne<FollowupHistoryProxy, T>(
                    new FollowupHistoryProxy
                    {
                        Followup = followup,
                        Question = followup.Question,
                        ModerationEntries = moderationEntries
                    });
            }
        }

        public void ReportQuestion(int userId, int questionId, ReportReason reportReason)
        {
            using (var context = new DilemmaContext())
            {
                var moderation = context.Moderations.AsNoTracking().Where(x => x.Question.QuestionId == questionId).Select(x => new { x.ModerationId, x.Question.Text }).SingleOrDefault();

                if (moderation != null)
                {
                    ReportToExisting(context, userId, reportReason, moderation.ModerationId, moderation.Text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void ReportAnswer(int userId, int answerId, ReportReason reportReason)
        {
            using (var context = new DilemmaContext())
            {
                var moderation = context.Moderations.AsNoTracking().Where(x => x.Answer.AnswerId == answerId).Select(x => new { x.ModerationId, x.Answer.Text }).SingleOrDefault();

                if (moderation != null)
                {
                    ReportToExisting(context, userId, reportReason, moderation.ModerationId, moderation.Text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void ReportFollowup(int userId, int followupId, ReportReason reportReason)
        {
            using (var context = new DilemmaContext())
            {
                var moderation = context.Moderations.AsNoTracking().Where(x => x.Followup.FollowupId == followupId).Select(x => new { x.ModerationId, x.Followup.Text }).SingleOrDefault();

                if (moderation != null)
                {
                    ReportToExisting(context, userId, reportReason, moderation.ModerationId, moderation.Text);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        private static void ReportToExisting(
            DilemmaContext context,
            int userId,
            ReportReason reportReason,
            int moderationId,
            string itemText)
        {
            UpdateModerationState(
                context,
                ModerationState.Reported,
                userId,
                moderationId,
                reportReason.ToString());

            UpdateModerationState(
                context,
                ModerationState.ReQueued,
                userId,
                moderationId,
                string.Format("REPORTED FOR {0}: {1}", reportReason.ToString().ToUpper(), itemText));

            Logger.Value.Info("Moderable Reported");
        }

        private static void OnModerableCreated(DilemmaContext context, Moderation moderation, string message)
        {
            context.Moderations.Add(moderation);

            context.SaveChangesVerbose();

            AddModerationEntry(context, moderation.ModerationId, ModerationState.Queued, moderation.ForUser.UserId, message);

            Logger.Value.Info("Moderable Created");
        }

        private static T GetNextForUser<T>(int? userId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations.Include(x => x.ForUser)
                        .Include(x => x.ModerationEntries)
                        .Where(x => userId == null || x.ForUser.UserId == userId.Value)
                        .Select(x => new
                        {
                            MostRecentEntry = x.ModerationEntries.OrderByDescending(y => y.CreatedDateTime).FirstOrDefault(),
                            x.ModerationEntries,
                            x.ModerationFor,
                            x.ModerationId
                        })
                        .Where(x => x.MostRecentEntry.State == ModerationState.Queued || x.MostRecentEntry.State == ModerationState.ReQueued)
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
                                FollowupId = x.Followup == null ? -1 : x.Followup.FollowupId
                            })
                     .ToList()
                     .Select(
                            x => new Moderation
                            {
                                ModerationId = x.ModerationId,
                                ModerationFor = x.ModerationFor,
                                ForUser = new User { UserId = x.ForUserId },
                                Answer = new Answer { AnswerId = x.AnswerId },
                                Question = new Question { QuestionId = x.QuestionId },
                                Followup = new Followup {  FollowupId = x.FollowupId }
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

        private static Question GetShortQuestion(IQueryable<Question> questionQuery, int questionId)
        {
            return questionQuery.Where(x => x.QuestionId == questionId)
                    .Select(
                        x => new
                        {
                            x.Text,
                            x.CreatedDateTime,
                            x.Category,
                        }).ToList().Select(
                                     x => new Question
                                     {
                                         Text = x.Text,
                                         CreatedDateTime = x.CreatedDateTime,
                                         Category = x.Category,
                                         User = new User { UserId = 0 }
                                     }).SingleOrDefault();
        }
    }
}
