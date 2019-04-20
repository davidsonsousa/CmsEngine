using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace CmsEngine.Ui.Middleware.RewriteRules
{
    public class RedirectToNonWwwRule : IRule
    {
        public readonly int _statusCode;

        public RedirectToNonWwwRule(int statusCode)
        {
            _statusCode = statusCode;
        }

        public virtual void ApplyRule(RewriteContext context)
        {
            var httpRequest = context.HttpContext.Request;
            if (httpRequest.Host.Host.Equals(Constants.Localhost, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            if (!httpRequest.Host.Value.StartsWith(Constants.WwwDot, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var wwwHost = new HostString(httpRequest.Host.Value.Replace(Constants.WwwDot, string.Empty));
            var newUrl = UriHelper.BuildAbsolute(httpRequest.Scheme, wwwHost, httpRequest.PathBase, httpRequest.Path, httpRequest.QueryString);
            var httpResponse = context.HttpContext.Response;
            httpResponse.StatusCode = _statusCode;
            httpResponse.Headers[HeaderNames.Location] = newUrl;
            context.Result = RuleResult.EndResponse;
        }
    }
}
