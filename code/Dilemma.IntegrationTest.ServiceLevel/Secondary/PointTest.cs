using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Secondary
{
    [TestClass]
    public class PointTest : Support.IntegrationTest
    {
        public PointTest() : base(true)
        {
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Administration.SetSystemServerConfiguration(
                new SystemServerConfigurationViewModel
                {
                    SystemConfigurationViewModel = new SystemConfigurationViewModel
                    {
                        MaxAnswers = 3,
                        QuestionLifetime = QuestionLifetime.FiveMinutes,
                        SystemEnvironment = SystemEnvironment.Testing
                    },
                    ServerConfigurationViewModel = new ServerConfigurationViewModel
                    {
                        ServerRole = ServerRole.Public
                    }
                });

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            Administration.UpdateTestingConfiguration(x => x.GetAnyUser = ActiveState.Active);
            Administration.UpdateTestingConfiguration(x => x.UseTestingPoints = ActiveState.Active);
        }

        [TestMethod]
        public void PointsAwardedOnQuestionAskedWithNoModeration()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            var user = Users.GetUser("Questioner");
            Assert.AreEqual(0, user.Points);
            
            Questions.CreateAndApproveQuestion("Question");

            user = Users.GetUser("Questioner");
            Assert.AreEqual(Points.For(PointType.QuestionAsked), user.Points);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnQuestionAnsweredWithNoModeration()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnGettingStarVote()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnVoting()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnPopularVote()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnComingBackToTheSite()
        {
            
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnRegularContributor()
        {
            
        }
    }
}