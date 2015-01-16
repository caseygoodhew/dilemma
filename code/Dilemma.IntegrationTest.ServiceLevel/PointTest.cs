using System;
using System.Linq;

using Dilemma.Business.ViewModels;
using Dilemma.Common;
using Dilemma.IntegrationTest.ServiceLevel.Domains;
using Dilemma.IntegrationTest.ServiceLevel.Support;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel
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
        public void PointsAwardedOnQuestionAsked()
        {
        }
    }
}