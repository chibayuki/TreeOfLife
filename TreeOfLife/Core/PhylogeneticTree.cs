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

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core
{
    // 系统发生树（演化树）。
    public sealed class PhylogeneticTree
    {
        private Taxon _Root; // 假设存在的顶级类群。

        //

        public PhylogeneticTree()
        {
            _Root = new Taxon() { Rank = Rank.Unranked };
        }

        //

        public Taxon Root => _Root;
    }
}