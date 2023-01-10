//
// NuGetConfiguration.cs
//
//   Created: 2022-10-15-12:08:05
//   Modified: 2022-11-01-05:25:37
//
//   Author: Justin Chase <justin@justinwritescode.com>
//
//   Copyright © 2022-2023 Justin Chase, All Rights Reserved
//      License: MIT (https://opensource.org/licenses/MIT)
//
#pragma warning disable
global using static global::NuGet.Configuration.ConfigurationConstants;
using static global::NuGet.Configuration.ConfigurationConstants;
namespace NuGetUtils.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using global::NuGet.Configuration;
using global::NuGet.Protocol;
using global::NuGetUtils.Configuration;

/// <summary>Represents a NuGet configuration file</summary>
public class NuGetConfiguration
{
    // private static readonly string[] ConfigurationSectionNames = {
    //     ActivePackageSourceSectionName,
    //     ApiKeys,
    //     BindingRedirectsSection,
    //     ClientCertificates,
    //     CredentialsSectionName,
    //     DisabledPackageSources,
    //     PackageManagementSection,
    //     PackageSources,
    //     TrustedSigners
    // };

    protected ISettings InnerConfig { get; init; }

    /// <summary>
    /// Gets the NuGet configuration file
    /// </summary>
    /// <param name="projectDirectory">The project directory.</param>
    /// <returns>The NuGet configuration file</returns>
    public static NuGetConfiguration GetNuGetConfiguration(string projectDirectory)
    {
        var nugetConfig = new Settings(projectDirectory);
        return new NuGetConfiguration(nugetConfig);
    }

    public static NuGetConfiguration GetMachineWideConfiguration()
    {
        var nugetConfig = Settings.LoadMachineWideSettings(Directory.GetDirectoryRoot(Environment.CurrentDirectory));
        return new NuGetConfiguration(nugetConfig);
    }

    /// <summary>Initializes a new instance of the <see cref="NuGetConfiguration"/> class.</summary>
    /// <param name="nugetConfigs">The NuGet configurations.</param>
    public NuGetConfiguration(params ISettings[] nugetConfigs)
    {
        NuGetConfigs = nugetConfigs.ToList();
    }

    /// <summary>Gets the NuGet configuration.</summary>
    /// <value>The NuGet configuration.</value>
    public IList<ISettings> NuGetConfigs { get; } = new List<ISettings>();

    public virtual IEnumerable<SourceItem> PackageSourceItems => NuGetConfigs.SelectMany(config => config.GetSection(ConfigurationConstants.PackageSources)?.Items?.OfType<SourceItem>());
    public virtual IEnumerable<CredentialsItem> CredentialsItems => NuGetConfigs.SelectMany(config => config.GetSection(CredentialsSectionName)?.Items?.OfType<CredentialsItem>());

    public IEnumerable<PackageSource> PackageSources =>
        from packageSource in PackageSourceItems
        join credentialsItems in NuGetConfigs.Select(config => config.GetSection(ConfigurationConstants.CredentialsSectionName))
        on packageSource?.Key equals credentialsItems?.ElementName
        select new PackageSource(packageSource.Value, packageSource.Key, true, true, true) { Credentials = new PackageSourceCredential(credentialsItems.ElementName, credentialsItems.Items.OfType<AddItem>().FirstOrDefault(item => item.Key.Equals("username", StringComparison.InvariantCultureIgnoreCase))?.Value, credentialsItems.Items.OfType<AddItem>().FirstOrDefault(item => item.Key.Equals("cleartextpassword", StringComparison.InvariantCultureIgnoreCase))?.Value, true, "basic") };
}
