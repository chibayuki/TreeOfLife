/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.209.1000.M3.201119-1900

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
    internal class PhylogeneticTree
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
            if (atoms is null || taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.Children.Count > 0)
            {
                for (int i = 0; i < taxon.Children.Count; i++)
                {
                    atoms.Add(PhylogeneticUnwindV1Atom.FromTaxon(taxon.Children[i]));

                    _RecursiveFillAtoms(atoms, taxon.Children[i]);
                }
            }
        }

        public PhylogeneticUnwindV1 Unwind()
        {
            PhylogeneticUnwindV1 unwindObject = new PhylogeneticUnwindV1();

            _RecursiveFillAtoms(unwindObject.Atoms, _Root);

            return unwindObject;
        }
    }
}