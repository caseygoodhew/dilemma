using System;
using System.Collections.Generic;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Business.WebPurify;
using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Security;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

namespace Dilemma.Business.Conversion
{
    /// <summary>
    /// Converts to and from the <see cref="AnswerViewModel"/>.
    /// </summary>
    public static class AnswerViewModelConverter
    {
        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        private static readonly Lazy<ISiteService> SiteService = Locator.Lazy<ISiteService>();

        /// <summary>
        /// Converts an <see cref="AnswerViewModel"/> to an <see cref="Answer"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="AnswerViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Answer"/>.</returns>
        public static Answer ToAnswer(AnswerViewModel viewModel)
        {
            return new Answer
                       {
                           AnswerId = viewModel.AnswerId.GuardedValue(),
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime,
                           WebPurifyAttempted = viewModel.WebPurifyStatus != null,
                           PassedWebPurify = viewModel.WebPurifyStatus == WebPurifyStatus.Ok
                       };
        }

        /// <summary>
        /// Converts an <see cref="Answer"/> to an <see cref="AnswerViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Answer"/> to convert.</param>
        /// <returns>The resultant <see cref="AnswerViewModel"/>.</returns>
        public static AnswerViewModel FromAnswer(Answer model)
        {
            var userId = SecurityManager.Value.GetUserId();

            var userVotes = model.UserVotes ?? new List<int>();

            var rank = SiteService.Value.GetRankByPoints(model.User.TotalPoints);

            return new AnswerViewModel
                {
                    AnswerId = model.AnswerId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    IsMyAnswer = model.User.UserId == userId,
                    IsApproved = model.AnswerState == AnswerState.Approved,
                    IsRejected = model.AnswerState == AnswerState.Rejected,
                    VoteCount = userVotes.Count,
                    HasMyVote = userVotes.Contains(userId),
                    UserLevel = rank.Level,
                    IsReservedSlot = model.AnswerState == AnswerState.ReservedSlot
                };
        }
    }
}
