using System.Linq;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;

namespace CmsEngine.Ui.RewriteRules
{
    public class RedirectLowerCaseRule : IRule
    {
        private readonly int _statusCode;
        public RedirectLowerCaseRule(int statusCode)
        {
            _statusCode = statusCode;
        }

        public void ApplyRule(RewriteContext context)
        {
            var request = context.HttpContext.Request;
            var path = context.HttpContext.Request.Path;
            var host = context.HttpContext.Request.Host;

            if ((path.HasValue && path.Value.Any(char.IsUpper) && !path.Value.Contains("/Identity/Account")) || (host.HasValue && host.Value.Any(char.IsUpper)))
            {
                var response = context.HttpContext.Response;
                response.StatusCode = _statusCode;
                response.Headers[HeaderNames.Location] = (request.Scheme + "://" + host.Value + request.PathBase + request.Path).ToLower() + request.QueryString;
                context.Result = RuleResult.EndResponse;
            }
            else
            {
                context.Result = RuleResult.ContinueRules;
            }
        }
    }
}
