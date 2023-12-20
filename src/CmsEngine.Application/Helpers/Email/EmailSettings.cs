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
        var jsonResult = new JsonObject
        {
            [nameof(Domain)] = Domain,
            [nameof(Port)] = Port,
            [nameof(Username)] = Username,
            [nameof(Password)] = Password,
            [nameof(FromEmail)] = FromEmail,
            [nameof(CcEmail)] = CcEmail,
            [nameof(BccEmail)] = BccEmail
        };
        return jsonResult.ToString();
    }
}
