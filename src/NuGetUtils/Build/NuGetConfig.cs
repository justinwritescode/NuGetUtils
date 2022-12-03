namespace JustinWritesCode.NuGetUtils;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using global::NuGet.Configuration;
using static global::NuGet.Configuration.ConfigurationConstants;

/// <summary>
/// Gets the NuGet configuration file
/// </summary>
public class NuGetConfig : MSBTask
{
    [Output]
    public ITaskItem[] PackageSources { get; set; } = Array.Empty<ITaskItem>();

    [Output]
    public ITaskItem[] ConfigFiles { get; set; } = Array.Empty<ITaskItem>();

    /// <summary>
    /// Runs the task
    /// </summary>
    /// <returns><c>true</c> if the task completed successfully, <c>false</c> otherwise.</returns>
    public override bool Execute()
    {
        ConfigFiles = Settings.OrderedSettingsFileNames.Select(x => new TaskItem(x)).ToArray();

        // var nugetConfig = new global::JustinWritesCode.NuGet.Configuration.Settings(Path.GetDirectoryName(this.BuildEngine.ProjectFileOfTaskNode));
        // Console.WriteLine($"GetConfigFilePaths: {string.Join(", ", nugetConfig.GetConfigFilePaths())}");
        // var packageSources = nugetConfig.GetSection(ConfigurationConstants.PackageSources);
        // var packageSourceItems = packageSources.Items.OfType<AddItem>();
        // PackageSources = packageSourceItems.Select(x => new TaskItem(x.Key, new Dictionary<string, string> { { ValueAttribute, x.Value } })).ToArray();
        return true;
    }
}
