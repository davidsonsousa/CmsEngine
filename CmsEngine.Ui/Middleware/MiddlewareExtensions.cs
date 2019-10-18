using System;
using CmsEngine.Ui.Middleware.SecurityHeaders;
using Microsoft.AspNetCore.Builder;

namespace CmsEngine.Ui.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureFileUpload(this IApplicationBuilder builder, FileUploadOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.UseMiddleware<ConfigureFileUploadMiddleware>(options);
        }

        public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder, SecurityHeadersBuilder securityHeadersBuilder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (securityHeadersBuilder is null)
            {
                throw new ArgumentNullException(nameof(securityHeadersBuilder));
            }

            return builder.UseMiddleware<SecurityHeadersMiddleware>(securityHeadersBuilder.Build());
        }
    }
}
