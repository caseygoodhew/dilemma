using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
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
            var answers = (ConverterFactory.ConvertMany<Answer, AnswerViewModel>(model.Answers) ?? Enumerable.Empty<AnswerViewModel>()).ToList();
            var followup = model.Followup == null ? null : ConverterFactory.ConvertOne<Followup, FollowupViewModel>(model.Followup);
            var includeFollowup = followup != null && !followup.IsRejected
                                  && (followup.IsApproved || followup.IsMyFollowup);
            
            var userId = SecurityManager.Value.GetUserId();

            var questionViewModel = new QuestionViewModel
                       {
                           QuestionId = model.QuestionId,
                           Text = model.Text,
                           CreatedDateTime = model.CreatedDateTime,
                           ClosesDateTime = model.ClosesDateTime,
                           ClosedDateTime = model.ClosedDateTime,
                           CategoryId = model.Category.CategoryId,
                           CategoryName = model.Category.Name,
                           TotalAnswers = model.TotalAnswers,
                           MaxAnswers = model.MaxAnswers,
                           IsMyQuestion = model.User.UserId == userId,
                           IsApproved = model.QuestionState == QuestionState.Approved,
                           IsRejected = model.QuestionState == QuestionState.Rejected,
                           IsOpen = model.QuestionState == QuestionState.Approved && !model.ClosedDateTime.HasValue,
                           IsClosed = model.ClosedDateTime.HasValue,
                           HasFollowup = followup != null,
                           Answers = answers.Where(x => !x.IsReservedSlot).ToList(),
                           Followup = includeFollowup ? followup : null,
                           IHaveAnswsered = answers.Any(x => x.IsMyAnswer),
                           IHaveVoted = answers.Any(x => x.HasMyVote)
                       };

            if (model.Answers == null)
            {
                return questionViewModel;
            }

            var approvedAnswers = model.Answers.Where(x => x.AnswerState == AnswerState.Approved).ToList();
            
            if (!approvedAnswers.Any())
            {
                return questionViewModel;
            }

            var lookup = approvedAnswers.Where(x => x.UserVotes.Any()).ToDictionary(x => x.AnswerId, x => x.UserVotes);
            
            if (!lookup.Any())
            {
                return questionViewModel;
            }
            
            var starAnswerId = lookup.Where(x => x.Value.Contains(model.User.UserId)).Select(x => (int?)x.Key).SingleOrDefault();
            if (starAnswerId.HasValue)
            {
                questionViewModel.Answers.Single(x => x.AnswerId == starAnswerId).IsStarVote = true;
            }

            var maxVotes = lookup.Max(x => x.Value.Count);
            if (maxVotes >= 10)
            {
                questionViewModel.Answers.Where(x => x.VoteCount == maxVotes).ToList().ForEach(x => x.IsPopularVote = true);
            }
            
            return questionViewModel;
        }
    }
}
