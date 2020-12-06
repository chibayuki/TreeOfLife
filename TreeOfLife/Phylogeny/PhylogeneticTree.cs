/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.415.1000.M5.201204-2200

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

        //

        private void _RecursiveFillAtoms(List<PhylogeneticUnwindV1Atom> atoms, Taxon taxon)
        {
            if (atoms == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                atoms.Add(PhylogeneticUnwindV1Atom.FromTaxon(child));

                _RecursiveFillAtoms(atoms, child);
            }
        }

        // 将系统发生树展开。
        public PhylogeneticUnwindV1 Unwind()
        {
            PhylogeneticUnwindV1 unwindObject = new PhylogeneticUnwindV1();

            _RecursiveFillAtoms(unwindObject.Atoms, _Root);

            return unwindObject;
        }
    }
}