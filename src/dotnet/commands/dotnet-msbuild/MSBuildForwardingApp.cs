// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Build.CommandLine;
using Microsoft.DotNet.Cli.Telemetry;
using Microsoft.DotNet.Cli.Utils;

namespace Microsoft.DotNet.Tools.MSBuild
{
    public class MSBuildForwardingApp
    {
        internal const string TelemetrySessionIdEnvironmentVariableName = "DOTNET_CLI_TELEMETRY_SESSIONID";

        private readonly IEnumerable<string> _args;

        private static IEnumerable<string> ConcatTelemetryLogger(IEnumerable<string> argsToForward)
        {
            if (Telemetry.CurrentSessionId != null)
            {
                try
                {
                    Type loggerType = typeof(MSBuildLogger);

                    return argsToForward
                        .Concat(new[]
                        {
                            $"/Logger:{loggerType.FullName},{loggerType.GetTypeInfo().Assembly.Location}"
                        });
                }
                catch (Exception)
                {
                    // Exceptions during telemetry shouldn't cause anything else to fail
                }
            }
            return argsToForward;
        }

        public IEnumerable<string> Arguments
        {
            get
            {
                return MSBuildArguments.PrepareArgumentsForMSBuild(_args).ToArray();
            }
        }

        public MSBuildForwardingApp(IEnumerable<string> argsToForward, string msbuildPath = null)
        {
            _args = ConcatTelemetryLogger(argsToForward);
        }

        public virtual int Execute()
        {
            // prepare arguments
            var msBuildEnvironmentVariables = new MSBuildEnvironmentVariables();

            foreach(KeyValuePair<string, string> msbuildEnvironmentVariable in 
                msBuildEnvironmentVariables.MSBuildRequiredEnvironmentVariables)
            {
                Environment.SetEnvironmentVariable(msbuildEnvironmentVariable.Key, msbuildEnvironmentVariable.Value);
            }

            Environment.SetEnvironmentVariable(TelemetrySessionIdEnvironmentVariableName, Telemetry.CurrentSessionId);

            return MSBuildApp.Main(Arguments.ToArray());
        }
    }
}
