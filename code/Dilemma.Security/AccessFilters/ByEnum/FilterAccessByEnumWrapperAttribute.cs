using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Dilemma.Security.AccessFilters.ByEnum
{
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
}