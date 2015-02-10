using System;
using System.Collections.Generic;
using System.Linq;

using Disposable.Common;

namespace Dilemma.Common
{
    public class TestingConfiguration : ITestingConfiguration
    {
        public ActiveState ManualModeration { get; set; }
        
        public ActiveState GetAnyUser { get; set; }
        
        public ActiveState UseTestingPoints { get; set; }
        
        public IDictionary<PointType, int> PointDictionary { get; private set; }

        public TestingConfiguration()
        {
            ManualModeration = TestingConfigurationDefaults.Instance.ManualModeration;
            GetAnyUser = TestingConfigurationDefaults.Instance.GetAnyUser;
            UseTestingPoints = TestingConfigurationDefaults.Instance.UseTestingPoints;

            PointDictionary = TestingConfigurationDefaults.Instance.PointDictionary.ToDictionary(
                x => x.Key,
                x => x.Value);
        }

        public static NaturalComparison<T> NaturalComparison<T>(TestingConfigurationContext testingConfigurationContext, Func<ITestingConfiguration, T> settingLocator) where T : IComparable
        {
            ITestingConfiguration instance = TestingConfigurationDefaults.Instance;

            if (testingConfigurationContext.SystemEnvironment == SystemEnvironment.Testing)
            {
                instance = testingConfigurationContext.TestingConfiguration;
            }

            return new NaturalComparison<T>(settingLocator.Invoke(instance));
        }
    }
}