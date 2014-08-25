using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    public interface IQuestionRepository
    {
        void Create<T>(T questionType) where T : class;

        T Get<T>(int questionId, GetQuestionAs config) where T : class;
        
        IEnumerable<T> List<T>() where T : class;

        int? RequestAnswerSlot(int questionId);

        T GetAnswerInProgress<T>(int questionId, int answerId) where T : class;

        bool CompleteAnswer<T>(int questionId, T answerType) where T : class;
    }
}
