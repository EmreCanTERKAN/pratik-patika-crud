using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    // Giriþ , Çýkýþ , Eriþim Reddi durumlarýnda anasayfaya yönlendir...
    option.LoginPath = new PathString("/");
    option.LogoutPath = new PathString("/");
    option.AccessDeniedPath = new PathString("/");
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
