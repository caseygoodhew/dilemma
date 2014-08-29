using Disposable.Common.ServiceLocator;

namespace Dilemma.Security
{
    /// <summary>
    /// Registers data services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Registers data services with the <see cref="ILocator"/> <see cref="IRegistrar"/>.
        /// </summary>
        /// <param name="registrar">The <see cref="IRegistrar"/> to use.</param>
        public static void Register(IRegistrar registrar)
        {
            registrar.Register<ISecurityManager>(() => new SecurityManager());
        }
    }
}
