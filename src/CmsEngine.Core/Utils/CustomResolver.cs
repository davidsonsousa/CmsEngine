namespace CmsEngine.Core.Utils;

public sealed class CustomResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var prop = base.CreateProperty(member, memberSerialization);
        var propInfo = member as PropertyInfo;
        if (propInfo != null)
        {
            if (propInfo.GetMethod.IsVirtual && !propInfo.GetMethod.IsFinal)
            {
                prop.ShouldSerialize = obj => false;
            }
        }
        return prop;
    }
}
