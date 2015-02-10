using System;

using Dilemma.IntegrationTest.ServiceLevel.Domains;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Primary
{
    [TestClass]
    public class UserTest : Support.IntegrationTest
    {
        public UserTest() : base(false)
        {
        }

        [TestMethod]
        public void CanCreateUser()
        {
            Users.AnonymousUser("Homer");
        }

        [TestMethod]
        public void CanLoginNewAnonymous()
        {
            SecurityManager.LoginNewAnonymous("Simpson");
        }

        [TestMethod]
        public void CanLoginExplicit()
        {
            // Arrange
            var firstId = 100;
            var secondId = 999;
            
            // Act, Assert
            SecurityManager.SetUserId(firstId);
            Assert.AreEqual(firstId, SecurityManager.GetUserId());

            SecurityManager.SetUserId(secondId);
            Assert.AreEqual(secondId, SecurityManager.GetUserId());
        }

        [TestMethod]
        public void ValidateClaimsCreatesUser()
        {
            try
            {
                SecurityManager.GetUserId();
                Assert.IsTrue(false);
            }
            catch (InvalidOperationException)
            {
                // expected catch
            }
            
            SecurityManager.ValidateClaims();
            var userId = SecurityManager.GetUserId();
            Assert.IsTrue(userId > 0);
        }

        [TestMethod]
        public void UserDictionaryCrossesDomains()
        {
            var reference = "Homer";
            var firstUserId = Users.AnonymousUser(reference);
            SecurityManager.SetUserId(reference);
            var secondUserId = SecurityManager.GetUserId();

            Assert.AreEqual(firstUserId, secondUserId);
        }
    }
}
