/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.415.1000.M5.201204-2200

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeOfLife
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

    // 生物分类阶元的中文名称及相关方法。
    public static class TaxonomicCategoryChineseName
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

        //

        private static Dictionary<TaxonomicCategory, string> _CategoryNameTable = null;

        private static void _EnsureCategoryNameTable()
        {
            if (_CategoryNameTable == null)
            {
                _CategoryNameTable = new Dictionary<TaxonomicCategory, string>
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
            }
        }

        public static string GetCategoryName(TaxonomicCategory category)
        {
            _EnsureCategoryNameTable();

            if (_CategoryNameTable.ContainsKey(category))
            {
                return _CategoryNameTable[category];
            }
            else
            {
                return null;
            }
        }

        //

        private static TaxonomicCategory[] _CategoryParseArray =
        {
            TaxonomicCategory.Unranked,

            TaxonomicCategory.Clade, // 演化支
            TaxonomicCategory.Clade, // 支

            TaxonomicCategory.Strain,

            TaxonomicCategory.Subform,
            TaxonomicCategory.Form,

            TaxonomicCategory.Subseries,
            TaxonomicCategory.Series,

            TaxonomicCategory.Infradivision,
            TaxonomicCategory.Subdivision,
            TaxonomicCategory.Superdivision,
            // TaxonomicCategory.Division,
            TaxonomicCategory.Clade, // "类"一般指演化支

            TaxonomicCategory.Infrasection,
            TaxonomicCategory.Subsection, // 亚派
            TaxonomicCategory.Subsection, // 亚组
            TaxonomicCategory.Subsection, // 亚节
            TaxonomicCategory.Supersection,
            TaxonomicCategory.Section, // 派
            TaxonomicCategory.Section, // 组
            TaxonomicCategory.Section, // 节

            TaxonomicCategory.Infracohort,
            TaxonomicCategory.Subcohort,
            TaxonomicCategory.Supercohort,
            TaxonomicCategory.Megacohort,
            TaxonomicCategory.Cohort,

            TaxonomicCategory.Infratribe,
            TaxonomicCategory.Subtribe,
            TaxonomicCategory.Supertribe, // 总族
            TaxonomicCategory.Supertribe, // 超族
            TaxonomicCategory.Tribe,

            TaxonomicCategory.Subvariety,
            TaxonomicCategory.Variety,
            TaxonomicCategory.Subspecies,
            TaxonomicCategory.Superspecies,
            TaxonomicCategory.Species,

            TaxonomicCategory.Infragenus,
            TaxonomicCategory.Subgenus,
            TaxonomicCategory.Genus,

            TaxonomicCategory.Infrafamily,
            TaxonomicCategory.Subfamily,
            TaxonomicCategory.Epifamily,
            TaxonomicCategory.Hyperfamily,
            TaxonomicCategory.Grandfamily,
            TaxonomicCategory.Superfamily, // 总科
            TaxonomicCategory.Superfamily, // 超科
            TaxonomicCategory.Megafamily,
            TaxonomicCategory.Gigafamily,
            TaxonomicCategory.Family,

            TaxonomicCategory.Parvorder,
            TaxonomicCategory.Infraorder,
            TaxonomicCategory.Suborder,
            TaxonomicCategory.Minorder,
            TaxonomicCategory.Hypoorder,
            TaxonomicCategory.Nanorder,
            TaxonomicCategory.Hyperorder,
            TaxonomicCategory.Grandorder,
            TaxonomicCategory.Superorder, // 总目
            TaxonomicCategory.Superorder, // 超目
            TaxonomicCategory.Megaorder,
            TaxonomicCategory.Gigaorder,
            TaxonomicCategory.Order,

            TaxonomicCategory.Parvclass,
            TaxonomicCategory.Infraclass,
            TaxonomicCategory.Subclass,
            TaxonomicCategory.Hyperclass,
            TaxonomicCategory.Grandclass,
            TaxonomicCategory.Superclass, // 总纲
            TaxonomicCategory.Superclass, // 超纲
            TaxonomicCategory.Megaclass,
            TaxonomicCategory.Class,

            TaxonomicCategory.Parvphylum,
            TaxonomicCategory.Infraphylum,
            TaxonomicCategory.Subphylum,
            TaxonomicCategory.Superphylum, // 总门
            TaxonomicCategory.Superphylum, // 超门
            TaxonomicCategory.Phylum,

            TaxonomicCategory.Infrakingdom,
            TaxonomicCategory.Subkingdom,
            TaxonomicCategory.Superkingdom,
            TaxonomicCategory.Kingdom,

            TaxonomicCategory.Superdomain,
            TaxonomicCategory.Domain
        };

        private static string[] _ChsParseArray =
        {
            Unranked,

            Clade, // 演化支
            "支",

            Strain,

            Subform,
            Form,

            Subseries,
            Series,

            Infradivision,
            Subdivision,
            Superdivision,
            // Division,
            "类", // "类"一般指演化支

            Infrasection,
            Subsection, // 亚派
            "亚组",
            "亚节",
            Supersection,
            Section, // 派
            "组",
            "节",

            Infracohort,
            Subcohort,
            Supercohort,
            Megacohort,
            Cohort,

            Infratribe,
            Subtribe,
            Supertribe, // 总族
            "超族",
            Tribe,

            Subvariety,
            Variety,
            Subspecies,
            Superspecies,
            Species,

            Infragenus,
            Subgenus,
            Genus,

            Infrafamily,
            Subfamily,
            Epifamily,
            Hyperfamily,
            Grandfamily,
            Superfamily, // 总科
            "超科",
            Megafamily,
            Gigafamily,
            Family,

            Parvorder,
            Infraorder,
            Suborder,
            Minorder,
            Hypoorder,
            Nanorder,
            Hyperorder,
            Grandorder,
            Superorder, // 总目
            "超目",
            Megaorder,
            Gigaorder,
            Order,

            Parvclass,
            Infraclass,
            Subclass,
            Hyperclass,
            Grandclass,
            Superclass, // 总纲
            "超纲",
            Megaclass,
            Class,

            Parvphylum,
            Infraphylum,
            Subphylum,
            Superphylum, // 总门
            "超门",
            Phylum,

            Infrakingdom,
            Subkingdom,
            Superkingdom,
            Kingdom,

            Superdomain,
            Domain
        };

        public static bool TryParseCategory(string name, out TaxonomicCategory category, out string parsedCategoryName)
        {
            category = TaxonomicCategory.Unranked;
            parsedCategoryName = null;

            if (!string.IsNullOrWhiteSpace(name))
            {
                for (int i = 0; i < _CategoryParseArray.Length; i++)
                {
                    if (name.EndsWith(_ChsParseArray[i]))
                    {
                        category = _CategoryParseArray[i];
                        parsedCategoryName = _ChsParseArray[i];

                        return true;
                    }
                }
            }

            return false;
        }
    }
}