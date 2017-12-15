using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.DotNet.Cli.Utils
{
    internal class MSBuildEnvironmentVariables
    {
        private const string SdksDirectoryName = "Sdks";

        public readonly Dictionary<string, string> MSBuildRequiredEnvironmentVariables =
            new Dictionary<string, string>
            {
                { "MSBuildExtensionsPath", AppContext.BaseDirectory },
                { "MSBuildSDKsPath", GetMSBuildSDKsPath() },
                { "DOTNET_HOST_PATH", GetDotnetPath() },
                { "MSBUILDNOINPROCNODE", "1" }
            };

        private static string GetMSBuildSDKsPath()
        {
            var envMSBuildSDKsPath = Environment.GetEnvironmentVariable("MSBuildSDKsPath");

            if (envMSBuildSDKsPath != null)
            {
                return envMSBuildSDKsPath;
            }

            return Path.Combine(
                AppContext.BaseDirectory,
                SdksDirectoryName);
        }

        private static string GetDotnetPath()
        {
            return new Muxer().MuxerPath;
        }
    }
}
