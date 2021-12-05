/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

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
        public enum TabPage
        {
            None = -1,
            File,
            Evo,
            Search,
            Statistics,
            Validation,
            About
        }

        private static TabPage _CurrentTabPage = TabPage.None;

        public static EventHandler<TabPage> CurrentTabPageChanged;

        public static TabPage CurrentTabPage
        {
            get => _CurrentTabPage;

            set
            {
                if (_CurrentTabPage != value)
                {
                    _CurrentTabPage = value;

                    CurrentTabPageChanged?.Invoke(null, _CurrentTabPage);
                }
            }
        }

        //

        private static bool _IsEditMode = false;

        public static EventHandler<bool> IsEditModeChanged;

        public static bool IsEditMode
        {
            get => _IsEditMode;

            set
            {
                if (_IsEditMode != value)
                {
                    _IsEditMode = value;

                    IsEditModeChanged?.Invoke(null, _IsEditMode);
                }
            }
        }

        //

        private static Taxon _CurrentTaxon = null;

        public static EventHandler<Taxon> CurrentTaxonChanged;

        public static Taxon CurrentTaxon
        {
            get => _CurrentTaxon;

            set
            {
                if (_CurrentTaxon != value)
                {
                    _CurrentTaxon = value;

                    CurrentTaxonChanged?.Invoke(null, _CurrentTaxon);
                }
            }
        }

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
            DescriptionUpdated, // { currentTaxon }

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

        public static EventHandler<(EditOperation editOperation, object[] args)> EditOperationOccurred;

        public static void NotifyEditOperation(EditOperation editOperation, params object[] args) => EditOperationOccurred?.Invoke(null, (editOperation, args));

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