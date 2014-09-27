using System;
using System.Collections.Generic;
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
    internal class ModerationRepository : IModerationRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();
        
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
                    context.Moderations.Include(x => x.ModerationEntries)
                        .Where(x => x.State == ModerationState.Queued)
                        .OrderByDescending(x => x.MostRecentEntry.CreatedDateTime)
                        .FirstOrDefault();

                if (moderation != null)
                {
                    moderation.ModerationEntries = moderation.ModerationEntries.OrderBy(x => x.CreatedDateTime).ToList();
                }

                return ConverterFactory.ConvertOne<Moderation, T>(moderation);
            }
        }

        private static void OnModerableCreated(DilemmaContext context, Moderation moderation, string text)
        {
            moderation.State = ModerationState.Queued;
            
            var moderationEntry = new ModerationEntry
            {
                CreatedDateTime = TimeSource.Value.Now,
                Moderation = moderation,
                EntryType = ModerationEntryType.Created,
                Message = text
            };
            
            moderation.ModerationEntries = new List<ModerationEntry> { moderationEntry };
            
            context.Moderations.Add(moderation);
            context.SaveChangesVerbose();

            moderation.MostRecentEntry = moderationEntry;

            context.Entry(moderation).State = EntityState.Modified;
            context.SaveChangesVerbose();
        }
    }
}
