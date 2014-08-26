﻿using Dilemma.Data.Models;

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
    }
}
