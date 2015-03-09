using System;

using Disposable.Common.Services;

namespace Dilemma.IntegrationTest.ServiceLevel.Support
{
    internal interface ITimeWarpSource : ITimeSource
    {
        void DoThe(TimeWarpTo timeWarpTo);

        void FreezeTime(DateTime? dateTime = null);
        
        void UnFreezeTime();
    }
}
