using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StorageManagement_MVC.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<StorageManagement_MVCContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StorageManagement_MVCContext") ?? throw new InvalidOperationException("Connection string 'StorageManagement_MVCContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options 
    =>
{
    Options.LoginPath = "/";
    Options.AccessDeniedPath = "/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
                name: "Products",
                pattern: "Products/{action=Edit}/{Id?}",
                defaults: new { controller = "Products" });
app.MapControllerRoute(
                name: "Users",
                pattern: "Users/{action=Edit}/{userId?}",
                defaults: new { controller = "Users" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Login}");

app.Run();
