
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using System.IO;
using Domain.Shared;
using Infrastructure.Extensions;
using Sender.Models;
using Sender.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(); 
builder.Configuration.AddConfigurationFiles(builder.Environment.ContentRootPath, builder.Environment.EnvironmentName);

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
    pattern: "{controller=Sender}/{action=Index}");

app.Run();

