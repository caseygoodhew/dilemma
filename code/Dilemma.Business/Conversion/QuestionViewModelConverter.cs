using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Data.Models;
using Dilemma.Security;

using Disposable.Common.Conversion;
using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="QuestionViewModel"/>.
    /// </summary>
    public static class QuestionViewModelConverter
    {
        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Converts a <see cref="QuestionViewModel"/> to a <see cref="Question"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="QuestionViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Question"/>.</returns>
        public static Question ToQuestion(QuestionViewModel viewModel)
        {
            return new Question
                       {
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime.GuardedValue("CreatedDateTime"),
                           ClosesDateTime = viewModel.ClosesDateTime.GuardedValue("ClosesDateTime"),
                           Category = new Category { CategoryId = viewModel.CategoryId.GuardedValue("CategoryId") },
                           MaxAnswers = viewModel.MaxAnswers.GuardedValue("MaxAnswers")
                       };
        }

        /// <summary>
        /// Converts a <see cref="Question"/> to a <see cref="QuestionViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Question"/> to convert.</param>
        /// <returns>The resultant <see cref="QuestionViewModel"/>.</returns>
        public static QuestionViewModel FromQuestion(Question model)
        {
            var answers = ConverterFactory.ConvertMany<Answer, AnswerViewModel>(model.Answers);
            
            var userId = SecurityManager.Value.GetUserId();

            return new QuestionViewModel
                       {
                           QuestionId = model.QuestionId,
                           Text = model.Text,
                           CreatedDateTime = model.CreatedDateTime,
                           ClosesDateTime = model.ClosesDateTime,
                           CategoryId = model.Category.CategoryId,
                           CategoryName = model.Category.Name,
                           TotalAnswers = model.TotalAnswers,
                           MaxAnswers = model.MaxAnswers,
                           IsMyQuestion = model.User.UserId == userId,
                           IsApproved = model.IsApproved,
                           Answers = (answers ?? Enumerable.Empty<AnswerViewModel>()).ToList()
                       };
        }
    }
}
