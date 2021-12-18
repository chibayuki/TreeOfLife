/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

// 版本2：
// 新增支持并系群与复系群。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Core.IO.Version2.Details;

namespace TreeOfLife.Core.IO.Version2
{
    // 文件封装内容。
    public sealed class PackageContent : IPackageContent
    {
        private static readonly PackageVersion _Version = new PackageVersion(2);

        private PhylogeneticTreeUnwind _Unwind = null;

        //

        public PackageContent()
        {
        }

        //

        public PackageVersion Version => _Version;

        //

        public void TranslateFrom(object data)
        {
            _Unwind = PhylogeneticTreeUnwind.UnwindFrom(data as PhylogeneticTree);
        }

        public void TranslateTo(object data)
        {
            _Unwind.RebuildTo(data as PhylogeneticTree);
        }

        public void Serialize(string directory)
        {
            _Unwind.Serialize(directory);
        }

        public void Deserialize(string directory)
        {
            _Unwind = PhylogeneticTreeUnwind.Deserialize(directory);
        }
    }
}