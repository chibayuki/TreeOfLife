/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1132.1000.M11.210516-1800

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Packaging
{
    // 包内容行为。
    public interface IPackageContent
    {
        PackageVersion Version { get; }

        void TranslateFrom(object data);
        void TranslateTo(object data);

        void Serialize(string directory);
        void Deserialize(string directory);
    }
}