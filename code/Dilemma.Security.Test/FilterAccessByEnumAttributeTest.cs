using Dilemma.Security.Test.FilterAccessByEnumSupport;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.Security.Test
{
    [TestClass]
    public class FilterAccessByEnumTest
    {
        [TestMethod]
        public void ControllerDenyXY_ActionAllowX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.DenyXY, AllowDenyXYZ.AllowX);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
        }

        [TestMethod]
        public void ControllerAllowXY_ActionDenyX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.AllowXY, AllowDenyXYZ.DenyX);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
        }

        [TestMethod]
        public void ControllerDenyXY_ActionAllowZ()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.DenyXY, AllowDenyXYZ.AllowZ);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
        }

        [TestMethod]
        public void ControllerAllowXY_ActionDenyZ()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.AllowXY, AllowDenyXYZ.DenyZ);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
        }


        [TestMethod]
        public void ControllerDenyXY_ActionDenyZ()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.DenyXY, AllowDenyXYZ.DenyZ);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
        }

        [TestMethod]
        public void ControllerAllowXY_ActionAllowZ()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.AllowXY, AllowDenyXYZ.AllowZ);

            TestEnumProvider.Value = TestEnum.X;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Y;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Z;
            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
        }

        [TestMethod]
        public void ControllerDenyX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.DenyX, AllowDenyXYZ.NotSet);

            TestEnumProvider.Value = TestEnum.X;
            AssertControllerResult(context.Simulate(), AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Y;
            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
        }

        [TestMethod]
        public void ControllerAllowX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.AllowX, AllowDenyXYZ.NotSet);

            TestEnumProvider.Value = TestEnum.X;
            AssertControllerResult(context.Simulate(), AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Y;
            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
        }

        [TestMethod]
        public void ActionDenyX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.NotSet, AllowDenyXYZ.DenyX);

            TestEnumProvider.Value = TestEnum.X;
            AssertActionResult(context.Simulate(), AllowDeny.Deny);

            TestEnumProvider.Value = TestEnum.Y;
            AssertActionResult(context.Simulate(), AllowDeny.Allow);
        }

        [TestMethod]
        public void ActionAllowX()
        {
            var context = Setup.ControllerAction(AllowDenyXYZ.NotSet, AllowDenyXYZ.AllowX);

            TestEnumProvider.Value = TestEnum.X;
            AssertActionResult(context.Simulate(), AllowDeny.Allow);

            TestEnumProvider.Value = TestEnum.Y;
            AssertActionResult(context.Simulate(), AllowDeny.Deny);
        }

        private void AssertResult(SimulateResult result, AllowDeny? expectedControllerResult, AllowDeny? expectedActionResult)
        {
            AssertControllerResult(result, expectedControllerResult);
            AssertActionResult(result, expectedActionResult);
        }

        private void AssertControllerResult(SimulateResult result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Controller);
            Assert.AreEqual(expectedResult, result.Controller.LastAnnounced);
        }

        private void AssertActionResult(SimulateResult result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Action);
            Assert.AreEqual(expectedResult, result.Action.LastAnnounced);
        }
    }
}
