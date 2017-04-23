using System;

namespace CmsEngine.Attributes
{
    /// <summary>
    /// Enables the properto to be visible in the DataTable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShowOnDataTable : Attribute
    {
        private int order;
        public int Order { get => order; }

        public ShowOnDataTable(int Order)
        {
            order = Order;
        }
    }
}
