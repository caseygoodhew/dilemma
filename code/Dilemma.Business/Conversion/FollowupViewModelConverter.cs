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
    /// Converts to and from the <see cref="FollowupViewModel"/>.
    /// </summary>
    public static class FollowupViewModelConverter
    {
        private static readonly Lazy<ISecurityManager> SecurityManager = Locator.Lazy<ISecurityManager>();

        /// <summary>
        /// Converts an <see cref="FollowupViewModel"/> to an <see cref="Followup"/>.
        /// </summary>
        /// <param name="viewModel">The <see cref="FollowupViewModel"/> to convert.</param>
        /// <returns>The resultant <see cref="Followup"/>.</returns>
        public static Followup ToFollowup(FollowupViewModel viewModel)
        {
            return new Followup
                       {
                           Text = viewModel.Text.Trim(),
                           CreatedDateTime = viewModel.CreatedDateTime,
                           WebPurifyAttempted = viewModel.WebPurifyStatus != null,
                           PassedWebPurify = viewModel.WebPurifyStatus == WebPurifyStatus.Ok
                       };
        }

        /// <summary>
        /// Converts an <see cref="Followup"/> to an <see cref="FollowupViewModel"/>.
        /// </summary>
        /// <param name="model">The <see cref="Followup"/> to convert.</param>
        /// <returns>The resultant <see cref="FollowupViewModel"/>.</returns>
        public static FollowupViewModel FromFollowup(Followup model)
        {
            var userId = SecurityManager.Value.GetUserId();

            return new FollowupViewModel
                {
                    FollowupId = model.FollowupId,
                    Text = model.Text,
                    CreatedDateTime = model.CreatedDateTime,
                    IsMyFollowup = model.User.UserId == userId,
                    IsApproved = model.FollowupState == FollowupState.Approved,
                    IsRejected = model.FollowupState == FollowupState.Rejected
                };
        }
    }
}
