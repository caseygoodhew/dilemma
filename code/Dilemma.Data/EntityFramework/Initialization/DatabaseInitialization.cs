using System;
using System.Diagnostics;
using EfEnumToLookup.LookupGenerator;

namespace Dilemma.Data.EntityFramework.Initialization
{
    /// <summary>
    /// Database initialization
    /// </summary>
    public static class DatabaseInitialization
    {
        /// <summary>
        /// Calls component specific initializers.
        /// </summary>
        /// <param name="context">The <see cref="DilemmaContext"/> to initialize.</param>
        public static void Initialize(DilemmaContext context)
        {
            //*
            var enumToLookup = new EnumToLookup();
            var migrationSql = enumToLookup.GenerateMigrationSql(context);
            Debug.WriteLine("********************************************************");
            Debug.WriteLine("EnumToLookup");
            Debug.WriteLine("********************************************************");
            Debug.Write(migrationSql);
            Debug.WriteLine("********************************************************");
            //*/
            SystemConfigurationInitialization.Seed(context);
            CategoryInitialization.Seed(context);
            PointConfigurationInitialization.Seed(context);
            LastRunLogInitialization.Seed(context);
            RankInitialization.Seed(context);
        }
    }
}
