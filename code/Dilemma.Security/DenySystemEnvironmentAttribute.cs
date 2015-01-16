using System;
using System.Linq;

using Dilemma.Common;
using Disposable.Common.Extensions;

namespace Dilemma.Security
{
    /// <summary>
    /// Security attribute to deny access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DenySystemEnvironmentAttribute : FilterAccessBySystemEnvironmentAttribute
    {
        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public DenySystemEnvironmentAttribute(SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(AllowDeny.Deny, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).ToList())
        {
        }

        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should allow.
        /// </summary>
        /// <param name="controller">The controller that should be redirected to in the event of a failure.</param>
        /// <param name="action">The action that should be redirected to in the event of a failure.</param>
        /// <param name="systemEnvironment">The system environment to check for.</param>
        /// <param name="additionalSystemEnvironments">Additional system environments to check for.</param>
        public DenySystemEnvironmentAttribute(string controller, string action, SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(controller, action, AllowDeny.Deny, new[] { systemEnvironment }.Concat(additionalSystemEnvironments).ToList())
        {
        }
    }
    /*
    /// <summary>
    /// Security attribute to deny access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class DenyExceptSystemEnvironmentAttribute : FilterAccessBySystemEnvironmentAttribute
    {
        public DenyExceptSystemEnvironmentAttribute()
            : base(AllowDeny.Deny, EnumExtensions)
        {
        }

        public DenyExceptSystemEnvironmentAttribute(string controller, string action, AllowDeny allowDeny, SystemEnvironment systemEnvironment, params SystemEnvironment[] additionalSystemEnvironments)
            : base(controller, action, allowDeny, systemEnvironment, additionalSystemEnvironments)
        {
        }
    }*/
}