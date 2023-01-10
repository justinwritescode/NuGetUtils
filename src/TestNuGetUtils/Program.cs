// See https://aka.ms/new-console-template for more information
using NuGet.Configuration;

Console.WriteLine("Hello, World!");

var nugetConfig = NuGetUtils.Configuration.NuGetConfiguration.GetMachineWideConfiguration();//(Directory.GetCurrentDirectory());

var queryResult =
    await NuGetUtils.NuGet.Query(
        new Microsoft.Extensions.Logging.ConsoleLogger(),
        "Microsoft.Build",
        "https://pkgs.dev.azure.com/justinwritescode/_packaging/justinwritescode/nuget/v3/index.json",
        "az"
    );

Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(queryResult));


// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nugetConfig));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nugetConfig.NuGetConfigs));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nugetConfig.PackageSourceItems));
// Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(nugetConfig.CredentialsItems));

// foreach(var config in nugetConfig.NuGetConfigs)
// {
//     Console.WriteLine(string.Join(", ", config.GetConfigFilePaths()));
// }

// foreach(var item in nugetConfig.NuGetConfigs.Select(config => config.GetSection(ConfigurationConstants.PackageSources) as P/*.Items.OfType<AddItem>()*/))
// {
//     Console.WriteLine($"Element: {item?.ElementName}");
// }
