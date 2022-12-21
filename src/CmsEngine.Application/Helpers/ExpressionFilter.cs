namespace CmsEngine.Application.Helpers;

public sealed class ExpressionFilter
{
    public string PropertyName { get; set; } = string.Empty;

    public Operation Operation { get; set; }

    public object? Value { get; set; }
}
