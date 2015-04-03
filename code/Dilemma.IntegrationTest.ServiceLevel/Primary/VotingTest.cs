using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Disposable.Common.Extensions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Primary
{
    [TestClass]
    public class VotingTest : Support.IntegrationTest
    {
        public VotingTest()
            : base(true)
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
        public void LotsOfVotes()
        {
            var allAnswers = new List<int?>();

            var firstUser = SecurityManager.LoginNewAnonymous("A");
            SecurityManager.LoginNewAnonymous("B");
            SecurityManager.LoginNewAnonymous("C");
            SecurityManager.LoginNewAnonymous("D");
            SecurityManager.LoginNewAnonymous("E");
            SecurityManager.LoginNewAnonymous("F");

            var baseUserId = firstUser;
            
            SecurityManager.SetUserId(baseUserId + 5);
            Questions.CreateNewQuestion("Question One");
            Questions.CreateNewQuestion("Question Two");
            Questions.CreateNewQuestion("Question Three");

            SecurityManager.SetUserId(baseUserId + 4);
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question One", "Question One Best"));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Two", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Three", "Question Three Best"));

            SecurityManager.SetUserId(baseUserId + 3);
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question One", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Two", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Three", Guid.NewGuid().ToString()));

            SecurityManager.SetUserId(baseUserId + 2);
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question One", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Two", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Three", Guid.NewGuid().ToString()));

            SecurityManager.SetUserId(baseUserId + 1);
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question One", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Two", "Question Two Best"));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Three", Guid.NewGuid().ToString()));

            SecurityManager.SetUserId(baseUserId);
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question One", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Two", Guid.NewGuid().ToString()));
            allAnswers.Add(Answers.RequestAndCompleteAnswer("Question Three", Guid.NewGuid().ToString()));

            SecurityManager.SetUserId(baseUserId + 5);
            Answers.RegisterVote("Question One Best");
            Answers.RegisterVote("Question Two Best");
            Answers.RegisterVote("Question Three Best");

            var rand = new Random();
            var validAnswers = allAnswers.Where(x => x.HasValue).Select(x => x.Value).ToList();

            Assert.AreEqual(allAnswers.Count, validAnswers.Count);

            for (var i = 0; i < 1000; i++)
            {
                SecurityManager.LoginNewAnonymous(Guid.NewGuid().ToString());
                Answers.RegisterVote(validAnswers.RandomUsing(rand));
            }
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