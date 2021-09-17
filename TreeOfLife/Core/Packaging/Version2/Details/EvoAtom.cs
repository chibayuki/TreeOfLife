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

using System.Text.Json.Serialization;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.Packaging.Version2.Details
{
    // 系统发生树展开的表示演化关系的原子数据结构，表示一个类群。
    public sealed class EvoAtom
    {
        private static Category _ConvertCategory(Taxonomy.Category category)
        {
            return category switch
            {
                Taxonomy.Category.Unranked => Category.Unranked,
                Taxonomy.Category.Clade => Category.Clade,
                Taxonomy.Category.Strain => Category.Strain,
                Taxonomy.Category.Subform => Category.Subform,
                Taxonomy.Category.Form => Category.Form,
                Taxonomy.Category.Subseries => Category.Subseries,
                Taxonomy.Category.Series => Category.Series,
                Taxonomy.Category.Infradivision => Category.Infradivision,
                Taxonomy.Category.Subdivision => Category.Subdivision,
                Taxonomy.Category.Division => Category.Division,
                Taxonomy.Category.Superdivision => Category.Superdivision,
                Taxonomy.Category.Infrasection => Category.Infrasection,
                Taxonomy.Category.Subsection => Category.Subsection,
                Taxonomy.Category.Section => Category.Section,
                Taxonomy.Category.Supersection => Category.Supersection,
                Taxonomy.Category.Infracohort => Category.Infracohort,
                Taxonomy.Category.Subcohort => Category.Subcohort,
                Taxonomy.Category.Cohort => Category.Cohort,
                Taxonomy.Category.Supercohort => Category.Supercohort,
                Taxonomy.Category.Megacohort => Category.Megacohort,
                Taxonomy.Category.Infratribe => Category.Infratribe,
                Taxonomy.Category.Subtribe => Category.Subtribe,
                Taxonomy.Category.Tribe => Category.Tribe,
                Taxonomy.Category.Supertribe => Category.Supertribe,
                Taxonomy.Category.Subvariety => Category.Subvariety,
                Taxonomy.Category.Variety => Category.Variety,
                Taxonomy.Category.Subspecies => Category.Subspecies,
                Taxonomy.Category.Species => Category.Species,
                Taxonomy.Category.Superspecies => Category.Superspecies,
                Taxonomy.Category.Infragenus => Category.Infragenus,
                Taxonomy.Category.Subgenus => Category.Subgenus,
                Taxonomy.Category.Genus => Category.Genus,
                Taxonomy.Category.Infrafamily => Category.Infrafamily,
                Taxonomy.Category.Subfamily => Category.Subfamily,
                Taxonomy.Category.Family => Category.Family,
                Taxonomy.Category.Epifamily => Category.Epifamily,
                Taxonomy.Category.Hyperfamily => Category.Hyperfamily,
                Taxonomy.Category.Grandfamily => Category.Grandfamily,
                Taxonomy.Category.Superfamily => Category.Superfamily,
                Taxonomy.Category.Megafamily => Category.Megafamily,
                Taxonomy.Category.Gigafamily => Category.Gigafamily,
                Taxonomy.Category.Parvorder => Category.Parvorder,
                Taxonomy.Category.Infraorder => Category.Infraorder,
                Taxonomy.Category.Suborder => Category.Suborder,
                Taxonomy.Category.Minorder => Category.Minorder,
                Taxonomy.Category.Hypoorder => Category.Hypoorder,
                Taxonomy.Category.Nanorder => Category.Nanorder,
                Taxonomy.Category.Order => Category.Order,
                Taxonomy.Category.Hyperorder => Category.Hyperorder,
                Taxonomy.Category.Grandorder => Category.Grandorder,
                Taxonomy.Category.Superorder => Category.Superorder,
                Taxonomy.Category.Megaorder => Category.Megaorder,
                Taxonomy.Category.Gigaorder => Category.Gigaorder,
                Taxonomy.Category.Parvclass => Category.Parvclass,
                Taxonomy.Category.Infraclass => Category.Infraclass,
                Taxonomy.Category.Subclass => Category.Subclass,
                Taxonomy.Category.Class => Category.Class,
                Taxonomy.Category.Hyperclass => Category.Hyperclass,
                Taxonomy.Category.Grandclass => Category.Grandclass,
                Taxonomy.Category.Superclass => Category.Superclass,
                Taxonomy.Category.Megaclass => Category.Megaclass,
                Taxonomy.Category.Parvphylum => Category.Parvphylum,
                Taxonomy.Category.Infraphylum => Category.Infraphylum,
                Taxonomy.Category.Subphylum => Category.Subphylum,
                Taxonomy.Category.Phylum => Category.Phylum,
                Taxonomy.Category.Superphylum => Category.Superphylum,
                Taxonomy.Category.Infrakingdom => Category.Infrakingdom,
                Taxonomy.Category.Subkingdom => Category.Subkingdom,
                Taxonomy.Category.Kingdom => Category.Kingdom,
                Taxonomy.Category.Superkingdom => Category.Superkingdom,
                Taxonomy.Category.Domain => Category.Domain,
                Taxonomy.Category.Superdomain => Category.Superdomain,
                _ => Category.Unranked
            };
        }

        private static Taxonomy.Category _ConvertCategory(Category category)
        {
            return category switch
            {
                Category.Unranked => Taxonomy.Category.Unranked,
                Category.Clade => Taxonomy.Category.Clade,
                Category.Strain => Taxonomy.Category.Strain,
                Category.Subform => Taxonomy.Category.Subform,
                Category.Form => Taxonomy.Category.Form,
                Category.Subseries => Taxonomy.Category.Subseries,
                Category.Series => Taxonomy.Category.Series,
                Category.Infradivision => Taxonomy.Category.Infradivision,
                Category.Subdivision => Taxonomy.Category.Subdivision,
                Category.Division => Taxonomy.Category.Division,
                Category.Superdivision => Taxonomy.Category.Superdivision,
                Category.Infrasection => Taxonomy.Category.Infrasection,
                Category.Subsection => Taxonomy.Category.Subsection,
                Category.Section => Taxonomy.Category.Section,
                Category.Supersection => Taxonomy.Category.Supersection,
                Category.Infracohort => Taxonomy.Category.Infracohort,
                Category.Subcohort => Taxonomy.Category.Subcohort,
                Category.Cohort => Taxonomy.Category.Cohort,
                Category.Supercohort => Taxonomy.Category.Supercohort,
                Category.Megacohort => Taxonomy.Category.Megacohort,
                Category.Infratribe => Taxonomy.Category.Infratribe,
                Category.Subtribe => Taxonomy.Category.Subtribe,
                Category.Tribe => Taxonomy.Category.Tribe,
                Category.Supertribe => Taxonomy.Category.Supertribe,
                Category.Subvariety => Taxonomy.Category.Subvariety,
                Category.Variety => Taxonomy.Category.Variety,
                Category.Subspecies => Taxonomy.Category.Subspecies,
                Category.Species => Taxonomy.Category.Species,
                Category.Superspecies => Taxonomy.Category.Superspecies,
                Category.Infragenus => Taxonomy.Category.Infragenus,
                Category.Subgenus => Taxonomy.Category.Subgenus,
                Category.Genus => Taxonomy.Category.Genus,
                Category.Infrafamily => Taxonomy.Category.Infrafamily,
                Category.Subfamily => Taxonomy.Category.Subfamily,
                Category.Family => Taxonomy.Category.Family,
                Category.Epifamily => Taxonomy.Category.Epifamily,
                Category.Hyperfamily => Taxonomy.Category.Hyperfamily,
                Category.Grandfamily => Taxonomy.Category.Grandfamily,
                Category.Superfamily => Taxonomy.Category.Superfamily,
                Category.Megafamily => Taxonomy.Category.Megafamily,
                Category.Gigafamily => Taxonomy.Category.Gigafamily,
                Category.Parvorder => Taxonomy.Category.Parvorder,
                Category.Infraorder => Taxonomy.Category.Infraorder,
                Category.Suborder => Taxonomy.Category.Suborder,
                Category.Minorder => Taxonomy.Category.Minorder,
                Category.Hypoorder => Taxonomy.Category.Hypoorder,
                Category.Nanorder => Taxonomy.Category.Nanorder,
                Category.Order => Taxonomy.Category.Order,
                Category.Hyperorder => Taxonomy.Category.Hyperorder,
                Category.Grandorder => Taxonomy.Category.Grandorder,
                Category.Superorder => Taxonomy.Category.Superorder,
                Category.Megaorder => Taxonomy.Category.Megaorder,
                Category.Gigaorder => Taxonomy.Category.Gigaorder,
                Category.Parvclass => Taxonomy.Category.Parvclass,
                Category.Infraclass => Taxonomy.Category.Infraclass,
                Category.Subclass => Taxonomy.Category.Subclass,
                Category.Class => Taxonomy.Category.Class,
                Category.Hyperclass => Taxonomy.Category.Hyperclass,
                Category.Grandclass => Taxonomy.Category.Grandclass,
                Category.Superclass => Taxonomy.Category.Superclass,
                Category.Megaclass => Taxonomy.Category.Megaclass,
                Category.Parvphylum => Taxonomy.Category.Parvphylum,
                Category.Infraphylum => Taxonomy.Category.Infraphylum,
                Category.Subphylum => Taxonomy.Category.Subphylum,
                Category.Phylum => Taxonomy.Category.Phylum,
                Category.Superphylum => Taxonomy.Category.Superphylum,
                Category.Infrakingdom => Taxonomy.Category.Infrakingdom,
                Category.Subkingdom => Taxonomy.Category.Subkingdom,
                Category.Kingdom => Taxonomy.Category.Kingdom,
                Category.Superkingdom => Taxonomy.Category.Superkingdom,
                Category.Domain => Taxonomy.Category.Domain,
                Category.Superdomain => Taxonomy.Category.Superdomain,
                _ => Taxonomy.Category.Unranked
            };
        }

        //

        private string _ScientificName; // 学名。
        private string _ChineseName; // 中文名。
        private List<string> _Synonyms; // 异名、别名、旧名等。
        private List<string> _Tags; // 标签。
        private string _Description; // 描述。

        private Category _Category; // 分类阶元。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _IsUnsure = false; // 存疑。

        private int _Level = 0; // 当前类群与顶级类群的距离。
        private int _Index = -1; // 当前类群在姊妹类群中的次序。

        private List<int> _ParentsIndex = new List<int>();

        //

        public EvoAtom()
        {
        }

        //

        [JsonPropertyName("Name")]
        public string ScientificName
        {
            get => _ScientificName;
            set => _ScientificName = value;
        }

        [JsonPropertyName("ChsName")]
        public string ChineseName
        {
            get => _ChineseName;
            set => _ChineseName = value;
        }

        [JsonPropertyName("Synonyms")]
        public List<string> Synonyms
        {
            get => _Synonyms;
            set => _Synonyms = value;
        }

        [JsonPropertyName("Tags")]
        public List<string> Tags
        {
            get => _Tags;
            set => _Tags = value;
        }

        [JsonPropertyName("Desc")]
        public string Description
        {
            get => _Description;
            set => _Description = value;
        }

        [JsonPropertyName("Rank")]
        public Category Category
        {
            get => _Category;
            set => _Category = value;
        }

        [JsonPropertyName("EX")]
        public int IsExtinct
        {
            get => Convert.ToInt32(_IsExtinct);
            set => _IsExtinct = Convert.ToBoolean(value);
        }

        [JsonPropertyName("Unsure")]
        public int IsUnsure
        {
            get => Convert.ToInt32(_IsUnsure);
            set => _IsUnsure = Convert.ToBoolean(value);
        }

        [JsonIgnore]
        public int Level => _Level;

        [JsonIgnore]
        public int Index => _Index;

        [JsonIgnore]
        public IReadOnlyList<int> ParentsIndex => _ParentsIndex;

        // 序列化/反序列化时，自行构造/解析字符串比直接使用整数数组快很多
        [JsonPropertyName("ID")]
        public string ID
        {
            get => Common.IndexListToIdString(_ParentsIndex, _Index);

            set
            {
                Common.IdStringToIndexList(value, out _ParentsIndex, out _Index);

                _Level = _ParentsIndex.Count + 1;
            }
        }

        //

        public Taxon ToTaxon()
        {
            Taxon taxon = new Taxon()
            {
                ScientificName = _ScientificName,
                ChineseName = _ChineseName,

                Description = _Description,

                Category = _ConvertCategory(_Category),

                IsExtinct = _IsExtinct,
                IsUnsure = _IsUnsure
            };

            taxon.Synonyms.AddRange(_Synonyms);
            taxon.Tags.AddRange(_Tags);

            // 匿名类群的分类阶元始终为未分级
            if (string.IsNullOrEmpty(taxon.ScientificName) && string.IsNullOrEmpty(taxon.ChineseName))
            {
                taxon.Category = Taxonomy.Category.Unranked;
            }

            return taxon;
        }

        public static EvoAtom FromTaxon(Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            EvoAtom atom = new EvoAtom()
            {
                _ScientificName = taxon.ScientificName,
                _ChineseName = taxon.ChineseName,
                _Synonyms = new List<string>(taxon.Synonyms),
                _Tags = new List<string>(taxon.Tags),
                _Description = taxon.Description,

                _Category = _ConvertCategory(taxon.Category),

                _IsExtinct = taxon.IsExtinct,
                _IsUnsure = taxon.IsUnsure,

                _Level = taxon.Level,
                _Index = taxon.Index,

                _ParentsIndex = Common.GetIndexListOfTaxon(taxon.Parent)
            };

            return atom;
        }
    }
}