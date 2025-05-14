using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Profile.Controllers;
using Report.Controllers;
using Report.Services.Implements;
using Report.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ProfileController).Assembly));
builder.Services.AddControllersWithViews()
    .PartManager.ApplicationParts.Add(new AssemblyPart(typeof(ReportController).Assembly));
builder.Services.AddHttpClient();
builder.Services.AddSignalR();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IReportService, ReportService>();

builder.Services.AddLogging();
builder.Services.AddMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
