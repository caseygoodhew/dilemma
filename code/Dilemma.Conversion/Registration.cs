using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

using Disposable.Common.ServiceLocator;
using Disposable.Common.Conversion;

namespace Dilemma.Conversion
{
    public static class Registration
    {
        public static void Register(IRegistrar registrar)
        {
            ConverterFactory.Register<Question, QuestionViewModel>(registrar, QuestionConverter.ToQuestionViewModel);
            ConverterFactory.Register<QuestionViewModel, Question>(registrar, QuestionViewModelConverter.ToQuestion);
        }
    }
}
