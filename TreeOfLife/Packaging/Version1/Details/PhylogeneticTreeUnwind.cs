/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.800.1000.M7.201231-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using TreeOfLife.Phylogeny;
using TreeOfLife.Taxonomy;

namespace TreeOfLife.Packaging.Version1.Details
{
    // 系统发生树的展开。
    public class PhylogeneticTreeUnwind
    {
        private List<PhylogeneticTreeUnwindAtom> _Atoms = new List<PhylogeneticTreeUnwindAtom>();

        //

        public PhylogeneticTreeUnwind()
        {
        }

        //

        [JsonPropertyName("Taxons")]
        public List<PhylogeneticTreeUnwindAtom> Atoms
        {
            get => _Atoms;
            set => _Atoms = value;
        }

        //

        private static void _RecursiveFillAtoms(List<PhylogeneticTreeUnwindAtom> atoms, Taxon taxon)
        {
            if (atoms == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                atoms.Add(PhylogeneticTreeUnwindAtom.FromTaxon(child));

                _RecursiveFillAtoms(atoms, child);
            }
        }

        // 将系统发生树展开。
        public static PhylogeneticTreeUnwind UnwindFrom(PhylogeneticTree tree)
        {
            PhylogeneticTreeUnwind unwindObject = new PhylogeneticTreeUnwind();

            _RecursiveFillAtoms(unwindObject.Atoms, tree.Root);

            return unwindObject;
        }

        private static Taxon _GetTaxonOfTree(PhylogeneticTree tree, IReadOnlyList<int> index)
        {
            if (tree == null || index == null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon taxon = tree.Root;

            foreach (var id in index)
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
            var atoms = _Atoms.OrderBy(atom => atom.Level).ThenBy(atom => atom.Index);

            foreach (PhylogeneticTreeUnwindAtom atom in atoms)
            {
                atom.ToTaxon().SetParent(_GetTaxonOfTree(tree, atom.ParentsIndex));
            }
        }

        // 数据文件。
        private static string _GetDataFileName(string directory)
        {
            return Path.Combine(directory, "_evo");
        }

        // 序列化。
        public void Serialize(string directory)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            string jsonText = JsonSerializer.Serialize(this, options);

            File.WriteAllText(_GetDataFileName(directory), jsonText);
        }

        // 反序列化。
        public static PhylogeneticTreeUnwind Deserialize(string directory)
        {
            string jsonText = File.ReadAllText(_GetDataFileName(directory));

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            return JsonSerializer.Deserialize<PhylogeneticTreeUnwind>(jsonText, options);
        }
    }
}