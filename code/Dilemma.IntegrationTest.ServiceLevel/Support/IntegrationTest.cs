using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    public abstract class IntegrationTest
    {
        protected readonly static TestDataManager TestDataManager = new TestDataManager();

        protected TransactionScope Transaction;

        protected readonly bool DoCommit;

        protected IntegrationTest(bool doCommit)
        {
            DoCommit = doCommit;
        }

        [TestInitialize]
        public virtual void Initialize()
        {
            Transaction = new TransactionScope();
            TestDataManager.Prepare();
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            if (DoCommit)
            {
                Transaction.Complete();
            }

            Transaction.Dispose();
            Transaction = null;
        }
    }
}