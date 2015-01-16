using System.Linq;

namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public class FilterAccessStubAttribute : FilterAccessByEnumWrapperAttribute
    {
        public FilterAccessStubAttribute(AllowDeny allowDeny, params TestEnum[] comparisonObjects)
            : base(typeof(FilterAccessStub), allowDeny, comparisonObjects.Cast<object>())
        {
        }

        public FilterAccessStubAttribute(string controller, string action, AllowDeny allowDeny, params TestEnum[] comparisonObjects)
            : base(typeof(FilterAccessStub), controller, action, allowDeny, comparisonObjects.Cast<object>())
        {
        }
    }
}