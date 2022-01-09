/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.IO.Version4.Details
{
    // 系统发生树的线性展开。
    public sealed class PhylogeneticTreeUnwind
    {
        private Evo _Evo = new Evo();
        private Ref _Ref = new Ref();

        //

        public PhylogeneticTreeUnwind()
        {
        }

        //

        private static void _RecursiveFillAtoms(Taxon taxon, List<EvoAtom> evoAtoms, List<RefAtom> refAtoms)
        {
            if (taxon is null || evoAtoms is null || refAtoms is null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                evoAtoms.Add(EvoAtom.FromTaxon(child));

                if (child.Excludes.Count > 0 || child.Includes.Count > 0)
                {
                    refAtoms.Add(RefAtom.FromTaxon(child));
                }

                _RecursiveFillAtoms(child, evoAtoms, refAtoms);
            }
        }

        // 将系统发生树展开。
        public static PhylogeneticTreeUnwind UnwindFrom(PhylogeneticTree tree)
        {
            PhylogeneticTreeUnwind unwind = new PhylogeneticTreeUnwind();

            _RecursiveFillAtoms(tree.Root, unwind._Evo.Atoms, unwind._Ref.Atoms);

            return unwind;
        }

        private static Taxon _GetTaxonOfTree(PhylogeneticTree tree, IReadOnlyList<int> indexList)
        {
            if (tree is null || indexList is null)
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
            // 先按照层次升序排序，用于确保总是先创建父类群
            // 再按照索引升序排序，用于确保每个类群的次序与原先相同
            var evoAtoms = from atom in _Evo.Atoms
                           where atom is not null
                           orderby atom.Level ascending,
                           atom.Index ascending
                           select atom;

            // 重建单系群关系
            foreach (var evoAtom in evoAtoms)
            {
                evoAtom.ToTaxon().SetParent(_GetTaxonOfTree(tree, evoAtom.ParentsIndex));
            }

            // 重建并系群、复系群关系
            foreach (var refAtom in _Ref.Atoms)
            {
                Common.IdStringToIndexList(refAtom.ID, out List<int> indexList);

                Taxon taxon = _GetTaxonOfTree(tree, indexList);

                foreach (var exclude in refAtom.Excludes)
                {
                    Common.IdStringToIndexList(exclude, out List<int> refIndexList);

                    taxon.AddExclude(_GetTaxonOfTree(tree, refIndexList));
                }

                foreach (var include in refAtom.Includes)
                {
                    Common.IdStringToIndexList(include, out List<int> refIndexList);

                    taxon.AddInclude(_GetTaxonOfTree(tree, refIndexList));
                }
            }
        }

        // Evo数据文件。
        private static string _GetEvoFileName(string directory)
        {
            return Path.Combine(directory, "_evo");
        }

        // Ref数据文件。
        private static string _GetRefFileName(string directory)
        {
            return Path.Combine(directory, "_ref");
        }

        // 序列化。
        public void Serialize(string directory)
        {
            _Evo.Serialize(_GetEvoFileName(directory));
            _Ref.Serialize(_GetRefFileName(directory));
        }

        // 反序列化。
        public static PhylogeneticTreeUnwind Deserialize(string directory)
        {
            return new PhylogeneticTreeUnwind()
            {
                _Evo = Evo.Deserialize(_GetEvoFileName(directory)),
                _Ref = Ref.Deserialize(_GetRefFileName(directory))
            };
        }
    }
}