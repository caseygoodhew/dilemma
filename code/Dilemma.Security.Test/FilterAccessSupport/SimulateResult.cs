using System.Web.Mvc;

using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class SimulateResult<T>
    {
        public readonly ActionExecutingContext FilterContext;
        
        public readonly T Controller;

        public readonly T Action;

        public SimulateResult(ActionExecutingContext context, IFilterAccessByEnumWrapper controller, IFilterAccessByEnumWrapper action)
        {
            FilterContext = context;
            
            if (controller != null)
            {
                Controller = ((T)controller.FilterAccessByEnum);
            }

            if (action != null)
            {
                Action = ((T)action.FilterAccessByEnum);
            }
        }
    }
}