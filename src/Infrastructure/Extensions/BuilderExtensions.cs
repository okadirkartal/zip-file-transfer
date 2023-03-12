using Microsoft.Extensions.Configuration;

namespace Infrastructure.Extensions;

public static class BuilderExtensions
{
    public static void AddConfigurationFiles(this ConfigurationManager configurationManager, string contentRootPath,
        string environmentName)
    {
        var sharedFolder = Path.Combine(contentRootPath, "..", "Shared");
        configurationManager.AddJsonFile(Path.Combine(sharedFolder, "SharedSettings.json"), true)
            .AddJsonFile("SharedSettings.json", true)
            .AddJsonFile("appsettings.json", true)
            .AddJsonFile($"appsettings.{environmentName}.json", true);
    }
}