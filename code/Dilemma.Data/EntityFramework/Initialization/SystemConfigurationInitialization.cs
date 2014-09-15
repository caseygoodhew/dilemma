using System.Linq;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    /// <summary>
    /// Seeds the <see cref="SystemConfiguration"/>s in the database.
    /// </summary>
    internal static class SystemConfigurationInitialization
    {
        /// <summary>
        /// Seeds the <see cref="SystemConfiguration"/>s in the database.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to seed.</param>
        internal static void Seed(DilemmaContext context)
        {
            if (!context.SystemConfiguration.Any())
            {
                context.SystemConfiguration.Add(new SystemConfiguration
                {
                    MaxAnswers = 10,
                    QuestionLifetime = QuestionLifetime.OneDay,
                    SystemEnvironment = SystemEnvironment.Production
                });
            }
        }
    }
}
