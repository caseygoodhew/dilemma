using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;

using Dilemma.Common;
using Dilemma.Data.Models;

namespace Dilemma.Data.Repositories
{
    /// <summary>
    /// Administration repository services interface.
    /// </summary>
    public interface IAdministrationRepository
    {
        /// <summary>
        /// Sets the <see cref="SystemConfiguration"/> from the specified type. There must be a converter registered between <see cref="T"/> and <see cref="SystemConfiguration"/>.
        /// </summary>
        /// <typeparam name="T">The type to receive.</typeparam>
        /// <param name="systemConfigurationType">The convertable instance.</param>
        void SetSystemConfiguration<T>(T systemConfigurationType) where T : class;

        /// <summary>
        /// Gets the <see cref="SystemConfiguration"/> in the specified type. There must be a converter registered between <see cref="SystemConfiguration"/> and <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>The <see cref="SystemConfiguration"/> converted to type T.</returns>
        T GetSystemConfiguration<T>() where T : class;

        void SetServerConfiguration<T>(T serverConfigurationType) where T : class;

        T GetServerConfiguration<T>() where T : class;

        void SetTestingConfiguration(TestingConfiguration configuration);

        TestingConfiguration GetTestingConfiguration();
        
        void SetPointConfiguration<T>(T pointConfiguration) where T : class;

        T GetPointConfiguration<T>(PointType pointType) where T : class;

        IEnumerable<T> GetPointConfigurations<T>() where T : class;

        void RetireOldQuestions();

        void CloseQuestions();
    }
}
