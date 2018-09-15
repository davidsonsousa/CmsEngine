namespace CmsEngine.Data.ViewModels
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
    }
}
