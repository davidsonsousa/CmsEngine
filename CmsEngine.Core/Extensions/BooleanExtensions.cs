namespace CmsEngine.Extensions
{
    public static class BooleanExtensions
    {
        /// <summary>
        /// Convert boolean values to "Yes" or "No"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYesNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}
