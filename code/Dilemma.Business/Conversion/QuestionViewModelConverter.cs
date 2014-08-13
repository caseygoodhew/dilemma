using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Business.Conversion
{
    public static class QuestionViewModelConverter
    {
        public static Question ToQuestion(QuestionViewModel viewModel)
        {
            return new Question { Text = viewModel.Text };
        }

        public static QuestionViewModel FromQuestion(Question model)
        {
            return new QuestionViewModel { Text = model.Text };
        }
    }
}
