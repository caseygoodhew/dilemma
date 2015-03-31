using System;
using System.Collections.Generic;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Disposable.Common.Extensions;
using Disposable.Common.ServiceLocator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Primary
{
    [TestClass]
    public class QuestionAnswerTest : Support.IntegrationTest
    {
        private static readonly Lazy<ITimeWarpSource> TimeWarpSource = Locator.Lazy<ITimeWarpSource>();

        public QuestionAnswerTest() : base(false)
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
            SetupQuestionWithAnswer();

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
        public void QuestionAndAnswersDoNotEnterModerationUsingTestConfiguration()
        {
            SetupQuestionWithAnswer();

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
            Answers.RequestAnswerSlot("Question", "Answer");

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
        public void GetAllQuestionsOnlyInInternalEnvironment()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");
            SecurityManager.LoginNewAnonymous("Somebody Else");

            EnumExtensions.All<SystemEnvironment>().ToList().ForEach(
                systemEnvironment =>
                    {
                        Administration.UpdateSystemConfiguration(x => x.SystemEnvironment = systemEnvironment);
                        var systemConfiguration =
                            Administration.GetSystemServerConfiguration().SystemConfigurationViewModel;

                        Assert.AreEqual(systemEnvironment, systemConfiguration.SystemEnvironment);

                        var result = Questions.GetAllQuestions();
                        
                        // we should only get results for internal system environments
                        Assert.AreEqual(SystemEnvironmentValidation.IsInternalEnvironment(systemEnvironment), result.Any());
                    });
        }

        [TestMethod]
        public void UserCanOnlyTakeOneAnswerSlotPerQuestion()
        {
            SecurityManager.LoginNewAnonymous("Questioner");
            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");
            var firstAnswerId = Answers.RequestAnswerSlot("Question", "Answer");
            var secondAnswerId = Answers.RequestAnswerSlot("Question", "Answer");
            
            Assert.IsNotNull(firstAnswerId);
            Assert.AreEqual(firstAnswerId, secondAnswerId);

            Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer"));

            var thirdAnswerId = Answers.RequestAnswerSlot("Question", "Answer");
            Assert.AreEqual(firstAnswerId, thirdAnswerId);
        }

        [TestMethod]
        public void AnswerSlotsActuallyExpire()
        {
            SetupQuestionWithAnswer();

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenYearsFromNow);
            
            Administration.ExpireAnswerSlots();

            Assert.IsFalse(Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer")));
        }

        [TestMethod]
        public void ActiveAnswerSlotsDoNotExpire()
        {
            SetupQuestionWithAnswer();

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenYearsFromNow);
            
            Answers.TouchAnswer("Answer");

            Administration.ExpireAnswerSlots();

            Assert.IsTrue(Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer")));;
        }

        [TestMethod]
        public void TouchingAnotherUsersAnswerHasNoEffect()
        {
            SetupQuestionWithAnswer();

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenYearsFromNow);

            SecurityManager.LoginNewAnonymous("Somebody Else");
            Answers.TouchAnswer("Answer");
            SecurityManager.SetUserId("Answerer");
            
            Administration.ExpireAnswerSlots();

            Assert.IsFalse(Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer")));
        }

        [TestMethod]
        public void OnlyExpectedAnswersExpire()
        {
            Administration.UpdateSystemConfiguration(x => x.QuestionLifetime = QuestionLifetime.OneDay);
            
            SetupQuestionWithAnswer();

            TimeWarpSource.Value.DoThe(TimeWarpTo.TenMinutesFromNow);

            SecurityManager.LoginNewAnonymous("Somebody Else");
            var answerId = Answers.RequestAnswerSlot("Question", "Another Answer");
            Assert.IsNotNull(answerId);

            SecurityManager.SetUserId("Answerer");
            
            Administration.ExpireAnswerSlots();

            Assert.IsFalse(Answers.CompleteAnswer("Question", Answers.FillDefaults("Answer")));
            
            SecurityManager.SetUserId("Somebody Else");
            Assert.IsTrue(Answers.CompleteAnswer("Question", Answers.FillDefaults("Another Answer")));
        }

        [TestMethod]
        public void CloseQuestions()
        {
            SetupQuestionWithAnswer();
            SecurityManager.LoginNewAnonymous("Another User");
            
            Administration.CloseQuestions();
            var question = Questions.GetQuestion("Question").QuestionViewModel;
            Assert.IsTrue(question.IsOpen);
            Assert.IsFalse(question.IsClosed);

            TimeWarpSource.Value.FreezeTime(question.ClosesDateTime);

            Administration.CloseQuestions();
            question = Questions.GetQuestion("Question").QuestionViewModel;
            Assert.IsTrue(question.IsOpen);
            Assert.IsFalse(question.IsClosed);

            TimeWarpSource.Value.FreezeTime(question.ClosesDateTime.Value.AddSeconds(1));

            Administration.CloseQuestions();
            question = Questions.GetQuestion("Question").QuestionViewModel;
            Assert.IsFalse(question.IsOpen);
            Assert.IsTrue(question.IsClosed);
        }

        [TestMethod]
        [ExpectedException(typeof(TestNotWrittenException))]
        public void QuestionsExpire()
        {
            Administration.RetireOldQuestions();
        }

        private static void SetupQuestionWithAnswer()
        {
            SecurityManager.LoginNewAnonymous("Questioner");

            Questions.CreateNewQuestion("Question");

            SecurityManager.LoginNewAnonymous("Answerer");

            var answerId = Answers.RequestAnswerSlot("Question", "Answer");

            Assert.IsNotNull(answerId);
        }
    }
}
