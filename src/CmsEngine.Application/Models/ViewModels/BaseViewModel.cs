namespace CmsEngine.Application.Models.ViewModels;

public class BaseViewModel : IViewModel
{
    public int Id { get; set; }

    private Guid _vanityId;
    private string? _vanityIdString;

    public Guid VanityId
    {
        get => _vanityId;
        set
        {
            _vanityId = value;
            // Invalidate cached string so it will be regenerated on next access
            _vanityIdString = null;
        }
    }

    /// <summary>
    /// Cached string representation of <see cref="VanityId"/> to avoid repeated Guid.ToString() allocations.
    /// </summary>
    public string VanityIdString => _vanityIdString ??= _vanityId.ToString();
}
