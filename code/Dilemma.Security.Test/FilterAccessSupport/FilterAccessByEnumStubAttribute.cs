using System.Linq;

using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class FilterAccessByEnumStubAttribute : FilterAccessByEnumWrapperAttribute
    {
        public FilterAccessByEnumStubAttribute(AllowDeny allowDeny, params TestEnum[] comparisonObjects)
            : base(typeof(FilterAccessStub), allowDeny, comparisonObjects.Cast<object>())
        {
        }

        public FilterAccessByEnumStubAttribute(string controller, string action, AllowDeny allowDeny, params TestEnum[] comparisonObjects)
            : base(typeof(FilterAccessStub), controller, action, allowDeny, comparisonObjects.Cast<object>())
        {
        }
    
    }
}