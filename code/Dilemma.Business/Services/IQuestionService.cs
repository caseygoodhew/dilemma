using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;

namespace Dilemma.Business.Services
{
    public interface IQuestionService
    {
        void SaveNew(QuestionViewModel questionViewModel);

        IEnumerable<QuestionViewModel> GetAll();
    }
}
