using Microsoft.AspNetCore.Http;

namespace ContentCdnifier;

/// <summary>
/// Asp.Net Core middleware to serve static files from a CDN by rewriting the request path on relative paths for tags such as img, script, link, etc.
/// </summary>
internal class ContentCdnifierMiddleware
{
    private readonly RequestDelegate next;
    private readonly ContentCdnifierOptions options;

    public ContentCdnifierMiddleware(RequestDelegate next, ContentCdnifierOptions options)
    {
        this.next = next;
        this.options = options;
    }

    public async Task Invoke(HttpContext context)
    {
        if (string.IsNullOrEmpty(options.CdnHost))
        {
            await next(context).ConfigureAwait(false);
            return;
        }

        var originalBody = context.Response.Body;

        try
        {
            var bodySnatcher = new MemoryStream();

            context.Response.Body = bodySnatcher;

            await next(context).ConfigureAwait(false);

            bodySnatcher.Position = 0;

            if (context.Response.ContentType?.Contains("text/html") ?? false)
            {
                var updatedBody = new MemoryStream();
                using var reader = new StreamReader(bodySnatcher);
                var html = await reader.ReadToEndAsync().ConfigureAwait(false);

                var updater = new HtmlUpdater(options);
                var updatedHtml = updater.Update(html);

                using var writer = new StreamWriter(updatedBody);
                await writer.WriteAsync(updatedHtml).ConfigureAwait(false);
                await writer.FlushAsync().ConfigureAwait(false);
                updatedBody.Position = 0;
                await updatedBody.CopyToAsync(originalBody).ConfigureAwait(false);
            }
            else
            {
                await bodySnatcher.CopyToAsync(originalBody).ConfigureAwait(false);
            }
        }
        finally
        {
            context.Response.Body = originalBody;
        }
    }
}