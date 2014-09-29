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

        private static readonly Lazy<INotificationRepository> NotificationRepository = Locator.Lazy<INotificationRepository>();
        
        public void OnQuestionCreated(Question question)
        {
            using (var context = new DilemmaContext())
            {
                context.Questions.Attach(question);
                context.Users.Attach(question.User);
                
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
        }

        public void OnAnswerCreated(Answer answer)
        {
            using (var context = new DilemmaContext())
            {
                context.Answers.Attach(answer);
                context.Users.Attach(answer.User);
                
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
        }

        public T GetNext<T>() where T : class
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations.Include(x => x.ForUser)
                        .Include(x => x.ModerationEntries)
                        .Where(x => x.State == ModerationState.Queued)
                        .OrderBy(
                            x =>
                            x.ModerationEntries.OrderByDescending(y => y.CreatedDateTime)
                                .Select(y => y.CreatedDateTime)
                                .FirstOrDefault())
                        .FirstOrDefault();

                if (moderation != null)
                {
                    moderation.ModerationEntries = moderation.ModerationEntries.OrderBy(x => x.CreatedDateTime).ToList();
                }

                return ConverterFactory.ConvertOne<Moderation, T>(moderation);
            }
        }

        public void Approve(int userId, int moderationId)
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations
                        .Include(x => x.Question)
                        .Include(x => x.Answer)
                        .Single(x => x.ModerationId == moderationId);

                if (moderation.State == ModerationState.Approved)
                {
                    return;
                }

                moderation.State = ModerationState.Approved;
                context.Entry(moderation).State = EntityState.Modified;

                switch (moderation.ModerationFor)
                {
                    case ModerationFor.Question:
                        moderation.Question.IsApproved = true;
                        context.Entry(moderation.Question).State = EntityState.Modified;
                        break;
                    case ModerationFor.Answer:
                        moderation.Answer.IsApproved = true;
                        context.Entry(moderation.Answer).State = EntityState.Modified;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var user = new User { UserId = userId };
                context.Users.Attach(user);
                
                AddModerationEntry(context, moderation, ModerationEntryType.Approved, user, string.Empty);
            }
        }

        public void Reject(int userId, int moderationId, string message)
        {
            using (var context = new DilemmaContext())
            {
                var moderation =
                    context.Moderations
                        .Include(x => x.Question)
                        .Include(x => x.Answer)
                        .Include(x => x.ForUser)
                        .Single(x => x.ModerationId == moderationId);
                
                if (moderation.State == ModerationState.Rejected)
                {
                    return;
                }

                moderation.State = ModerationState.Rejected;
                context.Entry(moderation).State = EntityState.Modified;
                
                switch (moderation.ModerationFor)
                {
                    case ModerationFor.Question:
                        moderation.Question.IsApproved = false;
                        context.Entry(moderation.Question).State = EntityState.Modified;
                        break;
                    case ModerationFor.Answer:
                        moderation.Answer.IsApproved = false;
                        context.Entry(moderation.Answer).State = EntityState.Modified;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var user = context.EnsureAttached<User, int>(x => x.UserId, userId);
                
                AddModerationEntry(context, moderation, ModerationEntryType.Rejected, user, message);

                NotificationRepository.Value.Raise(moderation.ForUser.UserId, NotificationType.PostRejected, moderation.ModerationId);
            }
        }

        private static void OnModerableCreated(DilemmaContext context, Moderation moderation, string message)
        {
            moderation.State = ModerationState.Queued;
            
            context.Moderations.Add(moderation);
            
            context.SaveChangesVerbose();

            AddModerationEntry(context, moderation, ModerationEntryType.Created, moderation.ForUser, message);
        }

        private static void AddModerationEntry(
            DilemmaContext context,
            Moderation moderation,
            ModerationEntryType entryType,
            User currentUser,
            string message)
        {
            
            var moderationEntry = new ModerationEntry
            {
                CreatedDateTime = TimeSource.Value.Now,
                Moderation = moderation,
                EntryType = entryType,
                AddedByUser = currentUser,
                Message = message
            };

            context.ModerationEntries.Add(moderationEntry);

            context.SaveChangesVerbose();
        }
    }
}
