using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace CmsEngine.Data
{
    public class DbConfig : DbConfiguration
    {
        public DbConfig()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }
}
