



using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Authentication;
using Infrastructure.Handlers;
using Recipient.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.ConfigureCors();
builder.Services.AddServices();
builder.Services.AddAuthentication("BasicAuthentication")
               .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
var env = builder.Environment;
var sharedFolder = Path.Combine(env.ContentRootPath, "..", "Shared");
builder.Configuration.AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: true)
.AddJsonFile("SharedSettings.json", optional: true)
.AddJsonFile("appsettings.json", optional: true)
.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
.AddEnvironmentVariables();

builder.Services.Configure<Domain.Shared.SharedSettings>(builder.Configuration.GetSection("SharedSettings"));



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

