using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Disposable.Common.Extensions;

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
            var random = new Random();
            var value1 = 0;
            var value2 = 1;

            // set the values as a fibonacci sequence to reduce the possibility of confusing duplicate point assignment as a single value (1, 2, 3, 4...)
            // randomize the order that the dictionary is generated in to reduce repeatability and error masking
            PointDictionary =
                new ReadOnlyDictionary<PointType, int>(
                    EnumExtensions.All<PointType>().OrderBy(order => random.Next()).ToDictionary(
                        x => x,
                        x =>
                            {
                                var result = value1 + value2;
                                value1 = value2;
                                return value2 = result;
                            }));
        }

        public ActiveState ManualModeration { get { return ActiveState.Active; } }
        
        public ActiveState GetAnyUser { get { return ActiveState.Inactive; } }

        public ActiveState UseTestingPoints { get { return ActiveState.Inactive; } }

        public IDictionary<PointType, int> PointDictionary { get; private set; }
    }
}