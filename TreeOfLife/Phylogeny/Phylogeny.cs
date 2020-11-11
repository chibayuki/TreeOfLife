/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.112.1000.M2.201110-2050

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife
{
    // 系统发生树。
    internal static class PhylogeneticTree
    {
        private static Taxon _Root = new Taxon() { Category = TaxonomicCategory.Unranked }; // 假设作为的系统发生树的根的生物分类单元。

        public static Taxon Root => _Root;
    }
}