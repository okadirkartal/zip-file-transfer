using Microsoft.Extensions.Configuration;
namespace Infrastructure.Extensions;

public static class BuilderExtensions
{
    public static void AddConfigurationFiles(this ConfigurationManager configurationManager,string contentRootPath,string environmentName)
    {
        var sharedFolder = Path.Combine(contentRootPath, "..", "Shared");
        configurationManager.AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), optional: true)
            .AddJsonFile("SharedSettings.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environmentName}.json", optional: true);
    }
}