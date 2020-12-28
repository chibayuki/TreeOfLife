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

using System.Drawing;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 生物分类单元（类群）的UI相关扩展方法。
    public static class TaxonUIExtension
    {
        // 获取分类阶元的主题颜色。
        public static ColorX GetThemeColor(this TaxonomicCategory category)
        {
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

        // 获取类群的主题颜色。
        public static ColorX GetThemeColor(this Taxon taxon)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            return taxon.GetInheritedCategory().GetThemeColor();
        }

        // 获取父类群摘要。
        public static List<Taxon> GetSummaryParents(this Taxon taxon, bool editMode)
        {
            if (taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> result = new List<Taxon>();

            if (!taxon.IsRoot)
            {
                if (editMode)
                {
                    // 上溯到任何主要分类阶元为止
                    result.AddRange(taxon.GetParents(
                        TaxonParentFilterCondition.AnyTaxon(allowAnonymous: true, allowUnranked: true, allowClade: true),
                        recursiveInsteadOfLoop: false,
                        TaxonParentFilterTerminationCondition.UntilAnyPrimaryCategory()));

                    // 如果没有上溯到任何主要分类阶元，直接上溯到最高级别
                    if (result.Count <= 0)
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonParentFilterCondition.AnyTaxon(allowAnonymous: true, allowUnranked: true, allowClade: true),
                            recursiveInsteadOfLoop: false,
                            TaxonParentFilterTerminationCondition.UntilRoot()));
                    }
                }
                else
                {
                    // 如果当前类群是主要分类阶元，上溯到高于该级别的主要分类阶元为止
                    if (taxon.Category.IsPrimaryCategory())
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonParentFilterCondition.AnyTaxon(allowAnonymous: false, allowUnranked: true, allowClade: true),
                            recursiveInsteadOfLoop: false,
                            TaxonParentFilterTerminationCondition.UntilUplevelPrimaryCategory(taxon.Category, allowEquals: false)));
                    }
                    // 如果当前类群不是主要分类阶元，上溯到任何主要分类阶元为止
                    else
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonParentFilterCondition.AnyTaxon(allowAnonymous: false, allowUnranked: true, allowClade: true),
                            recursiveInsteadOfLoop: false,
                            TaxonParentFilterTerminationCondition.UntilAnyPrimaryCategory()));
                    }

                    // 如果上溯到任何主要分类阶元，继续上溯到最高级别，并且忽略演化支与更低的主要分类阶元
                    if (result.Count > 0)
                    {
                        Taxon parent = result[^1];

                        result.AddRange(parent.GetParents(
                           TaxonParentFilterCondition.OnlyPrimaryCategory(onlyUplevel: true, allowEquals: false),
                           recursiveInsteadOfLoop: true,
                           TaxonParentFilterTerminationCondition.UntilRoot()));
                    }
                    // 如果没有上溯到任何主要分类阶元，直接上溯到最高级别
                    else
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonParentFilterCondition.AnyTaxon(allowAnonymous: false, allowUnranked: true, allowClade: true),
                            recursiveInsteadOfLoop: false,
                            TaxonParentFilterTerminationCondition.UntilRoot()));
                    }
                }
            }

            return result;
        }
    }
}