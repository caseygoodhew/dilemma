using System.Collections.Generic;
using System.Web.Mvc;

namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public class FilterAccessStub : FilterAccessByEnum<TestEnum>
    {
        public FilterAccessStub(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(allowDeny, comparisonEnums)
        {
        }

        public FilterAccessStub(string controller, string action, AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(controller, action, allowDeny, comparisonEnums)
        {
        }

        public AllowDeny? LastAnnounced { get; private set; }
        
        protected override void AnnounceAllow(ActionExecutingContext filterContext)
        {
            LastAnnounced = AllowDeny.Allow;
        }

        protected override void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<TestEnum> attribute)
        {
            LastAnnounced = AllowDeny.Deny;
        }

        protected override TestEnum GetComparisonValue()
        {
            return TestEnumProvider.Value;
        }

        protected void ResetLast()
        {
            LastAnnounced = null;
        }
    }
}