namespace CmsEngine.Data.Entities;

public class Email : BaseEntity
{
    public string From { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public DateTime? DateReceived { get; set; }

    public override string ToString()
    {
        return $"Email(Id={Id},Subject={Subject})";
    }
}
