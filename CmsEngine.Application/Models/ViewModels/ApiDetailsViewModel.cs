namespace CmsEngine.Application.ViewModels
{
    public class ApiDetailsViewModel
    {
        public string FacebookAppId { get; set; }
        public string FacebookApiVersion { get; set; }

        public bool HasFacebookDetails
        {
            get
            {
                return !string.IsNullOrWhiteSpace(FacebookAppId) && !string.IsNullOrWhiteSpace(FacebookApiVersion);
            }
        }

        public string DisqusShortName { get; set; }

        public bool HasDisqusDetails
        {
            get
            {
                return !string.IsNullOrWhiteSpace(DisqusShortName);
            }
        }

    }
}
