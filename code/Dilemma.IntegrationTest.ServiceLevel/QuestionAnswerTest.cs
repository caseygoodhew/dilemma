using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Disposable.Common.ServiceLocator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel
{
    [TestClass]
    public class QuestionAnswerTest : Support.IntegrationTest
    {
        private static readonly Lazy<ITimeWarpSource> TimeWarpSource = Locator.Lazy<ITimeWarpSource>();

        public QuestionAnswerTest() : base(true)
        {
        }

        [TestInitialize]
        public override void Initialize()
        {
            base.Initialize();

            Administration.SetSystemConfiguration(
                new SystemConfigurationViewModel
                    {
                        MaxAnswers = 3,
                        QuestionLifetime = QuestionLifetime.FiveMinutes,
                        SystemEnvironment = SystemEnvironment.Testing
                    });

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Inactive);
        }

        [TestMethod]
        public void CanCreateQuestion()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            
            var id = Questions.CreateNewQuestion("Question");

            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public void CanAnswerQuestion()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");

            var answerId = Answers.RequestAnswerSlot("Question", "Answer");

            Assert.IsNotNull(answerId);
            Assert.IsTrue(answerId > 0);

            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
        }

        [TestMethod]
        public void AnswersAreLimited()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            var answerIds = new List<int?>();

            for (var i = 1; i <= 4; i++)
            {
                SecurityManager.LoginNewAnonymous(string.Format("Answerer {0}", i));
                answerIds.Add(Answers.RequestAnswerSlot("Question", string.Format("Answer {0}", i)));
            }
            
            Assert.IsNotNull(answerIds[0]);
            Assert.IsNotNull(answerIds[1]);
            Assert.IsNotNull(answerIds[2]);
            Assert.IsNull(answerIds[3]);
        }

        [TestMethod]
        public void QuestionsActuallyExpire()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenMinutesFromNow);

            var answerId = Answers.RequestAnswerSlot("Question", "Answer");

            Assert.IsNull(answerId);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void AnswerSlotsActuallyExpire()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");

            Answers.RequestAnswerSlot("Question", "Answer");

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenYearsFromNow);

            // this method is expected to fail 
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
        }

        [TestMethod]
        public void QuestionAndAnswersDoNotEnterModerationUsingTestConfiguration()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");
            Answers.RequestAnswerSlot("Question", "Answer");
            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var question = Questions.GetQuestion("Question");
            
            Assert.IsTrue(question.QuestionViewModel.IsApproved);
            Assert.IsFalse(question.QuestionViewModel.IsRejected);
            
            Assert.IsTrue(question.QuestionViewModel.Answers.Single().IsApproved);
            Assert.IsFalse(question.QuestionViewModel.Answers.Single().IsRejected);
        }

        [TestMethod]
        public void GetMyActivityOnlyGetsMyActivity()
        {
            SecurityManager.LoginNewAnonymous("First User");
            var firstQuestionId = Questions.CreateNewQuestion("First Question");

            SecurityManager.LoginNewAnonymous("Second User");
            var secondQuestionId = Questions.CreateNewQuestion("Second Question");
            Answers.RequestAnswerSlot("First Question", "First Answer");
            Answers.CompleteAnswer("First Question", Answers.FillDefaults("First Answer"));

            SecurityManager.LoginNewAnonymous("Me");
            var myQuestionId = Questions.CreateNewQuestion("My Question");
            Answers.RequestAnswerSlot("Second Question", "My Answer");
            Answers.CompleteAnswer("Second Question", Answers.FillDefaults("My Answer"));

            var myActivity = Questions.GetMyActivity().ToList();

            Assert.IsFalse(myActivity.Any(x => x.QuestionId == firstQuestionId));
            Assert.IsTrue(myActivity.Any(x => x.QuestionId == secondQuestionId));
            Assert.IsTrue(myActivity.Any(x => x.QuestionId == myQuestionId));
        }

        [TestMethod]
        public void CannotAnswerMyOwnQuestion()
        {
            SecurityManager.LoginNewAnonymous("User");
            Questions.CreateNewQuestion("Question");

            var anserSlotId = Answers.RequestAnswerSlot("Question", "Answer");

            Assert.IsNull(anserSlotId);
        }

        [TestMethod]
        public void CannotCompleteAnotherUsersAnswer()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("First Answerer");
            var anserSlotId = Answers.RequestAnswerSlot("Question", "Answer");

            SecurityManager.LoginNewAnonymous("Second Answerer");
            var secondAnswererResult = Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            Assert.IsFalse(secondAnswererResult);

            SecurityManager.SetUserId("Questioner");
            var questionerAnswererResult = Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            Assert.IsFalse(questionerAnswererResult);
            
            SecurityManager.SetUserId("First Answerer");
            var firstAnswererResult = Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));
            Assert.IsTrue(firstAnswererResult);
        }

        [TestMethod]
        public void OnlyAdminsCanGetAllQuestions()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Somebody Else");

            var result = Questions.GetAllQuestions();
            Assert.IsFalse(result.Any());
        }
    }
}
