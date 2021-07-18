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

using System.Drawing;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Taxonomy.Extensions
{
    // 表示如何获取父类群的选项。
    public enum GetParentsOption
    {
        EditMode, // 仅用于编辑模式

        Least, // 最简
        Summary, // 适中
        Full // 完整
    }

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
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            return taxon.GetInheritedCategory().GetThemeColor();
        }

        // 获取若干层父类群。
        public static IReadOnlyList<Taxon> GetParents(this Taxon taxon, GetParentsOption option)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            List<Taxon> result = new List<Taxon>();

            if (!taxon.IsRoot)
            {
                if (option == GetParentsOption.EditMode)
                {
                    // 上溯到任何具名分类阶元类群，保留任何类群
                    result.AddRange(taxon.GetParents(
                        TaxonFilter.Any,
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        includeTermination: true,
                        skipParaphyly: false));

                    // 如果没有上溯到任何主要分类阶元类群，直接上溯到顶级类群，保留任何类群
                    if (result.Count <= 0)
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonFilter.Any,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: false));
                    }
                }
                else if (option == GetParentsOption.Least)
                {
                    // 首先上溯到任何具名类群，保留任何具名类群，不跳过并系群
                    result.AddRange(taxon.GetParents(
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        includeTermination: true,
                        skipParaphyly: false));

                    // 如果上溯到任何未分级或演化支类群，继续上溯到任何主要或次要分类阶元类群，保留任何具名类群，跳过并系群
                    if (result.Count > 0)
                    {
                        Taxon parent = result[^1];

                        if (parent.Category.IsUnranked() || parent.Category.IsClade())
                        {
                            result.AddRange(parent.GetParents(
                                TaxonFilter.Named | TaxonFilter.AnyCategory,
                                TaxonFilter.Named | TaxonFilter.PrimaryOrSecondaryCategory,
                                includeTermination: true,
                                skipParaphyly: true));
                        }
                    }

                    // 如果上溯到任何次要分类阶元类群，继续上溯到任何基本主要分类阶元类群，保留基本次要分类阶元类群，跳过并系群
                    if (result.Count > 0)
                    {
                        Taxon parent = result[^1];

                        if (parent.Category.IsSecondaryCategory())
                        {
                            result.AddRange(parent.GetParents(
                                TaxonFilter.Named | TaxonFilter.BasicSecondaryCategory,
                                TaxonFilter.Named | TaxonFilter.BasicPrimaryCategory,
                                includeTermination: true,
                                skipParaphyly: true));
                        }
                    }

                    // 如果上溯到任何类群，继续上溯到顶级类群，保留基本主要分类阶元具名类群，跳过并系群
                    if (result.Count > 0)
                    {
                        result.AddRange(result[^1].GetParents(
                            TaxonFilter.Named | TaxonFilter.BasicPrimaryCategory,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: true));
                    }
                    // 如果没有上溯到任何类群，直接上溯到顶级类群，保留任何类群
                    else
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonFilter.Any,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: false));
                    }
                }
                else if (option == GetParentsOption.Summary)
                {
                    // 首先上溯到任何具名类群，保留任何具名类群，不跳过并系群
                    result.AddRange(taxon.GetParents(
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        includeTermination: true,
                        skipParaphyly: false));

                    // 如果上溯到任何未分级或演化支类群，继续上溯到任何主要或次要分类阶元类群，保留任何具名类群，跳过并系群
                    if (result.Count > 0)
                    {
                        Taxon parent = result[^1];

                        if (parent.Category.IsUnranked() || parent.Category.IsClade())
                        {
                            result.AddRange(parent.GetParents(
                                TaxonFilter.Named | TaxonFilter.AnyCategory,
                                TaxonFilter.Named | TaxonFilter.PrimaryOrSecondaryCategory,
                                includeTermination: true,
                                skipParaphyly: true));
                        }
                    }

                    // 如果上溯到任何类群，继续上溯到顶级类群，保留主要或次要分类阶元具名类群，跳过并系群
                    if (result.Count > 0)
                    {
                        result.AddRange(result[^1].GetParents(
                            TaxonFilter.Named | TaxonFilter.PrimaryOrSecondaryCategory,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: true));
                    }
                    // 如果没有上溯到任何类群，直接上溯到顶级类群，保留任何类群
                    else
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonFilter.Any,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: false));
                    }
                }
                else if (option == GetParentsOption.Full)
                {
                    // 上溯到顶级类群，保留任何具名类群，不跳过并系群
                    result.AddRange(taxon.GetParents(
                        TaxonFilter.Named | TaxonFilter.AnyCategory,
                        TaxonFilter.None,
                        includeTermination: true,
                        skipParaphyly: false));

                    // 如果没有上溯到任何具名类群，直接上溯到顶级类群，保留任何类群
                    if (result.Count <= 0)
                    {
                        result.AddRange(taxon.GetParents(
                            TaxonFilter.Any,
                            TaxonFilter.None,
                            includeTermination: true,
                            skipParaphyly: false));
                    }
                }
            }

            return result;
        }
    }
}