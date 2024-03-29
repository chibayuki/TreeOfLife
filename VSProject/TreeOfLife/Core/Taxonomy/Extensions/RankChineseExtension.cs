﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类阶元的中文相关扩展方法。
    public static class RankChineseExtension
    {
        private const int _MaxLength = 3; // 中文名（后缀）的最大长度。

        //

        private static class _Names
        {
            public const string Unranked = "未指定";

            public const string Clade = "演化支";

            public const string Strain = "株";

            public const string Subform = "亚型";
            public const string Form = "型";

            public const string Subseries = "亚系";
            public const string Series = "系";

            public const string Subdivision = "亚类";
            public const string Division = "类";

            public const string Subsection = "亚派";
            public const string Section = "派";

            public const string Infracohort = "下群";
            public const string Subcohort = "亚群";
            public const string Cohort = "群";
            public const string Supercohort = "总群";
            public const string Megacohort = "高群";

            public const string Subtribe = "亚族";
            public const string Tribe = "族";

            public const string Subvariety = "亚变种";
            public const string Variety = "变种";
            public const string Subspecies = "亚种";
            public const string Species = "种";

            public const string Subgenus = "亚属";
            public const string Genus = "属";

            public const string Subfamily = "亚科";
            public const string Family = "科";
            public const string Superfamily = "总科";

            public const string Parvorder = "小目";
            public const string Infraorder = "下目";
            public const string Suborder = "亚目";
            public const string Order = "目";
            public const string Mirorder = "上目";
            public const string Grandorder = "大目";
            public const string Superorder = "总目";
            public const string Megaorder = "高目";

            public const string Parvclass = "小纲";
            public const string Infraclass = "下纲";
            public const string Subclass = "亚纲";
            public const string Class = "纲";
            public const string Superclass = "总纲";
            public const string Megaclass = "高纲";

            public const string Parvphylum = "小门";
            public const string Infraphylum = "下门";
            public const string Subphylum = "亚门";
            public const string Phylum = "门";
            public const string Superphylum = "总门";

            public const string Infrakingdom = "下界";
            public const string Subkingdom = "亚界";
            public const string Kingdom = "界";
            public const string Superkingdom = "总界";

            public const string Domain = "域";
            public const string Superdomain = "总域";
        }

        //

        private static readonly Dictionary<Rank, string> _RankToNameTable = new Dictionary<Rank, string>()
        {
            { Rank.Unranked, _Names.Unranked },

            { Rank.Clade, _Names.Clade },

            { Rank.Strain, _Names.Strain },

            { Rank.Subform, _Names.Subform },
            { Rank.Form, _Names.Form },

            { Rank.Subseries, _Names.Subseries },
            { Rank.Series, _Names.Series },

            { Rank.Subdivision, _Names.Subdivision },
            { Rank.Division, _Names.Division },

            { Rank.Subsection, _Names.Subsection },
            { Rank.Section, _Names.Section },

            { Rank.Infracohort, _Names.Infracohort },
            { Rank.Subcohort, _Names.Subcohort },
            { Rank.Cohort, _Names.Cohort },
            { Rank.Supercohort, _Names.Supercohort },
            { Rank.Megacohort, _Names.Megacohort },

            { Rank.Subtribe, _Names.Subtribe },
            { Rank.Tribe, _Names.Tribe },

            { Rank.Subvariety, _Names.Subvariety },
            { Rank.Variety, _Names.Variety },
            { Rank.Subspecies, _Names.Subspecies },
            { Rank.Species, _Names.Species },

            { Rank.Subgenus, _Names.Subgenus },
            { Rank.Genus, _Names.Genus },

            { Rank.Subfamily, _Names.Subfamily },
            { Rank.Family, _Names.Family },
            { Rank.Superfamily, _Names.Superfamily },

            { Rank.Parvorder, _Names.Parvorder },
            { Rank.Infraorder, _Names.Infraorder },
            { Rank.Suborder, _Names.Suborder },
            { Rank.Order, _Names.Order },
            { Rank.Mirorder, _Names.Mirorder },
            { Rank.Grandorder, _Names.Grandorder },
            { Rank.Superorder, _Names.Superorder },
            { Rank.Megaorder, _Names.Megaorder },

            { Rank.Parvclass, _Names.Parvclass },
            { Rank.Infraclass, _Names.Infraclass },
            { Rank.Subclass, _Names.Subclass },
            { Rank.Class, _Names.Class },
            { Rank.Superclass, _Names.Superclass },
            { Rank.Megaclass, _Names.Megaclass },

            { Rank.Parvphylum, _Names.Parvphylum },
            { Rank.Infraphylum, _Names.Infraphylum },
            { Rank.Subphylum, _Names.Subphylum },
            { Rank.Phylum, _Names.Phylum },
            { Rank.Superphylum, _Names.Superphylum },

            { Rank.Infrakingdom, _Names.Infrakingdom },
            { Rank.Subkingdom, _Names.Subkingdom },
            { Rank.Kingdom, _Names.Kingdom },
            { Rank.Superkingdom, _Names.Superkingdom },

            { Rank.Domain, _Names.Domain },
            { Rank.Superdomain, _Names.Superdomain }
        };

        // 获取分类阶元的中文名。
        public static string GetChineseName(this Rank rank)
        {
            if (_RankToNameTable.TryGetValue(rank, out string chineseName))
            {
                return chineseName;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        //

        private static readonly Dictionary<string, Rank> _NameToRankTable = new Dictionary<string, Rank>()
        {
            { _Names.Unranked, Rank.Unranked }, // 未指定

            { _Names.Clade, Rank.Clade }, // 演化支
            { "分支", Rank.Clade }, // 分支
            { "支", Rank.Clade }, // 支

            { _Names.Strain, Rank.Strain },

            { _Names.Subform, Rank.Subform },
            { _Names.Form, Rank.Form },

            { _Names.Subseries, Rank.Subseries },
            { _Names.Series, Rank.Series },

            { _Names.Subdivision, Rank.Subdivision },
            { _Names.Division,  Rank.Division },
            // { "类", TaxonomicRank.Clade }, // "类"虽然一般指演化支，但将其匹配为演化支会导致检索问题，仅在有匹配为演化支的需求处做特殊处理即可

            { _Names.Subsection, Rank.Subsection }, // 亚派
            { "亚组", Rank.Subsection }, // 亚组
            { "亚节", Rank.Subsection }, // 亚节
            { _Names.Section, Rank.Section }, // 派
            { "组", Rank.Section }, // 组
            { "节", Rank.Section }, // 节

            { _Names.Infracohort, Rank.Infracohort },
            { _Names.Subcohort, Rank.Subcohort },
            { _Names.Cohort, Rank.Cohort },
            { _Names.Supercohort, Rank.Supercohort },
            { _Names.Megacohort, Rank.Megacohort },

            { _Names.Subtribe, Rank.Subtribe },
            { _Names.Tribe, Rank.Tribe }, // 族
            { "世系", Rank.Tribe }, // 世系

            { _Names.Subvariety, Rank.Subvariety },
            { _Names.Variety, Rank.Variety },
            { _Names.Subspecies, Rank.Subspecies },
            { _Names.Species, Rank.Species },

            { _Names.Subgenus, Rank.Subgenus },
            { _Names.Genus, Rank.Genus },

            { _Names.Subfamily, Rank.Subfamily },
            { _Names.Family, Rank.Family },
            { _Names.Superfamily, Rank.Superfamily }, // 总科
            { "超科", Rank.Superfamily }, // 超科

            { _Names.Parvorder, Rank.Parvorder },
            { _Names.Infraorder, Rank.Infraorder },
            { _Names.Suborder, Rank.Suborder },
            { _Names.Order, Rank.Order },
            { _Names.Mirorder, Rank.Mirorder },
            { _Names.Grandorder, Rank.Grandorder },
            { _Names.Superorder, Rank.Superorder }, // 总目
            { "超目", Rank.Superorder }, // 超目
            { _Names.Megaorder, Rank.Megaorder },

            { _Names.Parvclass, Rank.Parvclass },
            { _Names.Infraclass, Rank.Infraclass },
            { _Names.Subclass, Rank.Subclass },
            { _Names.Class, Rank.Class },
            { _Names.Superclass, Rank.Superclass }, // 总纲
            { "超纲", Rank.Superclass }, // 超纲
            { _Names.Megaclass, Rank.Megaclass },

            { _Names.Parvphylum, Rank.Parvphylum },
            { _Names.Infraphylum, Rank.Infraphylum },
            { _Names.Subphylum, Rank.Subphylum },
            { _Names.Phylum, Rank.Phylum },
            { _Names.Superphylum, Rank.Superphylum }, // 总门
            { "超门", Rank.Superphylum }, // 超门

            { _Names.Infrakingdom, Rank.Infrakingdom },
            { _Names.Subkingdom, Rank.Subkingdom },
            { _Names.Kingdom, Rank.Kingdom },
            { _Names.Superkingdom, Rank.Superkingdom },

            { _Names.Domain, Rank.Domain },
            { _Names.Superdomain, Rank.Superdomain }
        };

        // 精确匹配分类阶元名称。
        public static Rank? ParseRank(string name)
        {
            string trimmedName = name?.Trim();

            if (!string.IsNullOrEmpty(trimmedName) && trimmedName.Length <= _MaxLength && _NameToRankTable.TryGetValue(trimmedName, out Rank rank))
            {
                return rank;
            }
            else
            {
                return null;
            }
        }

        // 尝试匹配可能包含分类阶元名称的字符串，并输出从字符串中匹配到的表示分类阶元名称的部分的起始索引。
        public static bool TryParseRank(string name, out Rank rank, out int rankNameIndex)
        {
            string trimmedName = name?.Trim();

            rank = Rank.Unranked;
            rankNameIndex = -1;

            if (!string.IsNullOrEmpty(trimmedName))
            {
                int len = trimmedName.Length;

                for (int i = _MaxLength; i >= 1; i--)
                {
                    if (len >= i)
                    {
                        string suffix = trimmedName[(^i)..];

                        if (_NameToRankTable.TryGetValue(suffix, out rank))
                        {
                            rankNameIndex = len - i;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // 将类群的中文名分割为三部分，第一部分为中文名不含分类阶元的部分，第二部分为中文名表示分类阶元的部分，第三部分为分类阶元。
        public static (string headPart, string tailPart, Rank? rank) SplitChineseName(string name)
        {
            string trimmedName = name?.Trim();
            Rank? rankN = ParseRank(trimmedName);

            if (rankN is not null)
            {
                return (string.Empty, trimmedName, rankN);
            }
            else
            {
                Rank rank;
                int rankNameIndex;

                if (TryParseRank(trimmedName, out rank, out rankNameIndex))
                {
                    if (rankNameIndex > 0)
                    {
                        return (trimmedName[..rankNameIndex].Trim(), trimmedName[rankNameIndex..], rank);
                    }
                    else
                    {
                        return (string.Empty, trimmedName, rank);
                    }
                }
                else
                {
                    return (trimmedName, string.Empty, null);
                }
            }
        }
    }
}