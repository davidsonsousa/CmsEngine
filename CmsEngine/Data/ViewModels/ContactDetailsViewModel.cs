namespace CmsEngine.Data.ViewModels
{
    public class ContactDetailsViewModel
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        // Social media
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }

        public bool HasSocialMediaOrEmail
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Email)
                    || !string.IsNullOrWhiteSpace(Facebook)
                    || !string.IsNullOrWhiteSpace(Twitter)
                    || !string.IsNullOrWhiteSpace(Instagram)
                    || !string.IsNullOrWhiteSpace(LinkedIn);
            }
        }
    }
}
