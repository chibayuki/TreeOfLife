﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Core.Packaging.Version1.Details;
using TreeOfLife.Core.Phylogeny;

namespace TreeOfLife.Core.Packaging.Version1
{
    // 包内容。
    public sealed class PackageContent : IPackageContent
    {
        private static readonly PackageVersion _Version = new PackageVersion(1);

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