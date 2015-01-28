namespace Dilemma.Business.ViewModels
{
    public class SystemServerConfigurationViewModel
    {
        public SystemConfigurationViewModel SystemConfigurationViewModel { get; set; }

        public ServerConfigurationViewModel ServerConfigurationViewModel { get; set; }

        public string AccessKey { get; set; }
    }
}