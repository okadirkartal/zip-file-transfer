using Infrastructure.Network;
using Infrastructure.Network.Contracts;
using Infrastructure.Security.Contracts;
using Infrastructure.Security.Cryptor;
using Infrastructure.Services;
using Infrastructure.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Sender.Services;
using Sender.Services.Contracts;

namespace Sender.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IEncrypter, Encrypter>();
        services.AddHttpClient();
        services.AddTransient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddTransient<ITransferService, TransferService>();
        services.AddTransient<IFileManagementService, FileManagementService>();
        services.AddTransient<IZipManagementService, ZipManagementService>();
    }
}