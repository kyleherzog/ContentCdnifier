namespace ContentCdnifier.UnitTests;

internal static class HtmlUpdaterFactory
{
    public static HtmlUpdater Create()
    {
        return new HtmlUpdater(new ContentCdnifierOptions
        {
            CdnHost = "https://cdn.example.com",
        });
    }
}