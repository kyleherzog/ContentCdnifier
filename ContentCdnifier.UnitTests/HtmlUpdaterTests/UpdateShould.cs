namespace ContentCdnifier.UnitTests.HtmlUpdaterTests;

[TestClass]
public class UpdateShould : VerifyBase
{
    [TestMethod]
    public Task UpdateHtmlGivenRelativeImgSrc()
    {
        var html =
"""
<html><body><img src="image.jpg" /></body></html>
""";
        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        return Verify(updatedHtml);
    }

    [TestMethod]
    public Task UpdateHtmlGivenRootRelativeImgSrc()
    {
        var html =
"""
<html><body><img src="/images/image.jpg" /></body></html>
""";
        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        return Verify(updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenAbsoluteImgSrc()
    {
        var html =
"""
<html><body><img src="https://acme.com/image.jpg"></body></html>
""";
        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenContentCdnifierFalse()
    {
        var html =
"""
<html><body><img src="/image.jpg" data-cdnify="false"></body></html>
""";
        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenNoImgSrc()
    {
        var html =
"""
<html><body><img src=""></body></html>
""";
        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenNoCdnHost()
    {
        var html =
"""
<html><body><img src="/images/image.jpg" /></body></html>
""";

        var updater = HtmlUpdaterFactory.Create();
        updater.Options.CdnHost = null;
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenEmptyHtml()
    {
        var html = string.Empty;

        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public void ReturnEmptyGivenNullHtml()
    {
        string? html = null;

        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(string.Empty, updatedHtml);
    }

    [TestMethod]
    public void NotUpdateHtmlGivenProtocolRelativeImgSrc()
    {
        var html =
"""
<html><body><img src="//acme.com/image.png"></body></html>
""";

        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        Assert.AreEqual(html, updatedHtml);
    }

    [TestMethod]
    public Task UpdateHtmlGivenMultipleImgTags()
    {
        var html =
"""
<html><body><div><img src="/images/image.jpg" /></div><img src="/images/image2.jpg" /</body></html>
""";

        var updater = HtmlUpdaterFactory.Create();
        var updatedHtml = updater.Update(html);

        return Verify(updatedHtml);
    }
}