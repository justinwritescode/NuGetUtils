/*
 * Program.cs
 *
 *   Created: 2022-12-02-09:05:37
 *   Modified: 2022-12-02-09:05:37
 *
 *   Author: Justin Chase <justin@justinwritescode.com>
 *
 *   Copyright © 2022 Justin Chase, All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using JustinWritesCode.NuGetUtils;
using NuGetUtils.Configuration;

namespace NuGetUtils;

public static class TestNuGetUtils
{
    public static void Main(string[] args)
    {
        var nugetConfig = NuGetConfiguration.GetNuGetConfiguration(Directory.GetCurrentDirectory());
    }
}
