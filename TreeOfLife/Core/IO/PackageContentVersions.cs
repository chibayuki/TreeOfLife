/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Core.IO
{
    // 用于创建指定文件封装版本的文件封装内容对象。
    public static class PackageContentVersions
    {
        public static IPackageContent CreateFromVersion(PackageVersion version)
        {
            return version.Version switch
            {
                1 => new Version1.PackageContent(),
                2 => new Version2.PackageContent(),
                3 => new Version3.PackageContent(),
                4 => new Version4.PackageContent(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static IPackageContent CreateLatestVersion() => CreateFromVersion(PackageVersion.Latest);
    }
}