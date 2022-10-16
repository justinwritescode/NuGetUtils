using System.Reflection;
namespace JustinWritesCode.NuGetUtils;
using global::JustinWritesCode.NuGet.Configuration;
using global::NuGet.Commands;

public class NuGet
{
    public static readonly Settings Config = new Settings(typeof(NuGet).Assembly.Location);
    public static IPackageSourceProvider PackageSourceProvider => new PackageSourceProvider(Config);

    public static async Task<PushResult> Push(FileInfo packageFile, string source)
    {
        return default;
        //PushRunner.Run(Config.)
    }
}
