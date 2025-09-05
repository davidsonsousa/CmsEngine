namespace CmsEngine.Tests.Services;

public class EmailServiceTests : BaseServiceTests
{

    public EmailServiceTests() : base()
    {
    }

    [Fact]
    public async Task Save_ShouldInsertAndSaveContactForm()
    {
        // Arrange
        var contactForm = new ContactForm("to@example.com", "Subject", "Message");
        var emailModel = new Email(); // Assuming Email is the mapped model
        // Simulate MapToModel
        var mapToModelCalled = false;
        contactForm.GetType().GetMethod("MapToModel")?.Invoke(contactForm, null);
        _emailRepoMock.Setup(r => r.Insert(It.IsAny<Email>())).Returns(Task.CompletedTask);
        _uowMock.Setup(u => u.Save(default)).ReturnsAsync(1);

        // Act
        var result = await _emailService.Save(contactForm);

        // Assert
        _emailRepoMock.Verify(r => r.Insert(It.IsAny<Email>()), Times.Once);
        _uowMock.Verify(u => u.Save(default), Times.Once);
        Assert.Contains("saved", result.Message, StringComparison.OrdinalIgnoreCase);
        Assert.False(result.IsError);
    }

    [Fact]
    public async Task Save_ShouldSetErrorMessage_OnException()
    {
        // Arrange
        var contactForm = new ContactForm("to@example.com", "Subject", "Message");
        _emailRepoMock.Setup(r => r.Insert(It.IsAny<Email>())).Throws(new Exception("fail"));

        // Act
        var result = await _emailService.Save(contactForm);

        // Assert
        Assert.True(result.IsError);
        Assert.Contains("error", result.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetOrderedByDate_ShouldReturnMappedContactForms()
    {
        // Arrange
        var emails = new List<Email> { new Email(), new Email() };
        _emailRepoMock.Setup(r => r.GetOrderedByDate()).ReturnsAsync(emails);

        // Simulate MapToViewModel extension
        IEnumerable<ContactForm> mapped = new List<ContactForm> { new ContactForm("to", "subj", "msg") };
        // You may need to mock the MapToViewModel extension if it's not static

        // Act
        var result = await _emailService.GetOrderedByDate();

        // Assert
        _emailRepoMock.Verify(r => r.GetOrderedByDate(), Times.Once);
        Assert.NotNull(result);
        // Optionally check the count if you can control the mapping
    }
}
