/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.900.1000.M9.210112-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TreeOfLife.Taxonomy;

namespace TreeOfLife.Phylogeny
{
    // 系统发生树。
    public class PhylogeneticTree
    {
        private Taxon _Root; // 假设存在的顶级类群。

        //

        public PhylogeneticTree()
        {
            _Root = new Taxon() { Category = TaxonomicCategory.Unranked };
        }

        //

        public Taxon Root => _Root;
    }
}