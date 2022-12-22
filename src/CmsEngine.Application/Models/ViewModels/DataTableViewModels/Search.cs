namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class Search
{
    [JsonProperty(PropertyName = "regex")]
    public bool Regex { get; set; }

    [JsonProperty(PropertyName = "value")]
    public string? Value { get; set; }
}
