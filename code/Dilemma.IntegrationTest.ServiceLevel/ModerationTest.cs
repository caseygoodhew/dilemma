using System;
using System.Collections.Generic;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Disposable.Common.ServiceLocator;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel
{
    [TestClass]
    public class ModerationTest : Support.IntegrationTest
    {
        private static readonly Lazy<ITimeWarpSource> TimeWarpSource = Locator.Lazy<ITimeWarpSource>();

        public ModerationTest() : base(false)
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
            testAction.Invoke("Question One", true, false);

            Administration.UpdateTestingConfiguration(x => x.ManualModeration = ActiveState.Active);
            testAction.Invoke("Question Two", false, false);
        }
    }
}