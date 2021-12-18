/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace TreeOfLife.Core.IO
{
    // 文件封装版本。
    public struct PackageVersion
    {
        private const int _LatestVersion = 4; // 最新的版本。

        private int _Version; // 版本。

        //

        public PackageVersion(int fileVersion)
        {
            _Version = fileVersion;
        }

        //

        public static readonly PackageVersion Latest = new PackageVersion(_LatestVersion);

        [JsonPropertyName("Version")]
        public int Version
        {
            get => _Version;
            set => _Version = value;
        }

        [JsonIgnore]
        public bool IsLatest => _Version == _LatestVersion;

#if DEBUG
        public override string ToString() => _Version.ToString();
#endif
    }
}