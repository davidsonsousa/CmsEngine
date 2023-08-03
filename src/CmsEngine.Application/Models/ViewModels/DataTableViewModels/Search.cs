namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class Search
{
    [JsonPropertyName("regex")]
    public bool Regex { get; set; }

    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
