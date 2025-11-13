namespace CmsEngine.Data.Entities;

public class BaseEntity
{
    [NotMapped]
    public bool IsNew {
        get {
            return Id == 0 && VanityId == Guid.Empty;
        }
    }

    public bool IsDeleted { get; set; }

    [Key]
    public int Id { get; set; }

    private Guid _vanityId;
    private string? _vanityIdString;

    public Guid VanityId
    {
        get => _vanityId;
        set
        {
            _vanityId = value;
            _vanityIdString = null;
        }
    }

    /// <summary>
    /// Cached string representation of <see cref="VanityId"/> to avoid repeated Guid.ToString() allocations.
    /// </summary>
    public string VanityIdString => _vanityIdString ??= _vanityId.ToString();

    public override string ToString()
    {
        // Return a small, allocation-minimal representation for diagnostics.
        return $"{GetType().Name}(Id={Id},VanityId={VanityIdString})";
    }
}
