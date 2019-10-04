namespace CmsEngine.Application.Helpers.Email
{
    public class EmailSettings
    {
        public string Domain { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string FromEmail { get; set; }

        public string CcEmail { get; set; }

        public string BccEmail { get; set; }
    }
}
