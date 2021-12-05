/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1322.1000.M13.210925-1400

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

// 版本4：
// 使用第二版Core.Taxonomy.Rank枚举，删除部分从未使用的生物分类阶元；
// 修改部分Json属性名，ChsName-->ChName，Unsure-->Undet。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Core.IO.Version4.Details;

namespace TreeOfLife.Core.IO.Version4
{
    // 文件封装内容。
    public sealed class PackageContent : IPackageContent
    {
        private static readonly PackageVersion _Version = new PackageVersion(4);

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