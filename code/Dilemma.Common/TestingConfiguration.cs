namespace Dilemma.Common
{
    public interface ITestingConfiguration
    {
        ActiveState ManualModeration { get; }
    }

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

    public class TestingConfiguration : ITestingConfiguration
    {
        public ActiveState ManualModeration { get; set; }

        public TestingConfiguration()
        {
            ManualModeration = TestingConfigurationDefaults.Instance.ManualModeration;
        }
    }
}