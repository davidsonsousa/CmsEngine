using CmsEngine.Data.Models;
using System.Data.Entity;

namespace CmsEngine.Data
{
    public class CmsEngineInitializer : CreateDatabaseIfNotExists<CmsEngineContext>
    {
        protected override void Seed(CmsEngineContext context)
        {
            // Create default website

            var website = new Website
            {
                Name = "Default Website",
                Description = "Welcome to the Default Website",
                Culture = "en-US",
                SiteUrl = "cmsengine-dev.com"
            };

            context.Websites.Add(website);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
