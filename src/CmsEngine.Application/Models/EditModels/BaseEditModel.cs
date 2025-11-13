namespace CmsEngine.Application.Models.EditModels;

public class BaseEditModel
{
    public bool IsNew {
        get {
            return Id == 0 && VanityId == Guid.Empty;
        }
    }

    public int Id { get; set; }

    private Guid _vanityId;
    private string? _vanityIdString;

    public Guid VanityId {
        get => _vanityId;
        set {
            _vanityId = value;
            _vanityIdString = null;
        }
    }

    /// <summary>
    /// Cached string representation of <see cref="VanityId"/> to avoid repeated Guid.ToString() allocations.
    /// </summary>
    public string VanityIdString 
    {
        get
        {
            return _vanityIdString ??= _vanityId.ToString();
        }
    }

    public override string ToString()
    {
        return $"BaseEditModel(Id={Id},VanityId={VanityIdString})";
    }
}
