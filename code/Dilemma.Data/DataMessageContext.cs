using System;

using Dilemma.Common;
using Dilemma.Data.EntityFramework;
using Dilemma.Data.Models;
using Dilemma.Data.Repositories;

using Disposable.Common;
using Disposable.Common.ServiceLocator;
using Disposable.MessagePipe;

namespace Dilemma.Data
{
    internal abstract class DataMessageContext<TMessageTypeEnum> : MessageContext<TMessageTypeEnum> where TMessageTypeEnum : struct, IConvertible
    {
        private readonly static Lazy<IAdministrationRepository> AdministrationRepository = Locator.Lazy<IAdministrationRepository>();
        
        public readonly DilemmaContext DataContext;

        protected DataMessageContext(TMessageTypeEnum messageType, DilemmaContext dataContext)
            : base(messageType)
        {
            DataContext = dataContext;
        }

        public SystemConfiguration SystemConfiguration
        {
            get
            {
                return AdministrationRepository.Value.GetSystemConfiguration<SystemConfiguration>();
            }
        }

        public TestingConfiguration TestingConfiguration
        {
            get
            {
                return AdministrationRepository.Value.GetTestingConfiguration();   
            }
        }

        public NaturalComparison<T> TestingConfigurationProperty<T>(Func<ITestingConfiguration, T> settingLocator) where T : IComparable
        {
            return TestingConfiguration.NaturalComparison(new TestingConfigurationContext(SystemConfiguration.SystemEnvironment, TestingConfiguration), settingLocator);
        }
    }
}