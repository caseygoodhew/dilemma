using System;
using System.Linq;

using Disposable.Common.Extensions;

namespace Dilemma.Common
{
    public static class SystemEnvironmentValidation
    {
        public static bool IsInternalEnvironment(SystemEnvironment systemEnvironment)
        {
            VerifySystemEnvironments();

            return systemEnvironment == SystemEnvironment.Development
                   || systemEnvironment == SystemEnvironment.Testing;
        }

        private static bool systemEnvironmentConfirmed;
        
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
                                 SystemEnvironment.Testing
                             }
                     .OrderBy(x => x)))
            {
                throw new InvalidOperationException("Actual and expected SystemEnvironments do not match");
            }

            systemEnvironmentConfirmed = true;
        }
    }
}