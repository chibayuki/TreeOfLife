﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.200.1000.M3.201111-0000

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

using ColorX = Com.ColorX;

namespace TreeOfLife
{
    // 生物分类单元（类群）的UI相关扩展方法。
    internal static class TaxonUIExtension
    {
        // 获取主题颜色。
        public static ColorX GetThemeColor(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            TaxonomicCategory category = taxon.GetInheritedCategory();

            if (category.IsPrimaryCategory())
            {
                double hue = 0;

                if (category.IsDomain())
                {
                    hue = 235;
                }
                else if (category.IsKingdom())
                {
                    hue = 160;
                }
                else if (category.IsPhylum())
                {
                    hue = 285;
                }
                else if (category.IsClass())
                {
                    hue = 195;
                }
                else if (category.IsOrder())
                {
                    hue = 350;
                }
                else if (category.IsFamily())
                {
                    hue = 50;
                }
                else if (category.IsGenus())
                {
                    hue = 25;
                }
                else if (category.IsSpecies())
                {
                    hue = 90;
                }

                return ColorX.FromHSL(hue, 50, 50);
            }
            else if (category.IsSecondaryCategory())
            {
                return Color.Black;
            }
            else if (category.IsClade() || category.IsUnranked())
            {
                return Color.Black;
            }
            else
            {
                return Color.Black;
            }
        }

        //

        // 获取父类群摘要。
        public static List<Taxon> GetSummaryParents(this Taxon taxon, bool editMode)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> result = new List<Taxon>();

            if (!taxon.IsRoot)
            {
                // 如果当前类群是主要分类阶元，上溯到高于该级别的主要分类阶元为止
                if (taxon.Category.IsPrimaryCategory())
                {
                    result.AddRange(taxon.GetParents(
                        TaxonParentFilterCondition.AnyTaxon(allowAnonymous: editMode, allowUnranked: true, allowClade: true),
                        recursiveInsteadOfLoop: false,
                        TaxonParentFilterTerminationCondition.UntilUplevelPrimaryCategory(taxon.Category, allowEquals: false)));
                }
                // 如果当前类群不是主要分类阶元，上溯到任何主要分类阶元为止
                else
                {
                    result.AddRange(taxon.GetParents(
                        TaxonParentFilterCondition.AnyTaxon(allowAnonymous: editMode, allowUnranked: true, allowClade: true),
                        recursiveInsteadOfLoop: false,
                        TaxonParentFilterTerminationCondition.UntilAnyPrimaryCategory()));
                }

                if (result.Count > 0)
                {
                    Taxon parent = result[result.Count - 1];

                    // 如果还未上溯到界，继续上溯到界为止，并且忽略演化支与更低的主要分类阶元
                    if (parent.Category < TaxonomicCategory.Kingdom)
                    {
                        result.AddRange(parent.GetParents(
                            TaxonParentFilterCondition.OnlyPrimaryCategory(onlyUplevel: true, allowEquals: false),
                            recursiveInsteadOfLoop: true,
                            TaxonParentFilterTerminationCondition.UntilUplevelPrimaryCategory(TaxonomicCategory.Kingdom, allowEquals: true)));
                    }
                    // 如果已经上溯到界，继续上溯到最高级别
                    else
                    {
                        result.AddRange(parent.GetParents(
                            TaxonParentFilterCondition.AnyTaxon(allowAnonymous: false, allowUnranked: true, allowClade: true),
                            recursiveInsteadOfLoop: false,
                            TaxonParentFilterTerminationCondition.UntilRoot()));
                    }
                }
                // 如果没有上溯到任何主要分类阶元，直接上溯到最高级别
                else
                {
                    result.AddRange(taxon.GetParents(
                        TaxonParentFilterCondition.AnyTaxon(allowAnonymous: editMode, allowUnranked: true, allowClade: true),
                        recursiveInsteadOfLoop: false,
                        TaxonParentFilterTerminationCondition.UntilRoot()));
                }
            }

            return result;
        }
    }
}