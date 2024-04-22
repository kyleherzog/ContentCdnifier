# ContentCdnifier
A .NET library that provides ASP.NET core middleware to optionally rewrite relative local static file links to an external CDN.

See the [changelog](CHANGELOG.md) for changes.

## Overview
Hosting static content from the wwwroot folder in an ASP.NET Core web application is convenient, but can have an impact on CPU usage and bandwidth used when hosted in an Azure App Service environment. The ContentCdnifier package enables one to easily offload the static files, allowing the hosting to occur on a separate web host like your favorite CDN provider.

The configuration settings allow for the functionality to be enabled only in certain environments. Therefore, when developing locally, the local wwwroot folder can be used, but when hosted in a production environment an alternative location can be specified.

## Getting Started
The ContentCdnifier middleware is easily initialized and added to the ASP.NET pipeline during startup.

```csharp
app.UseContentCdnifier();
```

During initialization, ContentCdnifier can be configured through the appsettings.json.
```json
{
  "ContentCdnifier": {
    "CdnHost": "https://example.azureedge.com/"
  }
}
```

## Tags/Attributes Targets
A standard set of media tags and corresponding attributes are configured to be modified. The tags and their target tags can be modified during startup.

```csharp
app.UseContentCdnifier(x =>
{
    x.TagAttributeMappings["script"] = ["src", "data-src"];
);
```

## Excluding Tags
Specific HTML tags can be excluded from modifications by setting the `data-cdnify` attribute to false.

```html
<img src='image.jpg' data-content-cdnify='false' />
```