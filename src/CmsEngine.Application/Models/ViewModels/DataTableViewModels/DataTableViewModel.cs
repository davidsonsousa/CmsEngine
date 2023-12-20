namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataTableViewModel
{
    [JsonPropertyName("draw")]
    public int Draw { get; set; }

    [JsonPropertyName("recordsTotal")]
    public int RecordsTotal { get; set; }

    [JsonPropertyName("recordsFiltered")]
    public int RecordsFiltered { get; set; }

    [JsonPropertyName("data")]
    public List<List<string>>? Data { get; set; }
}
