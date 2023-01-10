/*
 * NuGet.cs
 *
 *   Created: 2022-10-11-09:33:30
 *   Modified: 2022-11-29-02:23:36
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022-2023 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */
#pragma warning disable
namespace NuGetUtils;

using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;
using global::NuGet.Commands;
using global::NuGet.Common;
using global::NuGet.Configuration;
using global::NuGet.Protocol;
using global::NuGet.Protocol.Core.Types;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public static class NuGet
{
	public const string NuGetPackageFilenameRegexString = @"(?<packageId>[\w-]+)\.(?<version>[\d\.]+)\.nupkg";
	public const string DefaultNuGetApiUrl = "https://api.nuget.org/v3/index.json";
	public static readonly REx NuGetPackageFilenameRegex = new(NuGetPackageFilenameRegexString, Rxo.Compiled | Rxo.IgnoreCase);
	public static readonly ISettings NuGetConfig = Settings.LoadMachineWideSettings(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()), Directory.GetCurrentDirectory());
	public static readonly IDictionary<string, bool> NuGetPackageExistsCache = new Dictionary<string, bool>(JsonConvert.DeserializeObject<Dictionary<string, bool>>(File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NuGetPackageExistsCache.json"))), StringComparer.OrdinalIgnoreCase);
    public static async Task<NuGetCommandResult> Push(Microsoft.Extensions.Logging.ILogger msLogger, FileInfo packageFile, string source = DefaultNuGetApiUrl, string apiKey = null)
    {
		var repository = Repository.Factory.GetCoreV3(source);
		var resource = await repository.GetResourceAsync<PackageUpdateResource>();
		try
		{
			await resource.Push(packagePaths: new[] { packageFile.FullName },
				symbolSource: null,
				timeoutInSecond: 5 * 60,
				disableBuffering: false,
				getApiKey: packageSource => apiKey,
				getSymbolApiKey: packageSource => null,
				noServiceEndpoint: false,
				skipDuplicate: false,
				symbolPackageUpdateResource: null,
				log: new LoggerWrapper(msLogger));
			return new NuGetCommandResult { Message = $"Successfully pushed {packageFile.Name} to {source}", Success = true };
		}
		catch(Exception ex)
		{
			return new NuGetCommandResult { Message = ex.Message, Success = false };
		}
    }

    public static async Task<NuGetCommandResult> Query(Microsoft.Extensions.Logging.ILogger logger, string packageId, string source = DefaultNuGetApiUrl, string apiKey = null)
    {
        var repository = Repository.Factory.GetCoreV3(source);
        var resource = await repository.GetResourceAsync<PackageMetadataResource>();
        try
        {
            var metadata = await resource.GetMetadataAsync(packageId, includePrerelease: true, includeUnlisted: true, sourceCacheContext: null, log: new LoggerWrapper(logger), token: CancellationToken.None);
            foreach(var md in metadata)
            {
                logger.LogInformation($"PackageId: {md.Identity.Id}, Version: {md.Identity.Version}, Description: {md.Description}, Authors: {string.Join(", ", md.Authors)}, Published: {md.Published}, LicenseUrl: {md.LicenseUrl}, ProjectUrl: {md.ProjectUrl}, Dependencies: {string.Join(", ", md.DependencySets.SelectMany(ds => ds.Packages.Select(p => $"{p.Id} {p.VersionRange}")))}");
            }
        }
        catch(Exception ex)
        {
            return new NuGetCommandResult { Message = ex.Message, Success = false };
        }
        return new NuGetCommandResult { Message = $"Successfully queried {packageId} from {source}", Success = true };
    }

	public static string ExtractPackageIdFromFilename(this string filename) =>
		NuGetPackageFilenameRegex.Match(filename).Groups["packageId"].Value;
	public static string ExtractPackageVersionFromFilename(this string filename) =>
		NuGetPackageFilenameRegex.Match(filename).Groups["version"].Value;

	public static async Task<NuGetCommandResult> Delete(Microsoft.Extensions.Logging.ILogger logger, FileInfo packageFile, string source = DefaultNuGetApiUrl, string apiKey = null)
    {
		var packageId = packageFile.Name.ExtractPackageIdFromFilename();
		var packageVersion = packageFile.Name.ExtractPackageVersionFromFilename();
		var repository = Repository.Factory.GetCoreV3(source);
		var resource = await repository.GetResourceAsync<PackageUpdateResource>();
		try
		{
			await resource.Delete(
				packageId,
				packageVersion,
				getApiKey: packageSource => apiKey,
				confirm: packageSource => true,
				noServiceEndpoint: false,
				log: new LoggerWrapper(logger));
			return new NuGetCommandResult { Message = $"Successfully deleted {packageFile.Name} from {source}", Success = true };
		}
		catch(Exception ex)
		{
			return new NuGetCommandResult { Message = ex.Message, Success = false };
		}
    }
}
