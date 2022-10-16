namespace JustinWritesCode.NuGetUtils.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using global::JustinWritesCode.NuGet.Configuration;
    using static global::JustinWritesCode.NuGet.Configuration.ConfigurationConstants;

    /// <summary>
    /// Represents a NuGet configuration file
    /// </summary>
    public class NuGetConfiguration
    {
        /// <summary>
        /// Gets the NuGet configuration file
        /// </summary>
        /// <param name="projectDirectory">The project directory.</param>
        /// <returns>The NuGet configuration file</returns>
        public static NuGetConfiguration GetNuGetConfiguration(string projectDirectory)
        {
            var nugetConfig = new global::JustinWritesCode.NuGet.Configuration.Settings(projectDirectory);
            return new NuGetConfiguration(nugetConfig);
        }

        public static NuGetConfiguration GetMachineWideConfiguration()
        {
            var nugetConfig = Settings.LoadMachineWideSettings(Directory.GetDirectoryRoot(Environment.CurrentDirectory));
            return new NuGetConfiguration(nugetConfig);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetConfiguration"/> class.
        /// </summary>
        /// <param name="nugetConfigs">The NuGet configurations.</param>
        public NuGetConfiguration(params global::JustinWritesCode.NuGet.Configuration.ISettings[] nugetConfigs)
        {
            _ = nugetConfigs.Select(x => { NuGetConfigs.Add(x); return true; });
        }

        /// <summary>
        /// Gets the NuGet configuration.
        /// </summary>
        /// <value>
        /// The NuGet configuration.
        /// </value>
        public IList<global::JustinWritesCode.NuGet.Configuration.ISettings> NuGetConfigs { get; } = new List<global::JustinWritesCode.NuGet.Configuration.ISettings>();
    }
}
