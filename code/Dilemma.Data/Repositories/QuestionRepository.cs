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



namespace Dilemma.Data.Repositories
{
    internal class QuestionRepository : IQuestionRepository
    {
        public void Create<T>(T questionType) where T : class
        {
            var question = ConverterFactory.ConvertOne<T, Question>(questionType);
            
            var context = new DilemmaContext();
            context.Categories.Attach(question.Category);
            context.Questions.Add(question);
            context.SaveChanges();
        }

        public IEnumerable<T> List<T>() where T : class
        {
            // http://stackoverflow.com/questions/2010897/how-to-count-associated-entities-without-fetching-them-in-entity-framework
            var context = new DilemmaContext();
            var questions = context.Questions
                                   .Include(x => x.Category)
                                   .OrderByDescending(x => x.CreatedDateTime);
            
            return ConverterFactory.ConvertMany<Question, T>(questions.ToList());
        }
    }
}
