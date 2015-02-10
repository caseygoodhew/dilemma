using System.Collections.Generic;

namespace Dilemma.Common
{
    public interface ITestingConfiguration
    {
        ActiveState ManualModeration { get; }

        ActiveState GetAnyUser { get; }

        ActiveState UseTestingPoints { get; }

        IDictionary<PointType, int> PointDictionary { get; }
    }
}