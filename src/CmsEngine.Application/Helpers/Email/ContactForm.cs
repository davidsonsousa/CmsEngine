namespace CmsEngine.Application.Helpers.Email;

public class ContactForm
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string From { get; set; } = string.Empty;

    [DataType(DataType.EmailAddress)]
    public string? To { get; set; }

    [Required]
    [MaxLength(150)]
    public string Subject { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Message { get; set; } = string.Empty;

    public ContactForm()
    {

    }

    public ContactForm(string to, string subject, string message)
    {
        To = to;
        Subject = subject;
        Message = message;
    }

    public override string ToString()
    {
        var jsonResult = new JObject(
                                    new JProperty("From", From),
                                    new JProperty("To", To),
                                    new JProperty("Subject", Subject),
                                    new JProperty("Message", Message)
                                );
        return jsonResult.ToString();
    }
}
