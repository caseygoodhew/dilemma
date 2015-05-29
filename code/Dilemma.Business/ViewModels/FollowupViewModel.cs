using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dilemma.Business.WebPurify;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The Followup view model.
    /// </summary>
    public class FollowupViewModel : IWebPurifyable
    {
        /// <summary>
        /// Gets or sets the Followup id.
        /// </summary>
        public int? FollowupId { get; set; }

        /// <summary>
        /// Gets or sets the Followup text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current user added this Followup.
        /// </summary>
        public bool IsMyFollowup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Followup has passed moderation.
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Followup has been rejected.
        /// </summary>
        public bool IsRejected { get; set; }

        public bool IHaveFlagged { get; set; }

        public void SetWebPurifyStatus(WebPurifyStatus status)
        {
            WebPurifyStatus = status;
        }

        public WebPurifyStatus? WebPurifyStatus { get; set; }
    }
}
