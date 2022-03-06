namespace CmsEngine.Application.Models.ViewModels.DataTablesViewModels;

public class DataTableViewModel
{
    [JsonProperty(PropertyName = "draw")]
    public int Draw { get; set; }

    [JsonProperty(PropertyName = "recordsTotal")]
    public int RecordsTotal { get; set; }

    [JsonProperty(PropertyName = "recordsFiltered")]
    public int RecordsFiltered { get; set; }

    [JsonProperty(PropertyName = "data")]
    public List<List<string>> Data { get; set; }
}
