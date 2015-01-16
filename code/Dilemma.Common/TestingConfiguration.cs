namespace Dilemma.Common
{
    public class TestingConfiguration : ITestingConfiguration
    {
        public ActiveState ManualModeration { get; set; }

        public TestingConfiguration()
        {
            ManualModeration = TestingConfigurationDefaults.Instance.ManualModeration;
        }
    }
}