using System;
using System.Collections.Generic;

using Dilemma.Business.Services;
using Dilemma.Business.ViewModels;
using Dilemma.Common;

using Disposable.Common.ServiceLocator;

namespace Dilemma.IntegrationTest.ServiceLevel.Domains
{
    public static class Administration
    {
        private static readonly Lazy<IAdministrationService> _adminService = Locator.Lazy<IAdministrationService>();

        public static SystemConfigurationViewModel GetSystemConfiguration()
        {
            return _adminService.Value.GetSystemConfiguration();
        }

        public static void SetSystemConfiguration(SystemConfigurationViewModel viewModel)
        {
            _adminService.Value.SetSystemConfiguration(viewModel);
        }

        public static TestingConfiguration GetTestingConfiguration()
        {
            return _adminService.Value.GetTestingConfiguration();
        }

        public static void SetTestingConfiguration(TestingConfiguration configuration)
        {
            _adminService.Value.SetTestingConfiguration(configuration);
        }

        public static void UpdateTestingConfiguration(Action<TestingConfiguration> updateAction)
        {
            var configuration = GetTestingConfiguration();
            updateAction.Invoke(configuration);
            SetTestingConfiguration(configuration);
        }

        public static void SetPointConfiguration(PointConfigurationViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public static PointConfigurationViewModel GetPointConfiguration(int id)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<PointConfigurationViewModel> GetPointConfigurations()
        {
            throw new NotImplementedException();
        }

        public static PointConfigurationViewModel GetPointConfiguration(PointType pointType)
        {
            throw new NotImplementedException();
        }
    }
}
