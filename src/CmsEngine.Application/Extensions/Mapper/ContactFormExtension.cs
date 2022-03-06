namespace CmsEngine.Application.Extensions.Mapper;

public static class ContactFormExtension
{
    /// <summary>
    /// Maps a ContactFormEditModel into a ContactForm
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public static Email MapToModel(this ContactForm item)
    {
        return new Email
        {
            From = item.From,
            Subject = item.Subject,
            Message = item.Message
        };
    }

    /// <summary>
    /// Maps an IEnumerable<ContactForm> into an IEnumerable<ContactFormViewModel>
    /// </summary>
    /// <param name="emails"></param>
    /// <returns></returns>
    public static IEnumerable<ContactForm> MapToViewModel(this IEnumerable<Email> emails)
    {
        var viewModel = new List<ContactForm>();

        foreach (var item in emails)
        {
            viewModel.Add(new ContactForm(item.From, item.Subject, item.Message));
        }

        return viewModel;
    }

}
