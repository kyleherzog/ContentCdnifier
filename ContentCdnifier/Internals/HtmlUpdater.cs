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

        if (string.IsNullOrEmpty(Options.CdnHost))
        {
            return html;
        }

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        foreach (var tagName in Options.TagAttributeMappings.Keys)
        {
            var tagsFound = doc.DocumentNode.SelectNodes($"//{tagName}");
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
                            var updatedValue = new Uri(new Uri(Options.CdnHost), attributeValue);
                            tag.SetAttributeValue(attributeName, updatedValue.ToString());
                        }
                    }
                }
            }
        }

        return doc.DocumentNode.OuterHtml;
    }
}