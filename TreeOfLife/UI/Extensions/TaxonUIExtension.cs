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

using System.Drawing;

using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Extensions
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
        public static ColorX GetThemeColor(this Rank rank)
        {
            if (rank.IsPrimaryRank())
            {
                if (rank.IsDomain()) return _DomainColor;
                else if (rank.IsKingdom()) return _KingdomColor;
                else if (rank.IsPhylum()) return _PhylumColor;
                else if (rank.IsClass()) return _ClassColor;
                else if (rank.IsOrder()) return _OrderColor;
                else if (rank.IsFamily()) return _FamilyColor;
                else if (rank.IsGenus()) return _GenusColor;
                else if (rank.IsSpecies()) return _SpeciesColor;
            }
            else if (rank.IsSecondaryRank())
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

            return taxon.GetInheritedRank().GetThemeColor();
        }

        //

        // 获取类群的短名称。
        public static string GetShortName(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.IsAnonymous)
            {
                return "(未命名)";
            }
            else
            {
                StringBuilder taxonName = new StringBuilder();

                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);

                    if (!string.IsNullOrEmpty(taxon.ScientificName))
                    {
                        taxonName.Append(' ');
                        taxonName.Append(taxon.ScientificName);
                    }
                }
                else if (!string.IsNullOrEmpty(taxon.ScientificName))
                {
                    taxonName.Append(taxon.ScientificName);
                }

                return taxonName.ToString();
            }
        }

        // 获取类群的长名称。
        public static string GetLongName(this Taxon taxon)
        {
            if (taxon is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (taxon.IsAnonymous)
            {
                return "(未命名)";
            }
            else
            {
                StringBuilder taxonName = new StringBuilder();

                if (!string.IsNullOrEmpty(taxon.ChineseName))
                {
                    taxonName.Append(taxon.ChineseName);

                    if (!string.IsNullOrEmpty(taxon.ScientificName))
                    {
                        taxonName.Append(' ');
                        taxonName.Append(taxon.ScientificName);
                    }
                }
                else
                {
                    Rank rank = taxon.Rank;

                    if (rank.IsPrimaryOrSecondaryRank())
                    {
                        taxonName.Append(rank.GetChineseName());

                        if (!string.IsNullOrEmpty(taxon.ScientificName))
                        {
                            taxonName.Append(' ');
                            taxonName.Append(taxon.ScientificName);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(taxon.ScientificName))
                        {
                            taxonName.Append(taxon.ScientificName);
                        }
                    }
                }

                return taxonName.ToString();
            }
        }
    }
}