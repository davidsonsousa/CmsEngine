namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataParameters
{
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    [JsonPropertyName("columns")]
    public List<DataColumn>? Columns { get; set; }

    [JsonPropertyName("order")]
    public List<DataOrder>? Order { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("search")]
    public Search? Search { get; set; }
}
