namespace MyGarden_API.Models
{
    public class SimpleSmtpOptions
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool EnableSsl { get; set; }

        public string DefaultFromEmail { get; set; }

        public string DefaultFromName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
