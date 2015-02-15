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
            var values = new[] { 0, 1, 2 };
            
            // Set the values as a fibonacci sequence to reduce the possibility of confusing duplicate point assignment as a single value (1, 2, 3, 4...).
            // Use every other value in sequence to futher reduce.
            // Randomize the order that the dictionary is generated in to reduce repeatability and error masking.
            PointDictionary =
                new ReadOnlyDictionary<PointType, int>(
                    EnumExtensions.All<PointType>().OrderBy(order => random.Next()).ToDictionary(
                        x => x,
                        x =>
                            {
                                var result = values[1] + values[2];
                                
                                values[0] = values[2];
                                values[1] = result;
                                values[2] = values[0] + values[1];
                                
                                return result;
                            }));
        }

        public ActiveState ManualModeration { get { return ActiveState.Active; } }
        
        public ActiveState GetAnyUser { get { return ActiveState.Inactive; } }

        public ActiveState UseTestingPoints { get { return ActiveState.Inactive; } }

        public IDictionary<PointType, int> PointDictionary { get; private set; }
    }
}