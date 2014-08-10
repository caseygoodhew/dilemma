using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dilemma.Data.EntityFramework
{
    public class DilemmaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DilemmaContext>
    {
        protected override void Seed(DilemmaContext context)
        {
            // nothing for now
        }
    }
}
