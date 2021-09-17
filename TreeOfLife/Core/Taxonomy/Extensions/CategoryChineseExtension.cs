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
    public static class CategoryChineseExtension
    {
        private const int _MaxLength = 3; // 中文名（后缀）的最大长度。

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

        private static readonly Dictionary<Category, string> _CategoryToNameTable = new Dictionary<Category, string>()
        {
            { Category.Unranked, _Names.Unranked },

            { Category.Clade, _Names.Clade },

            { Category.Strain, _Names.Strain },

            { Category.Subform, _Names.Subform },
            { Category.Form, _Names.Form },

            { Category.Subseries, _Names.Subseries },
            { Category.Series, _Names.Series },

            { Category.Infradivision, _Names.Infradivision },
            { Category.Subdivision, _Names.Subdivision },
            { Category.Division, _Names.Division },
            { Category.Superdivision, _Names.Superdivision },

            { Category.Infrasection, _Names.Infrasection },
            { Category.Subsection, _Names.Subsection },
            { Category.Section, _Names.Section },
            { Category.Supersection, _Names.Supersection },

            { Category.Infracohort, _Names.Infracohort },
            { Category.Subcohort, _Names.Subcohort },
            { Category.Cohort, _Names.Cohort },
            { Category.Supercohort, _Names.Supercohort },
            { Category.Megacohort, _Names.Megacohort },

            { Category.Infratribe, _Names.Infratribe },
            { Category.Subtribe, _Names.Subtribe },
            { Category.Tribe, _Names.Tribe },
            { Category.Supertribe, _Names.Supertribe },

            { Category.Subvariety, _Names.Subvariety },
            { Category.Variety, _Names.Variety },
            { Category.Subspecies, _Names.Subspecies },
            { Category.Species, _Names.Species },
            { Category.Superspecies, _Names.Superspecies },

            { Category.Infragenus, _Names.Infragenus },
            { Category.Subgenus, _Names.Subgenus },
            { Category.Genus, _Names.Genus },

            { Category.Infrafamily, _Names.Infrafamily },
            { Category.Subfamily, _Names.Subfamily },
            { Category.Family, _Names.Family },
            { Category.Epifamily, _Names.Epifamily },
            { Category.Hyperfamily, _Names.Hyperfamily },
            { Category.Grandfamily, _Names.Grandfamily },
            { Category.Superfamily, _Names.Superfamily },
            { Category.Megafamily, _Names.Megafamily },
            { Category.Gigafamily, _Names.Gigafamily },

            { Category.Parvorder, _Names.Parvorder },
            { Category.Infraorder, _Names.Infraorder },
            { Category.Suborder, _Names.Suborder },
            { Category.Minorder, _Names.Minorder },
            { Category.Hypoorder, _Names.Hypoorder },
            { Category.Nanorder, _Names.Nanorder },
            { Category.Order, _Names.Order },
            { Category.Hyperorder, _Names.Hyperorder },
            { Category.Grandorder, _Names.Grandorder },
            { Category.Superorder, _Names.Superorder },
            { Category.Megaorder, _Names.Megaorder },
            { Category.Gigaorder, _Names.Gigaorder },

            { Category.Parvclass, _Names.Parvclass },
            { Category.Infraclass, _Names.Infraclass },
            { Category.Subclass, _Names.Subclass },
            { Category.Class, _Names.Class },
            { Category.Hyperclass, _Names.Hyperclass },
            { Category.Grandclass, _Names.Grandclass },
            { Category.Superclass, _Names.Superclass },
            { Category.Megaclass, _Names.Megaclass },

            { Category.Parvphylum, _Names.Parvphylum },
            { Category.Infraphylum, _Names.Infraphylum },
            { Category.Subphylum, _Names.Subphylum },
            { Category.Phylum, _Names.Phylum },
            { Category.Superphylum, _Names.Superphylum },

            { Category.Infrakingdom, _Names.Infrakingdom },
            { Category.Subkingdom, _Names.Subkingdom },
            { Category.Kingdom, _Names.Kingdom },
            { Category.Superkingdom, _Names.Superkingdom },

            { Category.Domain, _Names.Domain },
            { Category.Superdomain, _Names.Superdomain }
        };

        // 获取分类阶元的中文名。
        public static string GetChineseName(this Category category)
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

        private static readonly Dictionary<string, Category> _NameToCategoryTable = new Dictionary<string, Category>()
        {
            { _Names.Unranked, Category.Unranked }, // 未分级
            { "未指定", Category.Unranked }, // 未指定

            { _Names.Clade, Category.Clade }, // 演化支
            { "分支", Category.Clade }, // 分支
            { "支", Category.Clade }, // 支

            { _Names.Strain, Category.Strain },

            { _Names.Subform, Category.Subform },
            { _Names.Form, Category.Form },

            { _Names.Subseries, Category.Subseries },
            { _Names.Series, Category.Series },

            { _Names.Infrasection, Category.Infrasection },
            { _Names.Subsection, Category.Subsection }, // 亚派
            { "亚组", Category.Subsection }, // 亚组
            { "亚节", Category.Subsection }, // 亚节
            { _Names.Section, Category.Section }, // 派
            { "组", Category.Section }, // 组
            { "节", Category.Section }, // 节
            { _Names.Supersection, Category.Supersection },

            { _Names.Infradivision, Category.Infradivision },
            { _Names.Subdivision, Category.Subdivision },
            { _Names.Division,  Category.Division },
            // { "类", TaxonomicCategory.Clade }, // "类"虽然一般指演化支，但将其匹配为演化支会导致检索问题，仅在有匹配为演化支的需求处做特殊处理即可
            { _Names.Superdivision, Category.Superdivision },

            { _Names.Infracohort, Category.Infracohort },
            { _Names.Subcohort, Category.Subcohort },
            { _Names.Cohort, Category.Cohort },
            { _Names.Supercohort, Category.Supercohort },
            { _Names.Megacohort, Category.Megacohort },

            { _Names.Infratribe, Category.Infratribe },
            { _Names.Subtribe, Category.Subtribe },
            { "世系", Category.Tribe }, // 世系
            { _Names.Tribe, Category.Tribe },
            { _Names.Supertribe, Category.Supertribe }, // 总族
            { "超族", Category.Supertribe }, // 超族

            { _Names.Subvariety, Category.Subvariety },
            { _Names.Variety, Category.Variety },
            { _Names.Subspecies, Category.Subspecies },
            { _Names.Species, Category.Species },
            { _Names.Superspecies, Category.Superspecies },

            { _Names.Infragenus, Category.Infragenus },
            { _Names.Subgenus, Category.Subgenus },
            { _Names.Genus, Category.Genus },

            { _Names.Infrafamily, Category.Infrafamily },
            { _Names.Subfamily, Category.Subfamily },
            { _Names.Family, Category.Family },
            { _Names.Epifamily, Category.Epifamily },
            { _Names.Hyperfamily, Category.Hyperfamily },
            { _Names.Grandfamily, Category.Grandfamily },
            { _Names.Superfamily, Category.Superfamily }, // 总科
            { "超科", Category.Superfamily }, // 超科
            { _Names.Megafamily, Category.Megafamily },
            { _Names.Gigafamily, Category.Gigafamily },

            { _Names.Parvorder, Category.Parvorder },
            { _Names.Infraorder, Category.Infraorder },
            { _Names.Suborder, Category.Suborder },
            { _Names.Minorder, Category.Minorder },
            { _Names.Hypoorder, Category.Hypoorder },
            { _Names.Nanorder, Category.Nanorder },
            { _Names.Order, Category.Order },
            { _Names.Hyperorder, Category.Hyperorder },
            { _Names.Grandorder, Category.Grandorder },
            { _Names.Superorder, Category.Superorder }, // 总目
            { "超目", Category.Superorder }, // 超目
            { _Names.Megaorder, Category.Megaorder },
            { _Names.Gigaorder, Category.Gigaorder },

            { _Names.Parvclass, Category.Parvclass },
            { _Names.Infraclass, Category.Infraclass },
            { _Names.Subclass, Category.Subclass },
            { _Names.Class, Category.Class },
            { _Names.Hyperclass, Category.Hyperclass },
            { _Names.Grandclass, Category.Grandclass },
            { _Names.Superclass, Category.Superclass }, // 总纲
            { "超纲", Category.Superclass }, // 超纲
            { _Names.Megaclass, Category.Megaclass },

            { _Names.Parvphylum, Category.Parvphylum },
            { _Names.Infraphylum, Category.Infraphylum },
            { _Names.Subphylum, Category.Subphylum },
            { _Names.Phylum, Category.Phylum },
            { _Names.Superphylum, Category.Superphylum }, // 总门
            { "超门", Category.Superphylum }, // 超门

            { _Names.Infrakingdom, Category.Infrakingdom },
            { _Names.Subkingdom, Category.Subkingdom },
            { _Names.Kingdom, Category.Kingdom },
            { _Names.Superkingdom, Category.Superkingdom },

            { _Names.Domain, Category.Domain },
            { _Names.Superdomain, Category.Superdomain }
        };

        // 精确匹配分类阶元名称。
        public static Category? ParseCategory(string name)
        {
            if (!string.IsNullOrEmpty(name) && name.Length <= _MaxLength && _NameToCategoryTable.TryGetValue(name, out Category category))
            {
                return category;
            }
            else
            {
                return null;
            }
        }

        // 尝试匹配可能包含分类阶元名称的字符串，并输出从字符串中匹配到的表示分类阶元名称的部分的起始索引。
        public static bool TryParseCategory(string name, out Category category, out int categoryNameIndex)
        {
            category = Category.Unranked;
            categoryNameIndex = -1;

            if (!string.IsNullOrEmpty(name))
            {
                int len = name.Length;

                for (int i = _MaxLength; i >= 1; i--)
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
        public static (string headPart, string tailPart, Category? category) SplitChineseName(string name)
        {
            Category? categoryN = ParseCategory(name);

            if (categoryN is not null)
            {
                return (string.Empty, name, categoryN);
            }
            else
            {
                Category category;
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