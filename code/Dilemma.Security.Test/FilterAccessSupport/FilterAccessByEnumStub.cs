using System.Collections.Generic;
using System.Web.Mvc;

using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public class FilterAccessStub : FilterAccessByEnum<TestEnum>
    {
        public FilterAccessStub(AllowDeny allowDeny, IEnumerable<object> comparisonEnums)
            : base(allowDeny, comparisonEnums)
        {
        }

        public AllowDeny? LastAnnounced { get; private set; }
        
        protected override void AnnounceAllow(ActionExecutingContext filterContext)
        {
            LastAnnounced = AllowDeny.Allow;
        }

        protected override void AnnounceDeny(ActionExecutingContext filterContext, FilterAccessByEnum<TestEnum> accessFilter)
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