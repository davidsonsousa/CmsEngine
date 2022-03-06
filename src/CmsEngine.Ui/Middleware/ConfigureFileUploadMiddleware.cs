namespace CmsEngine.Ui.Middleware;

public class ConfigureFileUploadMiddleware
{
    private readonly RequestDelegate _next;

    public ConfigureFileUploadMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, FileUploadOptions options)
    {
        string uploadPath = Path.Combine(options.Root, options.Folder);

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        // Call the next delegate/middleware in the pipeline
        await _next(context);
    }
}
