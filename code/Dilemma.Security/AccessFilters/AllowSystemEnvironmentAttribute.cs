using System;
using System.Linq;

using Dilemma.Common;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.AccessFilters
{
    /// <summary>
    /// Security attribute to allow access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class AllowSystemEnvironmentAttribute : FilterAccessByEnumWrapperAttribute
    {
        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public AllowSystemEnvironmentAttribute(SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(typeof(FilterAccessBySystemEnvironment), AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).Cast<object>())
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
            : base(typeof(FilterAccessBySystemEnvironment), controller, action, AllowDeny.Allow, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).Cast<object>())
        {
        }
    }
}