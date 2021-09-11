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
        public static void UpdateTaxonListAndGroupByCategory(TaxonNameButtonGroup control, IReadOnlyList<Taxon> taxons, ContextMenu contextMenu = null)
        {
            var groups = new List<(string groupName, ColorX groupColor, IEnumerable<TaxonNameItem> items)>();

            int groupIndex = 0;
            TaxonomicCategory categoryOfGroup = TaxonomicCategory.Unranked;

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i];

                if (i == 0)
                {
                    categoryOfGroup = taxon.Category.BasicCategory();

                    groups.Add((categoryOfGroup.IsPrimaryOrSecondaryCategory() ? categoryOfGroup.GetChineseName() : string.Empty, taxon.GetThemeColor(), new List<TaxonNameItem>()));
                }
                else
                {
                    TaxonomicCategory basicCategory = taxon.GetInheritedBasicCategory();

                    if (categoryOfGroup != basicCategory)
                    {
                        categoryOfGroup = basicCategory;

                        groups.Add((categoryOfGroup.GetChineseName(), taxon.GetThemeColor(), new List<TaxonNameItem>()));

                        groupIndex++;
                    }
                }

                ((List<TaxonNameItem>)groups[groupIndex].items).Add(new TaxonNameItem()
                {
                    Taxon = taxon,
                    IsChecked = taxon == Views.Common.CurrentTaxon,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(30, groups);
        }

        // 更新类群列表。
        public static void UpdateTaxonList(TaxonNameButtonGroup control, IReadOnlyList<Taxon> taxons, ContextMenu contextMenu = null)
        {
            List<TaxonNameItem> items = new List<TaxonNameItem>();

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i];

                items.Add(new TaxonNameItem()
                {
                    Taxon = taxon,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(items);
        }

        // 更新类群列表（并标记类群的符号）。
        public static void UpdateTaxonList(TaxonNameButtonGroup control, IReadOnlyList<(Taxon taxon, int sign)> taxons, ContextMenu contextMenu = null)
        {
            List<TaxonNameItem> items = new List<TaxonNameItem>();

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i].taxon;
                int sign = taxons[i].sign;

                items.Add(new TaxonNameItem()
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