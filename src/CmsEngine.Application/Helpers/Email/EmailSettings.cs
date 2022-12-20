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
        var jsonResult = new JObject(
                        new JProperty("Domain", Domain),
                        new JProperty("Port", Port),
                        new JProperty("Username", Username),
                        new JProperty("Password", Password),
                        new JProperty("FromEmail", FromEmail),
                        new JProperty("CcEmail", CcEmail),
                        new JProperty("BccEmail", BccEmail)
                    );
        return jsonResult.ToString();
    }
}
