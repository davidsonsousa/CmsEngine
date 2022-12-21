namespace CmsEngine.Data.Entities;

public class PostCategory
{
    public int PostId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
