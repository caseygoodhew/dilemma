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
        private static ServerConfiguration serverConfiguration;
        
        [TestInitialize]
        public void Initialize()
        {
            if (serverConfiguration == null)
            {
                serverConfiguration = new ServerConfiguration();
                
                var administrationRepositoryMock = new Mock<IAdministrationRepository>(MockBehavior.Strict);
                administrationRepositoryMock.Setup(x => x.GetServerConfiguration<ServerConfiguration>())
                    .Returns(() => serverConfiguration);
                
                var locator = Locator.Current as Locator;
                locator.Register(() => administrationRepositoryMock.Object);
            }
        }

        [TestMethod]
        public void ControllerAllowAdministrator_ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.AllowAdministrator);

            ForEachServerRole(
                serverRole =>
                    {
                        switch (serverRole)
                        {
                            case ServerRole.Offline:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            case ServerRole.Moderation:
                                AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                                break;
                            case ServerRole.Public:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            case ServerRole.QuestionSeeder:
                                AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException("serverRole");
                        }
                    });
        }

        [TestMethod]
        public void ControllerAllowAdministrator_ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.DenyAdministrator);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.Public:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator_ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.AllowAdministrator);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case ServerRole.Public:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator_ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.DenyAdministrator);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertResult(context.Simulate(), AllowDeny.Deny, AllowDeny.Deny);
                            break;
                        case ServerRole.Public:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertResult(context.Simulate(), AllowDeny.Allow, AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ControllerAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.AllowAdministrator, AllowDenyRole.NotSet);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case ServerRole.Public:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ControllerDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.DenyAdministrator, AllowDenyRole.NotSet);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertControllerResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Public:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertControllerResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ActionAllowAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.NotSet, AllowDenyRole.AllowAdministrator);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case ServerRole.Public:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        [TestMethod]
        public void ActionDenyAdministrator()
        {
            var context = SetupUserRole.ControllerAction(AllowDenyRole.NotSet, AllowDenyRole.DenyAdministrator);

            ForEachServerRole(
                serverRole =>
                {
                    switch (serverRole)
                    {
                        case ServerRole.Offline:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Moderation:
                            AssertActionResult(context.Simulate(), AllowDeny.Deny);
                            break;
                        case ServerRole.Public:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        case ServerRole.QuestionSeeder:
                            AssertActionResult(context.Simulate(), AllowDeny.Allow);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("serverRole");
                    }
                });
        }

        private void ForEachServerRole(Action<ServerRole> action)
        {
            EnumExtensions.All<ServerRole>().ToList().ForEach(
                serverRole =>
                    {
                        serverConfiguration.ServerRole = serverRole;
                        action(serverRole);
                    });
        }

        private void AssertResult(SimulateResult<FilterAccessByServerRole> result, AllowDeny? expectedControllerResult, AllowDeny? expectedActionResult)
        {
            AssertControllerResult(result, expectedControllerResult);
            AssertActionResult(result, expectedActionResult);
        }

        private void AssertControllerResult(SimulateResult<FilterAccessByServerRole> result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Controller);
            AssertFilterContextResult(result, expectedResult);
        }

        private void AssertActionResult(SimulateResult<FilterAccessByServerRole> result, AllowDeny? expectedResult)
        {
            Assert.IsNotNull(result.Action);
            AssertFilterContextResult(result, expectedResult);
        }

        private void AssertFilterContextResult(
            SimulateResult<FilterAccessByServerRole> result,
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
