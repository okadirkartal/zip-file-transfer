using Core.Network.Contracts;
using Core.ZipManagementService.Contracts;
using Security.Cryptor;
using Security.Cryptor.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Sender.Services;
using Sender.Services.Contracts;

namespace Sender.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IEncrypter, Encrypter>();
            services.AddTransient<ITransferService, TransferService>();
            services.AddTransient<IFileManagementService, FileManagementService>();
            services.AddTransient<IZipManagementService, Core.ZipManagementService.ZipManagementService>();
            services.AddSingleton<IHttpClientWrapper, HttpClientWrapper>();
        }
    }
}