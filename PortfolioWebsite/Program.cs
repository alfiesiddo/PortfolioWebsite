var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var host = context.Request.Host.Host;

    if (host.Equals("azurewebsites.net", StringComparison.OrdinalIgnoreCase))
    {
        var newUrl = $"https://asiddons.co.uk{context.Request.Path}{context.Request.QueryString}";
        context.Response.Redirect(newUrl, permanent: true);
        return;
    }

    await next();
});

app.Use(async (context, next) =>
{
    var allowedHosts = new[] { "asiddons.co.uk", "localhost" };
    var requestHost = context.Request.Host.Host?.ToLowerInvariant();

    if (!allowedHosts.Contains(requestHost))
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Forbidden");
        return;
    }

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
