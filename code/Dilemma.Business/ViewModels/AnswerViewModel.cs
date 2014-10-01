using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Business.ViewModels
{
    /// <summary>
    /// The answer view model.
    /// </summary>
    public class AnswerViewModel
    {
        /// <summary>
        /// Gets or sets the answer id.
        /// </summary>
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets or sets the answer text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Flag indicating that the current user added this answer.
        /// </summary>
        public bool IsMyAnswer { get; set; }

        /// <summary>
        /// Flag indicating that the answer has passed moderation.
        /// </summary>
        public bool IsApproved { get; set; }
    }
}
