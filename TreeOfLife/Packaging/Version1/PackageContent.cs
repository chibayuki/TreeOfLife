/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.617.1000.M6.201226-1000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Packaging.Version1.Details;
using TreeOfLife.Phylogeny;

namespace TreeOfLife.Packaging.Version1
{
    // 包内容。
    public class PackageContent : IPackageContent
    {
        private PackageVersion _Version = new PackageVersion(1);

        PhylogeneticTreeUnwind _EvoTreeUnwind = null;

        //

        public PackageContent()
        {
        }

        //

        public PackageVersion Version => _Version;

        //

        public void TranslateFrom(object data)
        {
            _EvoTreeUnwind = PhylogeneticTreeUnwind.UnwindFrom(data as PhylogeneticTree);
        }

        public void TranslateTo(object data)
        {
            _EvoTreeUnwind.RebuildTo(data as PhylogeneticTree);
        }

        public void Serialize(string directory)
        {
            _EvoTreeUnwind.Serialize(directory);
        }

        public void Deserialize(string directory)
        {
            _EvoTreeUnwind = PhylogeneticTreeUnwind.Deserialize(directory);
        }
    }
}