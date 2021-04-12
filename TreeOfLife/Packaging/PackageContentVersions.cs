/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Packaging
{
    // 包内容版本。
    public static class PackageContentVersions
    {
        public static IPackageContent CreateFromVersion(PackageVersion version)
        {
            return version.Version switch
            {
                1 => new Version1.PackageContent(),
                2 => new Version2.PackageContent(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static IPackageContent CreateLatestVersion()
        {
            return CreateFromVersion(PackageVersion.Latest);
        }
    }
}