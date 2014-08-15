using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.EntityFramework.Initialization
{
    internal static class SystemConfigurationInitialization
    {
        internal static void Seed(DilemmaContext context)
        {
            if (!context.SystemConfiguration.Any())
            {
                context.SystemConfiguration.Add(new SystemConfiguration
                {
                    MaxAnswers = 10,
                    QuestionLifetime = QuestionLifetime.OneDay
                });
            }
        }
    }
}
