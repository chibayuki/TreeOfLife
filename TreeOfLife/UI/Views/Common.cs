﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.Windows;
using System.Windows.Controls;

using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Controls;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Views
{
    public static class Common
    {
        public static bool? EditMode { get; set; } = null; // 是否为编辑模式。

        public static Action EnterEditMode { get; set; }
        public static Action ExitEditMode { get; set; }

        //

        public static Taxon CurrentTaxon { get; set; } = null; // 当前聚焦的类群。

        public static Action<Taxon> SetCurrentTaxon { get; set; }

        //

        public static Taxon RightButtonTaxon { get; set; } = null; // 当前右键单击的类群。
        public static Taxon SelectedTaxon { get; set; } = null; // 当前选择的类群。

        //

        public enum EditOperation
        {
            ScientificNameUpdated, // { currentTaxon }
            ChineseNameUpdated, // { currentTaxon }
            RankUpdated, // { currentTaxon }
            IsExtinctUpdated, // { currentTaxon }
            IsUndetUpdated, // { currentTaxon }
            BirthUpdated, // { currentTaxon }
            ExtinctionUpdated, // { currentTaxon }
            SynonymsUpdated, // { currentTaxon }
            TagsUpdated, // { currentTaxon }

            ParentChanged, // { anyTaxon, oldParent, newParent }

            ChildrenReordered, // { parent }
            ChildrenAdded, // { currentTaxon }
            ChildrenRemoved, // { parent }

            ExcludeByAdded, // { anyTaxon, excludeBy }
            ExcludeByRemoved, // { currentTaxon, excludeBy }
            IncludeByAdded, // { anyTaxon, includeBy }
            IncludeByRemoved, // { currentTaxon, includeBy }
            ExcludesReordered, // { currentTaxon }
            ExcludesRemoved, // { currentTaxon, exclude }
            IncludesReordered, // { currentTaxon }
            IncludesRemoved, // { currentTaxon, include }
        }

        public static Action<EditOperation, object[]> NotifyEditOperation { get; set; }

        //

        // 更新类群列表（并按分类阶元分组）。
        public static void UpdateTaxonListAndGroupByRank(TaxonButtonGroup control, IReadOnlyList<Taxon> taxons, ContextMenu contextMenu = null)
        {
            var groups = new List<(string groupName, ColorX groupColor, IEnumerable<TaxonItem> items)>();

            int groupIndex = 0;
            Rank rankOfGroup = Rank.Unranked;

            for (int i = 0; i < taxons.Count; i++)
            {
                Taxon taxon = taxons[i];

                Rank basicRank = taxon.GetInheritedBasicRank();

                if (i == 0)
                {
                    rankOfGroup = basicRank;

                    groups.Add((rankOfGroup.IsPrimaryOrSecondaryRank() ? rankOfGroup.GetChineseName() : string.Empty, rankOfGroup.GetThemeColor(), new List<TaxonItem>()));
                }
                else
                {
                    if (rankOfGroup != basicRank)
                    {
                        rankOfGroup = basicRank;

                        groups.Add((rankOfGroup.GetChineseName(), rankOfGroup.GetThemeColor(), new List<TaxonItem>()));

                        groupIndex++;
                    }
                }

                ((List<TaxonItem>)groups[groupIndex].items).Add(new TaxonItem()
                {
                    Taxon = taxon,
                    IsChecked = taxon == CurrentTaxon,
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

        // 更新类群列表（并标记是否为引用）。
        public static void UpdateTaxonList(TaxonButtonGroup control, IReadOnlyList<(Taxon taxon, bool isRef)> taxons, ContextMenu contextMenu = null)
        {
            List<TaxonItem> items = new List<TaxonItem>();

            for (int i = 0; i < taxons.Count; i++)
            {
                (Taxon taxon, bool isRef) = taxons[i];

                items.Add(new TaxonItem()
                {
                    Taxon = taxon,
                    IsRef = isRef,
                    Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, contextMenu) }
                });
            }

            control.UpdateContent(items);
        }
    }
}