using System;

namespace CmsEngine.Application.Attributes
{
    /// <summary>
    /// Enables the properto to be visible in the DataTable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ShowOnDataTable : Attribute
    {
        private readonly int order;
        public int Order
        {
            get
            {
                return order;
            }
        }

        public ShowOnDataTable(int Order)
        {
            order = Order;
        }
    }
}
