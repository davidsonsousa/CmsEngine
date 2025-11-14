namespace CmsEngine.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Returns the Name property from DisplayAttribute
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetName(this Enum value)
    {
        // Get attributes
        var field = value.GetType().GetField(value.ToString());

        if (field is null)
        {
            return string.Empty;
        }

        var attributes = field.GetCustomAttributes(false);

        dynamic? displayAttribute = null;

        if (attributes.Any())
        {
            displayAttribute = attributes.ElementAt(0);
        }

        // Return name
        return displayAttribute?.Name ?? field.Name;
    }

    /// <summary>
    /// Returns the value of a DescriptionAttribute for a given Enum value
    /// </summary>
    /// <remarks>Source: http://blogs.msdn.com/b/abhinaba/archive/2005/10/21/483337.aspx </remarks>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value)
    {

        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());

        if (memberInfo is not null && memberInfo.Length > 0)
        {
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).ToArray();

            if (attrs is not null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }
        }

        return value.ToString();

    }
}
