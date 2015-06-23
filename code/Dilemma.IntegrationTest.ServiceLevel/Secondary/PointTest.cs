using System;

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
        public PointTest() : base(false)
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
                        QuestionLifetimeDays = QuestionLifetime.FiveMinutes,
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
        public void PointsAwardedOnQuestionAskedAfterModeration()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            AssertUserPoints("Questioner", 0);
            
            Questions.CreateNewQuestion("Question");

            AssertUserPoints("Questioner", 0);

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Approve(moderation.ModerationId);
            
            AssertUserPoints("Questioner", Points.For(PointType.QuestionAsked));
        }

        [TestMethod]
        public void PointsAwardedOnQuestionAnsweredAfterModeration()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateAndApproveQuestion("Question");
            AssertUserPoints("Questioner", Points.For(PointType.QuestionAsked));

            // verify one answer is added correctly
            SecurityManager.LoginNewAnonymous("Answerer One");
            AssertUserPoints("Answerer One", 0);

            Answers.RequestAnswerSlot("Question", "Answer One");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer One"));
            AssertUserPoints("Answerer One", 0);

            ManualModeration.Approve(ManualModeration.GetNextForUser("Answerer One").ModerationId);
            AssertUserPoints("Answerer One", Points.For(PointType.QuestionAnswered));

            // verify points are awarded to correct users
            Action<string, string> setupAnswer = (userReference, answerReference) =>
                {
                    SecurityManager.LoginNewAnonymous(userReference);
                    Answers.RequestAnswerSlot("Question", answerReference);
                    Answers.CompleteAnswer("Question", Answers.FillDefaults(answerReference));
                    AssertUserPoints(userReference, 0);
                };

            setupAnswer("Answerer Two", "Answer Two");
            setupAnswer("Answerer Three", "Answer Three");
            
            ManualModeration.Approve(ManualModeration.GetNextForUser("Answerer Two").ModerationId);
            
            AssertUserPoints("Questioner", Points.For(PointType.QuestionAsked));
            AssertUserPoints("Answerer One", Points.For(PointType.QuestionAnswered));
            AssertUserPoints("Answerer Two", Points.For(PointType.QuestionAnswered));
            AssertUserPoints("Answerer Three", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnVoting()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void PointsAwardedOnGettingStarVote()
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

        private static void AssertUserPoints(string userReference, int points)
        {
            var user = Users.GetUser(userReference);
            Assert.AreEqual(points, user.Points);
        }
    }
}