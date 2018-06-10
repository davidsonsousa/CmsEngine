using Newtonsoft.Json;

namespace CmsEngine.Data.ViewModels.DataTableViewModels
{
    public class DataOrder
    {
        [JsonProperty(PropertyName = "column")]
        public int Column { get; set; }

        [JsonProperty(PropertyName = "dir")]
        public string Dir { get; set; }
    }
}
