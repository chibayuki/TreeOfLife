/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1322.1000.M13.210925-1400

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.IO.Version1.Details
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
                Taxonomy.Rank.Infradivision => Rank.Infradivision,
                Taxonomy.Rank.Subdivision => Rank.Subdivision,
                Taxonomy.Rank.Division => Rank.Division,
                Taxonomy.Rank.Superdivision => Rank.Superdivision,
                Taxonomy.Rank.Infrasection => Rank.Infrasection,
                Taxonomy.Rank.Subsection => Rank.Subsection,
                Taxonomy.Rank.Section => Rank.Section,
                Taxonomy.Rank.Supersection => Rank.Supersection,
                Taxonomy.Rank.Infracohort => Rank.Infracohort,
                Taxonomy.Rank.Subcohort => Rank.Subcohort,
                Taxonomy.Rank.Cohort => Rank.Cohort,
                Taxonomy.Rank.Supercohort => Rank.Supercohort,
                Taxonomy.Rank.Megacohort => Rank.Megacohort,
                Taxonomy.Rank.Infratribe => Rank.Infratribe,
                Taxonomy.Rank.Subtribe => Rank.Subtribe,
                Taxonomy.Rank.Tribe => Rank.Tribe,
                Taxonomy.Rank.Supertribe => Rank.Supertribe,
                Taxonomy.Rank.Subvariety => Rank.Subvariety,
                Taxonomy.Rank.Variety => Rank.Variety,
                Taxonomy.Rank.Subspecies => Rank.Subspecies,
                Taxonomy.Rank.Species => Rank.Species,
                Taxonomy.Rank.Superspecies => Rank.Superspecies,
                Taxonomy.Rank.Infragenus => Rank.Infragenus,
                Taxonomy.Rank.Subgenus => Rank.Subgenus,
                Taxonomy.Rank.Genus => Rank.Genus,
                Taxonomy.Rank.Infrafamily => Rank.Infrafamily,
                Taxonomy.Rank.Subfamily => Rank.Subfamily,
                Taxonomy.Rank.Family => Rank.Family,
                Taxonomy.Rank.Epifamily => Rank.Epifamily,
                Taxonomy.Rank.Hyperfamily => Rank.Hyperfamily,
                Taxonomy.Rank.Grandfamily => Rank.Grandfamily,
                Taxonomy.Rank.Superfamily => Rank.Superfamily,
                Taxonomy.Rank.Megafamily => Rank.Megafamily,
                Taxonomy.Rank.Gigafamily => Rank.Gigafamily,
                Taxonomy.Rank.Parvorder => Rank.Parvorder,
                Taxonomy.Rank.Infraorder => Rank.Infraorder,
                Taxonomy.Rank.Suborder => Rank.Suborder,
                Taxonomy.Rank.Minorder => Rank.Minorder,
                Taxonomy.Rank.Hypoorder => Rank.Hypoorder,
                Taxonomy.Rank.Nanorder => Rank.Nanorder,
                Taxonomy.Rank.Order => Rank.Order,
                Taxonomy.Rank.Hyperorder => Rank.Hyperorder,
                Taxonomy.Rank.Grandorder => Rank.Grandorder,
                Taxonomy.Rank.Superorder => Rank.Superorder,
                Taxonomy.Rank.Megaorder => Rank.Megaorder,
                Taxonomy.Rank.Gigaorder => Rank.Gigaorder,
                Taxonomy.Rank.Parvclass => Rank.Parvclass,
                Taxonomy.Rank.Infraclass => Rank.Infraclass,
                Taxonomy.Rank.Subclass => Rank.Subclass,
                Taxonomy.Rank.Class => Rank.Class,
                Taxonomy.Rank.Hyperclass => Rank.Hyperclass,
                Taxonomy.Rank.Grandclass => Rank.Grandclass,
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
                Rank.Infradivision => Taxonomy.Rank.Infradivision,
                Rank.Subdivision => Taxonomy.Rank.Subdivision,
                Rank.Division => Taxonomy.Rank.Division,
                Rank.Superdivision => Taxonomy.Rank.Superdivision,
                Rank.Infrasection => Taxonomy.Rank.Infrasection,
                Rank.Subsection => Taxonomy.Rank.Subsection,
                Rank.Section => Taxonomy.Rank.Section,
                Rank.Supersection => Taxonomy.Rank.Supersection,
                Rank.Infracohort => Taxonomy.Rank.Infracohort,
                Rank.Subcohort => Taxonomy.Rank.Subcohort,
                Rank.Cohort => Taxonomy.Rank.Cohort,
                Rank.Supercohort => Taxonomy.Rank.Supercohort,
                Rank.Megacohort => Taxonomy.Rank.Megacohort,
                Rank.Infratribe => Taxonomy.Rank.Infratribe,
                Rank.Subtribe => Taxonomy.Rank.Subtribe,
                Rank.Tribe => Taxonomy.Rank.Tribe,
                Rank.Supertribe => Taxonomy.Rank.Supertribe,
                Rank.Subvariety => Taxonomy.Rank.Subvariety,
                Rank.Variety => Taxonomy.Rank.Variety,
                Rank.Subspecies => Taxonomy.Rank.Subspecies,
                Rank.Species => Taxonomy.Rank.Species,
                Rank.Superspecies => Taxonomy.Rank.Superspecies,
                Rank.Infragenus => Taxonomy.Rank.Infragenus,
                Rank.Subgenus => Taxonomy.Rank.Subgenus,
                Rank.Genus => Taxonomy.Rank.Genus,
                Rank.Infrafamily => Taxonomy.Rank.Infrafamily,
                Rank.Subfamily => Taxonomy.Rank.Subfamily,
                Rank.Family => Taxonomy.Rank.Family,
                Rank.Epifamily => Taxonomy.Rank.Epifamily,
                Rank.Hyperfamily => Taxonomy.Rank.Hyperfamily,
                Rank.Grandfamily => Taxonomy.Rank.Grandfamily,
                Rank.Superfamily => Taxonomy.Rank.Superfamily,
                Rank.Megafamily => Taxonomy.Rank.Megafamily,
                Rank.Gigafamily => Taxonomy.Rank.Gigafamily,
                Rank.Parvorder => Taxonomy.Rank.Parvorder,
                Rank.Infraorder => Taxonomy.Rank.Infraorder,
                Rank.Suborder => Taxonomy.Rank.Suborder,
                Rank.Minorder => Taxonomy.Rank.Minorder,
                Rank.Hypoorder => Taxonomy.Rank.Hypoorder,
                Rank.Nanorder => Taxonomy.Rank.Nanorder,
                Rank.Order => Taxonomy.Rank.Order,
                Rank.Hyperorder => Taxonomy.Rank.Hyperorder,
                Rank.Grandorder => Taxonomy.Rank.Grandorder,
                Rank.Superorder => Taxonomy.Rank.Superorder,
                Rank.Megaorder => Taxonomy.Rank.Megaorder,
                Rank.Gigaorder => Taxonomy.Rank.Gigaorder,
                Rank.Parvclass => Taxonomy.Rank.Parvclass,
                Rank.Infraclass => Taxonomy.Rank.Infraclass,
                Rank.Subclass => Taxonomy.Rank.Subclass,
                Rank.Class => Taxonomy.Rank.Class,
                Rank.Hyperclass => Taxonomy.Rank.Hyperclass,
                Rank.Grandclass => Taxonomy.Rank.Grandclass,
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
            get
            {
                if (_Level > 1)
                {
                    StringBuilder id = new StringBuilder();

                    foreach (var parentIndex in _ParentsIndex)
                    {
                        id.Append(parentIndex);
                        id.Append('-');
                    }

                    id.Append(_Index);

                    return id.ToString();
                }
                else
                {
                    return _Index.ToString();
                }
            }

            set
            {
                string[] id = value.Split('-');

                _Level = id.Length;

                if (_Level > 1)
                {
                    _ParentsIndex = new List<int>(_Level - 1);

                    for (int i = 0; i < _Level - 1; i++)
                    {
                        _ParentsIndex.Add(int.Parse(id[i]));
                    }

                    _Index = int.Parse(id[^1]);
                }
                else if (_Level == 1)
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
                ScientificName = _ScientificName,
                ChineseName = _ChineseName,

                Description = _Description,

                Rank = _ConvertRank(_Rank),

                IsExtinct = _IsExtinct,
                IsUndet = _IsUndet
            };

            taxon.Synonyms.AddRange(_Synonyms);
            taxon.Tags.AddRange(_Tags);

            // 匿名类群的分类阶元始终为未分级
            if (taxon.IsAnonymous)
            {
                taxon.Rank = Taxonomy.Rank.Unranked;
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

                _Level = taxon.Level,
                _Index = taxon.Index,

                _ParentsIndex = new List<int>(taxon.Level)
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
}