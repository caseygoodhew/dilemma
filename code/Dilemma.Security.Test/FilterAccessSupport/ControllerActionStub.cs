using Dilemma.Common;
using Dilemma.Security.AccessFilters;
using Dilemma.Security.AccessFilters.ByEnum;

namespace Dilemma.Security.Test.FilterAccessSupport
{
    public static class ControllerActionStub
    {
        [FilterAccessByEnumStub(AllowDeny.Allow, TestEnum.X, TestEnum.Y )]
        public static void AllowXY() { }

        [FilterAccessByEnumStub(AllowDeny.Deny, TestEnum.X, TestEnum.Y)]
        public static void DenyXY() { }

        [FilterAccessByEnumStub(AllowDeny.Allow, TestEnum.X)]
        public static void AllowX() { }

        [FilterAccessByEnumStub(AllowDeny.Deny, TestEnum.X)]
        public static void DenyX() { }

        [FilterAccessByEnumStub(AllowDeny.Allow, TestEnum.Z)]
        public static void AllowZ() { }

        [FilterAccessByEnumStub(AllowDeny.Deny, TestEnum.Z)]
        public static void DenyZ() { }

        [AllowUserRole(UserRole.Administrator)]
        public static void AllowAdministrator() { }

        [DenyUserRole(UserRole.Administrator)]
        public static void DenyAdministrator() { }

        public static void NotSet() { }
    }
}