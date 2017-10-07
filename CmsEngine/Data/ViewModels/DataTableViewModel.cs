using System.Collections.Generic;

namespace CmsEngine.Data.ViewModels
{
    public class DataTableViewModel
    {
        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public List<string> Columns { get; set; }

        public List<DataItem> Rows { get; set; }
    }

    public class DataItem
    {
        public List<DataProperty> DataProperties { get; set; }
    }

    public class DataProperty
    {
        public string DataType { get; set; }
        public string DataContent { get; set; }
    }
}
