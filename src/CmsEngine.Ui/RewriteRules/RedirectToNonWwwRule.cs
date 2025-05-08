namespace CmsEngine.Ui.RewriteRules;

public class RedirectToNonWwwRule : IRule
{
    private readonly int _statusCode;

    public RedirectToNonWwwRule(int statusCode)
    {
        _statusCode = statusCode;
    }

    public virtual void ApplyRule(RewriteContext context)
    {
        var httpRequest = context.HttpContext.Request;
        if (httpRequest.Host.Host.Equals(Main.Localhost, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = RuleResult.ContinueRules;
            return;
        }

        if (httpRequest.Host.Value is not null && !httpRequest.Host.Value.StartsWith(Main.WwwDot, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = RuleResult.ContinueRules;
            return;
        }

        var wwwHost = new HostString(httpRequest.Host.Value?.Replace(Main.WwwDot, string.Empty));
        var newUrl = UriHelper.BuildAbsolute(httpRequest.Scheme, wwwHost, httpRequest.PathBase, httpRequest.Path, httpRequest.QueryString);
        var httpResponse = context.HttpContext.Response;
        httpResponse.StatusCode = _statusCode;
        httpResponse.Headers[HeaderNames.Location] = newUrl;
        context.Result = RuleResult.EndResponse;
    }
}
