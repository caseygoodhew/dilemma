using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Data.Repositories
{
    internal class QuestionRepository : IQuestionRepository
    {
        private static readonly Lazy<ITimeSource> TimeSource = new Lazy<ITimeSource>(Locator.Current.Instance<ITimeSource>);
        
        public void Create<T>(T questionType) where T : class
        {
            var question = ConverterFactory.ConvertOne<T, Question>(questionType);

            using (var context = new DilemmaContext())
            {
                context.Categories.Attach(question.Category);
                context.Questions.Add(question);
                context.SaveChanges();
            }
        }

        public T Get<T>(int questionId, GetQuestionAs config) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return Get<T>(context, questionId, config);
            }
        }
        
        public IEnumerable<T> List<T>() where T : class
        {
            // http://stackoverflow.com/questions/2010897/how-to-count-associated-entities-without-fetching-them-in-entity-framework
            using (var context = new DilemmaContext())
            {
                var questions =
                    context.Questions.Include(x => x.Category)
                        .OrderByDescending(x => x.CreatedDateTime)
                        .Select(
                            x =>
                            new
                                {
                                    x.QuestionId,
                                    AnswerCount = x.Answers.Count,
                                    x.MaxAnswers,
                                    x.Category,
                                    x.Text,
                                    x.ClosesDateTime,
                                    x.CreatedDateTime
                                })
                        .AsEnumerable()
                        .Select(
                            x =>
                            new Question
                                {
                                    QuestionId = x.QuestionId,
                                    AnswerCount = x.AnswerCount,
                                    MaxAnswers = x.MaxAnswers,
                                    Category = x.Category,
                                    Text = x.Text,
                                    ClosesDateTime = x.ClosesDateTime,
                                    CreatedDateTime = x.CreatedDateTime
                                });

                return ConverterFactory.ConvertMany<Question, T>(questions.ToList());
            }
        }

        public int? RequestAnswerSlot(int questionId)
        {
            using (var context = new DilemmaContext())
            {
                var question = Get<Question>(context, questionId, GetQuestionAs.AnswerCount);

                if (question.AnswerCount >= question.MaxAnswers || question.ClosesDateTime < TimeSource.Value.Now)
                {
                    return null;
                }

                context.Questions.Attach(question);
                
                // TODO: Potential concurrency issue here if two people vie for the slot at the same time.
                var answer = context.Answers.Add(new Answer { CreatedDateTime = TimeSource.Value.Now, Question = question });
                try
                {
                    context.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting
                            // the current instance as InnerException
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
                

                return answer.AnswerId;
            }
        }

        public T GetAnswerInProgress<T>(int questionId, int answerId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var answer =
                        context.Answers.Include(x => x.Question)
                        .Where(x => x.AnswerId == answerId)
                        .Where(x => x.Question.QuestionId == questionId)
                        .FirstOrDefault(x => string.IsNullOrEmpty(x.Text));

                return ConverterFactory.ConvertOne<Answer, T>(answer);
            }
        }

        public void SaveAnswer<T>(int questionId, T answerType) where T : class
        {
            var answer = ConverterFactory.ConvertOne<T, Answer>(answerType);

            using (var context = new DilemmaContext())
            {
                throw new NotImplementedException();
            }
        }

        private T Get<T>(DilemmaContext context, int questionId, GetQuestionAs config) where T : class
        {
            Question question;

            if (config == GetQuestionAs.AnswerCount)
            {
                question =
                    context.Questions.Where(x => x.QuestionId == questionId)
                        .Select(x => new 
                        {
                            x.QuestionId,
                            x.MaxAnswers,
                            x.ClosesDateTime,
                            AnswerCount = x.Answers.Count
                        })
                        .AsEnumerable()
                        .Select(x => new Question
                        {
                            QuestionId = x.QuestionId,
                            MaxAnswers = x.MaxAnswers,
                            ClosesDateTime = x.ClosesDateTime,
                            AnswerCount = x.AnswerCount
                        })
                        .Single();
            }
            else if (config == GetQuestionAs.FullDetails)
            {
                question =
                    context.Questions.Where(x => x.QuestionId == questionId)
                        .Include(x => x.Category)
                        .Include(x => x.Answers)
                        .Single();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            return ConverterFactory.ConvertOne<Question, T>(question);
        }
    }
}
