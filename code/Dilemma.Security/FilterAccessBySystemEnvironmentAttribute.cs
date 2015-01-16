using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    public enum AllowDeny
    {
        Indeterminate,
        Allow,
        Deny
    }
    
    /// <summary>
    /// Baseline security attribute to allow or deny access to a controller or action based on the current <see cref="SystemEnvironment"/>.
    /// </summary>
    public abstract class FilterAccessBySystemEnvironmentAttribute : ActionFilterAttribute
    {
        private static readonly Lazy<IAdministrationRepository> AdministrationRepository =
            Locator.Lazy<IAdministrationRepository>();

        internal readonly AllowDeny AllowDeny;
        
        internal readonly IList<SystemEnvironment> SystemEnvironments;

        internal readonly string Controller = "Home";

        internal readonly string Action = "Index";

        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should check for.
        /// </summary>
        /// <param name="allowDeny">Flag to either allow or deny the provided <see cref="SystemEnvironment"/>s.</param>
        /// <param name="systemEnvironments">The system environment to check for.</param>
        protected FilterAccessBySystemEnvironmentAttribute(
            AllowDeny allowDeny,
            IList<SystemEnvironment> systemEnvironments)
        {
            if (allowDeny == AllowDeny.Indeterminate)
            {
                throw new InvalidOperationException("Indeterminate cannot be specified in the constructor.");
            }
            
            AllowDeny = allowDeny;
            SystemEnvironments = systemEnvironments;
        }

        /// <summary>
        /// The list of <see cref="SystemEnvironment"/>s that the attribute should check for.
        /// </summary>
        /// <param name="allowDeny">Flag to either allow or deny the provided <see cref="SystemEnvironment"/>s.</param>
        /// <param name="controller">The controller that should be redirected to in the event of a failure.</param>
        /// <param name="action">The action that should be redirected to in the event of a failure.</param>
        /// <param name="systemEnvironments">The system environments to check for.</param>
        protected FilterAccessBySystemEnvironmentAttribute(
            string controller,
            string action,
            AllowDeny allowDeny,
            IList<SystemEnvironment> systemEnvironments)
            : this(allowDeny, systemEnvironments)
        {
            Controller = controller;
            Action = action;
        }

        /// <summary>
        /// Called by the ASP.NET MVC framework before the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            FilterAccessBySystemEnvironmentAttribute controllerAttribute;
            FilterAccessBySystemEnvironmentAttribute actionAttribute;
            GetFilterAttributes(filterContext, out controllerAttribute, out actionAttribute);

            if (controllerAttribute == null || actionAttribute == null)
            {
                var attribute = controllerAttribute ?? actionAttribute;

                if (attribute.AllowDeny == AllowDeny.Allow && Check(attribute) != AllowDeny.Allow)
                {
                    DoRedirect(filterContext, attribute);
                }
                else if (attribute.AllowDeny == AllowDeny.Deny && Check(attribute) == AllowDeny.Deny)
                {
                    DoRedirect(filterContext, attribute);
                }
            }
            else if (controllerAttribute.AllowDeny == AllowDeny.Allow)
            {
                if (Check(controllerAttribute) == AllowDeny.Allow)
                {
                    if (Check(actionAttribute) == AllowDeny.Deny)
                    {
                        DoRedirect(filterContext, actionAttribute);
                    }
                }
                else if (Check(actionAttribute) == AllowDeny.Deny)
                {
                    DoRedirect(filterContext, actionAttribute);
                }
                else if (Check(actionAttribute) == AllowDeny.Indeterminate)
                {
                    DoRedirect(filterContext, controllerAttribute);
                }
            }
            else if (controllerAttribute.AllowDeny == AllowDeny.Deny)
            {
                if (Check(controllerAttribute) == AllowDeny.Deny)
                {
                    if (Check(actionAttribute) != AllowDeny.Allow)
                    {
                        DoRedirect(filterContext, controllerAttribute);
                    }
                }
                else if (Check(actionAttribute) == AllowDeny.Deny)
                {
                    DoRedirect(filterContext, actionAttribute);
                }
            }
        }

        private static void GetFilterAttributes(ActionExecutingContext filterContext, out FilterAccessBySystemEnvironmentAttribute controllerAttribute, out FilterAccessBySystemEnvironmentAttribute actionAttribute)
        {
            controllerAttribute =
                filterContext.Controller.GetType()
                    .GetCustomAttributes(typeof(FilterAccessBySystemEnvironmentAttribute), true)
                    .Cast<FilterAccessBySystemEnvironmentAttribute>()
                    .SingleOrDefault();

            actionAttribute =
                filterContext.ActionDescriptor.GetCustomAttributes(
                    typeof(FilterAccessBySystemEnvironmentAttribute),
                    true).Cast<FilterAccessBySystemEnvironmentAttribute>().SingleOrDefault();
        }

        private static AllowDeny Check(FilterAccessBySystemEnvironmentAttribute attribute)
        {
            var systemEnvironment = AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>().SystemEnvironment;

            return attribute.SystemEnvironments.Contains(systemEnvironment) 
                        ? attribute.AllowDeny
                        : AllowDeny.Indeterminate;
        }

        private static void DoRedirect(ActionExecutingContext filterContext, FilterAccessBySystemEnvironmentAttribute attribute)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = attribute.Controller,
                        action = attribute.Action
                    }));
        }
    }













    public abstract class FilterAccessByEnumWrapperAttribute : ActionFilterAttribute
    {
        public readonly IFilterAccessByEnum FilterAccessByEnum;
        
        protected FilterAccessByEnumWrapperAttribute(Type filterAccessByEnumType, AllowDeny allowDeny, IEnumerable<object> comparisonObjects)
        {
            if (!typeof(IFilterAccessByEnum).IsAssignableFrom(filterAccessByEnumType))
            {
                throw new ArgumentException("filterAccessByEnumType does not implement IFilterAccessByEnum");
            }
            
            FilterAccessByEnum = (IFilterAccessByEnum)Activator.CreateInstance(filterAccessByEnumType, allowDeny, comparisonObjects);
        }

        protected FilterAccessByEnumWrapperAttribute(Type filterAccessByEnumType, string controller, string action, AllowDeny allowDeny, IEnumerable<object> comparisonObjects)
        {
            if (!typeof(IFilterAccessByEnum).IsAssignableFrom(filterAccessByEnumType))
            {
                throw new ArgumentException("filterAccessByEnumType does not implement IFilterAccessByEnum");
            }

            FilterAccessByEnum = (IFilterAccessByEnum)Activator.CreateInstance(filterAccessByEnumType, controller, action, allowDeny, comparisonObjects);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var targetEnumType = FilterAccessByEnum.GetEnumType();
            
            var controllerAttribute =
                filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(typeof(FilterAccessByEnumWrapperAttribute), true)
                    .Cast<FilterAccessByEnumWrapperAttribute>()
                    .SingleOrDefault(x => x.FilterAccessByEnum.GetEnumType() == targetEnumType);

            var actionAttribute =
                filterContext.ActionDescriptor
                    .GetCustomAttributes(typeof(FilterAccessByEnumWrapperAttribute), true)
                    .Cast<FilterAccessByEnumWrapperAttribute>()
                    .SingleOrDefault(x => x.FilterAccessByEnum.GetEnumType() == targetEnumType);

            FilterAccessByEnum.OnActionExecuting(
                filterContext,
                controllerAttribute == null ? null : controllerAttribute.FilterAccessByEnum,
                actionAttribute == null ? null : actionAttribute.FilterAccessByEnum);
        }
    }

    public interface IFilterAccessByEnum
    {
        void OnActionExecuting(ActionExecutingContext filterContext, IFilterAccessByEnum controllerFilter, IFilterAccessByEnum actionFilter);

        Type GetEnumType();
    }
    
    public abstract class FilterAccessByEnum<TEnum> : IFilterAccessByEnum
    {
        internal readonly AllowDeny AllowDeny;

        internal readonly IList<TEnum> ComparisonEnums;

        internal readonly string Controller = "Home";

        internal readonly string Action = "Index";

        protected FilterAccessByEnum(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
        {
            if (allowDeny == AllowDeny.Indeterminate)
            {
                throw new InvalidOperationException("Indeterminate cannot be specified in the constructor.");
            }

            AllowDeny = allowDeny;
            ComparisonEnums = comparisonEnums.Cast<TEnum>().ToList();
        }

        protected FilterAccessByEnum(string controller, string action, AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : this(allowDeny, comparisonEnums)
        {
            Controller = controller;
            Action = action;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext, IFilterAccessByEnum controllerFilter, IFilterAccessByEnum actionFilter)
        {
            var controllerAttribute = (FilterAccessByEnum<TEnum>)controllerFilter;
            var actionAttribute = (FilterAccessByEnum<TEnum>)actionFilter;
            
            if (controllerAttribute == null || actionAttribute == null)
            {
                var attribute = controllerAttribute ?? actionAttribute;

                if (attribute.AllowDeny == AllowDeny.Allow && Check(attribute) != AllowDeny.Allow)
                {
                    AnnounceDeny(filterContext, attribute);
                    return;
                }
                else if (attribute.AllowDeny == AllowDeny.Deny && Check(attribute) == AllowDeny.Deny)
                {
                    AnnounceDeny(filterContext, attribute);
                    return;
                }
            }
            else if (controllerAttribute.AllowDeny == AllowDeny.Allow)
            {
                if (Check(controllerAttribute) == AllowDeny.Allow)
                {
                    if (Check(actionAttribute) == AllowDeny.Deny)
                    {
                        AnnounceDeny(filterContext, actionAttribute);
                        return;
                    }
                }
                else if (Check(actionAttribute) == AllowDeny.Deny)
                {
                    AnnounceDeny(filterContext, actionAttribute);
                    return;
                }
                else if (Check(actionAttribute) == AllowDeny.Indeterminate)
                {
                    AnnounceDeny(filterContext, controllerAttribute);
                    return;
                }
            }
            else if (controllerAttribute.AllowDeny == AllowDeny.Deny)
            {
                if (Check(controllerAttribute) == AllowDeny.Deny)
                {
                    if (Check(actionAttribute) != AllowDeny.Allow)
                    {
                        AnnounceDeny(filterContext, controllerAttribute);
                        return;
                    }
                }
                else if (Check(actionAttribute) == AllowDeny.Deny)
                {
                    AnnounceDeny(filterContext, actionAttribute);
                    return;
                }
            }

            AnnounceAllow(filterContext);
        }

        public Type GetEnumType()
        {
            return typeof(TEnum);
        }

        private AllowDeny Check(FilterAccessByEnum<TEnum> attribute)
        {
            var comparisonValue = GetComparisonValue();

            return attribute.ComparisonEnums.Contains(comparisonValue)
                        ? attribute.AllowDeny
                        : AllowDeny.Indeterminate;
        }

        protected abstract void AnnounceAllow(ActionExecutingContext filterContext);

        protected abstract void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<TEnum> attribute);

        protected abstract TEnum GetComparisonValue();
    }
}