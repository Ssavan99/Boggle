using System;
using System.Collections.Generic;
using Boggle.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Games are held in memory and were never reclaimed; this sweeps idle ones.
builder.Services.AddHostedService<StaleGameCleanupService>();

var app = builder.Build();

// Honour X-Forwarded-* so the app sees the original scheme when it runs behind
// a TLS-terminating proxy (Azure App Service, Render, etc). Without this the
// HTTPS redirect below can loop forever in production.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Serve the game client itself at "/". This replaces a meta-refresh redirect
// that briefly flashed a "Loading Boggle...." page on every visit.
app.UseDefaultFiles(new DefaultFilesOptions
{
    DefaultFileNames = new List<string> { "democlient.html" }
});

// The client is plain files with unversioned names, so a browser that cached
// them keeps running old code after a deploy. Asking it to revalidate markup,
// script and styles avoids that; ETags make the check a cheap 304. Images are
// stable, so they keep a long cache.
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        string path = ctx.File.Name;
        bool revalidate = path.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
                       || path.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                       || path.EndsWith(".css", StringComparison.OrdinalIgnoreCase);

        ctx.Context.Response.Headers["Cache-Control"] = revalidate
            ? "no-cache, must-revalidate"
            : "public, max-age=604800";
    }
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
