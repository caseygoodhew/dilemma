using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;

namespace Dilemma.Conversion
{
    public static class QuestionConverter
    {
        public static QuestionViewModel ToQuestionViewModel(Question question)
        {
            return new QuestionViewModel { Text = question.Text + "->QuestionViewModel" };
        }
    }
}
