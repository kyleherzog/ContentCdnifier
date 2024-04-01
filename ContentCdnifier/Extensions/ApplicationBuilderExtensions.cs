using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContentCdnifier;

/// <summary>
/// Provides extension methods for configuring the ContentCdnifier middleware.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the ContentCdnifier middleware to the pipeline.
    /// </summary>
    /// <param name="builder">The <see cref="IApplicationBuilder"/> instance to which the middleware is to be added.</param>
    /// <param name="optionsAction">An optional delegate to configure the <see cref="ContentCdnifierOptions"/> instance.</param>
    /// <returns>The original <see cref="IApplicationBuilder"/> instance which was passed in.</returns>
    public static IApplicationBuilder UseContentCdnifier(this IApplicationBuilder builder, Action<ContentCdnifierOptions>? optionsAction = null)
    {
        var options = new ContentCdnifierOptions();
        var configuration = builder.ApplicationServices.GetRequiredService<IConfiguration>();
        configuration.GetSection("ContentCdnifier").Bind(options);
        optionsAction?.Invoke(options);

        return builder.UseMiddleware<ContentCdnifierMiddleware>();
    }
}