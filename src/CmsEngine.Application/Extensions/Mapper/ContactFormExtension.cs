namespace CmsEngine.Application.Extensions.Mapper;

public static class ContactFormExtension
{
    extension(ContactForm item)
    {
        /// <summary>
        /// Maps a ContactFormEditModel into a ContactForm
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Email MapToModel()
        {
            return new Email
            {
                From = item.From,
                Subject = item.Subject,
                Message = item.Message
            };
        }
    }

    extension(IEnumerable<Email> emails)
    {
        /// <summary>
        /// Maps an IEnumerable<ContactForm> into an IEnumerable<ContactFormViewModel>
        /// </summary>
        /// <param name="emails"></param>
        /// <returns></returns>
        public IEnumerable<ContactForm> MapToViewModel()
        {
            return emails.Select(item => new ContactForm(item.From, item.Subject, item.Message)).ToList();
        }
    }
}
