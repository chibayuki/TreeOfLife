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

namespace TreeOfLife.Core.Packaging
{
    // 包内容行为。
    public interface IPackageContent
    {
        PackageVersion Version { get; } // 版本。

        void TranslateFrom(object data); // 从内存数据转换。
        void TranslateTo(object data); // 转换到内存数据。

        void Serialize(string directory); // 序列化文件到指定目录。
        void Deserialize(string directory); // 从指定目录反序列化文件。
    }
}