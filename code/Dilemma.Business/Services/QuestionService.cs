using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Services
{
    internal class QuestionService : IQuestionService
    {
        private static readonly Lazy<IQuestionRepository> QuestionRepository = new Lazy<IQuestionRepository>(Locator.Current.Instance<IQuestionRepository>);

        public void SaveNew(QuestionViewModel questionViewModel)
        {
            QuestionRepository.Value.Create(questionViewModel);
        }

        public IEnumerable<QuestionViewModel> GetAll()
        {
            return QuestionRepository.Value.List<QuestionViewModel>();
        }
    }
}
