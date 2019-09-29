using Microsoft.AspNetCore.Builder;

namespace CmsEngine.Ui.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder ConfigureFileUpload(this IApplicationBuilder builder, FileUploadOptions options)
        {
            return builder.UseMiddleware<ConfigureFileUploadMiddleware>(options);
        }
    }
}
