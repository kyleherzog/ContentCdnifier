using HtmlAgilityPack;

namespace ContentCdnifier;

internal class HtmlUpdater
{
    public HtmlUpdater(ContentCdnifierOptions options)
    {
        Options = options;
    }

    public ContentCdnifierOptions Options { get; set; }

    public string Update(string? html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return string.Empty;
        }

        if (Options.CdnAddress is null)
        {
            return html;
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        foreach (var tagName in Options.TagAttributeMappings.Keys)
        {
            var tagsFound = doc.DocumentNode.SelectNodes($"//{tagName}[not(@data-cdnify='false')]");
            if (tagsFound is null)
            {
                continue;
            }

            foreach (var tag in tagsFound)
            {
                foreach (var attributeName in Options.TagAttributeMappings[tagName])
                {
                    var attributeValue = tag.GetAttributeValue(attributeName, null);
                    if (!string.IsNullOrEmpty(attributeValue) && !attributeValue.StartsWith("//"))
                    {
                        var url = new Uri(attributeValue, UriKind.RelativeOrAbsolute);

                        if (!url.IsAbsoluteUri)
                        {
                            var updatedValue = new Uri(Options.CdnAddress, attributeValue.TrimStart('/'));
                            tag.SetAttributeValue(attributeName, updatedValue.ToString());
                        }
                    }
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }
}