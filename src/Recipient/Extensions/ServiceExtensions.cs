using DataAccess;
using DataAccess.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Security.Authenticator.Services;
using Security.Authenticator.Services.Contracts;
using Security.Cryptor;
using Security.Cryptor.Contracts;

namespace Recipient.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin());
            });
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDecrypter, Decrypter>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IDocumentPersistenceService, DocumentPersistenceService>();
        }
    }
}