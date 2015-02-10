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
    public class ManualModerationTest : Support.IntegrationTest
    {
        public ManualModerationTest() : base(false)
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
        }

        [TestMethod]
        public void QuestionEntersModeration()
        {
            Action<string, bool, bool> testAction = (reference, expectApproved, expectRejected) =>
                {
                    Questions.CreateNewQuestion(reference);
                    var question = Questions.GetQuestion(reference);
                    Assert.AreEqual(expectApproved, question.QuestionViewModel.IsApproved);
                    Assert.AreEqual(expectRejected, question.QuestionViewModel.IsRejected);
                };

            SecurityManager.LoginNewAnonymous("Questioner");

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            testAction("Question One", true, false);

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            testAction("Question Two", false, false);
        }

        [TestMethod]
        public void AnswerEntersModeration()
        {
            Action<string, bool, bool> testAction = (reference, expectApproved, expectRejected) =>
                {
                    var answerId = Answers.RequestAnswerSlot("Question", reference);
                    Answers.CompleteAnswer("Question", Answers.FillDefaults(reference));
                    var question = Questions.GetQuestion("Question");
                    var answer = question.QuestionViewModel.Answers.Single(x => x.AnswerId == answerId);

                    Assert.AreEqual(expectApproved, answer.IsApproved);
                    Assert.AreEqual(expectRejected, answer.IsRejected);
                };

            SecurityManager.LoginNewAnonymous("Questioner");

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("First Answerer");
            testAction("Answer One", true, false);

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            
            SecurityManager.LoginNewAnonymous("Second Answerer");
            testAction("Answer Two", false, false);
        }

        [TestMethod]
        public void OnlyQuestionerCanSeeTheirQuestionWhenAwaitingModeration()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var question = Questions.GetQuestion("Question");
            Assert.IsNotNull(question);

            // Viewer
            SecurityManager.LoginNewAnonymous("Viewer");
            question = Questions.GetQuestion("Question");
            Assert.IsNull(question);
        }

        [TestMethod]
        public void OnlyAnswererCanSeeTheirAnswerWhenAwaitingModeration()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            // Answerer
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            var question = Questions.GetQuestion("Question");
            Assert.AreEqual(1, question.QuestionViewModel.Answers.Count);

            // Viewer
            SecurityManager.LoginNewAnonymous("Viewer");
            question = Questions.GetQuestion("Question");
            Assert.AreEqual(0, question.QuestionViewModel.Answers.Count);
        }

        [TestMethod]
        public void QuestionInModerationCanBeApproved()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");
            
            var question = Questions.GetQuestion("Question");
            Assert.IsFalse(question.QuestionViewModel.IsApproved);
            Assert.IsFalse(question.QuestionViewModel.IsRejected);

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Approve(moderation.ModerationId);

            question = Questions.GetQuestion("Question");
            Assert.IsTrue(question.QuestionViewModel.IsApproved);
            Assert.IsFalse(question.QuestionViewModel.IsRejected);
        }

        [TestMethod]
        public void QuestionInModerationCanBeRejected()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var question = Questions.GetQuestion("Question");
            Assert.IsFalse(question.QuestionViewModel.IsApproved);
            Assert.IsFalse(question.QuestionViewModel.IsRejected);

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            question = Questions.GetQuestion("Question");
            Assert.IsFalse(question.QuestionViewModel.IsApproved);
            Assert.IsTrue(question.QuestionViewModel.IsRejected);
        }

        [TestMethod]
        public void AnswerInModerationCanBeApproved()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            // Answerer
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            
            var answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.Single();
            Assert.IsFalse(answer.IsApproved);
            Assert.IsFalse(answer.IsRejected);

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Approve(moderation.ModerationId);

            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.Single();
            Assert.IsTrue(answer.IsApproved);
            Assert.IsFalse(answer.IsRejected);
        }

        [TestMethod]
        public void AnswerInModerationCanBeRejected()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            // Answerer
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.Single();
            Assert.IsFalse(answer.IsApproved);
            Assert.IsFalse(answer.IsRejected);

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.Single();
            Assert.IsFalse(answer.IsApproved);
            Assert.IsTrue(answer.IsRejected);
        }

        [TestMethod]
        public void EveryoneCanSeeApprovedQuestion()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Approve(moderation.ModerationId);

            var question = Questions.GetQuestion("Question");
            Assert.IsNotNull(question);

            SecurityManager.LoginNewAnonymous("Viewer");
            question = Questions.GetQuestion("Question");
            Assert.IsNotNull(question);
        }
        
        [TestMethod]
        public void OnlyQuestionerCanSeeRejectedQuestion()
        {
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var moderation = ManualModeration.GetNextForUser("Questioner");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            var question = Questions.GetQuestion("Question");
            Assert.IsNotNull(question);

            SecurityManager.LoginNewAnonymous("Viewer");
            question = Questions.GetQuestion("Question");
            Assert.IsNull(question);
        }

        [TestMethod]
        public void EveryoneCanSeeApprovedAnswer()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            // Answerer
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Approve(moderation.ModerationId);

            var answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNotNull(answer);

            SecurityManager.SetUserId("Questioner");
            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNotNull(answer);

            SecurityManager.LoginNewAnonymous("Viewer");
            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNotNull(answer);
        }

        [TestMethod]
        public void OnlyAnswerCanSeeRejectedAnswer()
        {
            // Questioner
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            // Answerer
            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var moderation = ManualModeration.GetNextForUser("Answerer");
            ManualModeration.Reject(moderation.ModerationId, "Rejection Message");

            var answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNotNull(answer);

            SecurityManager.SetUserId("Questioner");
            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNull(answer);

            SecurityManager.LoginNewAnonymous("Viewer");
            answer = Questions.GetQuestion("Question").QuestionViewModel.Answers.SingleOrDefault();
            Assert.IsNull(answer);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void CannotAnswerQuestionWhenAwaitingModeration()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void CannotAnswerRejectedQuestion()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void QuestionEditReentersModeration()
        {
        }
        
        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void AnswerEditReentersModeration()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void CannotEditQuestionAfterAnswerHasBeenAdded()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void CannotEditAnswerAfterVotesHaveBeenCast()
        {
        }
    }
}