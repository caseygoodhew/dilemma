using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Common;
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
                context.SaveChangesVerbose();
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
                                    TotalAnswer = x.Answers.Count,
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
                                    TotalAnswers = x.TotalAnswer,
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
                var existingAnswer = GetAnswerInProgress(context, questionId);

                if (existingAnswer != null)
                {
                    return existingAnswer.AnswerId;
                }
                
                var question = Get<Question>(context, questionId, GetQuestionAs.AnswerCount);

                if (question.TotalAnswers >= question.MaxAnswers || question.ClosesDateTime < TimeSource.Value.Now)
                {
                    return null;
                }

                context.Questions.Attach(question);
                
                // TODO: Potential concurrency issue here if two people vie for the slot at the same time.
                var answer = context.Answers.Add(new Answer { CreatedDateTime = TimeSource.Value.Now, Question = question });
                context.SaveChangesVerbose();

                return answer.AnswerId;
            }
        }

        public T GetAnswerInProgress<T>(int questionId, int answerId) where T : class
        {
            using (var context = new DilemmaContext())
            {
                return ConverterFactory.ConvertOne<Answer, T>(GetAnswerInProgress(context, questionId, answerId));
            }
        }

        public bool CompleteAnswer<T>(int questionId, T answerType) where T : class
        {
            using (var context = new DilemmaContext())
            {
                var answer = ConverterFactory.ConvertOne<T, Answer>(answerType);

                var existingAnswer = GetAnswerInProgress(context, questionId, answer.AnswerId);

                if (existingAnswer == null)
                {
                    return false;
                }
            
                answer.CreatedDateTime = TimeSource.Value.Now;
                answer.AnswerType = AnswerType.Completed;

                context.Answers.Update(context, answer);
                context.SaveChangesVerbose();

                return true;
            }
        }

        private T Get<T>(DilemmaContext context, int questionId, GetQuestionAs config) where T : class
        {
            Question question;

            switch (config)
            {
                case GetQuestionAs.AnswerCount:
                    question =
                        context.Questions.Where(x => x.QuestionId == questionId)
                            .Select(x => new
                                             {
                                                 x.QuestionId,
                                                 x.MaxAnswers,
                                                 x.ClosesDateTime,
                                                 TotalAnswers = x.Answers.Count
                                             })
                            .AsEnumerable()
                            .Select(x => new Question
                                             {
                                                 QuestionId = x.QuestionId,
                                                 MaxAnswers = x.MaxAnswers,
                                                 ClosesDateTime = x.ClosesDateTime,
                                                 TotalAnswers = x.TotalAnswers
                                             })
                            .Single();

                    break;
                case GetQuestionAs.FullDetails:
                    question =
                        context.Questions.Where(x => x.QuestionId == questionId)
                            .Include(x => x.Category)
                            .Include(x => x.Answers)
                            .Single();
                    
                    question.TotalAnswers = question.Answers.Count;
                    question.Answers = question.Answers.Where(x => x.AnswerType == AnswerType.Completed).ToList();
                    
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return ConverterFactory.ConvertOne<Question, T>(question);
        }

        private Answer GetAnswerInProgress(DilemmaContext context, int questionId, int? answerId = null)
        {
            var query =
                context.Answers.AsNoTracking()
                    .Include(x => x.Question)
                    .Where(x => x.Question.QuestionId == questionId)
                    .Where(x => x.AnswerType == AnswerType.ReservedSlot);

            if (answerId.HasValue)
            {
                query = query.Where(x => x.AnswerId == answerId);
            }

            return query.FirstOrDefault();
        }
    }
}
