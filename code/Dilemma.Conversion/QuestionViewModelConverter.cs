using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Conversion
{
    public static class QuestionViewModelConverter
    {
        public static Question ToQuestion(QuestionViewModel questionViewModel)
        {
            return new Question { Text = questionViewModel.Text };
        }
    }
}
