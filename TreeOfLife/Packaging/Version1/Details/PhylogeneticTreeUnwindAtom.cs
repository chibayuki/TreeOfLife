/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.700.1000.M7.201226-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json.Serialization;

using TreeOfLife.Taxonomy;

namespace TreeOfLife.Packaging.Version1.Details
{
    // 系统发生树展开的原子数据结构。
    public class PhylogeneticTreeUnwindAtom
    {
        private static TaxonomicCategory _ConvertCategory(Taxonomy.TaxonomicCategory category)
        {
            return category switch
            {
                Taxonomy.TaxonomicCategory.Unranked => TaxonomicCategory.Unranked,
                Taxonomy.TaxonomicCategory.Clade => TaxonomicCategory.Clade,
                Taxonomy.TaxonomicCategory.Strain => TaxonomicCategory.Strain,
                Taxonomy.TaxonomicCategory.Subform => TaxonomicCategory.Subform,
                Taxonomy.TaxonomicCategory.Form => TaxonomicCategory.Form,
                Taxonomy.TaxonomicCategory.Subseries => TaxonomicCategory.Subseries,
                Taxonomy.TaxonomicCategory.Series => TaxonomicCategory.Series,
                Taxonomy.TaxonomicCategory.Infradivision => TaxonomicCategory.Infradivision,
                Taxonomy.TaxonomicCategory.Subdivision => TaxonomicCategory.Subdivision,
                Taxonomy.TaxonomicCategory.Division => TaxonomicCategory.Division,
                Taxonomy.TaxonomicCategory.Superdivision => TaxonomicCategory.Superdivision,
                Taxonomy.TaxonomicCategory.Infrasection => TaxonomicCategory.Infrasection,
                Taxonomy.TaxonomicCategory.Subsection => TaxonomicCategory.Subsection,
                Taxonomy.TaxonomicCategory.Section => TaxonomicCategory.Section,
                Taxonomy.TaxonomicCategory.Supersection => TaxonomicCategory.Supersection,
                Taxonomy.TaxonomicCategory.Infracohort => TaxonomicCategory.Infracohort,
                Taxonomy.TaxonomicCategory.Subcohort => TaxonomicCategory.Subcohort,
                Taxonomy.TaxonomicCategory.Cohort => TaxonomicCategory.Cohort,
                Taxonomy.TaxonomicCategory.Supercohort => TaxonomicCategory.Supercohort,
                Taxonomy.TaxonomicCategory.Megacohort => TaxonomicCategory.Megacohort,
                Taxonomy.TaxonomicCategory.Infratribe => TaxonomicCategory.Infratribe,
                Taxonomy.TaxonomicCategory.Subtribe => TaxonomicCategory.Subtribe,
                Taxonomy.TaxonomicCategory.Tribe => TaxonomicCategory.Tribe,
                Taxonomy.TaxonomicCategory.Supertribe => TaxonomicCategory.Supertribe,
                Taxonomy.TaxonomicCategory.Subvariety => TaxonomicCategory.Subvariety,
                Taxonomy.TaxonomicCategory.Variety => TaxonomicCategory.Variety,
                Taxonomy.TaxonomicCategory.Subspecies => TaxonomicCategory.Subspecies,
                Taxonomy.TaxonomicCategory.Species => TaxonomicCategory.Species,
                Taxonomy.TaxonomicCategory.Superspecies => TaxonomicCategory.Superspecies,
                Taxonomy.TaxonomicCategory.Infragenus => TaxonomicCategory.Infragenus,
                Taxonomy.TaxonomicCategory.Subgenus => TaxonomicCategory.Subgenus,
                Taxonomy.TaxonomicCategory.Genus => TaxonomicCategory.Genus,
                Taxonomy.TaxonomicCategory.Infrafamily => TaxonomicCategory.Infrafamily,
                Taxonomy.TaxonomicCategory.Subfamily => TaxonomicCategory.Subfamily,
                Taxonomy.TaxonomicCategory.Family => TaxonomicCategory.Family,
                Taxonomy.TaxonomicCategory.Epifamily => TaxonomicCategory.Epifamily,
                Taxonomy.TaxonomicCategory.Hyperfamily => TaxonomicCategory.Hyperfamily,
                Taxonomy.TaxonomicCategory.Grandfamily => TaxonomicCategory.Grandfamily,
                Taxonomy.TaxonomicCategory.Superfamily => TaxonomicCategory.Superfamily,
                Taxonomy.TaxonomicCategory.Megafamily => TaxonomicCategory.Megafamily,
                Taxonomy.TaxonomicCategory.Gigafamily => TaxonomicCategory.Gigafamily,
                Taxonomy.TaxonomicCategory.Parvorder => TaxonomicCategory.Parvorder,
                Taxonomy.TaxonomicCategory.Infraorder => TaxonomicCategory.Infraorder,
                Taxonomy.TaxonomicCategory.Suborder => TaxonomicCategory.Suborder,
                Taxonomy.TaxonomicCategory.Minorder => TaxonomicCategory.Minorder,
                Taxonomy.TaxonomicCategory.Hypoorder => TaxonomicCategory.Hypoorder,
                Taxonomy.TaxonomicCategory.Nanorder => TaxonomicCategory.Nanorder,
                Taxonomy.TaxonomicCategory.Order => TaxonomicCategory.Order,
                Taxonomy.TaxonomicCategory.Hyperorder => TaxonomicCategory.Hyperorder,
                Taxonomy.TaxonomicCategory.Grandorder => TaxonomicCategory.Grandorder,
                Taxonomy.TaxonomicCategory.Superorder => TaxonomicCategory.Superorder,
                Taxonomy.TaxonomicCategory.Megaorder => TaxonomicCategory.Megaorder,
                Taxonomy.TaxonomicCategory.Gigaorder => TaxonomicCategory.Gigaorder,
                Taxonomy.TaxonomicCategory.Parvclass => TaxonomicCategory.Parvclass,
                Taxonomy.TaxonomicCategory.Infraclass => TaxonomicCategory.Infraclass,
                Taxonomy.TaxonomicCategory.Subclass => TaxonomicCategory.Subclass,
                Taxonomy.TaxonomicCategory.Class => TaxonomicCategory.Class,
                Taxonomy.TaxonomicCategory.Hyperclass => TaxonomicCategory.Hyperclass,
                Taxonomy.TaxonomicCategory.Grandclass => TaxonomicCategory.Grandclass,
                Taxonomy.TaxonomicCategory.Superclass => TaxonomicCategory.Superclass,
                Taxonomy.TaxonomicCategory.Megaclass => TaxonomicCategory.Megaclass,
                Taxonomy.TaxonomicCategory.Parvphylum => TaxonomicCategory.Parvphylum,
                Taxonomy.TaxonomicCategory.Infraphylum => TaxonomicCategory.Infraphylum,
                Taxonomy.TaxonomicCategory.Subphylum => TaxonomicCategory.Subphylum,
                Taxonomy.TaxonomicCategory.Phylum => TaxonomicCategory.Phylum,
                Taxonomy.TaxonomicCategory.Superphylum => TaxonomicCategory.Superphylum,
                Taxonomy.TaxonomicCategory.Infrakingdom => TaxonomicCategory.Infrakingdom,
                Taxonomy.TaxonomicCategory.Subkingdom => TaxonomicCategory.Subkingdom,
                Taxonomy.TaxonomicCategory.Kingdom => TaxonomicCategory.Kingdom,
                Taxonomy.TaxonomicCategory.Superkingdom => TaxonomicCategory.Superkingdom,
                Taxonomy.TaxonomicCategory.Domain => TaxonomicCategory.Domain,
                Taxonomy.TaxonomicCategory.Superdomain => TaxonomicCategory.Superdomain,
                _ => TaxonomicCategory.Unranked,
            };
        }

        private static Taxonomy.TaxonomicCategory _ConvertCategory(TaxonomicCategory category)
        {
            return category switch
            {
                TaxonomicCategory.Unranked => Taxonomy.TaxonomicCategory.Unranked,
                TaxonomicCategory.Clade => Taxonomy.TaxonomicCategory.Clade,
                TaxonomicCategory.Strain => Taxonomy.TaxonomicCategory.Strain,
                TaxonomicCategory.Subform => Taxonomy.TaxonomicCategory.Subform,
                TaxonomicCategory.Form => Taxonomy.TaxonomicCategory.Form,
                TaxonomicCategory.Subseries => Taxonomy.TaxonomicCategory.Subseries,
                TaxonomicCategory.Series => Taxonomy.TaxonomicCategory.Series,
                TaxonomicCategory.Infradivision => Taxonomy.TaxonomicCategory.Infradivision,
                TaxonomicCategory.Subdivision => Taxonomy.TaxonomicCategory.Subdivision,
                TaxonomicCategory.Division => Taxonomy.TaxonomicCategory.Division,
                TaxonomicCategory.Superdivision => Taxonomy.TaxonomicCategory.Superdivision,
                TaxonomicCategory.Infrasection => Taxonomy.TaxonomicCategory.Infrasection,
                TaxonomicCategory.Subsection => Taxonomy.TaxonomicCategory.Subsection,
                TaxonomicCategory.Section => Taxonomy.TaxonomicCategory.Section,
                TaxonomicCategory.Supersection => Taxonomy.TaxonomicCategory.Supersection,
                TaxonomicCategory.Infracohort => Taxonomy.TaxonomicCategory.Infracohort,
                TaxonomicCategory.Subcohort => Taxonomy.TaxonomicCategory.Subcohort,
                TaxonomicCategory.Cohort => Taxonomy.TaxonomicCategory.Cohort,
                TaxonomicCategory.Supercohort => Taxonomy.TaxonomicCategory.Supercohort,
                TaxonomicCategory.Megacohort => Taxonomy.TaxonomicCategory.Megacohort,
                TaxonomicCategory.Infratribe => Taxonomy.TaxonomicCategory.Infratribe,
                TaxonomicCategory.Subtribe => Taxonomy.TaxonomicCategory.Subtribe,
                TaxonomicCategory.Tribe => Taxonomy.TaxonomicCategory.Tribe,
                TaxonomicCategory.Supertribe => Taxonomy.TaxonomicCategory.Supertribe,
                TaxonomicCategory.Subvariety => Taxonomy.TaxonomicCategory.Subvariety,
                TaxonomicCategory.Variety => Taxonomy.TaxonomicCategory.Variety,
                TaxonomicCategory.Subspecies => Taxonomy.TaxonomicCategory.Subspecies,
                TaxonomicCategory.Species => Taxonomy.TaxonomicCategory.Species,
                TaxonomicCategory.Superspecies => Taxonomy.TaxonomicCategory.Superspecies,
                TaxonomicCategory.Infragenus => Taxonomy.TaxonomicCategory.Infragenus,
                TaxonomicCategory.Subgenus => Taxonomy.TaxonomicCategory.Subgenus,
                TaxonomicCategory.Genus => Taxonomy.TaxonomicCategory.Genus,
                TaxonomicCategory.Infrafamily => Taxonomy.TaxonomicCategory.Infrafamily,
                TaxonomicCategory.Subfamily => Taxonomy.TaxonomicCategory.Subfamily,
                TaxonomicCategory.Family => Taxonomy.TaxonomicCategory.Family,
                TaxonomicCategory.Epifamily => Taxonomy.TaxonomicCategory.Epifamily,
                TaxonomicCategory.Hyperfamily => Taxonomy.TaxonomicCategory.Hyperfamily,
                TaxonomicCategory.Grandfamily => Taxonomy.TaxonomicCategory.Grandfamily,
                TaxonomicCategory.Superfamily => Taxonomy.TaxonomicCategory.Superfamily,
                TaxonomicCategory.Megafamily => Taxonomy.TaxonomicCategory.Megafamily,
                TaxonomicCategory.Gigafamily => Taxonomy.TaxonomicCategory.Gigafamily,
                TaxonomicCategory.Parvorder => Taxonomy.TaxonomicCategory.Parvorder,
                TaxonomicCategory.Infraorder => Taxonomy.TaxonomicCategory.Infraorder,
                TaxonomicCategory.Suborder => Taxonomy.TaxonomicCategory.Suborder,
                TaxonomicCategory.Minorder => Taxonomy.TaxonomicCategory.Minorder,
                TaxonomicCategory.Hypoorder => Taxonomy.TaxonomicCategory.Hypoorder,
                TaxonomicCategory.Nanorder => Taxonomy.TaxonomicCategory.Nanorder,
                TaxonomicCategory.Order => Taxonomy.TaxonomicCategory.Order,
                TaxonomicCategory.Hyperorder => Taxonomy.TaxonomicCategory.Hyperorder,
                TaxonomicCategory.Grandorder => Taxonomy.TaxonomicCategory.Grandorder,
                TaxonomicCategory.Superorder => Taxonomy.TaxonomicCategory.Superorder,
                TaxonomicCategory.Megaorder => Taxonomy.TaxonomicCategory.Megaorder,
                TaxonomicCategory.Gigaorder => Taxonomy.TaxonomicCategory.Gigaorder,
                TaxonomicCategory.Parvclass => Taxonomy.TaxonomicCategory.Parvclass,
                TaxonomicCategory.Infraclass => Taxonomy.TaxonomicCategory.Infraclass,
                TaxonomicCategory.Subclass => Taxonomy.TaxonomicCategory.Subclass,
                TaxonomicCategory.Class => Taxonomy.TaxonomicCategory.Class,
                TaxonomicCategory.Hyperclass => Taxonomy.TaxonomicCategory.Hyperclass,
                TaxonomicCategory.Grandclass => Taxonomy.TaxonomicCategory.Grandclass,
                TaxonomicCategory.Superclass => Taxonomy.TaxonomicCategory.Superclass,
                TaxonomicCategory.Megaclass => Taxonomy.TaxonomicCategory.Megaclass,
                TaxonomicCategory.Parvphylum => Taxonomy.TaxonomicCategory.Parvphylum,
                TaxonomicCategory.Infraphylum => Taxonomy.TaxonomicCategory.Infraphylum,
                TaxonomicCategory.Subphylum => Taxonomy.TaxonomicCategory.Subphylum,
                TaxonomicCategory.Phylum => Taxonomy.TaxonomicCategory.Phylum,
                TaxonomicCategory.Superphylum => Taxonomy.TaxonomicCategory.Superphylum,
                TaxonomicCategory.Infrakingdom => Taxonomy.TaxonomicCategory.Infrakingdom,
                TaxonomicCategory.Subkingdom => Taxonomy.TaxonomicCategory.Subkingdom,
                TaxonomicCategory.Kingdom => Taxonomy.TaxonomicCategory.Kingdom,
                TaxonomicCategory.Superkingdom => Taxonomy.TaxonomicCategory.Superkingdom,
                TaxonomicCategory.Domain => Taxonomy.TaxonomicCategory.Domain,
                TaxonomicCategory.Superdomain => Taxonomy.TaxonomicCategory.Superdomain,
                _ => Taxonomy.TaxonomicCategory.Unranked,
            };
        }

        //

        private string _BotanicalName; // 学名。
        private string _ChineseName; // 中文名。
        private List<string> _Synonyms; // 异名、别名、旧名等。
        private List<string> _Tags; // 标签。
        private string _Description; // 描述。

        private TaxonomicCategory _Category; // 分类阶元。

        private bool _IsExtinct = false; // 已灭绝。
        private bool _IsUnsure = false; // 存疑。

        private int _Level = 0; // 当前类群与顶级类群的距离。
        private int _Index = -1; // 当前类群在姊妹类群中的次序。

        private List<int> _ParentsIndex = new List<int>();

        //

        public PhylogeneticTreeUnwindAtom()
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
        public TaxonomicCategory Category
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
            get
            {
                if (_Level > 1)
                {
                    StringBuilder id = new StringBuilder();

                    foreach (var parentIndex in _ParentsIndex)
                    {
                        id.Append(parentIndex.ToString());
                        id.Append('-');
                    }

                    id.Append(_Index.ToString());

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

                    _Index = int.Parse(id[_Level - 1]);
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
                BotanicalName = _BotanicalName,
                ChineseName = _ChineseName,
                Description = _Description,

                Category = _ConvertCategory(_Category),

                IsExtinct = _IsExtinct,
                IsUnsure = _IsUnsure
            };

            taxon.Synonyms.AddRange(_Synonyms);
            taxon.Tags.AddRange(_Tags);

            return taxon;
        }

        public static PhylogeneticTreeUnwindAtom FromTaxon(Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            PhylogeneticTreeUnwindAtom atom = new PhylogeneticTreeUnwindAtom()
            {
                _BotanicalName = taxon.BotanicalName,
                _ChineseName = taxon.ChineseName,
                _Synonyms = new List<string>(taxon.Synonyms),
                _Tags = new List<string>(taxon.Tags),
                _Description = taxon.Description,

                _Category = _ConvertCategory(taxon.Category),

                _IsExtinct = taxon.IsExtinct,
                _IsUnsure = taxon.IsUnsure,

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