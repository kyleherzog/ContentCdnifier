namespace ContentCdnifier;

/// <summary>
/// Configuration options for the ContentCdnifier middleware.
/// </summary>
public class ContentCdnifierOptions
{
    /// <summary>
    /// Gets or sets the host name of the CDN to use for serving static files.
    /// </summary>
    public string? CdnHost { get; set; }

    /// <summary>
    /// Gets or sets the mappings of tags to their attributes that should be updated to use the CDN host.
    /// </summary>
    public Dictionary<string, IEnumerable<string>> TagAttributeMappings { get; set; } = new()
    {
        { "img", ["src", "data-src"] },
        { "script", ["src"] },
        { "link", ["href"] },
        { "src", ["src", "data-src", "srcset", "data-srcset"] },
        { "source", ["src", "data-src"] },
    };
}