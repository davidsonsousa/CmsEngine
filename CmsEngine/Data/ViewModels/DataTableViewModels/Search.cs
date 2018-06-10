using Newtonsoft.Json;

namespace CmsEngine.Data.ViewModels.DataTableViewModels
{
    public class Search
    {
        [JsonProperty(PropertyName = "regex")]
        public bool Regex { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
