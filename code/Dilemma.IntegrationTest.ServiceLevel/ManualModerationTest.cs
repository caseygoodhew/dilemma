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
    public class ManualModerationTest : Support.IntegrationTest
    {
        private static readonly Lazy<ITimeWarpSource> TimeWarpSource = Locator.Lazy<ITimeWarpSource>();

        public ManualModerationTest() : base(false)
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
    }
}