namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataOrder
{
    [JsonPropertyName("column")]
    public int Column { get; set; }

    [JsonPropertyName("dir")]
    public string Dir { get; set; } = string.Empty;
}
