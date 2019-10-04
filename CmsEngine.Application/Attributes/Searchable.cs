using System;

namespace CmsEngine.Application.Attributes
{
    /// <summary>
    /// Enables the property to be searchable in the Search functionality
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Searchable : Attribute
    {

    }
}
