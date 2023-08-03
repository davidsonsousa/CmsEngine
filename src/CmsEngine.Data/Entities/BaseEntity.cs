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

    public Guid VanityId { get; set; }

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            ["Id"] = Id,
            ["VanityId"] = VanityId
        };
        return jsonResult.ToString();
    }
}
