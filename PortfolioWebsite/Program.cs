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
    var requestHost = context.Request.Host.Host?.ToLowerInvariant();

    if (string.IsNullOrEmpty(requestHost) ||
        !(requestHost == "asiddons.co.uk" ||
          requestHost == "www.asiddons.co.uk" ||
          requestHost == "localhost"))
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
