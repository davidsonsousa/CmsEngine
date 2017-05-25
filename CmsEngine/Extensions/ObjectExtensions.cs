using CmsEngine.Attributes;
using System;
using System.Linq;

namespace CmsEngine.Extensions
{
    public static class ObjectExtensions
    {
        public static object MapTo(this object source, object target, bool ignoreId = false)
        {
            var sourceProperties = source.GetType().GetProperties();

            foreach (var sourceProp in sourceProperties)
            {
                if (ignoreId == true && (sourceProp.Name == "Id" || sourceProp.Name == "VanityId"))
                {
                    continue;
                }
                
                var targetProp = target.GetType().GetProperties().Where(p => p.Name == sourceProp.Name).FirstOrDefault();
                if (targetProp != null && targetProp.CanWrite && targetProp.GetSetMethod() != null && targetProp.GetType().Name == sourceProp.GetType().Name)
                {
                    targetProp.SetValue(target, sourceProp.GetValue(source));
                }
            }
            return target;
        }

    }
}
