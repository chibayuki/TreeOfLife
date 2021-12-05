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

namespace TreeOfLife.Core.IO.Version4.Details
{
    // 生物分类阶元（Version 2）。
    public enum Rank
    {
        Unranked = 0, // 未指定。

        Clade = 1, // 演化支。

        // 次要分类阶元：

        Strain = 2, // 株。

        Subform = 4, // 亚型。
        Form, // 型。

        Subseries = 8, // 亚系。
        Series, // 系。

        Subdivision = 16, // 亚类。
        Division, // 类。

        Subsection = 32, // 亚派/亚组/亚节。
        Section, // 派/组/节。

        Infracohort = 64, // 下群。
        Subcohort, // 亚群。
        Cohort, // 群。
        Supercohort, // 总群。
        Megacohort, // 高群。

        Subtribe = 128, // 亚族。
        Tribe, // 族。

        // 主要分类阶元：

        Subvariety = 256, // 亚变种。
        Variety, // 变种。
        Subspecies, // 亚种。
        Species, // 种。

        Subgenus = 512, // 亚属。
        Genus, // 属。

        Subfamily = 1024, // 亚科。
        Family, // 科。
        Superfamily, // 总科/超科。

        Parvorder = 2048, // 小目。
        Infraorder, // 下目。
        Suborder, // 亚目。
        Order, // 目。
        Mirorder, // 上目。
        Grandorder, // 大目。
        Superorder, // 总目/超目。
        Megaorder, // 高目。

        Parvclass = 4096, // 小纲。
        Infraclass, // 下纲。
        Subclass, // 亚纲。
        Class, // 纲。
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