using System;
using System.Web.Mvc;

namespace Dilemma.Security.AccessFilters.ByEnum
{
    public interface IFilterAccessByEnum
    {
        void OnActionExecuting<T>(ActionExecutingContext filterContext) where T : FilterAccessByEnumWrapperAttribute;

        Type GetEnumType();
    }
}