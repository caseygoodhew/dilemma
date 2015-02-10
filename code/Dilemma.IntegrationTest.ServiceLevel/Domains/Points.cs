using System;

using Dilemma.Business.Services;
using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Points
    {
        private static readonly Lazy<IAdministrationService> _administrationService = Locator.Lazy<IAdministrationService>();

        public static int For(PointType pointType)
        {
            return _administrationService.Value.GetPointConfiguration(pointType).Points;
        }
    }
}