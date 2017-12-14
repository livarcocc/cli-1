using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.DotNet.Cli.Utils
{
    internal class MSBuildArguments
    {
        private static readonly IEnumerable<string> _msbuildRequiredParameters =
            new List<string> { "/m", "/v:m" };

        public static IEnumerable<string> PrepareArgumentsForMSBuild(IEnumerable<string> args)
        {
            return _msbuildRequiredParameters.Concat(args.Select(Escape));
        }

        private static string Escape(string arg) =>
             // this is a workaround for https://github.com/Microsoft/msbuild/issues/1622
             (arg.StartsWith("/p:RestoreSources=", StringComparison.OrdinalIgnoreCase)) ?
                arg.Replace(";", "%3B")
                   .Replace("://", ":%2F%2F") :
                arg;
    }
}
