namespace CmsEngine.Ui.Extensions;

public static class MiddlewareExtensions
{
    extension(IApplicationBuilder builder)
    {
        public IApplicationBuilder ConfigureFileUpload(FileUploadOptions options)
        {
            ArgumentNullException.ThrowIfNull(builder);

            ArgumentNullException.ThrowIfNull(options);

            return builder.UseMiddleware<ConfigureFileUploadMiddleware>(options);
        }

        public IApplicationBuilder UseSecurityHeaders(SecurityHeadersBuilder securityHeadersBuilder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            ArgumentNullException.ThrowIfNull(securityHeadersBuilder);

            return builder.UseMiddleware<SecurityHeadersMiddleware>(securityHeadersBuilder.Build());
        }
    }
}
