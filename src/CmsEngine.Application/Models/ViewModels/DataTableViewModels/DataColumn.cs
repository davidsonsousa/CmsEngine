namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataColumn
{
    [JsonPropertyName("data")]
    public int Data { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("searchable")]
    public bool Searchable { get; set; }

    [JsonPropertyName("orderable")]
    public bool Orderable { get; set; }

    [JsonPropertyName("search")]
    public Search? Search { get; set; }
}
