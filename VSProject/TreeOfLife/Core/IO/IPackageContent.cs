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

namespace TreeOfLife.Core.IO
{
    // 文件封装内容行为。
    public interface IPackageContent
    {
        PackageVersion Version { get; } // 文件封装版本。

        void TranslateFrom(object data); // 从内存数据转换。
        void TranslateTo(object data); // 转换到内存数据。

        void Serialize(string directory); // 序列化文件到指定目录。
        void Deserialize(string directory); // 从指定目录反序列化文件。
    }
}