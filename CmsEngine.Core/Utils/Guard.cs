using System;

namespace CmsEngine.Core.Utils
{
    public static class Guard
    {
        public static void ThrownExceptionIfNull(object value, string objectName)
        {
            if(value is null)
            {
                throw new ArgumentNullException(objectName);
            }
        }
    }
}
