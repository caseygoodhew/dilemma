using System;
using System.Web.Mvc;

namespace Dilemma.Security.AccessFilters.ByEnum
{
    public interface IFilterAccessByEnum
    {
        void OnActionExecuting(ActionExecutingContext filterContext, IFilterAccessByEnum controllerFilter, IFilterAccessByEnum actionFilter);

        Type GetEnumType();
    }
}