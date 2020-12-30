/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.708.1000.M7.201230-2100

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类阶元的中文相关扩展方法。
    public static class TaxonomicCategoryChineseExtension
    {
        private const int MaxLength = 3; // 中文名（后缀）的最大长度。

        //

        private const string Unranked = "未分级";

        private const string Clade = "演化支";

        private const string Strain = "株";

        private const string Subform = "亚型";
        private const string Form = "型";

        private const string Subseries = "亚系";
        private const string Series = "系";

        private const string Infrasection = "下派";
        private const string Subsection = "亚派";
        private const string Section = "派";
        private const string Supersection = "总派";

        private const string Infradivision = "下类";
        private const string Subdivision = "亚类";
        private const string Division = "类";
        private const string Superdivision = "总类";

        private const string Infracohort = "下群";
        private const string Subcohort = "亚群";
        private const string Cohort = "群";
        private const string Supercohort = "总群";
        private const string Megacohort = "高群";

        private const string Infratribe = "下族";
        private const string Subtribe = "亚族";
        private const string Tribe = "族";
        private const string Supertribe = "总族";

        private const string Subvariety = "亚变种";
        private const string Variety = "变种";
        private const string Subspecies = "亚种";
        private const string Species = "种";
        private const string Superspecies = "种团";

        private const string Infragenus = "下属";
        private const string Subgenus = "亚属";
        private const string Genus = "属";

        private const string Infrafamily = "下科";
        private const string Subfamily = "亚科";
        private const string Family = "科";
        private const string Epifamily = "领科";
        private const string Hyperfamily = "上科";
        private const string Grandfamily = "大科";
        private const string Superfamily = "总科";
        private const string Megafamily = "高科";
        private const string Gigafamily = "宏科";

        private const string Parvorder = "小目";
        private const string Infraorder = "下目";
        private const string Suborder = "亚目";
        private const string Minorder = "若目";
        private const string Hypoorder = "次目";
        private const string Nanorder = "从目";
        private const string Order = "目";
        private const string Hyperorder = "上目";
        private const string Grandorder = "大目";
        private const string Superorder = "总目";
        private const string Megaorder = "高目";
        private const string Gigaorder = "宏目";

        private const string Parvclass = "小纲";
        private const string Infraclass = "下纲";
        private const string Subclass = "亚纲";
        private const string Class = "纲";
        private const string Hyperclass = "上纲";
        private const string Grandclass = "大纲";
        private const string Superclass = "总纲";
        private const string Megaclass = "高纲";

        private const string Parvphylum = "小门";
        private const string Infraphylum = "下门";
        private const string Subphylum = "亚门";
        private const string Phylum = "门";
        private const string Superphylum = "总门";

        private const string Kingdom = "界";
        private const string Superkingdom = "总界";
        private const string Infrakingdom = "下界";
        private const string Subkingdom = "亚界";

        private const string Domain = "域";
        private const string Superdomain = "总域";

        //

        private static Dictionary<TaxonomicCategory, string> _CategoryToNameTable = new Dictionary<TaxonomicCategory, string>()
        {
            { TaxonomicCategory.Unranked, Unranked },

            { TaxonomicCategory.Clade, Clade },

            { TaxonomicCategory.Strain, Strain },

            { TaxonomicCategory.Subform, Subform },
            { TaxonomicCategory.Form, Form },

            { TaxonomicCategory.Subseries, Subseries },
            { TaxonomicCategory.Series, Series },

            { TaxonomicCategory.Infradivision, Infradivision },
            { TaxonomicCategory.Subdivision, Subdivision },
            { TaxonomicCategory.Division, Division },
            { TaxonomicCategory.Superdivision, Superdivision },

            { TaxonomicCategory.Infrasection, Infrasection },
            { TaxonomicCategory.Subsection, Subsection },
            { TaxonomicCategory.Section, Section },
            { TaxonomicCategory.Supersection, Supersection },

            { TaxonomicCategory.Infracohort, Infracohort },
            { TaxonomicCategory.Subcohort, Subcohort },
            { TaxonomicCategory.Cohort, Cohort },
            { TaxonomicCategory.Supercohort, Supercohort },
            { TaxonomicCategory.Megacohort, Megacohort },

            { TaxonomicCategory.Infratribe, Infratribe },
            { TaxonomicCategory.Subtribe, Subtribe },
            { TaxonomicCategory.Tribe, Tribe },
            { TaxonomicCategory.Supertribe, Supertribe },

            { TaxonomicCategory.Subvariety, Subvariety },
            { TaxonomicCategory.Variety, Variety },
            { TaxonomicCategory.Subspecies, Subspecies },
            { TaxonomicCategory.Species, Species },
            { TaxonomicCategory.Superspecies, Superspecies },

            { TaxonomicCategory.Infragenus, Infragenus },
            { TaxonomicCategory.Subgenus, Subgenus },
            { TaxonomicCategory.Genus, Genus },

            { TaxonomicCategory.Infrafamily, Infrafamily },
            { TaxonomicCategory.Subfamily, Subfamily },
            { TaxonomicCategory.Family, Family },
            { TaxonomicCategory.Epifamily, Epifamily },
            { TaxonomicCategory.Hyperfamily, Hyperfamily },
            { TaxonomicCategory.Grandfamily, Grandfamily },
            { TaxonomicCategory.Superfamily, Superfamily },
            { TaxonomicCategory.Megafamily, Megafamily },
            { TaxonomicCategory.Gigafamily, Gigafamily },

            { TaxonomicCategory.Parvorder, Parvorder },
            { TaxonomicCategory.Infraorder, Infraorder },
            { TaxonomicCategory.Suborder, Suborder },
            { TaxonomicCategory.Minorder, Minorder },
            { TaxonomicCategory.Hypoorder, Hypoorder },
            { TaxonomicCategory.Nanorder, Nanorder },
            { TaxonomicCategory.Order, Order },
            { TaxonomicCategory.Hyperorder, Hyperorder },
            { TaxonomicCategory.Grandorder, Grandorder },
            { TaxonomicCategory.Superorder, Superorder },
            { TaxonomicCategory.Megaorder, Megaorder },
            { TaxonomicCategory.Gigaorder, Gigaorder },

            { TaxonomicCategory.Parvclass, Parvclass },
            { TaxonomicCategory.Infraclass, Infraclass },
            { TaxonomicCategory.Subclass, Subclass },
            { TaxonomicCategory.Class, Class },
            { TaxonomicCategory.Hyperclass, Hyperclass },
            { TaxonomicCategory.Grandclass, Grandclass },
            { TaxonomicCategory.Superclass, Superclass },
            { TaxonomicCategory.Megaclass, Megaclass },

            { TaxonomicCategory.Parvphylum, Parvphylum },
            { TaxonomicCategory.Infraphylum, Infraphylum },
            { TaxonomicCategory.Subphylum, Subphylum },
            { TaxonomicCategory.Phylum, Phylum },
            { TaxonomicCategory.Superphylum, Superphylum },

            { TaxonomicCategory.Infrakingdom, Infrakingdom },
            { TaxonomicCategory.Subkingdom, Subkingdom },
            { TaxonomicCategory.Kingdom, Kingdom },
            { TaxonomicCategory.Superkingdom, Superkingdom },

            { TaxonomicCategory.Domain, Domain },
            { TaxonomicCategory.Superdomain, Superdomain }
        };

        // 获取分类阶元的名称。
        public static string GetChineseName(this TaxonomicCategory category)
        {
            if (_CategoryToNameTable.ContainsKey(category))
            {
                return _CategoryToNameTable[category];
            }
            else
            {
                return null;
            }
        }

        //

        private static Dictionary<string, TaxonomicCategory> _NameToCategoryTable = new Dictionary<string, TaxonomicCategory>()
        {
            { Unranked, TaxonomicCategory.Unranked },

            { Clade, TaxonomicCategory.Clade }, // 演化支
            { "支", TaxonomicCategory.Clade }, // 支

            { Strain, TaxonomicCategory.Strain },

            { Subform, TaxonomicCategory.Subform },
            { Form, TaxonomicCategory.Form },

            { Subseries, TaxonomicCategory.Subseries },
            { Series, TaxonomicCategory.Series },

            { Infrasection, TaxonomicCategory.Infrasection },
            { Subsection, TaxonomicCategory.Subsection }, // 亚派
            { "亚组", TaxonomicCategory.Subsection }, // 亚组
            { "亚节", TaxonomicCategory.Subsection }, // 亚节
            { Section, TaxonomicCategory.Section }, // 派
            { "组", TaxonomicCategory.Section }, // 组
            { "节", TaxonomicCategory.Section }, // 节
            { Supersection, TaxonomicCategory.Supersection },

            { Infradivision, TaxonomicCategory.Infradivision },
            { Subdivision, TaxonomicCategory.Subdivision },
            // { Division,  TaxonomicCategory.Division },
            { "类", TaxonomicCategory.Clade }, // "类"一般指演化支
            { Superdivision, TaxonomicCategory.Superdivision },

            { Infracohort, TaxonomicCategory.Infracohort },
            { Subcohort, TaxonomicCategory.Subcohort },
            { Cohort, TaxonomicCategory.Cohort },
            { Supercohort, TaxonomicCategory.Supercohort },
            { Megacohort, TaxonomicCategory.Megacohort },

            { Infratribe, TaxonomicCategory.Infratribe },
            { Subtribe, TaxonomicCategory.Subtribe },
            { Tribe, TaxonomicCategory.Tribe },
            { Supertribe, TaxonomicCategory.Supertribe }, // 总族
            { "超族", TaxonomicCategory.Supertribe }, // 超族

            { Subvariety, TaxonomicCategory.Subvariety },
            { Variety, TaxonomicCategory.Variety },
            { Subspecies, TaxonomicCategory.Subspecies },
            { Species, TaxonomicCategory.Species },
            { Superspecies, TaxonomicCategory.Superspecies },

            { Infragenus, TaxonomicCategory.Infragenus },
            { Subgenus, TaxonomicCategory.Subgenus },
            { Genus, TaxonomicCategory.Genus },

            { Infrafamily, TaxonomicCategory.Infrafamily },
            { Subfamily, TaxonomicCategory.Subfamily },
            { Family, TaxonomicCategory.Family },
            { Epifamily, TaxonomicCategory.Epifamily },
            { Hyperfamily, TaxonomicCategory.Hyperfamily },
            { Grandfamily, TaxonomicCategory.Grandfamily },
            { Superfamily, TaxonomicCategory.Superfamily }, // 总科
            { "超科", TaxonomicCategory.Superfamily }, // 超科
            { Megafamily, TaxonomicCategory.Megafamily },
            { Gigafamily, TaxonomicCategory.Gigafamily },

            { Parvorder, TaxonomicCategory.Parvorder },
            { Infraorder, TaxonomicCategory.Infraorder },
            { Suborder, TaxonomicCategory.Suborder },
            { Minorder, TaxonomicCategory.Minorder },
            { Hypoorder, TaxonomicCategory.Hypoorder },
            { Nanorder, TaxonomicCategory.Nanorder },
            { Order, TaxonomicCategory.Order },
            { Hyperorder, TaxonomicCategory.Hyperorder },
            { Grandorder, TaxonomicCategory.Grandorder },
            { Superorder, TaxonomicCategory.Superorder }, // 总目
            { "超目", TaxonomicCategory.Superorder }, // 超目
            { Megaorder, TaxonomicCategory.Megaorder },
            { Gigaorder, TaxonomicCategory.Gigaorder },

            { Parvclass, TaxonomicCategory.Parvclass },
            { Infraclass, TaxonomicCategory.Infraclass },
            { Subclass, TaxonomicCategory.Subclass },
            { Class, TaxonomicCategory.Class },
            { Hyperclass, TaxonomicCategory.Hyperclass },
            { Grandclass, TaxonomicCategory.Grandclass },
            { Superclass, TaxonomicCategory.Superclass }, // 总纲
            { "超纲", TaxonomicCategory.Superclass }, // 超纲
            { Megaclass, TaxonomicCategory.Megaclass },

            { Parvphylum, TaxonomicCategory.Parvphylum },
            { Infraphylum, TaxonomicCategory.Infraphylum },
            { Subphylum, TaxonomicCategory.Subphylum },
            { Phylum, TaxonomicCategory.Phylum },
            { Superphylum, TaxonomicCategory.Superphylum }, // 总门
            { "超门", TaxonomicCategory.Superphylum }, // 超门

            { Infrakingdom, TaxonomicCategory.Infrakingdom },
            { Subkingdom, TaxonomicCategory.Subkingdom },
            { Kingdom, TaxonomicCategory.Kingdom },
            { Superkingdom, TaxonomicCategory.Superkingdom },

            { Domain, TaxonomicCategory.Domain },
            { Superdomain, TaxonomicCategory.Superdomain }
        };

        // 精确匹配分类阶元名称。
        public static TaxonomicCategory? ParseCategory(string name)
        {
            TaxonomicCategory category;

            if (!string.IsNullOrWhiteSpace(name) && name.Length <= MaxLength && _NameToCategoryTable.TryGetValue(name, out category))
            {
                return category;
            }
            else
            {
                return null;
            }
        }

        // 尝试匹配可能包含分类阶元名称的字符串，并输出从字符串中匹配到的表示分类阶元名称的部分。
        public static bool TryParseCategory(string name, out TaxonomicCategory category, out string parsedCategoryName)
        {
            category = TaxonomicCategory.Unranked;
            parsedCategoryName = null;

            if (!string.IsNullOrWhiteSpace(name))
            {
                int len = name.Length;

                for (int i = MaxLength; i >= 1; i--)
                {
                    if (len >= i)
                    {
                        string suffix = name[(^i)..];

                        if (_NameToCategoryTable.TryGetValue(suffix, out category))
                        {
                            parsedCategoryName = suffix;

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}