/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using TreeOfLife.Phylogeny;
using TreeOfLife.Taxonomy;

namespace TreeOfLife.Packaging.Version1.Details
{
    // 系统发生树的展开。
    public class PhylogeneticTreeUnwind
    {
        private Evo _Evo = new Evo();

        //

        public PhylogeneticTreeUnwind()
        {
        }

        //

        private static void _RecursiveFillAtoms(Taxon taxon, List<EvoAtom> evoAtoms)
        {
            if (taxon == null || evoAtoms == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                evoAtoms.Add(EvoAtom.FromTaxon(child));

                _RecursiveFillAtoms(child, evoAtoms);
            }
        }

        // 将系统发生树展开。
        public static PhylogeneticTreeUnwind UnwindFrom(PhylogeneticTree tree)
        {
            PhylogeneticTreeUnwind unwind = new PhylogeneticTreeUnwind();

            _RecursiveFillAtoms(tree.Root, unwind._Evo.Atoms);

            return unwind;
        }

        private static Taxon _GetTaxonOfTree(PhylogeneticTree tree, IReadOnlyList<int> indexList)
        {
            if (tree == null || indexList == null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon taxon = tree.Root;

            foreach (var id in indexList)
            {
                taxon = taxon.Children[id];
            }

            return taxon;
        }

        // 重建系统发生树。
        public void RebuildTo(PhylogeneticTree tree)
        {
            // 先按照层次升序排序，用于保证总是先创建父类群
            // 再按照索引升序排序，用于保证每个类群的次序与原先相同
            var evoAtoms = from atom in _Evo.Atoms
                           where atom != null
                           orderby atom.Level ascending,
                           atom.Index ascending
                           select atom;

            foreach (var evoAtom in evoAtoms)
            {
                evoAtom.ToTaxon().SetParent(_GetTaxonOfTree(tree, evoAtom.ParentsIndex));
            }
        }

        // Evo数据文件。
        private static string _GetEvoFileName(string directory)
        {
            return Path.Combine(directory, "_evo");
        }

        // 序列化。
        public void Serialize(string directory)
        {
            _Evo.Serialize(_GetEvoFileName(directory));
        }

        // 反序列化。
        public static PhylogeneticTreeUnwind Deserialize(string directory)
        {
            return new PhylogeneticTreeUnwind()
            {
                _Evo = Evo.Deserialize(_GetEvoFileName(directory))
            };
        }
    }
}