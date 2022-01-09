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

using System.Text.Json.Serialization;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.Core.IO.Version1.Details
{
    // 系统发生树展开的表示演化关系的原子数据结构，表示一个类群。
    public sealed class EvoAtom
    {
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
        public string Rank
        {
            get => _Rank.ToString();
            set => _Rank = RankParseExtension.ParseRank(value) ?? Taxonomy.Rank.Unranked;
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
                Synonyms = _Synonyms,
                Tags = _Tags,
                Description = _Description,

                Rank = _Rank,

                IsExtinct = _IsExtinct,
                IsUndet = _IsUndet
            };

            // 匿名类群的分类阶元始终为未指定
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

                _Rank = taxon.Rank,

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