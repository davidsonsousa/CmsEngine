namespace CmsEngine.Ui.Areas.Identity.Pages.Account.Manage;

public partial class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ICmsEngineEmailSender _emailSender;
    private readonly IService _service;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ICmsEngineEmailSender emailSender,
        IService service, ILogger<IndexModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _service = service;
        _logger = logger;
    }

    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Username { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
        ViewData["CurrentUser"] = _service?.CurrentUser;

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var userName = await _userManager.GetUserNameAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;

        Input = new InputModel
        {
            Username = user.UserName,
            Name = user.Name,
            Surname = user.Surname,
            Email = email,
            PhoneNumber = phoneNumber
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        _logger.LogDebug("Account > Manage > OnPostAsync()");
        ViewData["CurrentUser"] = _service?.CurrentUser;

        if (!ModelState.IsValid)
        {
            _logger.LogDebug("Invalid ModelState");
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            _logger.LogDebug($"User not found. Id: {_userManager.GetUserId(User)}");
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var email = await _userManager.GetEmailAsync(user);
        if (Input.Email != email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
            if (!setEmailResult.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
            }
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
            }
        }

        if (Input.Name != user.Name || Input.Surname != user.Surname)
        {
            user.Name = Input.Name;
            user.Surname = Input.Surname;
            await _userManager.UpdateAsync(user);
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";

        _logger.LogDebug($"User {_userManager.GetUserId(User)} updated");

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }


        var userId = await _userManager.GetUserIdAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { userId = userId, code = code },
            protocol: Request.Scheme);

        await _emailSender.SendEmailConfirmationAsync(email, HtmlEncoder.Default.Encode(callbackUrl));

        StatusMessage = "Verification email sent. Please check your email.";
        return RedirectToPage();
    }
}
