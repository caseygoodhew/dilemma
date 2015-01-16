namespace Dilemma.Security.Test.FilterAccessByEnumSupport
{
    public static class ControllerActionStub
    {
        [FilterAccessStub(AllowDeny.Allow, TestEnum.X, TestEnum.Y )]
        public static void AllowXY() { }

        [FilterAccessStub(AllowDeny.Deny, TestEnum.X, TestEnum.Y)]
        public static void DenyXY() { }

        [FilterAccessStub(AllowDeny.Allow, TestEnum.X)]
        public static void AllowX() { }

        [FilterAccessStub(AllowDeny.Deny, TestEnum.X)]
        public static void DenyX() { }

        [FilterAccessStub(AllowDeny.Allow, TestEnum.Z)]
        public static void AllowZ() { }

        [FilterAccessStub(AllowDeny.Deny, TestEnum.Z)]
        public static void DenyZ() { }

        public static void NotSet() { }
    }
}