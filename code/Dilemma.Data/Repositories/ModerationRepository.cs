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
                        .Include(x => x.Question)
                        .Include(x => x.Answer)
                        .Include(x => x.Answer.Question)
                        .Include(x => x.Answer.Question.User)
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

                var user = context.EnsureAttached(new User { UserId = userId }, x => x.UserId);
                
                AddModerationEntry(context, moderation, ModerationEntryType.Approved, user, string.Empty);

                if (moderation.ModerationFor == ModerationFor.Answer)
                {
                    NotificationRepository.Value.Raise(context, moderation.Answer.Question.User.UserId, NotificationType.QuestionAnswered, moderation.Answer.AnswerId);
                }
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

                var user = context.EnsureAttached(new User { UserId = userId }, x => x.UserId);
                
                AddModerationEntry(context, moderation, ModerationEntryType.Rejected, user, message);

                NotificationRepository.Value.Raise(context, moderation.ForUser.UserId, NotificationType.PostRejected, moderation.ModerationId);
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
