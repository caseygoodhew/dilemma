using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Data.Repositories
{
    public interface IQuestionRepository
    {
        void Create<T>(T questionType) where T : class;

        IEnumerable<T> List<T>() where T : class;
    }
}
