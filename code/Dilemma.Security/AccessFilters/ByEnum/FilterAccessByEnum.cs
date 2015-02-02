using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Dilemma.Security.AccessFilters.ByEnum
{
    public struct ControllerAction
    {
        public readonly string Controller;

        public readonly string Action;

        public ControllerAction(string controller, string action)
            : this()
        {
            Controller = controller;
            Action = action;
        }
    }
    
    public abstract class FilterAccessByEnum<TEnum> : IFilterAccessByEnum
    {
        internal readonly AllowDeny AllowDeny;

        internal readonly IList<TEnum> ComparisonEnums;

        protected FilterAccessByEnum(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
        {
            if (allowDeny == AllowDeny.Indeterminate)
            {
                throw new InvalidOperationException("Indeterminate cannot be specified in the constructor.");
            }

            AllowDeny = allowDeny;
            ComparisonEnums = comparisonEnums.Cast<TEnum>().ToList();
        }

        public void OnActionExecuting<T>(ActionExecutingContext filterContext) where T : IFilterAccessByEnumWrapper
        {
            var targetEnumType = GetEnumType();

            var controllerAttribute =
                filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(typeof(T), true)
                    .Cast<T>()
                    .SingleOrDefault(x => x.FilterAccessByEnum.GetEnumType() == targetEnumType);

            var actionAttribute =
                filterContext.ActionDescriptor
                    .GetCustomAttributes(typeof(T), true)
                    .Cast<T>()
                    .SingleOrDefault(x => x.FilterAccessByEnum.GetEnumType() == targetEnumType);

            OnActionExecuting(
                filterContext,
                controllerAttribute == null ? null : controllerAttribute.FilterAccessByEnum,
                actionAttribute == null ? null : actionAttribute.FilterAccessByEnum);
        }

        private void OnActionExecuting(ActionExecutingContext filterContext, IFilterAccessByEnum controllerFilter, IFilterAccessByEnum actionFilter)
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
                
                if (attribute.AllowDeny == AllowDeny.Deny && Check(attribute) == AllowDeny.Deny)
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

        protected abstract void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<TEnum> accessFilter);

        protected abstract TEnum GetComparisonValue();
    }
}