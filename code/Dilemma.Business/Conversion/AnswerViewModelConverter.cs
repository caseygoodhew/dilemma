﻿using System;

using Dilemma.Business.ViewModels;
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
                           CreatedDateTime = viewModel.CreatedDateTime
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
            
            return new AnswerViewModel
                {
                    AnswerId = model.AnswerId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    IsMyAnswer = model.User.UserId == userId,
                    IsApproved = model.AnswerState == AnswerState.Approved,
                    IsRejected = model.AnswerState == AnswerState.Rejected,
                    VoteCount = model.UserVotes.Count,
                    HasMyVote = model.UserVotes.Contains(userId)
                };
        }
    }
}
