using System;
using System.Web.Mvc;

namespace Dilemma.Security.AccessFilters.ByEnum
{
    public interface IFilterAccessByEnum
    {
        void OnActionExecuting<T>(ActionExecutingContext filterContext) where T : IFilterAccessByEnumWrapper;

        Type GetEnumType();
    }

    public interface IFilterAccessByEnumWrapper
    {
        IFilterAccessByEnum FilterAccessByEnum { get; }
    }
}