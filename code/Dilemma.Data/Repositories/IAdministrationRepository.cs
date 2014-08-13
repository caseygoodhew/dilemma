namespace Dilemma.Data.Repositories
{
    public interface IAdministrationRepository
    {
        void SetSystemConfiguration<T>(T systemConfigurationType) where T : class;

        T GetSystemConfiguration<T>() where T : class;
    }
}
