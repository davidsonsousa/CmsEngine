using System;
using CmsEngine.Core.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace CmsEngine.Ui.RewriteRules
{
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
            if (httpRequest.Host.Host.Equals(CmsEngineConstants.Localhost, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            if (!httpRequest.Host.Value.StartsWith(CmsEngineConstants.WwwDot, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = RuleResult.ContinueRules;
                return;
            }

            var wwwHost = new HostString(httpRequest.Host.Value.Replace(CmsEngineConstants.WwwDot, string.Empty));
            var newUrl = UriHelper.BuildAbsolute(httpRequest.Scheme, wwwHost, httpRequest.PathBase, httpRequest.Path, httpRequest.QueryString);
            var httpResponse = context.HttpContext.Response;
            httpResponse.StatusCode = _statusCode;
            httpResponse.Headers[HeaderNames.Location] = newUrl;
            context.Result = RuleResult.EndResponse;
        }
    }
}
