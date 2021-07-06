/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1134.1000.M11.210518-2200

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Packaging.Version3.Details;
using TreeOfLife.Phylogeny;

namespace TreeOfLife.Packaging.Version3
{
    // 包内容。
    public sealed class PackageContent : IPackageContent
    {
        private static readonly PackageVersion _Version = new PackageVersion(3);

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