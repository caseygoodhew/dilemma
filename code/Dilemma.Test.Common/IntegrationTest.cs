using System.IO;
using System.Transactions;
using System.Web;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dilemma.IntegrationTest.ServiceLevel
{
    public abstract class IntegrationTest
    {
        protected readonly static Startup Startup = new Startup();

        protected TransactionScope Transaction;

        [TestInitialize]
        public virtual void Initialize()
        {
            Transaction = new TransactionScope();
            Startup.Initialize();

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://dilemma.integration.test", ""),
                new HttpResponse(new StringWriter())
            );
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            Transaction.Dispose();
            Transaction = null;
        }
    }
}