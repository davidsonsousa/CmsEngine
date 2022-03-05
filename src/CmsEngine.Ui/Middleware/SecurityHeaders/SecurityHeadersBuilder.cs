namespace CmsEngine.Ui.Middleware.SecurityHeaders;

/// <summary>
/// Exposes methods to build a policy.
/// </summary>
public class SecurityHeadersBuilder
{
    private readonly SecurityHeadersPolicy _policy = new();

    /// <summary>
    /// The number of seconds in one year
    /// </summary>
    public const int OneYearInSeconds = 60 * 60 * 24 * 365;

    /// <summary>
    /// Add default headers in accordance with most secure approach
    /// </summary>
    public SecurityHeadersBuilder AddDefaultSecurePolicy()
    {
        // TODO: Have these settings in a configuration file

        AddFrameOptionsDeny();
        AddXssProtectionBlock();
        AddContentTypeOptionsNoSniff();
        AddStrictTransportSecurityMaxAge();
        RemoveServerHeader();

        AddCustomHeader("Referrer-Policy", "strict-origin-when-cross-origin");
        AddCustomHeader("Feature-Policy", "geolocation 'none';midi 'none';notifications 'none';push 'none';sync-xhr 'none';" +
                                          "microphone 'none';camera 'none';magnetometer 'none';gyroscope 'none';speaker 'self';" +
                                          "vibrate 'none';fullscreen 'self';payment 'none';");
        AddCustomHeader("Content-Security-Policy", "default-src https: 'unsafe-inline' 'unsafe-eval'; " +
                                                   "img-src * 'self' data: https: blob:;" +
                                                   "style-src 'self' 'unsafe-inline' github.githubassets.com www.google.com platform.twitter.com cdn.syndication.twimg.com fonts.googleapis.com;" +
                                                   "script-src 'self' 'unsafe-inline' 'unsafe-eval' www.gstatic.com gist.github.com *.disqus.com www.googletagmanager.com www.google.com cse.google.com cdn.syndication.twimg.com platform.twitter.com cdn1.developermedia.com cdn2.developermedia.com apis.google.com www.googletagservices.com adservice.google.com securepubads.g.doubleclick.net ajax.aspnetcdn.com *.google-analytics.com");

        RemoveHeader("X-Powered-By");

        return this;
    }

    /// <summary>
    /// Add X-Frame-Options DENY to all requests.
    /// The page cannot be displayed in a frame, regardless of the site attempting to do so
    /// </summary>
    public SecurityHeadersBuilder AddFrameOptionsDeny()
    {
        _policy.SetHeaders[FrameOptions.Header] = FrameOptions.Deny;
        return this;
    }

    /// <summary>
    /// Add X-Frame-Options SAMEORIGIN to all requests.
    /// The page can only be displayed in a frame on the same origin as the page itself.
    /// </summary>
    public SecurityHeadersBuilder AddFrameOptionsSameOrigin()
    {
        _policy.SetHeaders[FrameOptions.Header] = FrameOptions.SameOrigin;
        return this;
    }

    /// <summary>
    /// Add X-Frame-Options ALLOW-FROM {uri} to all requests, where the uri is provided
    /// The page can only be displayed in a frame on the specified origin.
    /// </summary>
    /// <param name="uri">The uri of the origin in which the page may be displayed in a frame</param>
    public SecurityHeadersBuilder AddFrameOptionsSameOrigin(string uri)
    {
        _policy.SetHeaders[FrameOptions.Header] = string.Format(FrameOptions.AllowFromUri, uri);
        return this;
    }


    /// <summary>
    /// Add X-XSS-Protection 1 to all requests.
    /// Enables the XSS Protections
    /// </summary>
    public SecurityHeadersBuilder AddXssProtectionEnabled()
    {
        _policy.SetHeaders[XssProtection.Header] = XssProtection.Enabled;
        return this;
    }

    /// <summary>
    /// Add X-XSS-Protection 0 to all requests.
    /// Disables the XSS Protections offered by the user-agent.
    /// </summary>
    public SecurityHeadersBuilder AddXssProtectionDisabled()
    {
        _policy.SetHeaders[XssProtection.Header] = XssProtection.Disabled;
        return this;
    }

    /// <summary>
    /// Add X-XSS-Protection 1; mode=block to all requests.
    /// Enables XSS protections and instructs the user-agent to block the response in the event that script has been inserted from user input, instead of sanitizing.
    /// </summary>
    public SecurityHeadersBuilder AddXssProtectionBlock()
    {
        _policy.SetHeaders[XssProtection.Header] = XssProtection.Block;
        return this;
    }

    /// <summary>
    /// Add X-XSS-Protection 1; report=http://site.com/report to all requests.
    /// A partially supported directive that tells the user-agent to report potential XSS attacks to a single URL. Data will be POST'd to the report URL in JSON format.
    /// </summary>
    public SecurityHeadersBuilder AddXssProtectionReport(string reportUrl)
    {
        _policy.SetHeaders[XssProtection.Header] =
            string.Format(XssProtection.Report, reportUrl);
        return this;
    }

    /// <summary>
    /// Add Strict-Transport-Security max-age=<see cref="maxAge"/> to all requests.
    /// Tells the user-agent to cache the domain in the STS list for the number of seconds provided.
    /// </summary>
    public SecurityHeadersBuilder AddStrictTransportSecurityMaxAge(int maxAge = OneYearInSeconds)
    {
        _policy.SetHeaders[StrictTransportSecurity.Header] =
            string.Format(StrictTransportSecurity.MaxAge, maxAge);
        return this;
    }

    /// <summary>
    /// Add Strict-Transport-Security max-age=<see cref="maxAge"/>; includeSubDomains to all requests.
    /// Tells the user-agent to cache the domain in the STS list for the number of seconds provided and include any sub-domains.
    /// </summary>
    public SecurityHeadersBuilder AddStrictTransportSecurityMaxAgeIncludeSubDomains(int maxAge = OneYearInSeconds)
    {
        _policy.SetHeaders[StrictTransportSecurity.Header] =
            string.Format(StrictTransportSecurity.MaxAgeIncludeSubdomains, maxAge);
        return this;
    }

    /// <summary>
    /// Add Strict-Transport-Security max-age=0 to all requests.
    /// Tells the user-agent to remove, or not cache the host in the STS cache
    /// </summary>
    public SecurityHeadersBuilder AddStrictTransportSecurityNoCache()
    {
        _policy.SetHeaders[StrictTransportSecurity.Header] =
            StrictTransportSecurity.NoCache;
        return this;
    }

    /// <summary>
    /// Add X-Content-Type-Options nosniff to all requests.
    /// Can be set to protect against MIME type confusion attacks.
    /// </summary>
    public SecurityHeadersBuilder AddContentTypeOptionsNoSniff()
    {
        _policy.SetHeaders[ContentTypeOptions.Header] = ContentTypeOptions.NoSniff;
        return this;
    }

    /// <summary>
    /// Removes the Server header from all responses
    /// </summary>
    public SecurityHeadersBuilder RemoveServerHeader()
    {
        _policy.RemoveHeaders.Add(Server.Header);
        return this;
    }

    /// <summary>
    /// Adds a custom header to all requests
    /// </summary>
    /// <param name="header">The header name</param>
    /// <param name="value">The value for the header</param>
    /// <returns></returns>
    public SecurityHeadersBuilder AddCustomHeader(string header, string value)
    {
        if (string.IsNullOrEmpty(header))
        {
            throw new ArgumentNullException(nameof(header));
        }

        _policy.SetHeaders[header] = value;
        return this;
    }

    /// <summary>
    /// Remove a header from all requests
    /// </summary>
    /// <param name="header">The to remove</param>
    /// <returns></returns>
    public SecurityHeadersBuilder RemoveHeader(string header)
    {
        if (string.IsNullOrEmpty(header))
        {
            throw new ArgumentNullException(nameof(header));
        }

        _policy.RemoveHeaders.Add(header);
        return this;
    }

    /// <summary>
    /// Builds a new <see cref="SecurityHeadersPolicy"/> using the entries added.
    /// </summary>
    /// <returns>The constructed <see cref="SecurityHeadersPolicy"/>.</returns>
    public SecurityHeadersPolicy Build()
    {
        return _policy;
    }
}
