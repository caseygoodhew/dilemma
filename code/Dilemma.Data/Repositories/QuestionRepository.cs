using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    public class QuestionRepository
    {
        public void Create(Question question)
        {
            var context = new DilemmaContext();
            context.Questions.Add(question);
            context.SaveChanges();
        }

        public IEnumerable<Question> List()
        {
            var context = new DilemmaContext();
            return context.Questions;
        }
    }
}
