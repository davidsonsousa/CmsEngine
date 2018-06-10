using System.Collections.Generic;
using Newtonsoft.Json;

namespace CmsEngine.Data.ViewModels.DataTableViewModels
{
    public class DataParameters
    {
        [JsonProperty(PropertyName = "draw")]
        public int Draw { get; set; }

        [JsonProperty(PropertyName = "columns")]
        public List<DataColumn> Columns { get; set; }

        [JsonProperty(PropertyName = "order")]
        public List<DataOrder> Order { get; set; }

        [JsonProperty(PropertyName = "start")]
        public int Start { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "search")]
        public Search Search { get; set; }
    }
}
