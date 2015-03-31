using System;

using Dilemma.Data.Annotations;
using Dilemma.IntegrationTest.ServiceLevel.Domains;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.LiveSetup
{
    [TestClass]
    public class QuestionsAndAnswers : Support.IntegrationTest
    {
        public QuestionsAndAnswers() : base(false) { }

        private readonly static int UserId = 10000;

        private static readonly bool AllowRun = false;
        
        [TestInitialize]
        public void Initialize()
        {
            if (!AllowRun)
            {
                throw new AccessViolationException();
            }

            base.Initialize();

            SecurityManager.SetUserId(UserId);
        }
        
        [TestMethod]
        public void CreateQuestion()
        {
            Questions.CreateAndApproveQuestion()
        }
    }
}
