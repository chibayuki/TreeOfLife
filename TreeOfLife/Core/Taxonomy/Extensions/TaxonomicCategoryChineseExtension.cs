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

namespace TreeOfLife.Core.Taxonomy.Extensions
{
    // 生物分类阶元的中文相关扩展方法。
    public static class TaxonomicCategoryChineseExtension
    {
        private const int MaxLength = 3; // 中文名（后缀）的最大长度。

        //

        private static class _Names
        {
            public const string Unranked = "未分级";

            public const string Clade = "演化支";

            public const string Strain = "株";

            public const string Subform = "亚型";
            public const string Form = "型";

            public const string Subseries = "亚系";
            public const string Series = "系";

            public const string Infrasection = "下派";
            public const string Subsection = "亚派";
            public const string Section = "派";
            public const string Supersection = "总派";

            public const string Infradivision = "下类";
            public const string Subdivision = "亚类";
            public const string Division = "类";
            public const string Superdivision = "总类";

            public const string Infracohort = "下群";
            public const string Subcohort = "亚群";
            public const string Cohort = "群";
            public const string Supercohort = "总群";
            public const string Megacohort = "高群";

            public const string Infratribe = "下族";
            public const string Subtribe = "亚族";
            public const string Tribe = "族";
            public const string Supertribe = "总族";

            public const string Subvariety = "亚变种";
            public const string Variety = "变种";
            public const string Subspecies = "亚种";
            public const string Species = "种";
            public const string Superspecies = "种团";

            public const string Infragenus = "下属";
            public const string Subgenus = "亚属";
            public const string Genus = "属";

            public const string Infrafamily = "下科";
            public const string Subfamily = "亚科";
            public const string Family = "科";
            public const string Epifamily = "领科";
            public const string Hyperfamily = "上科";
            public const string Grandfamily = "大科";
            public const string Superfamily = "总科";
            public const string Megafamily = "高科";
            public const string Gigafamily = "宏科";

            public const string Parvorder = "小目";
            public const string Infraorder = "下目";
            public const string Suborder = "亚目";
            public const string Minorder = "若目";
            public const string Hypoorder = "次目";
            public const string Nanorder = "从目";
            public const string Order = "目";
            public const string Hyperorder = "上目";
            public const string Grandorder = "大目";
            public const string Superorder = "总目";
            public const string Megaorder = "高目";
            public const string Gigaorder = "宏目";

            public const string Parvclass = "小纲";
            public const string Infraclass = "下纲";
            public const string Subclass = "亚纲";
            public const string Class = "纲";
            public const string Hyperclass = "上纲";
            public const string Grandclass = "大纲";
            public const string Superclass = "总纲";
            public const string Megaclass = "高纲";

            public const string Parvphylum = "小门";
            public const string Infraphylum = "下门";
            public const string Subphylum = "亚门";
            public const string Phylum = "门";
            public const string Superphylum = "总门";

            public const string Kingdom = "界";
            public const string Superkingdom = "总界";
            public const string Infrakingdom = "下界";
            public const string Subkingdom = "亚界";

            public const string Domain = "域";
            public const string Superdomain = "总域";
        }

        //

        private static readonly Dictionary<TaxonomicCategory, string> _CategoryToNameTable = new Dictionary<TaxonomicCategory, string>()
        {
            { TaxonomicCategory.Unranked, _Names.Unranked },

            { TaxonomicCategory.Clade, _Names.Clade },

            { TaxonomicCategory.Strain, _Names.Strain },

            { TaxonomicCategory.Subform, _Names.Subform },
            { TaxonomicCategory.Form, _Names.Form },

            { TaxonomicCategory.Subseries, _Names.Subseries },
            { TaxonomicCategory.Series, _Names.Series },

            { TaxonomicCategory.Infradivision, _Names.Infradivision },
            { TaxonomicCategory.Subdivision, _Names.Subdivision },
            { TaxonomicCategory.Division, _Names.Division },
            { TaxonomicCategory.Superdivision, _Names.Superdivision },

            { TaxonomicCategory.Infrasection, _Names.Infrasection },
            { TaxonomicCategory.Subsection, _Names.Subsection },
            { TaxonomicCategory.Section, _Names.Section },
            { TaxonomicCategory.Supersection, _Names.Supersection },

            { TaxonomicCategory.Infracohort, _Names.Infracohort },
            { TaxonomicCategory.Subcohort, _Names.Subcohort },
            { TaxonomicCategory.Cohort, _Names.Cohort },
            { TaxonomicCategory.Supercohort, _Names.Supercohort },
            { TaxonomicCategory.Megacohort, _Names.Megacohort },

            { TaxonomicCategory.Infratribe, _Names.Infratribe },
            { TaxonomicCategory.Subtribe, _Names.Subtribe },
            { TaxonomicCategory.Tribe, _Names.Tribe },
            { TaxonomicCategory.Supertribe, _Names.Supertribe },

            { TaxonomicCategory.Subvariety, _Names.Subvariety },
            { TaxonomicCategory.Variety, _Names.Variety },
            { TaxonomicCategory.Subspecies, _Names.Subspecies },
            { TaxonomicCategory.Species, _Names.Species },
            { TaxonomicCategory.Superspecies, _Names.Superspecies },

            { TaxonomicCategory.Infragenus, _Names.Infragenus },
            { TaxonomicCategory.Subgenus, _Names.Subgenus },
            { TaxonomicCategory.Genus, _Names.Genus },

            { TaxonomicCategory.Infrafamily, _Names.Infrafamily },
            { TaxonomicCategory.Subfamily, _Names.Subfamily },
            { TaxonomicCategory.Family, _Names.Family },
            { TaxonomicCategory.Epifamily, _Names.Epifamily },
            { TaxonomicCategory.Hyperfamily, _Names.Hyperfamily },
            { TaxonomicCategory.Grandfamily, _Names.Grandfamily },
            { TaxonomicCategory.Superfamily, _Names.Superfamily },
            { TaxonomicCategory.Megafamily, _Names.Megafamily },
            { TaxonomicCategory.Gigafamily, _Names.Gigafamily },

            { TaxonomicCategory.Parvorder, _Names.Parvorder },
            { TaxonomicCategory.Infraorder, _Names.Infraorder },
            { TaxonomicCategory.Suborder, _Names.Suborder },
            { TaxonomicCategory.Minorder, _Names.Minorder },
            { TaxonomicCategory.Hypoorder, _Names.Hypoorder },
            { TaxonomicCategory.Nanorder, _Names.Nanorder },
            { TaxonomicCategory.Order, _Names.Order },
            { TaxonomicCategory.Hyperorder, _Names.Hyperorder },
            { TaxonomicCategory.Grandorder, _Names.Grandorder },
            { TaxonomicCategory.Superorder, _Names.Superorder },
            { TaxonomicCategory.Megaorder, _Names.Megaorder },
            { TaxonomicCategory.Gigaorder, _Names.Gigaorder },

            { TaxonomicCategory.Parvclass, _Names.Parvclass },
            { TaxonomicCategory.Infraclass, _Names.Infraclass },
            { TaxonomicCategory.Subclass, _Names.Subclass },
            { TaxonomicCategory.Class, _Names.Class },
            { TaxonomicCategory.Hyperclass, _Names.Hyperclass },
            { TaxonomicCategory.Grandclass, _Names.Grandclass },
            { TaxonomicCategory.Superclass, _Names.Superclass },
            { TaxonomicCategory.Megaclass, _Names.Megaclass },

            { TaxonomicCategory.Parvphylum, _Names.Parvphylum },
            { TaxonomicCategory.Infraphylum, _Names.Infraphylum },
            { TaxonomicCategory.Subphylum, _Names.Subphylum },
            { TaxonomicCategory.Phylum, _Names.Phylum },
            { TaxonomicCategory.Superphylum, _Names.Superphylum },

            { TaxonomicCategory.Infrakingdom, _Names.Infrakingdom },
            { TaxonomicCategory.Subkingdom, _Names.Subkingdom },
            { TaxonomicCategory.Kingdom, _Names.Kingdom },
            { TaxonomicCategory.Superkingdom, _Names.Superkingdom },

            { TaxonomicCategory.Domain, _Names.Domain },
            { TaxonomicCategory.Superdomain, _Names.Superdomain }
        };

        // 获取分类阶元的中文名。
        public static string GetChineseName(this TaxonomicCategory category)
        {
            if (_CategoryToNameTable.TryGetValue(category, out string chineseName))
            {
                return chineseName;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        //

        private static readonly Dictionary<string, TaxonomicCategory> _NameToCategoryTable = new Dictionary<string, TaxonomicCategory>()
        {
            { _Names.Unranked, TaxonomicCategory.Unranked }, // 未分级
            { "未指定", TaxonomicCategory.Unranked }, // 未指定

            { _Names.Clade, TaxonomicCategory.Clade }, // 演化支
            { "分支", TaxonomicCategory.Clade }, // 分支
            { "支", TaxonomicCategory.Clade }, // 支

            { _Names.Strain, TaxonomicCategory.Strain },

            { _Names.Subform, TaxonomicCategory.Subform },
            { _Names.Form, TaxonomicCategory.Form },

            { _Names.Subseries, TaxonomicCategory.Subseries },
            { _Names.Series, TaxonomicCategory.Series },

            { _Names.Infrasection, TaxonomicCategory.Infrasection },
            { _Names.Subsection, TaxonomicCategory.Subsection }, // 亚派
            { "亚组", TaxonomicCategory.Subsection }, // 亚组
            { "亚节", TaxonomicCategory.Subsection }, // 亚节
            { _Names.Section, TaxonomicCategory.Section }, // 派
            { "组", TaxonomicCategory.Section }, // 组
            { "节", TaxonomicCategory.Section }, // 节
            { _Names.Supersection, TaxonomicCategory.Supersection },

            { _Names.Infradivision, TaxonomicCategory.Infradivision },
            { _Names.Subdivision, TaxonomicCategory.Subdivision },
            { _Names.Division,  TaxonomicCategory.Division },
            // { "类", TaxonomicCategory.Clade }, // "类"虽然一般指演化支，但将其匹配为演化支会导致检索问题，仅在有匹配为演化支的需求处做特殊处理即可
            { _Names.Superdivision, TaxonomicCategory.Superdivision },

            { _Names.Infracohort, TaxonomicCategory.Infracohort },
            { _Names.Subcohort, TaxonomicCategory.Subcohort },
            { _Names.Cohort, TaxonomicCategory.Cohort },
            { _Names.Supercohort, TaxonomicCategory.Supercohort },
            { _Names.Megacohort, TaxonomicCategory.Megacohort },

            { _Names.Infratribe, TaxonomicCategory.Infratribe },
            { _Names.Subtribe, TaxonomicCategory.Subtribe },
            { "世系", TaxonomicCategory.Tribe }, // 世系
            { _Names.Tribe, TaxonomicCategory.Tribe },
            { _Names.Supertribe, TaxonomicCategory.Supertribe }, // 总族
            { "超族", TaxonomicCategory.Supertribe }, // 超族

            { _Names.Subvariety, TaxonomicCategory.Subvariety },
            { _Names.Variety, TaxonomicCategory.Variety },
            { _Names.Subspecies, TaxonomicCategory.Subspecies },
            { _Names.Species, TaxonomicCategory.Species },
            { _Names.Superspecies, TaxonomicCategory.Superspecies },

            { _Names.Infragenus, TaxonomicCategory.Infragenus },
            { _Names.Subgenus, TaxonomicCategory.Subgenus },
            { _Names.Genus, TaxonomicCategory.Genus },

            { _Names.Infrafamily, TaxonomicCategory.Infrafamily },
            { _Names.Subfamily, TaxonomicCategory.Subfamily },
            { _Names.Family, TaxonomicCategory.Family },
            { _Names.Epifamily, TaxonomicCategory.Epifamily },
            { _Names.Hyperfamily, TaxonomicCategory.Hyperfamily },
            { _Names.Grandfamily, TaxonomicCategory.Grandfamily },
            { _Names.Superfamily, TaxonomicCategory.Superfamily }, // 总科
            { "超科", TaxonomicCategory.Superfamily }, // 超科
            { _Names.Megafamily, TaxonomicCategory.Megafamily },
            { _Names.Gigafamily, TaxonomicCategory.Gigafamily },

            { _Names.Parvorder, TaxonomicCategory.Parvorder },
            { _Names.Infraorder, TaxonomicCategory.Infraorder },
            { _Names.Suborder, TaxonomicCategory.Suborder },
            { _Names.Minorder, TaxonomicCategory.Minorder },
            { _Names.Hypoorder, TaxonomicCategory.Hypoorder },
            { _Names.Nanorder, TaxonomicCategory.Nanorder },
            { _Names.Order, TaxonomicCategory.Order },
            { _Names.Hyperorder, TaxonomicCategory.Hyperorder },
            { _Names.Grandorder, TaxonomicCategory.Grandorder },
            { _Names.Superorder, TaxonomicCategory.Superorder }, // 总目
            { "超目", TaxonomicCategory.Superorder }, // 超目
            { _Names.Megaorder, TaxonomicCategory.Megaorder },
            { _Names.Gigaorder, TaxonomicCategory.Gigaorder },

            { _Names.Parvclass, TaxonomicCategory.Parvclass },
            { _Names.Infraclass, TaxonomicCategory.Infraclass },
            { _Names.Subclass, TaxonomicCategory.Subclass },
            { _Names.Class, TaxonomicCategory.Class },
            { _Names.Hyperclass, TaxonomicCategory.Hyperclass },
            { _Names.Grandclass, TaxonomicCategory.Grandclass },
            { _Names.Superclass, TaxonomicCategory.Superclass }, // 总纲
            { "超纲", TaxonomicCategory.Superclass }, // 超纲
            { _Names.Megaclass, TaxonomicCategory.Megaclass },

            { _Names.Parvphylum, TaxonomicCategory.Parvphylum },
            { _Names.Infraphylum, TaxonomicCategory.Infraphylum },
            { _Names.Subphylum, TaxonomicCategory.Subphylum },
            { _Names.Phylum, TaxonomicCategory.Phylum },
            { _Names.Superphylum, TaxonomicCategory.Superphylum }, // 总门
            { "超门", TaxonomicCategory.Superphylum }, // 超门

            { _Names.Infrakingdom, TaxonomicCategory.Infrakingdom },
            { _Names.Subkingdom, TaxonomicCategory.Subkingdom },
            { _Names.Kingdom, TaxonomicCategory.Kingdom },
            { _Names.Superkingdom, TaxonomicCategory.Superkingdom },

            { _Names.Domain, TaxonomicCategory.Domain },
            { _Names.Superdomain, TaxonomicCategory.Superdomain }
        };

        // 精确匹配分类阶元名称。
        public static TaxonomicCategory? ParseCategory(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length <= MaxLength && _NameToCategoryTable.TryGetValue(name, out TaxonomicCategory category))
            {
                return category;
            }
            else
            {
                return null;
            }
        }

        // 尝试匹配可能包含分类阶元名称的字符串，并输出从字符串中匹配到的表示分类阶元名称的部分的起始索引。
        public static bool TryParseCategory(string name, out TaxonomicCategory category, out int categoryNameIndex)
        {
            category = TaxonomicCategory.Unranked;
            categoryNameIndex = -1;

            if (!string.IsNullOrEmpty(name))
            {
                int len = name.Length;

                for (int i = MaxLength; i >= 1; i--)
                {
                    if (len >= i)
                    {
                        string suffix = name[(^i)..];

                        if (_NameToCategoryTable.TryGetValue(suffix, out category))
                        {
                            categoryNameIndex = len - i;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // 将类群的中文名分割为三部分，第一部分为中文名不含分类阶元的部分，第二部分为中文名表示分类阶元的部分，第三部分为分类阶元。
        public static (string headPart, string tailPart, TaxonomicCategory? category) SplitChineseName(string name)
        {
            TaxonomicCategory? categoryN = ParseCategory(name);

            if (categoryN is not null)
            {
                return (string.Empty, name, categoryN);
            }
            else
            {
                TaxonomicCategory category;
                int categoryNameIndex;

                if (TryParseCategory(name, out category, out categoryNameIndex))
                {
                    if (categoryNameIndex > 0)
                    {
                        return (name[0..categoryNameIndex], name[categoryNameIndex..], category);
                    }
                    else
                    {
                        return (string.Empty, name, category);
                    }
                }
                else
                {
                    return (name, string.Empty, null);
                }
            }
        }
    }
}