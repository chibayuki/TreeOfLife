/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

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
        public PhylogeneticTree()
        {
        }

        //

        public Taxon Root { get; } = new Taxon(); // 假设存在的顶级类群。
    }
}