namespace CmsEngine.Data.Models
{
    public class Page : Document
    {
        #region Navigation

        public int WebsiteId { get; set; }
        public virtual Website Website { get; set; }

        #endregion
    }
}
