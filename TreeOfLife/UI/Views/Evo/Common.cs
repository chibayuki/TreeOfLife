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

using System.Windows;
using System.Windows.Controls;

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Controls;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Views.Evo
{
    public static class Common
    {
        // 更新类群列表（并按分类阶元分组）。
        public static void UpdateTaxonListAndGroupByRank(TaxonButtonGroup control, IReadOnlyList<Taxon> taxons, ContextMenu contextMenu = null)
        {
            var groups = new List<(string groupName, ColorX groupColor, IEnumerable<TaxonItem> items)>();

            int groupIndex = 0;
            Rank rankOfGroup = Rank.Unranked;

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i];

                if (i == 0)
                {
                    rankOfGroup = taxon.Rank.BasicRank();

                    groups.Add((rankOfGroup.IsPrimaryOrSecondaryRank() ? rankOfGroup.GetChineseName() : string.Empty, taxon.GetThemeColor(), new List<TaxonItem>()));
                }
                else
                {
                    Rank basicRank = taxon.GetInheritedBasicRank();

                    if (rankOfGroup != basicRank)
                    {
                        rankOfGroup = basicRank;

                        groups.Add((rankOfGroup.GetChineseName(), taxon.GetThemeColor(), new List<TaxonItem>()));

                        groupIndex++;
                    }
                }

                ((List<TaxonItem>)groups[groupIndex].items).Add(new TaxonItem()
                {
                    Taxon = taxon,
                    IsChecked = taxon == Views.Common.CurrentTaxon,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(30, groups);
        }

        // 更新类群列表。
        public static void UpdateTaxonList(TaxonButtonGroup control, IReadOnlyList<Taxon> taxons, ContextMenu contextMenu = null)
        {
            List<TaxonItem> items = new List<TaxonItem>();

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i];

                items.Add(new TaxonItem()
                {
                    Taxon = taxon,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(items);
        }

        // 更新类群列表（并标记类群的符号）。
        public static void UpdateTaxonList(TaxonButtonGroup control, IReadOnlyList<(Taxon taxon, int sign)> taxons, ContextMenu contextMenu = null)
        {
            List<TaxonItem> items = new List<TaxonItem>();

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i].taxon;
                int sign = taxons[i].sign;

                items.Add(new TaxonItem()
                {
                    Taxon = taxon,
                    Sign = sign,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(items);
        }
    }
}