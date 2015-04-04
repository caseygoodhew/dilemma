using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Security;

using Disposable.Common.Conversion;
using Disposable.Common.ServiceLocator;
using Disposable.Common.Services;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="QuestionDetailsViewModel"/>.
    /// </summary>
    public static class QuestionDetailsViewModelConverter
    {
        private static readonly Lazy<ITimeSource> TimeSource = Locator.Lazy<ITimeSource>();

        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Converts a <see cref="QuestionDetailsViewModel"/> to a <see cref="Question"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="QuestionDetailsViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Question"/>.</returns>
        public static Question ToQuestion(QuestionDetailsViewModel viewModel)
        {
            return ConverterFactory.ConvertOne<QuestionViewModel, Question>(viewModel.QuestionViewModel);
        }

        /// <summary>
        /// Converts a <see cref="Question"/> to a <see cref="QuestionDetailsViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Question"/> to convert.</param>
        /// <returns>The resultant <see cref="QuestionDetailsViewModel"/>.</returns>
        public static QuestionDetailsViewModel FromQuestion(Question model)
        {
            var questionViewModel = ConverterFactory.ConvertOne<Question, QuestionViewModel>(model);
            
            return new QuestionDetailsViewModel
                       {
                           QuestionViewModel = questionViewModel,
                           CanAnswer = CanAnswer(model),
                           CanVote = CanVote(model),
                           CanFollowup = !questionViewModel.HasFollowup && CanFollowup(model)
                       };
        }

        private static bool CanAnswer(Question model)
        {
            if (model.QuestionState != QuestionState.Approved)
            {
                return false;
            }

            if (model.ClosedDateTime.HasValue)
            {
                return false;
            }
            
            if (model.TotalAnswers > model.MaxAnswers)
            {
                return false;
            }

            if (model.ClosesDateTime < TimeSource.Value.Now)
            {
                return false;
            }

            var userId = SecurityManager.Value.GetUserId();
            
            if (model.User.UserId == userId)
            {
                return false;
            }

            if (model.Answers.Any(x => x.User.UserId == userId))
            {
                return false;
            }

            return true;
        }

        private static bool CanVote(Question model)
        {
            if (model.QuestionState != QuestionState.Approved)
            {
                return false;
            }

            if (!model.ClosedDateTime.HasValue)
            {
                return false;
            }

            var userId = SecurityManager.Value.GetUserId();
            
            if (model.Answers.Where(x => x.UserVotes != null).SelectMany(x => x.UserVotes).Any(x => x == userId))
            {
                return false;
            }

            return true;
        }

        private static bool CanFollowup(Question model)
        {
            if (model.QuestionState != QuestionState.Approved)
            {
                return false;
            }

            if (!model.ClosedDateTime.HasValue)
            {
                return false;
            }

            var userId = SecurityManager.Value.GetUserId();

            if (model.User.UserId != userId)
            {
                return false;
            }

            return model.Answers.SelectMany(x => x.UserVotes).Contains(userId);
        }
    }
}
