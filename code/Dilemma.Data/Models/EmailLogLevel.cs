namespace Dilemma.Data.Models
{
    public class EmailLogLevel
    {
        public int Id { get; set; }

        public string LogLevel { get; set; }

        public bool EnableEmails { get; set; }

        public string SendToEmailAddresses { get; set; }
    }
}