namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataColumn
{
    [JsonProperty(PropertyName = "data")]
    public int Data { get; set; }

    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "searchable")]
    public bool Searchable { get; set; }

    [JsonProperty(PropertyName = "orderable")]
    public bool Orderable { get; set; }

    [JsonProperty(PropertyName = "search")]
    public Search? Search { get; set; }
}
