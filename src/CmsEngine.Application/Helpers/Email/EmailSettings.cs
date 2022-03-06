namespace CmsEngine.Application.Helpers.Email;

public class EmailSettings
{
    public string Domain { get; set; }

    public int Port { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string FromEmail { get; set; }

    public string CcEmail { get; set; }

    public string BccEmail { get; set; }

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
