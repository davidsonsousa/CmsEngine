using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMSEngine.Data.Models
{
    public class Website : BaseModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Culture { get; set; }

        public string UrlFormat { get; set; } = $"{Constants.SiteUrl}/{Constants.Type}/{Constants.Slug}";

        public string DateFormat { get; set; } = "MM/dd/yyyy";

        public string SiteUrl { get; set; }
    }
}
