using Infrastructure.Persistence;
using Infrastructure.Persistence.Contracts;
using Infrastructure.Security;
using Infrastructure.Security.Contracts;
using Infrastructure.Services;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Recipient.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
        });
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDecrypter, Decrypter>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IDocumentPersistenceService, DocumentPersistenceService>();
    }
}