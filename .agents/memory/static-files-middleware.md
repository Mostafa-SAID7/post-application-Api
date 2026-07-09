---
name: Static files vs RequestLoggingMiddleware
description: Two quirks that block UseStaticFiles from serving wwwroot in this project.
---

## Rule 1 — UseStaticFiles must come BEFORE RequestLoggingMiddleware

The `RequestLoggingMiddleware` swaps `context.Response.Body` for a `MemoryStream` to capture the response.
`StaticFileMiddleware` internally uses Kestrel's `IHttpSendFileFeature` to write the file directly to the socket, bypassing the substituted MemoryStream.
This causes static files to silently 404 when logging middleware runs first.

**Why:** Static files are handled completely and the pipeline is short-circuited; the logging middleware never needs to see the body.

**How to apply:** Register `UseStaticFiles()` (and `UseDefaultFiles()`) *before* `UseMiddleware<RequestLoggingMiddleware>()`.

---

## Rule 2 — UseDefaultFiles does NOT rewrite "/" when using an explicit PhysicalFileProvider

Even with `new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(wwwroot), RequestPath = "" }`, the root path "/" is not rewritten to "/index.html".

**Why:** `DefaultFilesMiddleware.GetDirectoryContents("")` returns an empty result from `PhysicalFileProvider` when the path is empty string vs "/" — a known edge case.

**Fix applied:** Use `app.MapGet("/", ...)` to explicitly serve `index.html`, and skip `UseDefaultFiles` entirely:
```csharp
app.MapGet("/", async (HttpContext ctx) =>
{
    ctx.Response.ContentType = "text/html; charset=utf-8";
    await ctx.Response.SendFileAsync(Path.Combine(wwwroot, "index.html"));
});
```
