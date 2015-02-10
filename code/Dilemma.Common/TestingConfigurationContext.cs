namespace Dilemma.Common
{
    public struct TestingConfigurationContext
    {
        public readonly SystemEnvironment SystemEnvironment;

        public readonly ITestingConfiguration TestingConfiguration;

        public TestingConfigurationContext(SystemEnvironment systemEnvironment, ITestingConfiguration testingConfiguration)
            : this()
        {
            SystemEnvironment = systemEnvironment;
            TestingConfiguration = testingConfiguration;
        }
    }
}