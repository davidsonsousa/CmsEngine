namespace CmsEngine.Application.Attributes;

/// <summary>
/// Enables the properto to be visible in the DataTable
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ShowOnDataTable : Attribute
{
    public int Order { get; }

    public ShowOnDataTable(int order)
    {
        Order = order;
    }
}
