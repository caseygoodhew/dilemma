namespace Dilemma.Common
{
    public class TestingConfigurationDefaults : ITestingConfiguration
    {
        private static TestingConfigurationDefaults instance;

        public static TestingConfigurationDefaults Instance
        {
            get
            {
                return instance ?? (instance = new TestingConfigurationDefaults());
            }
        }

        private TestingConfigurationDefaults()
        {
        }

        public ActiveState ManualModeration { get { return ActiveState.Active; } }
    }
}