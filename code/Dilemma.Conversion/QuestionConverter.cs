using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Conversion
{
    public static class QuestionConverter
    {
        public static QuestionViewModel ToQuestionViewModel(Question questio)
        {
            return new QuestionViewModel { Text = questio.Text };
        }
    }
}
