﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.DotNet.Tools.Clean;
using FluentAssertions;
using Xunit;
using System;
using System.Linq;

namespace Microsoft.DotNet.Cli.MSBuild.Tests
{
    public class GivenDotnetCleanInvocation
    {
        const string ExpectedPrefix = "/m /v:m /t:Clean";

        [Fact]
        public void ItAddsProjectToMsbuildInvocation()
        {
            var msbuildPath = "<msbuildpath>";
            string.Join(" ", CleanCommand.FromArgs(new string[] { "<project>" }, msbuildPath)
                .Arguments).Should().Be("/m /v:m <project> /t:Clean");
        }

        [Theory]
        [InlineData(new string[] { }, "")]
        [InlineData(new string[] { "-o", "<output>" }, "/p:OutputPath=<output>")]
        [InlineData(new string[] { "--output", "<output>" }, "/p:OutputPath=<output>")]
        [InlineData(new string[] { "-f", "<framework>" }, "/p:TargetFramework=<framework>")]
        [InlineData(new string[] { "--framework", "<framework>" }, "/p:TargetFramework=<framework>")]
        [InlineData(new string[] { "-c", "<configuration>" }, "/p:Configuration=<configuration>")]
        [InlineData(new string[] { "--configuration", "<configuration>" }, "/p:Configuration=<configuration>")]
        [InlineData(new string[] { "-v", "diag" }, "/verbosity:diag")]
        [InlineData(new string[] { "--verbosity", "diag" }, "/verbosity:diag")]
        public void MsbuildInvocationIsCorrect(string[] args, string expectedAdditionalArgs)
        {
            expectedAdditionalArgs = (string.IsNullOrEmpty(expectedAdditionalArgs) ? "" : $" {expectedAdditionalArgs}");

            var msbuildPath = "<msbuildpath>";
            string.Join(" ", CleanCommand.FromArgs(args, msbuildPath)
                .Arguments).Should().Be($"{ExpectedPrefix}{expectedAdditionalArgs}");
        }
    }
}
