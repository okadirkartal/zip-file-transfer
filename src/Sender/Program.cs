
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Domain.Shared;
using Sender.Models;
using Sender.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices();
var env = builder.Environment;
var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");
builder.Configuration.AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: true)
.AddJsonFile("SharedSettings.json", optional: true)
.AddJsonFile("appsettings.json", optional: true)
.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables();

builder.Services.AddOptions<UploadSettings>()
.BindConfiguration("UploadSettings")
.ValidateDataAnnotations()
.ValidateOnStart();

builder.Services.Configure<SharedSettings>(builder.Configuration.GetSection("SharedSettings"));



builder.Services.AddControllersWithViews();
builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Sender/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

