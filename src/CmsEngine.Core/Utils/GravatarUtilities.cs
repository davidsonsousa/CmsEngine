namespace CmsEngine.Core.Utils;

public static class GravatarUtilities
{
    /// <summary>
    /// Generates an MD5 hash of the given string
    /// </summary>
    /// <remarks>Source: http://msdn.microsoft.com/en-us/library/system.security.cryptography.md5.aspx </remarks>
    public static string GetMd5Hash(string input)
    {
        // Compute the MD5 hash bytes
        var hashBytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));

        // Convert to hex string without intermediate StringBuilder allocations
        // Convert.ToHexString returns upper-case hex; previous implementation returned lower-case.
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
