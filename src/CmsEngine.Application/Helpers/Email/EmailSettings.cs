namespace CmsEngine.Application.Helpers.Email;

public class EmailSettings
{
    public string Domain { get; set; } = string.Empty;

    public int Port { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string CcEmail { get; set; } = string.Empty;

    public string BccEmail { get; set; } = string.Empty;

    public override string ToString()
    {
        // Lightweight representation for diagnostics; avoid including secret values (Password)
        return $"EmailSettings(Domain={Domain},Port={Port},Username={Username},From={FromEmail})";
    }
}
