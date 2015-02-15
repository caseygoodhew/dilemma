using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Primary
{
    [TestClass]
    public class VotingTest : Support.IntegrationTest
    {
        public VotingTest()
            : base(false)
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

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            Administration.UpdateTestingConfiguration(x => x.GetAnyUser = ActiveState.Active);
            Administration.UpdateTestingConfiguration(x => x.UseTestingPoints = ActiveState.Active);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void QuestionerCanAddStarVote()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAndCompleteAnswer("Question", "Answer");

            SecurityManager.SetUserId("Questioner");
            Answers.RegisterVote("Answer");

            var question = Questions.GetQuestion("Question");
            int y = 0;
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void QuestionerCannotRemoveStarVote()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void QuestionerCannotAddTwoStarVotes()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void AnswererCanVoteOnTheirOwnAnswer()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void AnswererCanVoteOnAnotherAnswer()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void ViewerCanVoteOnAnswers()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void ViewerCanRecastTheirVote()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void VotingCloses()
        {

        }
    }
}