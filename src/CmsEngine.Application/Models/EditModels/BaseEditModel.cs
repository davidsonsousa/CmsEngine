namespace CmsEngine.Application.Models.EditModels;

public class BaseEditModel
{
    public bool IsNew {
        get {
            return Id == 0 && VanityId == Guid.Empty;
        }
    }

    public int Id { get; set; }

    public Guid VanityId { get; set; }

    public override string ToString()
    {
        var jsonResult = new JsonObject
        {
            [nameof(Id)] = Id,
            [nameof(VanityId)] = VanityId
        };
        return jsonResult.ToString();
    }

}
