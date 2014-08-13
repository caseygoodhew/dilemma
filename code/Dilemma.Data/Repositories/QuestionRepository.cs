using System;
using System.Collections.Generic;
using System.Linq;
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
            context.Questions.Add(question);
            context.SaveChanges();
        }

        public IEnumerable<T> List<T>() where T : class
        {
            var context = new DilemmaContext();
            return ConverterFactory.ConvertMany<Question, T>(context.Questions);
        }
    }
}
