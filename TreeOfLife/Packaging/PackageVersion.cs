/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.812.1000.M8.210108-2100

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace TreeOfLife.Packaging
{
    // 包版本。
    public struct PackageVersion
    {
        private const int _LatestVersion = 1; // 最新的版本。

        private int _Version; // 版本。

        //

        public PackageVersion(int fileVersion)
        {
            _Version = fileVersion;
        }

        //

        public static PackageVersion Latest => new PackageVersion(_LatestVersion);

        [JsonPropertyName("Version")]
        public int Version
        {
            get => _Version;
            set => _Version = value;
        }
    }
}