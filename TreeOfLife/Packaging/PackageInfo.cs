/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1240.1000.M12.210718-2000

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
    // 包信息。
    public struct PackageInfo
    {
        private DateTime _CreationTime; // 创建时间。
        private DateTime _ModificationTime; // 修改时间。

        //

        public PackageInfo(DateTime creationTime)
        {
            _CreationTime = creationTime;
            _ModificationTime = creationTime;
        }

        public PackageInfo(DateTime creationTime, DateTime modificationTime)
        {
            _CreationTime = creationTime;
            _ModificationTime = modificationTime;
        }

        //

        [JsonPropertyName("CreationTime")]
        public DateTime CreationTime
        {
            get => _CreationTime;
            set => _CreationTime = value;
        }

        [JsonPropertyName("ModificationTime")]
        public DateTime ModificationTime
        {
            get => _ModificationTime;
            set => _ModificationTime = value;
        }
    }
}