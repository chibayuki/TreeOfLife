/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1000.1000.M10.210130-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Windows;
using System.Windows.Controls;

using TreeOfLife.Controls;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Views.Evo
{
    public static class Common
    {
        // 更新父类群控件。
        public static void UpdateParents(TaxonNameButtonGroup control, IReadOnlyList<Taxon> parents, ContextMenu contextMenu = null)
        {
            var groups = new List<(string groupName, ColorX groupColor, IEnumerable<TaxonNameItem> items)>();

            int groupIndex = 0;
            TaxonomicCategory categoryOfGroup = TaxonomicCategory.Unranked;

            for (int i = 0; i < parents.Count; i++)
            {
                Taxon taxon = parents[i];

                if (i == 0)
                {
                    categoryOfGroup = taxon.Category.BasicCategory();

                    groups.Add((((categoryOfGroup.IsPrimaryCategory() || categoryOfGroup.IsSecondaryCategory()) ? categoryOfGroup.GetChineseName() : string.Empty), taxon.GetThemeColor(), new List<TaxonNameItem>()));
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
                    Checked = (taxon == Views.Common.CurrentTaxon),
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(30, groups);
        }

        // 更新子类群控件。
        public static void UpdateChildren(TaxonNameButtonGroup control, IReadOnlyList<Taxon> children, ContextMenu contextMenu = null)
        {
            List<TaxonNameItem> items = new List<TaxonNameItem>();

            for (int i = 0; i < children.Count; i++)
            {
                Taxon taxon = children[i];

                items.Add(new TaxonNameItem()
                {
                    Taxon = taxon,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(items);
        }

        // 更新子类群控件。
        public static void UpdateChildren(TaxonNameButtonGroup control, IReadOnlyList<(Taxon taxon, int sign)> children, ContextMenu contextMenu = null)
        {
            List<TaxonNameItem> items = new List<TaxonNameItem>();

            for (int i = 0; i < children.Count; i++)
            {
                Taxon taxon = children[i].taxon;
                int sign = children[i].sign;

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