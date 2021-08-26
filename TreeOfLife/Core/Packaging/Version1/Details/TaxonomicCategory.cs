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

namespace TreeOfLife.Core.Packaging.Version1.Details
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
}