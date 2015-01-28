﻿using System;
using System.Diagnostics.CodeAnalysis;
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
        /// Gets or sets the maximum lifetime that <see cref="Question"/>s are allowed to be open. Changing 
        /// this setting will not change the lifetime of existing <see cref="Question"/>s as each 
        /// <see cref="Question"/> stores its own copy of this value when it is created. This allows the 
        /// system to remain dynamic.
        /// </summary>
        public QuestionLifetime QuestionLifetime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SystemEnvironment"/>.
        /// </summary>
        public SystemEnvironment SystemEnvironment { get; set; }

        /// <summary>
        /// Gets a flag indicating if the <see cref="SystemEnvironment"/> is to Development or Testing.
        /// </summary>
        public bool IsInternalEnvironment
        {
            get
            {
                VerifySystemEnvironments();
                
                return SystemEnvironment == SystemEnvironment.Development
                       || SystemEnvironment == SystemEnvironment.Testing;
            }
        }

        private static void VerifySystemEnvironments()
        {
            if (systemEnvironmentConfirmed)
            {
                return;
            }

            if (!EnumExtensions.All<SystemEnvironment>()
                               .OrderBy(x => x)
                               .SequenceEqual(
                                    new[]
                                    {
                                        SystemEnvironment.Production, 
                                        SystemEnvironment.Development, 
                                        SystemEnvironment.Testing,
                                        Common.SystemEnvironment.QuestionSeeder,
                                        Common.SystemEnvironment.Administration
                                    }
                                    .OrderBy(x => x)))
            {
                throw new InvalidOperationException("Actual and expected SystemEnvironments do not match");
            }

            systemEnvironmentConfirmed = true;
        }

        private static bool systemEnvironmentConfirmed;
    }
}
