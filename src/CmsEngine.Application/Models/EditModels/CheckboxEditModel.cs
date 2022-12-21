namespace CmsEngine.Application.Models.EditModels;

public class CheckboxEditModel
{
    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public bool Selected { get; set; }

    public bool Enabled { get; set; }
}
