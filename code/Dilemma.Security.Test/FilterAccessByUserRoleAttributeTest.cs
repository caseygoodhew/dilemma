using System;
using System.Linq;
using System.Web.Mvc;

using Dilemma.Common;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;
using Dilemma.Security.AccessFilters.ByEnum;
using Dilemma.Security.AccessFilters.ByUserRole;
using Dilemma.Security.Test.FilterAccessSupport;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace Dilemma.Security.Test
{
    [TestClass]
    public class FilterAccessByUserRoleAttributeTest
    {
        private static SystemConfiguration systemConfiguration;
        
        [TestInitialize]
        public void Initialize()
        {
            if (systemConfiguration == null)
            {
                systemConfiguration = new SystemConfiguration();
                
                var administrationRepositoryMock = new Mock<IAdministrationRepository>(MockBehavior.Strict);
                administrationRepositoryMock.Setup(x => x.GetSystemConfiguration<SystemConfiguration>())
                    .Returns(() => systemConfiguration);
                
                var locator = Locator.Current as Locator;
                locator.Register(() => administrationRepositoryMock.Object);
            }
        }

        [TestMethod]
        public void ControllerAllowAdministrator_ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.AllowAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                    {
                        switch (systemEnvironment)
                        {
                            case SystemEnvironment.Development:
                                AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                                break;
                            case SystemEnvironment.Testing:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            case SystemEnvironment.Production:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            case SystemEnvironment.Administration:
                                AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                                break;
                            case SystemEnvironment.QuestionSeeder:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("systemEnvironment");
                        }
                    });
        }

        [TestMethod]
        public void ControllerAllowAdministrator_ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.DenyAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Production:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Administration:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator_ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.AllowAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Production:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Administration:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator_ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.DenyAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Production:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Administration:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ControllerAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.NotSet);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Production:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Administration:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.NotSet);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Production:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Administration:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.NotSet, AllowDenyRole.AllowAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Production:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.Administration:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        [TestMethod]
        public void ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.NotSet, AllowDenyRole.DenyAdministrator);

            ForEachSystemEnvironment(
                systemEnvironment =>
                {
                    switch (systemEnvironment)
                    {
                        case SystemEnvironment.Development:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Testing:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Production:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case SystemEnvironment.Administration:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case SystemEnvironment.QuestionSeeder:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("systemEnvironment");
                    }
                });
        }

        private void ForEachSystemEnvironment(Action<SystemEnvironment> action)
        {
            EnumExtensions.All<SystemEnvironment>().ToList().ForEach(
                systemEnvironment =>
                    {
                        systemConfiguration.SystemEnvironment = systemEnvironment;
                        action(systemEnvironment);
                    });
        }
        
        private void AssertResult(SimulateResult<FilterAccessBySystemEnvironment> result, AllowDeny? expectedControllerResult, AllowDeny? expectedActionResult)
        {
            AssertControllerResult(result, expectedControllerResult);
            AssertActionResult(result, expectedActionResult);
        }

        private void AssertControllerResult(SimulateResult<FilterAccessBySystemEnvironment> result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Controller);
            AssertFilterContextResult(result, expectedResult);
        }

        private void AssertActionResult(SimulateResult<FilterAccessBySystemEnvironment> result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Action);
            AssertFilterContextResult(result, expectedResult);
        }

        private void AssertFilterContextResult(
            SimulateResult<FilterAccessBySystemEnvironment> result,
            AllowDeny? expectedResult)
        {
            switch (expectedResult)
            {
                case AllowDeny.Allow:
                    Assert.IsNull(result.FilterContext.Result);
                    break;
                case AllowDeny.Deny:
                    Assert.IsInstanceOfType(result.FilterContext.Result, typeof(RedirectToRouteResult));
                    break;
                case null:
                    Assert.IsNull(result.FilterContext.Result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("expectedResult");
            }
        }
    }
}
