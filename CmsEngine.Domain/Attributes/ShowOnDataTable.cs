using System;

namespace CmsEngine.Domain.Attributes
{
    /// <summary>
    /// Enables the properto to be visible in the DataTable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ShowOnDataTable : Attribute
    {
        private readonly int order;
        public int Order { get => order; }

        public ShowOnDataTable(int Order)
        {
            order = Order;
        }
    }
}
