﻿using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Dilemma.Common;

using Disposable.Common.Extensions;

namespace Dilemma.Data.Models
{
    /// <summary>
    /// The system configuration model.
    /// </summary>
    public class SystemConfiguration
    {
        /// <summary>
        /// Always 1. There is only one system configuration. Getter and setter are implemented for EF compatibility only.
        /// </summary>
        public int Id 
        { 
            get { return 1; }
            // ReSharper disable once ValueParameterNotUsed
            set { }
        }

        /// <summary>
        /// Gets or sets the maximum number of <see cref="Answer"/>s that <see cref="Question"/>s are allowed to have. 
        /// Changing this setting will not change the maximum number of <see cref="Answer"/>s that existing 
        /// <see cref="Question"/>s are allowed to have as each <see cref="Question"/> stores its own 
        /// copy of this value when it is created. This allows the system to remain dynamic.
        /// </summary>
        public int MaxAnswers { get; set; }

        /// <summary>
        /// Gets or sets the maximum lifetime in days that questions are allowed to be open. Changing 
        /// this setting will not change the lifetime of existing <see cref="Question"/>s as each 
        /// <see cref="Question"/> stores its own copy of this value when it is created. This allows the 
        /// system to remain dynamic.
        /// </summary>
        public int QuestionLifetimeDays { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SystemEnvironment"/>.
        /// </summary>
        public SystemEnvironment SystemEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the number of days until a question is retired (deleted) after it is scheduled to close.
        /// </summary>
        public int RetireQuestionAfterDays { get; set; }

        /// <summary>
        /// Gets or sets the number of minutes until an answer slot is expired (deleted) after it was last touched.
        /// </summary>
        public int ExpireAnswerSlotsAfterMinutes { get; set; }

        public bool EnableWebPurify { get; set; }

        public bool EmailErrors { get; set; }

        public string EmailErrorsTo { get; set; }
    }
}
