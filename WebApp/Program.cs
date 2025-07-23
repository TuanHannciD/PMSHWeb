using DevExpress.XtraCharts;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Profile.Controllers;
using Report.Controllers;
using Report.Services.Implements;
using Report.Services.Interfaces;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.CodeParser;
using Microsoft.Extensions.FileProviders;
using User.Controllers;
using User.Services.Implements;
using User.Services.Interfaces;
using Reservation.Controllers;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Commons.Containts;
using Reservation.Services.Interfaces;
using Reservation.Services.Implements;
using HouseKeeping.Controllers;
using HouseKeeping.Services.Interfaces;
using HouseKeeping.Services.Implements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(HouseKeepingController).Assembly));
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ProfileController).Assembly));
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ReportController).Assembly));
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(UserController).Assembly));
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ReservationController).Assembly));
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddDevExpressControls();
builder.Services.AddMvc();
builder.Services.ConfigureReportingServices(configurator => {
    configurator.ConfigureWebDocumentViewer(viewerconfigurator =>
    {
        viewerconfigurator.UseCachedReportSourceBuilder();
    });
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IHouseKeepingService, HouseKeepingService>();
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IReservationService, ReservationService>();
builder.Services.AddSingleton<IFolioDetailService, FolioDetailService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
               .AddCookie(options =>
               {
                   options.ExpireTimeSpan = TimeSpan.FromMinutes(AppConstants.EXPIRE_TIME);
                   options.Cookie.IsEssential = true;
                   options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.Cookie.Path = "/";
                   options.LoginPath = "/User/Index";
                   options.LoginPath = "/User/Index";
                   options.Cookie.HttpOnly = true;
                   options.Cookie.SameSite = SameSiteMode.Lax;
               });
builder.Services.AddLogging();
builder.Services.AddMemoryCache();
var app = builder.Build();
app.UseDevExpressControls();
System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var env = builder.Environment;
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
    RequestPath = "/node_modules",
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Index}/{id?}")
    .WithStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=User}/{action=Index}/{id?}")
//    .WithStaticAssets();


app.Run();
