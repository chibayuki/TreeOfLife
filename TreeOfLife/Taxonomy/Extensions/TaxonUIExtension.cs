/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1030.1000.M10.210405-1400

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
        private static ColorX _DomainColor = ColorX.FromHSL(235, 50, 50);
        private static ColorX _KingdomColor = ColorX.FromHSL(160, 50, 50);
        private static ColorX _PhylumColor = ColorX.FromHSL(285, 50, 50);
        private static ColorX _ClassColor = ColorX.FromHSL(195, 50, 50);
        private static ColorX _OrderColor = ColorX.FromHSL(350, 50, 50);
        private static ColorX _FamilyColor = ColorX.FromHSL(50, 50, 50);
        private static ColorX _GenusColor = ColorX.FromHSL(25, 50, 50);
        private static ColorX _SpeciesColor = ColorX.FromHSL(90, 50, 50);
        private static ColorX _SecondaryColor = Color.Black;
        private static ColorX _OthersColor = Color.Black;

        // 获取分类阶元的主题颜色。
        public static ColorX GetThemeColor(this TaxonomicCategory category)
        {
            if (category.IsPrimaryCategory())
            {
                if (category.IsDomain()) return _DomainColor;
                else if (category.IsKingdom()) return _KingdomColor;
                else if (category.IsPhylum()) return _PhylumColor;
                else if (category.IsClass()) return _ClassColor;
                else if (category.IsOrder()) return _OrderColor;
                else if (category.IsFamily()) return _FamilyColor;
                else if (category.IsGenus()) return _GenusColor;
                else if (category.IsSpecies()) return _SpeciesColor;
            }
            else if (category.IsSecondaryCategory())
            {
                return _SecondaryColor;
            }

            return _OthersColor;
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