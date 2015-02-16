using System;
using System.Linq;

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
                                                               MaxAnswers = 5,
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
        public void QuestionerCanAddStarVote()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.SetUserId("Questioner");
            Answers.RegisterVote(answerTwoId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;
            
            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void QuestionerCannotRemoveStarVote()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWER ***/
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAndCompleteAnswer("Question", "Answer");

            /*** REGISTER VOTE ***/
            SecurityManager.SetUserId("Questioner");
            Answers.RegisterVote("Answer");

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(1, question.Answers.Single().VoteCount);
            Assert.IsTrue(question.Answers.Single().HasMyVote);

            // this should have no effect
            Answers.DeregisterVote("Answer");

            Assert.AreEqual(1, question.Answers.Single().VoteCount);
            Assert.IsTrue(question.Answers.Single().HasMyVote);
        }

        [TestMethod]
        public void QuestionerCannotAddTwoStarVotes()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.SetUserId("Questioner");
            Answers.RegisterVote(answerTwoId);
            Answers.RegisterVote(answerThreeId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void AnswererCanVoteOnTheirOwnAnswer()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.SetUserId("Answerer Two");
            Answers.RegisterVote(answerTwoId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void AnswererCanVoteOnAnotherAnswer()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.SetUserId("Answerer Two");
            Answers.RegisterVote(answerOneId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);
            
            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void ViewerCanVoteOnAnswers()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.LoginNewAnonymous("Viewer");
            Answers.RegisterVote(answerOneId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void ViewerCanRecastTheirVote()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.LoginNewAnonymous("Viewer");
            Answers.RegisterVote(answerOneId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);

            /*** RECAST VOTE ***/
            Answers.DeregisterVote(answerOneId);
            Answers.RegisterVote(answerTwoId);

            /*** ASSERT ***/
            question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        public void ViewerCannotRegisterMultipleVotes()
        {
            /*** CREATE QUESTION ***/
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            /*** ADD ANSWERS ***/
            SecurityManager.LoginNewAnonymous("Answerer One");
            var answerOneId = Answers.RequestAndCompleteAnswer("Question", "Answer One").Value;

            SecurityManager.LoginNewAnonymous("Answerer Two");
            var answerTwoId = Answers.RequestAndCompleteAnswer("Question", "Answer Two").Value;

            SecurityManager.LoginNewAnonymous("Answerer Three");
            var answerThreeId = Answers.RequestAndCompleteAnswer("Question", "Answer Three").Value;

            /*** REGISTER VOTE ***/
            SecurityManager.LoginNewAnonymous("Viewer");
            Answers.RegisterVote(answerOneId);
            Answers.RegisterVote(answerTwoId);

            /*** ASSERT ***/
            var question = Questions.GetQuestion("Question").QuestionViewModel;

            Assert.AreEqual(1, question.Answers.Single(x => x.AnswerId == answerOneId).VoteCount);
            Assert.IsTrue(question.Answers.Single(x => x.AnswerId == answerOneId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerTwoId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerTwoId).HasMyVote);

            Assert.AreEqual(0, question.Answers.Single(x => x.AnswerId == answerThreeId).VoteCount);
            Assert.IsFalse(question.Answers.Single(x => x.AnswerId == answerThreeId).HasMyVote);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void VotingCloses()
        {

        }
    }
}