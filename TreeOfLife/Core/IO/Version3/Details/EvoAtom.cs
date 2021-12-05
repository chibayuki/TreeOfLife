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

using System.Text.Json.Serialization;

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.IO.Version3.Details
{
    // 系统发生树展开的表示演化关系的原子数据结构，表示一个类群。
    public sealed class EvoAtom
    {
        private static Rank _ConvertRank(Taxonomy.Rank rank)
        {
            return rank switch
            {
                Taxonomy.Rank.Unranked => Rank.Unranked,

                Taxonomy.Rank.Clade => Rank.Clade,

                Taxonomy.Rank.Strain => Rank.Strain,

                Taxonomy.Rank.Subform => Rank.Subform,
                Taxonomy.Rank.Form => Rank.Form,

                Taxonomy.Rank.Subseries => Rank.Subseries,
                Taxonomy.Rank.Series => Rank.Series,

                Taxonomy.Rank.Subdivision => Rank.Subdivision,
                Taxonomy.Rank.Division => Rank.Division,

                Taxonomy.Rank.Subsection => Rank.Subsection,
                Taxonomy.Rank.Section => Rank.Section,

                Taxonomy.Rank.Infracohort => Rank.Infracohort,
                Taxonomy.Rank.Subcohort => Rank.Subcohort,
                Taxonomy.Rank.Cohort => Rank.Cohort,
                Taxonomy.Rank.Supercohort => Rank.Supercohort,
                Taxonomy.Rank.Megacohort => Rank.Megacohort,

                Taxonomy.Rank.Subtribe => Rank.Subtribe,
                Taxonomy.Rank.Tribe => Rank.Tribe,

                Taxonomy.Rank.Subvariety => Rank.Subvariety,
                Taxonomy.Rank.Variety => Rank.Variety,
                Taxonomy.Rank.Subspecies => Rank.Subspecies,
                Taxonomy.Rank.Species => Rank.Species,

                Taxonomy.Rank.Subgenus => Rank.Subgenus,
                Taxonomy.Rank.Genus => Rank.Genus,

                Taxonomy.Rank.Subfamily => Rank.Subfamily,
                Taxonomy.Rank.Family => Rank.Family,
                Taxonomy.Rank.Superfamily => Rank.Superfamily,

                Taxonomy.Rank.Parvorder => Rank.Parvorder,
                Taxonomy.Rank.Infraorder => Rank.Infraorder,
                Taxonomy.Rank.Suborder => Rank.Suborder,
                Taxonomy.Rank.Order => Rank.Order,
                Taxonomy.Rank.Mirorder => Rank.Hyperorder,
                Taxonomy.Rank.Grandorder => Rank.Grandorder,
                Taxonomy.Rank.Superorder => Rank.Superorder,
                Taxonomy.Rank.Megaorder => Rank.Megaorder,

                Taxonomy.Rank.Parvclass => Rank.Parvclass,
                Taxonomy.Rank.Infraclass => Rank.Infraclass,
                Taxonomy.Rank.Subclass => Rank.Subclass,
                Taxonomy.Rank.Class => Rank.Class,
                Taxonomy.Rank.Superclass => Rank.Superclass,
                Taxonomy.Rank.Megaclass => Rank.Megaclass,

                Taxonomy.Rank.Parvphylum => Rank.Parvphylum,
                Taxonomy.Rank.Infraphylum => Rank.Infraphylum,
                Taxonomy.Rank.Subphylum => Rank.Subphylum,
                Taxonomy.Rank.Phylum => Rank.Phylum,
                Taxonomy.Rank.Superphylum => Rank.Superphylum,

                Taxonomy.Rank.Infrakingdom => Rank.Infrakingdom,
                Taxonomy.Rank.Subkingdom => Rank.Subkingdom,
                Taxonomy.Rank.Kingdom => Rank.Kingdom,
                Taxonomy.Rank.Superkingdom => Rank.Superkingdom,

                Taxonomy.Rank.Domain => Rank.Domain,
                Taxonomy.Rank.Superdomain => Rank.Superdomain,

                _ => Rank.Unranked
            };
        }

        private static Taxonomy.Rank _ConvertRank(Rank rank)
        {
            return rank switch
            {
                Rank.Unranked => Taxonomy.Rank.Unranked,

                Rank.Clade => Taxonomy.Rank.Clade,

                Rank.Strain => Taxonomy.Rank.Strain,

                Rank.Subform => Taxonomy.Rank.Subform,
                Rank.Form => Taxonomy.Rank.Form,

                Rank.Subseries => Taxonomy.Rank.Subseries,
                Rank.Series => Taxonomy.Rank.Series,

                Rank.Subdivision => Taxonomy.Rank.Subdivision,
                Rank.Division => Taxonomy.Rank.Division,

                Rank.Subsection => Taxonomy.Rank.Subsection,
                Rank.Section => Taxonomy.Rank.Section,

                Rank.Infracohort => Taxonomy.Rank.Infracohort,
                Rank.Subcohort => Taxonomy.Rank.Subcohort,
                Rank.Cohort => Taxonomy.Rank.Cohort,
                Rank.Supercohort => Taxonomy.Rank.Supercohort,
                Rank.Megacohort => Taxonomy.Rank.Megacohort,

                Rank.Subtribe => Taxonomy.Rank.Subtribe,
                Rank.Tribe => Taxonomy.Rank.Tribe,

                Rank.Subvariety => Taxonomy.Rank.Subvariety,
                Rank.Variety => Taxonomy.Rank.Variety,
                Rank.Subspecies => Taxonomy.Rank.Subspecies,
                Rank.Species => Taxonomy.Rank.Species,

                Rank.Subgenus => Taxonomy.Rank.Subgenus,
                Rank.Genus => Taxonomy.Rank.Genus,

                Rank.Subfamily => Taxonomy.Rank.Subfamily,
                Rank.Family => Taxonomy.Rank.Family,
                Rank.Superfamily => Taxonomy.Rank.Superfamily,

                Rank.Parvorder => Taxonomy.Rank.Parvorder,
                Rank.Infraorder => Taxonomy.Rank.Infraorder,
                Rank.Suborder => Taxonomy.Rank.Suborder,
                Rank.Order => Taxonomy.Rank.Order,
                Rank.Hyperorder => Taxonomy.Rank.Mirorder,
                Rank.Grandorder => Taxonomy.Rank.Grandorder,
                Rank.Superorder => Taxonomy.Rank.Superorder,
                Rank.Megaorder => Taxonomy.Rank.Megaorder,

                Rank.Parvclass => Taxonomy.Rank.Parvclass,
                Rank.Infraclass => Taxonomy.Rank.Infraclass,
                Rank.Subclass => Taxonomy.Rank.Subclass,
                Rank.Class => Taxonomy.Rank.Class,
                Rank.Superclass => Taxonomy.Rank.Superclass,
                Rank.Megaclass => Taxonomy.Rank.Megaclass,

                Rank.Parvphylum => Taxonomy.Rank.Parvphylum,
                Rank.Infraphylum => Taxonomy.Rank.Infraphylum,
                Rank.Subphylum => Taxonomy.Rank.Subphylum,
                Rank.Phylum => Taxonomy.Rank.Phylum,
                Rank.Superphylum => Taxonomy.Rank.Superphylum,

                Rank.Infrakingdom => Taxonomy.Rank.Infrakingdom,
                Rank.Subkingdom => Taxonomy.Rank.Subkingdom,
                Rank.Kingdom => Taxonomy.Rank.Kingdom,
                Rank.Superkingdom => Taxonomy.Rank.Superkingdom,

                Rank.Domain => Taxonomy.Rank.Domain,
                Rank.Superdomain => Taxonomy.Rank.Superdomain,

                _ => Taxonomy.Rank.Unranked
            };
        }

        //

        private string _ScientificName; // 学名。
        private string _ChineseName; // 中文名。
        private List<string> _Synonyms; // 异名、别名、旧名等。
        private List<string> _Tags; // 标签。
        private string _Description; // 描述。

        private Rank _Rank; // 分类阶元。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _IsUndet = false; // 存疑。

        private GeoChron _Birth = GeoChron.Empty; // 诞生年代。
        private GeoChron _Extinction = GeoChron.Empty; // 灭绝年代。

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
        public Rank Rank
        {
            get => _Rank;
            set => _Rank = value;
        }

        [JsonPropertyName("EX")]
        public int IsExtinct
        {
            get => Convert.ToInt32(_IsExtinct);
            set => _IsExtinct = Convert.ToBoolean(value);
        }

        [JsonPropertyName("Unsure")]
        public int IsUndet
        {
            get => Convert.ToInt32(_IsUndet);
            set => _IsUndet = Convert.ToBoolean(value);
        }

        [JsonPropertyName("From")]
        public string Birth
        {
            get => _Birth.ToString();
            set => _Birth = GeoChron.Parse(value);
        }

        [JsonPropertyName("To")]
        public string Extinction
        {
            get => _Extinction.ToString();
            set => _Extinction = GeoChron.Parse(value);
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
                Synonyms = _Synonyms,
                Tags = _Tags,
                Description = _Description,

                Rank = _ConvertRank(_Rank),

                IsExtinct = _IsExtinct,
                IsUndet = _IsUndet,

                Birth = _Birth,
                Extinction = _Extinction
            };

            // 匿名类群的分类阶元始终为未指定
            if (taxon.IsAnonymous)
            {
                taxon.Rank = Taxonomy.Rank.Unranked;
            }

            // 类群的诞生年代不应是现代
            if (taxon.Birth.IsPresent)
            {
                taxon.Birth = GeoChron.Empty;
            }

            // 未灭绝的类群不应具有有意义的灭绝年代，已灭绝的类群的灭绝年代不应是现代
            if (!taxon.IsExtinct || taxon.Extinction.IsPresent)
            {
                taxon.Extinction = GeoChron.Empty;
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

                _Rank = _ConvertRank(taxon.Rank),

                _IsExtinct = taxon.IsExtinct,
                _IsUndet = taxon.IsUndet,

                _Birth = taxon.Birth,
                _Extinction = taxon.Extinction,

                _Level = taxon.Level,
                _Index = taxon.Index,

                _ParentsIndex = Common.GetIndexListOfTaxon(taxon.Parent)
            };

            return atom;
        }
    }
}