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
            SystemConfigurationInitialization.Seed(context);
            CategoryInitialization.Seed(context);
            PointConfigurationInitialization.Seed(context);
            LastRunLogInitialization.Seed(context);
        }
    }
}
