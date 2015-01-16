using System;
using System.Linq;

using Dilemma.Common;
using Disposable.Common.Extensions;

namespace Dilemma.Security
{
    /// <summary>
    /// Security attribute to allow access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AllowSystemEnvironmentAttribute : FilterAccessBySystemEnvironmentAttribute
    {
        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public AllowSystemEnvironmentAttribute(SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).ToList())
        {
        }

        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="controller">The controller that should be redirected to in the event of a failure.</param>
        /// <param name="action">The action that should be redirected to in the event of a failure.</param>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public AllowSystemEnvironmentAttribute(string controller, string action, SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(controller, action, AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).ToList())
        {
        }
    }
}