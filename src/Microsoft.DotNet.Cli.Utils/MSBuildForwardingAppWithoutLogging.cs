// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Microsoft.DotNet.Cli.Utils
{
    internal class MSBuildForwardingAppWithoutLogging
    {
        private const string MSBuildExeName = "MSBuild.dll";

        private const string SdksDirectoryName = "Sdks";

        private readonly ForwardingAppImplementation _forwardingApp;

        public MSBuildForwardingAppWithoutLogging(IEnumerable<string> argsToForward, string msbuildPath = null)
        {
            _forwardingApp = new ForwardingAppImplementation(
                msbuildPath ?? GetMSBuildExePath(),
                MSBuildArguments.PrepareArgumentsForMSBuild(argsToForward),
                environmentVariables: new MSBuildEnvironmentVariables().MSBuildRequiredEnvironmentVariables);
        }

        public virtual ProcessStartInfo GetProcessStartInfo()
        {
            return _forwardingApp
                .GetProcessStartInfo();
        }

        public int Execute()
        {
            return GetProcessStartInfo().Execute();
        }

        private static string GetMSBuildExePath()
        {
            return Path.Combine(
                AppContext.BaseDirectory,
                MSBuildExeName);
        }
    }
}

