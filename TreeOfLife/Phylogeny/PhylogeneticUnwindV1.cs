/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.305.1000.M4.201120-0000

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

namespace TreeOfLife
{
    // 系统发生树展开后的原子数据结构。
    internal class PhylogeneticUnwindV1Atom
    {
        // 生物分类阶元。
        public enum TaxonomicCategory
        {
            Unranked = 0, // 未分级。

            Clade = 1, // 演化支。

            // 次要分类阶元：

            Strain = 2, // 株。

            Subform = 4, // 亚型。
            Form, // 型。

            Subseries = 8, // 亚系。
            Series, // 系。

            Infradivision = 16, // 下类。
            Subdivision, // 亚类。
            Division, // 类。
            Superdivision, // 总类。

            Infrasection = 32, // 下派。
            Subsection, // 亚派/亚组/亚节。
            Section, // 派/组/节。
            Supersection, // 总派。

            Infracohort = 64, // 下群。
            Subcohort, // 亚群。
            Cohort, // 群。
            Supercohort, // 总群。
            Megacohort, // 高群。

            Infratribe = 128, // 下族。
            Subtribe, // 亚族。
            Tribe, // 族。
            Supertribe, // 总族/超族。

            // 主要分类阶元：

            Subvariety = 256, // 亚变种。
            Variety, // 变种。
            Subspecies, // 亚种。
            Species, // 种。
            Superspecies, // 种团。

            Infragenus = 512, // 下属。
            Subgenus, // 亚属。
            Genus, // 属。

            Infrafamily = 1024, // 下科。
            Subfamily, // 亚科。
            Family, // 科。
            Epifamily, // 领科。
            Hyperfamily, // 上科。
            Grandfamily, // 大科。
            Superfamily, // 总科/超科。
            Megafamily, // 高科。
            Gigafamily, // 宏科。

            Parvorder = 2048, // 小目。
            Infraorder, // 下目。
            Suborder, // 亚目。
            Minorder, // 若目。
            Hypoorder, // 次目。
            Nanorder, // 从目。
            Order, // 目。
            Hyperorder, // 上目。
            Grandorder, // 大目。
            Superorder, // 总目/超目。
            Megaorder, // 高目。
            Gigaorder, // 宏目。

            Parvclass = 4096, // 小纲。
            Infraclass, // 下纲。
            Subclass, // 亚纲。
            Class, // 纲。
            Hyperclass, // 上纲。
            Grandclass, // 大纲。
            Superclass, // 总纲/超纲。
            Megaclass, // 高纲。

            Parvphylum = 8192, // 小门。
            Infraphylum, // 下门。
            Subphylum, // 亚门。
            Phylum, // 门。
            Superphylum, // 总门/超门。

            Infrakingdom = 16384, // 下界。
            Subkingdom, // 亚界。
            Kingdom, // 界。
            Superkingdom, // 总界。

            Domain = 32768, // 域。
            Superdomain // 总域。
        }

        private static TaxonomicCategory _ConvertCategory(TreeOfLife.TaxonomicCategory category)
        {
            switch (category)
            {
                case TreeOfLife.TaxonomicCategory.Unranked: return TaxonomicCategory.Unranked;
                case TreeOfLife.TaxonomicCategory.Clade: return TaxonomicCategory.Clade;
                case TreeOfLife.TaxonomicCategory.Strain: return TaxonomicCategory.Strain;
                case TreeOfLife.TaxonomicCategory.Subform: return TaxonomicCategory.Subform;
                case TreeOfLife.TaxonomicCategory.Form: return TaxonomicCategory.Form;
                case TreeOfLife.TaxonomicCategory.Subseries: return TaxonomicCategory.Subseries;
                case TreeOfLife.TaxonomicCategory.Series: return TaxonomicCategory.Series;
                case TreeOfLife.TaxonomicCategory.Infradivision: return TaxonomicCategory.Infradivision;
                case TreeOfLife.TaxonomicCategory.Subdivision: return TaxonomicCategory.Subdivision;
                case TreeOfLife.TaxonomicCategory.Division: return TaxonomicCategory.Division;
                case TreeOfLife.TaxonomicCategory.Superdivision: return TaxonomicCategory.Superdivision;
                case TreeOfLife.TaxonomicCategory.Infrasection: return TaxonomicCategory.Infrasection;
                case TreeOfLife.TaxonomicCategory.Subsection: return TaxonomicCategory.Subsection;
                case TreeOfLife.TaxonomicCategory.Section: return TaxonomicCategory.Section;
                case TreeOfLife.TaxonomicCategory.Supersection: return TaxonomicCategory.Supersection;
                case TreeOfLife.TaxonomicCategory.Infracohort: return TaxonomicCategory.Infracohort;
                case TreeOfLife.TaxonomicCategory.Subcohort: return TaxonomicCategory.Subcohort;
                case TreeOfLife.TaxonomicCategory.Cohort: return TaxonomicCategory.Cohort;
                case TreeOfLife.TaxonomicCategory.Supercohort: return TaxonomicCategory.Supercohort;
                case TreeOfLife.TaxonomicCategory.Megacohort: return TaxonomicCategory.Megacohort;
                case TreeOfLife.TaxonomicCategory.Infratribe: return TaxonomicCategory.Infratribe;
                case TreeOfLife.TaxonomicCategory.Subtribe: return TaxonomicCategory.Subtribe;
                case TreeOfLife.TaxonomicCategory.Tribe: return TaxonomicCategory.Tribe;
                case TreeOfLife.TaxonomicCategory.Supertribe: return TaxonomicCategory.Supertribe;
                case TreeOfLife.TaxonomicCategory.Subvariety: return TaxonomicCategory.Subvariety;
                case TreeOfLife.TaxonomicCategory.Variety: return TaxonomicCategory.Variety;
                case TreeOfLife.TaxonomicCategory.Subspecies: return TaxonomicCategory.Subspecies;
                case TreeOfLife.TaxonomicCategory.Species: return TaxonomicCategory.Species;
                case TreeOfLife.TaxonomicCategory.Superspecies: return TaxonomicCategory.Superspecies;
                case TreeOfLife.TaxonomicCategory.Infragenus: return TaxonomicCategory.Infragenus;
                case TreeOfLife.TaxonomicCategory.Subgenus: return TaxonomicCategory.Subgenus;
                case TreeOfLife.TaxonomicCategory.Genus: return TaxonomicCategory.Genus;
                case TreeOfLife.TaxonomicCategory.Infrafamily: return TaxonomicCategory.Infrafamily;
                case TreeOfLife.TaxonomicCategory.Subfamily: return TaxonomicCategory.Subfamily;
                case TreeOfLife.TaxonomicCategory.Family: return TaxonomicCategory.Family;
                case TreeOfLife.TaxonomicCategory.Epifamily: return TaxonomicCategory.Epifamily;
                case TreeOfLife.TaxonomicCategory.Hyperfamily: return TaxonomicCategory.Hyperfamily;
                case TreeOfLife.TaxonomicCategory.Grandfamily: return TaxonomicCategory.Grandfamily;
                case TreeOfLife.TaxonomicCategory.Superfamily: return TaxonomicCategory.Superfamily;
                case TreeOfLife.TaxonomicCategory.Megafamily: return TaxonomicCategory.Megafamily;
                case TreeOfLife.TaxonomicCategory.Gigafamily: return TaxonomicCategory.Gigafamily;
                case TreeOfLife.TaxonomicCategory.Parvorder: return TaxonomicCategory.Parvorder;
                case TreeOfLife.TaxonomicCategory.Infraorder: return TaxonomicCategory.Infraorder;
                case TreeOfLife.TaxonomicCategory.Suborder: return TaxonomicCategory.Suborder;
                case TreeOfLife.TaxonomicCategory.Minorder: return TaxonomicCategory.Minorder;
                case TreeOfLife.TaxonomicCategory.Hypoorder: return TaxonomicCategory.Hypoorder;
                case TreeOfLife.TaxonomicCategory.Nanorder: return TaxonomicCategory.Nanorder;
                case TreeOfLife.TaxonomicCategory.Order: return TaxonomicCategory.Order;
                case TreeOfLife.TaxonomicCategory.Hyperorder: return TaxonomicCategory.Hyperorder;
                case TreeOfLife.TaxonomicCategory.Grandorder: return TaxonomicCategory.Grandorder;
                case TreeOfLife.TaxonomicCategory.Superorder: return TaxonomicCategory.Superorder;
                case TreeOfLife.TaxonomicCategory.Megaorder: return TaxonomicCategory.Megaorder;
                case TreeOfLife.TaxonomicCategory.Gigaorder: return TaxonomicCategory.Gigaorder;
                case TreeOfLife.TaxonomicCategory.Parvclass: return TaxonomicCategory.Parvclass;
                case TreeOfLife.TaxonomicCategory.Infraclass: return TaxonomicCategory.Infraclass;
                case TreeOfLife.TaxonomicCategory.Subclass: return TaxonomicCategory.Subclass;
                case TreeOfLife.TaxonomicCategory.Class: return TaxonomicCategory.Class;
                case TreeOfLife.TaxonomicCategory.Hyperclass: return TaxonomicCategory.Hyperclass;
                case TreeOfLife.TaxonomicCategory.Grandclass: return TaxonomicCategory.Grandclass;
                case TreeOfLife.TaxonomicCategory.Superclass: return TaxonomicCategory.Superclass;
                case TreeOfLife.TaxonomicCategory.Megaclass: return TaxonomicCategory.Megaclass;
                case TreeOfLife.TaxonomicCategory.Parvphylum: return TaxonomicCategory.Parvphylum;
                case TreeOfLife.TaxonomicCategory.Infraphylum: return TaxonomicCategory.Infraphylum;
                case TreeOfLife.TaxonomicCategory.Subphylum: return TaxonomicCategory.Subphylum;
                case TreeOfLife.TaxonomicCategory.Phylum: return TaxonomicCategory.Phylum;
                case TreeOfLife.TaxonomicCategory.Superphylum: return TaxonomicCategory.Superphylum;
                case TreeOfLife.TaxonomicCategory.Infrakingdom: return TaxonomicCategory.Infrakingdom;
                case TreeOfLife.TaxonomicCategory.Subkingdom: return TaxonomicCategory.Subkingdom;
                case TreeOfLife.TaxonomicCategory.Kingdom: return TaxonomicCategory.Kingdom;
                case TreeOfLife.TaxonomicCategory.Superkingdom: return TaxonomicCategory.Superkingdom;
                case TreeOfLife.TaxonomicCategory.Domain: return TaxonomicCategory.Domain;
                case TreeOfLife.TaxonomicCategory.Superdomain: return TaxonomicCategory.Superdomain;
                default: return TaxonomicCategory.Unranked;
            }
        }

        private static TreeOfLife.TaxonomicCategory _ConvertCategory(TaxonomicCategory category)
        {
            switch (category)
            {
                case TaxonomicCategory.Unranked: return TreeOfLife.TaxonomicCategory.Unranked;
                case TaxonomicCategory.Clade: return TreeOfLife.TaxonomicCategory.Clade;
                case TaxonomicCategory.Strain: return TreeOfLife.TaxonomicCategory.Strain;
                case TaxonomicCategory.Subform: return TreeOfLife.TaxonomicCategory.Subform;
                case TaxonomicCategory.Form: return TreeOfLife.TaxonomicCategory.Form;
                case TaxonomicCategory.Subseries: return TreeOfLife.TaxonomicCategory.Subseries;
                case TaxonomicCategory.Series: return TreeOfLife.TaxonomicCategory.Series;
                case TaxonomicCategory.Infradivision: return TreeOfLife.TaxonomicCategory.Infradivision;
                case TaxonomicCategory.Subdivision: return TreeOfLife.TaxonomicCategory.Subdivision;
                case TaxonomicCategory.Division: return TreeOfLife.TaxonomicCategory.Division;
                case TaxonomicCategory.Superdivision: return TreeOfLife.TaxonomicCategory.Superdivision;
                case TaxonomicCategory.Infrasection: return TreeOfLife.TaxonomicCategory.Infrasection;
                case TaxonomicCategory.Subsection: return TreeOfLife.TaxonomicCategory.Subsection;
                case TaxonomicCategory.Section: return TreeOfLife.TaxonomicCategory.Section;
                case TaxonomicCategory.Supersection: return TreeOfLife.TaxonomicCategory.Supersection;
                case TaxonomicCategory.Infracohort: return TreeOfLife.TaxonomicCategory.Infracohort;
                case TaxonomicCategory.Subcohort: return TreeOfLife.TaxonomicCategory.Subcohort;
                case TaxonomicCategory.Cohort: return TreeOfLife.TaxonomicCategory.Cohort;
                case TaxonomicCategory.Supercohort: return TreeOfLife.TaxonomicCategory.Supercohort;
                case TaxonomicCategory.Megacohort: return TreeOfLife.TaxonomicCategory.Megacohort;
                case TaxonomicCategory.Infratribe: return TreeOfLife.TaxonomicCategory.Infratribe;
                case TaxonomicCategory.Subtribe: return TreeOfLife.TaxonomicCategory.Subtribe;
                case TaxonomicCategory.Tribe: return TreeOfLife.TaxonomicCategory.Tribe;
                case TaxonomicCategory.Supertribe: return TreeOfLife.TaxonomicCategory.Supertribe;
                case TaxonomicCategory.Subvariety: return TreeOfLife.TaxonomicCategory.Subvariety;
                case TaxonomicCategory.Variety: return TreeOfLife.TaxonomicCategory.Variety;
                case TaxonomicCategory.Subspecies: return TreeOfLife.TaxonomicCategory.Subspecies;
                case TaxonomicCategory.Species: return TreeOfLife.TaxonomicCategory.Species;
                case TaxonomicCategory.Superspecies: return TreeOfLife.TaxonomicCategory.Superspecies;
                case TaxonomicCategory.Infragenus: return TreeOfLife.TaxonomicCategory.Infragenus;
                case TaxonomicCategory.Subgenus: return TreeOfLife.TaxonomicCategory.Subgenus;
                case TaxonomicCategory.Genus: return TreeOfLife.TaxonomicCategory.Genus;
                case TaxonomicCategory.Infrafamily: return TreeOfLife.TaxonomicCategory.Infrafamily;
                case TaxonomicCategory.Subfamily: return TreeOfLife.TaxonomicCategory.Subfamily;
                case TaxonomicCategory.Family: return TreeOfLife.TaxonomicCategory.Family;
                case TaxonomicCategory.Epifamily: return TreeOfLife.TaxonomicCategory.Epifamily;
                case TaxonomicCategory.Hyperfamily: return TreeOfLife.TaxonomicCategory.Hyperfamily;
                case TaxonomicCategory.Grandfamily: return TreeOfLife.TaxonomicCategory.Grandfamily;
                case TaxonomicCategory.Superfamily: return TreeOfLife.TaxonomicCategory.Superfamily;
                case TaxonomicCategory.Megafamily: return TreeOfLife.TaxonomicCategory.Megafamily;
                case TaxonomicCategory.Gigafamily: return TreeOfLife.TaxonomicCategory.Gigafamily;
                case TaxonomicCategory.Parvorder: return TreeOfLife.TaxonomicCategory.Parvorder;
                case TaxonomicCategory.Infraorder: return TreeOfLife.TaxonomicCategory.Infraorder;
                case TaxonomicCategory.Suborder: return TreeOfLife.TaxonomicCategory.Suborder;
                case TaxonomicCategory.Minorder: return TreeOfLife.TaxonomicCategory.Minorder;
                case TaxonomicCategory.Hypoorder: return TreeOfLife.TaxonomicCategory.Hypoorder;
                case TaxonomicCategory.Nanorder: return TreeOfLife.TaxonomicCategory.Nanorder;
                case TaxonomicCategory.Order: return TreeOfLife.TaxonomicCategory.Order;
                case TaxonomicCategory.Hyperorder: return TreeOfLife.TaxonomicCategory.Hyperorder;
                case TaxonomicCategory.Grandorder: return TreeOfLife.TaxonomicCategory.Grandorder;
                case TaxonomicCategory.Superorder: return TreeOfLife.TaxonomicCategory.Superorder;
                case TaxonomicCategory.Megaorder: return TreeOfLife.TaxonomicCategory.Megaorder;
                case TaxonomicCategory.Gigaorder: return TreeOfLife.TaxonomicCategory.Gigaorder;
                case TaxonomicCategory.Parvclass: return TreeOfLife.TaxonomicCategory.Parvclass;
                case TaxonomicCategory.Infraclass: return TreeOfLife.TaxonomicCategory.Infraclass;
                case TaxonomicCategory.Subclass: return TreeOfLife.TaxonomicCategory.Subclass;
                case TaxonomicCategory.Class: return TreeOfLife.TaxonomicCategory.Class;
                case TaxonomicCategory.Hyperclass: return TreeOfLife.TaxonomicCategory.Hyperclass;
                case TaxonomicCategory.Grandclass: return TreeOfLife.TaxonomicCategory.Grandclass;
                case TaxonomicCategory.Superclass: return TreeOfLife.TaxonomicCategory.Superclass;
                case TaxonomicCategory.Megaclass: return TreeOfLife.TaxonomicCategory.Megaclass;
                case TaxonomicCategory.Parvphylum: return TreeOfLife.TaxonomicCategory.Parvphylum;
                case TaxonomicCategory.Infraphylum: return TreeOfLife.TaxonomicCategory.Infraphylum;
                case TaxonomicCategory.Subphylum: return TreeOfLife.TaxonomicCategory.Subphylum;
                case TaxonomicCategory.Phylum: return TreeOfLife.TaxonomicCategory.Phylum;
                case TaxonomicCategory.Superphylum: return TreeOfLife.TaxonomicCategory.Superphylum;
                case TaxonomicCategory.Infrakingdom: return TreeOfLife.TaxonomicCategory.Infrakingdom;
                case TaxonomicCategory.Subkingdom: return TreeOfLife.TaxonomicCategory.Subkingdom;
                case TaxonomicCategory.Kingdom: return TreeOfLife.TaxonomicCategory.Kingdom;
                case TaxonomicCategory.Superkingdom: return TreeOfLife.TaxonomicCategory.Superkingdom;
                case TaxonomicCategory.Domain: return TreeOfLife.TaxonomicCategory.Domain;
                case TaxonomicCategory.Superdomain: return TreeOfLife.TaxonomicCategory.Superdomain;
                default: return TreeOfLife.TaxonomicCategory.Unranked;
            }
        }

        //

        private string _BotanicalName; // 学名。
        private string _ChineseName; // 中文名。
        private List<string> _Synonym; // 异名、别名、旧名等。
        private List<string> _Tag; // 标签。
        private string _Description; // 描述。

        private TaxonomicCategory _Category; // 分类阶元。

        private int _IsExtinct; // 已灭绝。
        private int _InDoubt; // 分类地位存疑。

        private List<int> _ParentsIndex = new List<int>();
        private int _Index = -1; // 当前类群在姊妹类群中的次序。

        //

        public PhylogeneticUnwindV1Atom()
        {
        }

        //

        [JsonPropertyName("Name")]
        public string BotanicalName
        {
            get => _BotanicalName;
            set => _BotanicalName = value;
        }

        [JsonPropertyName("ChsName")]
        public string ChineseName
        {
            get => _ChineseName;
            set => _ChineseName = value;
        }

        [JsonPropertyName("Synonym")]
        public List<string> Synonym
        {
            get => _Synonym;
            set => _Synonym = value;
        }

        [JsonPropertyName("Tag")]
        public List<string> Tag
        {
            get => _Tag;
            set => _Tag = value;
        }

        [JsonPropertyName("Desc")]
        public string Description
        {
            get => _Description;
            set => _Description = value;
        }

        [JsonPropertyName("Rank")]
        public TaxonomicCategory Category
        {
            get => _Category;
            set => _Category = value;
        }

        [JsonPropertyName("EX")]
        public int IsExtinct
        {
            get => _IsExtinct;
            set => _IsExtinct = value;
        }

        [JsonPropertyName("Doubt")]
        public int InDoubt
        {
            get => _InDoubt;
            set => _InDoubt = value;
        }

        [JsonIgnore]
        public List<int> ParentsIndex => _ParentsIndex;

        [JsonIgnore]
        public int Index => _Index;

        [JsonPropertyName("ID")]
        public string ID
        {
            get
            {
                int count = _ParentsIndex.Count;

                if (count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < count; i++)
                    {
                        sb.Append(_ParentsIndex[i].ToString());
                        sb.Append('-');
                    }

                    sb.Append(_Index.ToString());

                    return sb.ToString();
                }
                else
                {
                    return _Index.ToString();
                }
            }

            set
            {
                string[] id = value.Split('-');

                int count = id.Length;

                if (count > 1)
                {
                    _ParentsIndex = new List<int>(count - 1);

                    for (int i = 0; i < count - 1; i++)
                    {
                        _ParentsIndex.Add(int.Parse(id[i]));
                    }

                    _Index = int.Parse(id[count - 1]);
                }
                else if (count == 1)
                {
                    _ParentsIndex = new List<int>();
                    _Index = int.Parse(id[0]);
                }
            }
        }

        //

        public Taxon ToTaxon()
        {
            Taxon taxon = new Taxon()
            {
                BotanicalName = _BotanicalName,
                ChineseName = _ChineseName,

                Category = _ConvertCategory(_Category),

                IsExtinct = Convert.ToBoolean(_IsExtinct),
                InDoubt = Convert.ToBoolean(_InDoubt),

                Description = _Description
            };

            taxon.Synonym.AddRange(_Synonym);
            taxon.Tag.AddRange(_Tag);

            return taxon;
        }

        public static PhylogeneticUnwindV1Atom FromTaxon(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            PhylogeneticUnwindV1Atom atom = new PhylogeneticUnwindV1Atom()
            {
                _BotanicalName = taxon.BotanicalName,
                _ChineseName = taxon.ChineseName,
                _Synonym = new List<string>(taxon.Synonym),
                _Tag = new List<string>(taxon.Tag),
                _Description = taxon.Description,

                _Category = _ConvertCategory(taxon.Category),

                _IsExtinct = Convert.ToInt32(taxon.IsExtinct),
                _InDoubt = Convert.ToInt32(taxon.InDoubt),

                _ParentsIndex = new List<int>(taxon.Level),
                _Index = taxon.Index
            };

            Taxon parent = taxon.Parent;

            while (!parent.IsRoot)
            {
                atom._ParentsIndex.Add(parent.Index);

                parent = parent.Parent;
            }

            atom._ParentsIndex.Reverse();

            return atom;
        }
    }

    // 系统发生树的展开，用于序列化/反序列化。
    internal class PhylogeneticUnwindV1
    {
        private List<PhylogeneticUnwindV1Atom> _Atoms = new List<PhylogeneticUnwindV1Atom>();

        //

        public PhylogeneticUnwindV1()
        {
        }

        //

        [JsonPropertyName(Phylogenesis.FileVersionJsonPropertyName)]
        public int FileVersion => 1;

        [JsonPropertyName(Phylogenesis.AppVersionJsonPropertyName)]
        public string AppVersion => System.Windows.Forms.Application.ProductVersion;

        [JsonPropertyName("Taxons")]
        public List<PhylogeneticUnwindV1Atom> Atoms
        {
            get => _Atoms;
            set => _Atoms = value;
        }

        //

        private static Taxon _GetTaxonOfTree(PhylogeneticTree tree, List<int> index)
        {
            if (tree is null || index is null)
            {
                throw new ArgumentNullException();
            }

            //

            Taxon taxon = tree.Root;

            if (index.Count > 0)
            {
                for (int i = 0; i < index.Count; i++)
                {
                    taxon = taxon.Children[index[i]];
                }
            }

            return taxon;
        }

        public PhylogeneticTree ToPhylogeneticTree()
        {
            PhylogeneticTree tree = new PhylogeneticTree();

            var atoms = _Atoms.OrderBy(atom => atom.ParentsIndex.Count).ThenBy(atom => atom.Index);

            foreach (PhylogeneticUnwindV1Atom atom in atoms)
            {
                atom.ToTaxon().SetParent(_GetTaxonOfTree(tree, atom.ParentsIndex));
            }

            return tree;
        }
    }
}