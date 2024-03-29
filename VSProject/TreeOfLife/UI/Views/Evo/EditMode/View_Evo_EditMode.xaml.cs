﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.Core.Validation;
using TreeOfLife.Core.Validation.Extensions;
using TreeOfLife.UI.Extensions;

namespace TreeOfLife.UI.Views
{
    public partial class View_Evo_EditMode : UserControl
    {
        public ViewModel_Evo_EditMode ViewModel => this.DataContext as ViewModel_Evo_EditMode;

        //

        public View_Evo_EditMode()
        {
            InitializeComponent();

            //

            _InitContextMenus();

            //

            Common.EditOperationOccurred += (s, e) => _ProcessEditOperationNotification(e.editOperation, e.args);

            //

            button_Back.Click += (s, e) => Common.IsEditMode = false;

            grid_Title.ContextMenu = _ContextMenu_Current;
            grid_Title.MouseRightButtonUp += (s, e) =>
            {
                Common.RightButtonTaxon = Common.CurrentTaxon;
                (_ContextMenu_Current.DataContext as Action)?.Invoke();
            };

            button_Rename.Click += (s, e) => textBox_ChName.Text = _ChsRename;
            button_Rerank.Click += (s, e) => rankSelector.Rank = _Rerank;

            button_AddParentUplevel.Click += Button_AddParentUplevel_Click;
            button_AddParentDownlevel.Click += Button_AddParentDownlevel_Click;
            button_AddChildren.Click += Button_AddChildren_Click;

            taxonButtonGroup_Parents.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_Parents.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_Parent.DataContext as Action)?.Invoke();
            };

            taxonButtonGroup_Children.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_Children.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_Children.DataContext as Action)?.Invoke();
            };

            taxonButtonGroup_Excludes.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_Excludes.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_Excludes.DataContext as Action)?.Invoke();
            };

            taxonButtonGroup_ExcludeBy.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_ExcludeBy.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_ExcludeBy.DataContext as Action)?.Invoke();
            };

            taxonButtonGroup_Includes.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_Includes.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_Includes.DataContext as Action)?.Invoke();
            };

            taxonButtonGroup_IncludeBy.MouseLeftButtonClick += (s, e) => Common.CurrentTaxon = e.Taxon;
            taxonButtonGroup_IncludeBy.MouseRightButtonClick += (s, e) =>
            {
                Common.RightButtonTaxon = e.Taxon;
                (_ContextMenu_IncludeBy.DataContext as Action)?.Invoke();
            };

            //

            warningMessage_NodeStructure.Message = NodeStructureValidator.Instance.ToString();
            warningMessage_Monophyly.Message = MonophylyValidator.Instance.ToString();

            warningMessage_NameMissing.Message = NameMissingValidator.Instance.ToString();
            warningMessage_NameLength.Message = NameLengthValidator.Instance.ToString();
            warningMessage_NameUppercase.Message = NameUppercaseValidator.Instance.ToString();
            warningMessage_NameWordCount.Message = NameWordCountValidator.Instance.ToString();
            warningMessage_NameCharacter.Message = NameCharacterValidator.Instance.ToString();
            warningMessage_NameSeparator.Message = NameSeparatorValidator.Instance.ToString();

            warningMessage_ChineseSuffix.Message = ChineseSuffixValidator.Instance.ToString();

            warningMessage_RankMissing.Message = RankMissingValidator.Instance.ToString();

            warningMessage_TimelineCompleteness_Birth.Message = TimelineCompletenessValidator.Instance.ToString("灭绝年代");
            warningMessage_EvolutionOrder.Message = EvolutionOrderValidator.Instance.ToString();
            warningMessage_TimelineCompleteness_Extinction.Message = TimelineCompletenessValidator.Instance.ToString("诞生年代");
            warningMessage_TimelineConsistency.Message = TimelineConsistencyValidator.Instance.ToString();

            warningMessage_SynonymsUnique.Message = SynonymsUniqueValidator.Instance.ToString();
            warningMessage_SynonymsTagUnique_Synonyms.Message = SynonymsTagUniqueValidator.Instance.ToString();
            warningMessage_TagsUnique.Message = TagsUniqueValidator.Instance.ToString();
            warningMessage_SynonymsTagUnique_Tags.Message = SynonymsTagUniqueValidator.Instance.ToString();

            warningMessage_NodeInfo_State.Message = NodeInformationValidator.Instance.ToString("状态");
            warningMessage_NodeInfo_Birth.Message = NodeInformationValidator.Instance.ToString("诞生年代");
            warningMessage_NodeInfo_Extinction.Message = NodeInformationValidator.Instance.ToString("灭绝年代");
            warningMessage_NodeInfo_Synonyms.Message = NodeInformationValidator.Instance.ToString("异名");
            warningMessage_NodeInfo_Tags.Message = NodeInformationValidator.Instance.ToString("标签");
            warningMessage_NodeInfo_Desc.Message = NodeInformationValidator.Instance.ToString("描述");

            warningMessage_NodeMonophyly_Excludes.Message = NodeMonophylyValidator.Instance.ToString("并系群");
            warningMessage_NodeMonophyly_Includes.Message = NodeMonophylyValidator.Instance.ToString("复系群");

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                taxonTitle.IsDarkTheme = Theme.IsDarkTheme;
                rankSelector.IsDarkTheme = Theme.IsDarkTheme;
                geoChronSelector_Birth.IsDarkTheme = Theme.IsDarkTheme;
                geoChronSelector_Extinction.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Parents.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Children.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Excludes.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_ExcludeBy.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_Includes.IsDarkTheme = Theme.IsDarkTheme;
                taxonButtonGroup_IncludeBy.IsDarkTheme = Theme.IsDarkTheme;
            };
        }

        private ContextMenu _ContextMenu_Current;
        private ContextMenu _ContextMenu_Parent;
        private ContextMenu _ContextMenu_Children;
        private ContextMenu _ContextMenu_Excludes;
        private ContextMenu _ContextMenu_ExcludeBy;
        private ContextMenu _ContextMenu_Includes;
        private ContextMenu _ContextMenu_IncludeBy;

        private void _UpdateSelectOfMenuItem(MenuItem selected, MenuItem select)
        {
            if (selected is not null && select is not null)
            {
                Taxon selectedTaxon = Common.SelectedTaxon;

                selected.IsEnabled = false;
                select.IsEnabled = selectedTaxon != Common.RightButtonTaxon;

                if (selectedTaxon is null)
                {
                    selected.Visibility = Visibility.Collapsed;
                }
                else
                {
                    selected.Visibility = Visibility.Visible;

                    if (selectedTaxon.IsAnonymous)
                    {
                        selected.Header = "已选择: <节点>";
                    }
                    else
                    {
                        string taxonName = selectedTaxon.GetLongName();

                        if (taxonName.Length > 32)
                        {
                            selected.Header = string.Concat("已选择：\"", taxonName[..32], "...\"");
                        }
                        else
                        {
                            selected.Header = string.Concat("已选择：\"", taxonName, "\"");
                        }
                    }
                }
            }
        }

        private void _InitContextMenus()
        {
            MenuItem item_Current_Selected = new MenuItem();

            MenuItem item_Current_Select = new MenuItem() { Header = "选择" };

            item_Current_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_Current_SetParent = new MenuItem() { Header = "继承选择的类群" };

            item_Current_SetParent.Click += async (s, e) =>
            {
                Taxon oldParent = Common.RightButtonTaxon.Parent;

                await Common.RightButtonTaxon.SetParentAsync(Common.SelectedTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ParentChanged, Common.RightButtonTaxon, oldParent, Common.SelectedTaxon);
            };

            MenuItem item_Current_ExcludeBy = new MenuItem() { Header = "排除自选择的类群（并系群）" };

            item_Current_ExcludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddExcludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ExcludeByAdded, Common.RightButtonTaxon, Common.SelectedTaxon);
            };

            MenuItem item_Current_IncludeBy = new MenuItem() { Header = "包含至选择的类群（复系群）" };

            item_Current_IncludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddIncludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.IncludeByAdded, Common.RightButtonTaxon, Common.SelectedTaxon);
            };

            Action updateMenuItems_Current = () =>
            {
                _UpdateSelectOfMenuItem(item_Current_Selected, item_Current_Select);

                Taxon currentTaxon = Common.CurrentTaxon;
                Taxon selectedTaxon = Common.SelectedTaxon;

                item_Current_SetParent.IsEnabled = currentTaxon.CanSetParent(selectedTaxon);
                item_Current_ExcludeBy.IsEnabled = selectedTaxon?.CanAddExclude(currentTaxon) ?? false;
                item_Current_IncludeBy.IsEnabled = selectedTaxon?.CanAddInclude(currentTaxon) ?? false;
            };

            _ContextMenu_Current = new ContextMenu();
            _ContextMenu_Current.Items.Add(item_Current_Selected);
            _ContextMenu_Current.Items.Add(item_Current_Select);
            _ContextMenu_Current.Items.Add(new Separator());
            _ContextMenu_Current.Items.Add(item_Current_SetParent);
            _ContextMenu_Current.Items.Add(item_Current_ExcludeBy);
            _ContextMenu_Current.Items.Add(item_Current_IncludeBy);
            _ContextMenu_Current.DataContext = updateMenuItems_Current;

            //

            MenuItem item_Parent_Selected = new MenuItem();

            MenuItem item_Parent_Select = new MenuItem() { Header = "选择" };

            item_Parent_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            Action updateMenuItems_Parent = () => _UpdateSelectOfMenuItem(item_Parent_Selected, item_Parent_Select);

            _ContextMenu_Parent = new ContextMenu();
            _ContextMenu_Parent.Items.Add(item_Parent_Selected);
            _ContextMenu_Parent.Items.Add(item_Parent_Select);
            _ContextMenu_Parent.DataContext = updateMenuItems_Parent;

            //

            MenuItem item_Children_Selected = new MenuItem();

            MenuItem item_Children_Select = new MenuItem() { Header = "选择" };

            item_Children_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_Children_SetParent = new MenuItem() { Header = "继承选择的类群" };

            item_Children_SetParent.Click += async (s, e) =>
            {
                Taxon oldParent = Common.RightButtonTaxon.Parent;

                await Common.RightButtonTaxon.SetParentAsync(Common.SelectedTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ParentChanged, Common.RightButtonTaxon, oldParent, Common.SelectedTaxon);
            };

            MenuItem item_Children_ExcludeBy = new MenuItem() { Header = "排除自选择的类群（并系群）" };

            item_Children_ExcludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddExcludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ExcludeByAdded, Common.RightButtonTaxon, Common.SelectedTaxon);
            };

            MenuItem item_Children_IncludeBy = new MenuItem() { Header = "包含至选择的类群（复系群）" };

            item_Children_IncludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddIncludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.IncludeByAdded, Common.RightButtonTaxon, Common.SelectedTaxon);
            };

            MenuItem item_Children_MoveTop = new MenuItem() { Header = "移至最上" };

            item_Children_MoveTop.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.MoveChildAsync(rightButtonTaxon.Index, 0);

                Common.NotifyEditOperation(Common.EditOperation.ChildrenReordered, rightButtonTaxon.Parent);
            };

            MenuItem item_Children_MoveUp = new MenuItem() { Header = "上移" };

            item_Children_MoveUp.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.SwapChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Index - 1);

                Common.NotifyEditOperation(Common.EditOperation.ChildrenReordered, rightButtonTaxon.Parent);
            };

            MenuItem item_Children_MoveDown = new MenuItem() { Header = "下移" };

            item_Children_MoveDown.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.SwapChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Index + 1);

                Common.NotifyEditOperation(Common.EditOperation.ChildrenReordered, rightButtonTaxon.Parent);
            };

            MenuItem item_Children_MoveBottom = new MenuItem() { Header = "移至最下" };

            item_Children_MoveBottom.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.MoveChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Parent.Children.Count - 1);

                Common.NotifyEditOperation(Common.EditOperation.ChildrenReordered, rightButtonTaxon.Parent);
            };

            MenuItem item_Children_DeleteWithoutChildren = new MenuItem() { Header = "删除 (并且保留下级类群)" };

            item_Children_DeleteWithoutChildren.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;
                Taxon parent = rightButtonTaxon.Parent;

                await rightButtonTaxon.RemoveCurrentAsync(false);

                if (Common.SelectedTaxon == rightButtonTaxon)
                {
                    Common.SelectedTaxon = null;
                }

                Common.RightButtonTaxon = null;

                Common.NotifyEditOperation(Common.EditOperation.ChildrenRemoved, parent);
            };

            MenuItem item_Children_DeleteWithinChildren = new MenuItem() { Header = "删除 (并且删除下级类群)" };

            item_Children_DeleteWithinChildren.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;
                Taxon parent = rightButtonTaxon.Parent;

                await rightButtonTaxon.RemoveCurrentAsync(true);

                if (Common.SelectedTaxon == rightButtonTaxon)
                {
                    Common.SelectedTaxon = null;
                }

                Common.RightButtonTaxon = null;

                Common.NotifyEditOperation(Common.EditOperation.ChildrenRemoved, parent);
            };

            Action updateMenuItems_Children = () =>
            {
                _UpdateSelectOfMenuItem(item_Children_Selected, item_Children_Select);

                Taxon rightButtonTaxon = Common.RightButtonTaxon;
                Taxon selectedTaxon = Common.SelectedTaxon;

                item_Children_SetParent.IsEnabled = rightButtonTaxon.CanSetParent(selectedTaxon);
                item_Children_ExcludeBy.IsEnabled = selectedTaxon?.CanAddExclude(rightButtonTaxon) ?? false;
                item_Children_IncludeBy.IsEnabled = selectedTaxon?.CanAddInclude(rightButtonTaxon) ?? false;

                item_Children_MoveTop.IsEnabled = item_Children_MoveUp.IsEnabled = !rightButtonTaxon.IsRoot && rightButtonTaxon.Index > 0;
                item_Children_MoveBottom.IsEnabled = item_Children_MoveDown.IsEnabled = !rightButtonTaxon.IsRoot && rightButtonTaxon.Index < rightButtonTaxon.Parent.Children.Count - 1;

                if (rightButtonTaxon.IsFinal)
                {
                    item_Children_DeleteWithoutChildren.Visibility = Visibility.Collapsed;

                    item_Children_DeleteWithinChildren.Header = "删除";
                }
                else
                {
                    item_Children_DeleteWithoutChildren.Visibility = Visibility.Visible;

                    item_Children_DeleteWithinChildren.Header = "删除 (并且删除下级类群)";
                }
            };

            _ContextMenu_Children = new ContextMenu();
            _ContextMenu_Children.Items.Add(item_Children_Selected);
            _ContextMenu_Children.Items.Add(item_Children_Select);
            _ContextMenu_Children.Items.Add(new Separator());
            _ContextMenu_Children.Items.Add(item_Children_SetParent);
            _ContextMenu_Children.Items.Add(item_Children_ExcludeBy);
            _ContextMenu_Children.Items.Add(item_Children_IncludeBy);
            _ContextMenu_Children.Items.Add(new Separator());
            _ContextMenu_Children.Items.Add(item_Children_MoveTop);
            _ContextMenu_Children.Items.Add(item_Children_MoveUp);
            _ContextMenu_Children.Items.Add(item_Children_MoveDown);
            _ContextMenu_Children.Items.Add(item_Children_MoveBottom);
            _ContextMenu_Children.Items.Add(new Separator());
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithoutChildren);
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithinChildren);
            _ContextMenu_Children.DataContext = updateMenuItems_Children;

            //

            MenuItem item_Excludes_MoveTop = new MenuItem() { Header = "移至最上" };

            item_Excludes_MoveTop.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                await currentTaxon.MoveExcludeAsync(await currentTaxon.GetIndexOfExcludeAsync(Common.RightButtonTaxon), 0);

                Common.NotifyEditOperation(Common.EditOperation.ExcludesReordered, currentTaxon);
            };

            MenuItem item_Excludes_MoveUp = new MenuItem() { Header = "上移" };

            item_Excludes_MoveUp.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = await currentTaxon.GetIndexOfExcludeAsync(Common.RightButtonTaxon);
                await currentTaxon.SwapExcludeAsync(index, index - 1);

                Common.NotifyEditOperation(Common.EditOperation.ExcludesReordered, currentTaxon);
            };

            MenuItem item_Excludes_MoveDown = new MenuItem() { Header = "下移" };

            item_Excludes_MoveDown.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = await currentTaxon.GetIndexOfExcludeAsync(Common.RightButtonTaxon);
                await currentTaxon.SwapExcludeAsync(index, index + 1);

                Common.NotifyEditOperation(Common.EditOperation.ExcludesReordered, currentTaxon);
            };

            MenuItem item_Excludes_MoveBottom = new MenuItem() { Header = "移至最下" };

            item_Excludes_MoveBottom.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                await currentTaxon.MoveExcludeAsync(await currentTaxon.GetIndexOfExcludeAsync(Common.RightButtonTaxon), currentTaxon.Excludes.Count - 1);

                Common.NotifyEditOperation(Common.EditOperation.ExcludesReordered, currentTaxon);
            };

            MenuItem item_Excludes_Remove = new MenuItem() { Header = "解除排除关系" };

            item_Excludes_Remove.Click += async (s, e) =>
            {
                await Common.CurrentTaxon.RemoveExcludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ExcludesRemoved, Common.CurrentTaxon, Common.RightButtonTaxon);
            };

            Action updateMenuItems_Excludes = () =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon);

                item_Excludes_MoveTop.IsEnabled = item_Excludes_MoveUp.IsEnabled = index > 0;
                item_Excludes_MoveBottom.IsEnabled = item_Excludes_MoveDown.IsEnabled = index < currentTaxon.Excludes.Count - 1;
            };

            _ContextMenu_Excludes = new ContextMenu();
            _ContextMenu_Excludes.Items.Add(item_Excludes_MoveTop);
            _ContextMenu_Excludes.Items.Add(item_Excludes_MoveUp);
            _ContextMenu_Excludes.Items.Add(item_Excludes_MoveDown);
            _ContextMenu_Excludes.Items.Add(item_Excludes_MoveBottom);
            _ContextMenu_Excludes.Items.Add(new Separator());
            _ContextMenu_Excludes.Items.Add(item_Excludes_Remove);
            _ContextMenu_Excludes.DataContext = updateMenuItems_Excludes;

            //

            MenuItem item_ExcludeBy_Remove = new MenuItem() { Header = "解除排除关系" };

            item_ExcludeBy_Remove.Click += async (s, e) =>
            {
                await Common.RightButtonTaxon.RemoveExcludeAsync(Common.CurrentTaxon);

                Common.NotifyEditOperation(Common.EditOperation.ExcludeByRemoved, Common.CurrentTaxon, Common.RightButtonTaxon);
            };

            _ContextMenu_ExcludeBy = new ContextMenu();
            _ContextMenu_ExcludeBy.Items.Add(item_ExcludeBy_Remove);

            //

            MenuItem item_Includes_MoveTop = new MenuItem() { Header = "移至最上" };

            item_Includes_MoveTop.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                await currentTaxon.MoveIncludeAsync(await currentTaxon.GetIndexOfIncludeAsync(Common.RightButtonTaxon), 0);

                Common.NotifyEditOperation(Common.EditOperation.IncludesReordered, currentTaxon);
            };

            MenuItem item_Includes_MoveUp = new MenuItem() { Header = "上移" };

            item_Includes_MoveUp.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = await currentTaxon.GetIndexOfIncludeAsync(Common.RightButtonTaxon);
                await currentTaxon.SwapIncludeAsync(index, index - 1);

                Common.NotifyEditOperation(Common.EditOperation.IncludesReordered, currentTaxon);
            };

            MenuItem item_Includes_MoveDown = new MenuItem() { Header = "下移" };

            item_Includes_MoveDown.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = await currentTaxon.GetIndexOfIncludeAsync(Common.RightButtonTaxon);
                await currentTaxon.SwapIncludeAsync(index, index + 1);

                Common.NotifyEditOperation(Common.EditOperation.IncludesReordered, currentTaxon);
            };

            MenuItem item_Includes_MoveBottom = new MenuItem() { Header = "移至最下" };

            item_Includes_MoveBottom.Click += async (s, e) =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                await currentTaxon.MoveIncludeAsync(await currentTaxon.GetIndexOfIncludeAsync(Common.RightButtonTaxon), currentTaxon.Includes.Count - 1);

                Common.NotifyEditOperation(Common.EditOperation.IncludesReordered, currentTaxon);
            };

            MenuItem item_Includes_Remove = new MenuItem() { Header = "解除包含关系" };

            item_Includes_Remove.Click += async (s, e) =>
            {
                await Common.CurrentTaxon.RemoveIncludeAsync(Common.RightButtonTaxon);

                Common.NotifyEditOperation(Common.EditOperation.IncludesRemoved, Common.CurrentTaxon, Common.RightButtonTaxon);
            };

            Action updateMenuItems_Includes = () =>
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon);

                item_Includes_MoveTop.IsEnabled = item_Includes_MoveUp.IsEnabled = index > 0;
                item_Includes_MoveBottom.IsEnabled = item_Includes_MoveDown.IsEnabled = index < currentTaxon.Includes.Count - 1;
            };

            _ContextMenu_Includes = new ContextMenu();
            _ContextMenu_Includes.Items.Add(item_Includes_MoveTop);
            _ContextMenu_Includes.Items.Add(item_Includes_MoveUp);
            _ContextMenu_Includes.Items.Add(item_Includes_MoveDown);
            _ContextMenu_Includes.Items.Add(item_Includes_MoveBottom);
            _ContextMenu_Includes.Items.Add(new Separator());
            _ContextMenu_Includes.Items.Add(item_Includes_Remove);
            _ContextMenu_Includes.DataContext = updateMenuItems_Includes;

            //

            MenuItem item_IncludeBy_Remove = new MenuItem() { Header = "解除包含关系" };

            item_IncludeBy_Remove.Click += async (s, e) =>
            {
                await Common.RightButtonTaxon.RemoveIncludeAsync(Common.CurrentTaxon);

                Common.NotifyEditOperation(Common.EditOperation.IncludeByRemoved, Common.CurrentTaxon, Common.RightButtonTaxon);
            };

            _ContextMenu_IncludeBy = new ContextMenu();
            _ContextMenu_IncludeBy.Items.Add(item_IncludeBy_Remove);
        }

        //

        #region 回调函数

        private async void Button_AddParentUplevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon oldParent = Common.CurrentTaxon.Parent;
            Taxon parent = await Common.CurrentTaxon.AddParentUplevelAsync();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                await parent.ParseCurrentAsync(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            Common.NotifyEditOperation(Common.EditOperation.ParentChanged, Common.CurrentTaxon, oldParent, parent);
        }

        private async void Button_AddParentDownlevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon parent = await Common.CurrentTaxon.AddParentDownlevelAsync();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                await parent.ParseCurrentAsync(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            Common.NotifyEditOperation(Common.EditOperation.ChildrenAdded, Common.CurrentTaxon);
        }

        private async void Button_AddChildren_Click(object sender, RoutedEventArgs e)
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (string.IsNullOrEmpty(textBox_Children.Text))
            {
                await currentTaxon.AddChildAsync();
            }
            else
            {
                string children = textBox_Children.Text;

                await currentTaxon.ParseChildrenAsync(children.Split(Environment.NewLine));

                textBox_Children.Clear();
            }

            //

            Common.NotifyEditOperation(Common.EditOperation.ChildrenAdded, Common.CurrentTaxon);
        }

        #endregion

        #region 类群

        private string _ChsRename = string.Empty;
        private Rank _Rerank = Rank.Unranked;

        // 更新中文名重命名与分级更新提示。
        private void _UpdateRenameAndRerank()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (!currentTaxon.IsRoot)
            {
                if (!ChineseSuffixValidator.Instance.IsValid(currentTaxon))
                {
                    (string chNameWithoutRank, _, Rank? rank) = RankChineseExtension.SplitChineseName(ViewModel.ChName);

                    _Rerank = rank ?? Rank.Unranked;

                    if (ViewModel.Rank.IsUnranked())
                    {
                        _ChsRename = chNameWithoutRank;
                    }
                    else if (ViewModel.Rank.IsClade())
                    {
                        _ChsRename = chNameWithoutRank + "类";
                    }
                    else if (ViewModel.Rank.IsSpecies())
                    {
                        _ChsRename = chNameWithoutRank;
                    }
                    else
                    {
                        _ChsRename = chNameWithoutRank + ViewModel.Rank.GetChineseName();
                    }

                    label_Rename.Content = _ChsRename;
                    label_Rerank.Content = _Rerank.GetChineseName();

                    grid_Rename.Visibility = Visibility.Visible;
                    grid_Rerank.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_Rename.Visibility = Visibility.Collapsed;
                    grid_Rerank.Visibility = Visibility.Collapsed;
                }
            }
        }

        // 更新父类群。
        private void _UpdateParents()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (currentTaxon.IsRoot)
            {
                taxonButtonGroup_Parents.Clear();
            }
            else
            {
                List<Taxon> parents = new List<Taxon>(currentTaxon.GetParents(GetParentsOption.EditMode));

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }

                Common.UpdateTaxonList(taxonButtonGroup_Parents, parents, _ContextMenu_Parent);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Common.UpdateTaxonList(taxonButtonGroup_Children, currentTaxon.Children, _ContextMenu_Children);
        }

        // 更新子类群及其可见性。
        private void _UpdateChildrenWithVisibility()
        {
            _UpdateChildren();

            grid_Children.Visibility = !Common.CurrentTaxon.IsFinal ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新 Excludes。
        private void _UpdateExcludes()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Common.UpdateTaxonList(taxonButtonGroup_Excludes, currentTaxon.Excludes, _ContextMenu_Excludes);
        }

        // 更新 Excludes 及其可见性。
        private void _UpdateExcludesWithVisibility()
        {
            _UpdateExcludes();

            grid_Excludes.Visibility = Common.CurrentTaxon.Excludes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新 ExcludeBy。
        private void _UpdateExcludeBy()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Common.UpdateTaxonList(taxonButtonGroup_ExcludeBy, currentTaxon.ExcludeBy, _ContextMenu_ExcludeBy);
        }

        // 更新 ExcludeBy 及其可见性。
        private void _UpdateExcludeByWithVisibility()
        {
            _UpdateExcludeBy();

            grid_ExcludeBy.Visibility = Common.CurrentTaxon.ExcludeBy.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新 Includes。
        private void _UpdateIncludes()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Common.UpdateTaxonList(taxonButtonGroup_Includes, currentTaxon.Includes, _ContextMenu_Includes);
        }

        // 更新 Includes 及其可见性。
        private void _UpdateIncludesWithVisibility()
        {
            _UpdateIncludes();

            grid_Includes.Visibility = Common.CurrentTaxon.Includes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新 IncludeBy。
        private void _UpdateIncludeBy()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Common.UpdateTaxonList(taxonButtonGroup_IncludeBy, currentTaxon.IncludeBy, _ContextMenu_IncludeBy);
        }

        // 更新 IncludeBy 及其可见性。
        private void _UpdateIncludeByWithVisibility()
        {
            _UpdateIncludeBy();

            grid_IncludeBy.Visibility = Common.CurrentTaxon.IncludeBy.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            bool isRoot = currentTaxon.IsRoot;
            bool isNamed = currentTaxon.IsNamed;

            grid_Name.Visibility = !isRoot ? Visibility.Visible : Visibility.Collapsed;
            grid_Rank.Visibility = !isRoot && isNamed ? Visibility.Visible : Visibility.Collapsed;
            grid_State.Visibility = !isRoot && (isNamed || currentTaxon.IsExtinct || currentTaxon.IsUndet) ? Visibility.Visible : Visibility.Collapsed;
            grid_Chron.Visibility = !isRoot && (isNamed || !currentTaxon.Birth.IsEmpty || !currentTaxon.Extinction.IsEmpty) ? Visibility.Visible : Visibility.Collapsed;
            grid_Synonyms.Visibility = !isRoot && (isNamed || currentTaxon.Synonyms.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
            grid_Tags.Visibility = !isRoot && (isNamed || currentTaxon.Tags.Count > 0) ? Visibility.Visible : Visibility.Collapsed;
            grid_Desc.Visibility = !isRoot && (isNamed || !string.IsNullOrWhiteSpace(currentTaxon.Description)) ? Visibility.Visible : Visibility.Collapsed;
            grid_Parents.Visibility = !isRoot ? Visibility.Visible : Visibility.Collapsed;
            button_AddParentUplevel.Visibility = !isRoot ? Visibility.Visible : Visibility.Hidden;
            grid_Children.Visibility = !currentTaxon.IsFinal ? Visibility.Visible : Visibility.Collapsed;
            grid_Excludes.Visibility = currentTaxon.Excludes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_ExcludeBy.Visibility = currentTaxon.ExcludeBy.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Includes.Visibility = currentTaxon.Includes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_IncludeBy.Visibility = currentTaxon.IncludeBy.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 更新警告信息提示。
        private void _UpdateWarningMessage()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            Dictionary<IValidator, object> validators = new Dictionary<IValidator, object>();

            foreach (var validator in currentTaxon.Validate())
            {
                validators.Add(validator, null);
            }

            warningMessage_NodeStructure.Visibility = validators.ContainsKey(NodeStructureValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_Monophyly.Visibility = validators.ContainsKey(MonophylyValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_Node.Visibility = validators.ContainsKey(NodeStructureValidator.Instance) || validators.ContainsKey(MonophylyValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_NameMissing.Visibility = validators.ContainsKey(NameMissingValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NameLength.Visibility = validators.ContainsKey(NameLengthValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NameUppercase.Visibility = validators.ContainsKey(NameUppercaseValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NameWordCount.Visibility = validators.ContainsKey(NameWordCountValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NameCharacter.Visibility = validators.ContainsKey(NameCharacterValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NameSeparator.Visibility = validators.ContainsKey(NameSeparatorValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_ChineseSuffix.Visibility = validators.ContainsKey(ChineseSuffixValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_RankMissing.Visibility = validators.ContainsKey(RankMissingValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_TimelineCompleteness_Birth.Visibility = validators.ContainsKey(TimelineCompletenessValidator.Instance) && currentTaxon.Birth.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_EvolutionOrder.Visibility = validators.ContainsKey(EvolutionOrderValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_TimelineCompleteness_Extinction.Visibility = validators.ContainsKey(TimelineCompletenessValidator.Instance) && currentTaxon.Extinction.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_TimelineConsistency.Visibility = validators.ContainsKey(TimelineConsistencyValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_SynonymsUnique.Visibility = validators.ContainsKey(SynonymsUniqueValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_SynonymsTagUnique_Synonyms.Visibility = validators.ContainsKey(SynonymsTagUniqueValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_TagsUnique.Visibility = validators.ContainsKey(TagsUniqueValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_SynonymsTagUnique_Tags.Visibility = validators.ContainsKey(SynonymsTagUniqueValidator.Instance) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_NodeInfo_State.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && (currentTaxon.IsExtinct || currentTaxon.IsUndet) ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Birth.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && !currentTaxon.Birth.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Extinction.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && !currentTaxon.Extinction.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Extinction.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && !currentTaxon.Extinction.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Synonyms.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && currentTaxon.Synonyms.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Tags.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && currentTaxon.Tags.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeInfo_Desc.Visibility = validators.ContainsKey(NodeInformationValidator.Instance) && !string.IsNullOrWhiteSpace(currentTaxon.Description) ? Visibility.Visible : Visibility.Collapsed;

            warningMessage_NodeMonophyly_Excludes.Visibility = validators.ContainsKey(NodeMonophylyValidator.Instance) && currentTaxon.Excludes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            warningMessage_NodeMonophyly_Includes.Visibility = validators.ContainsKey(NodeMonophylyValidator.Instance) && currentTaxon.Includes.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _ProcessEditOperationNotification(Common.EditOperation editOperation, object[] args)
        {
            switch (editOperation)
            {
                case Common.EditOperation.ScientificNameUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    taxonButtonGroup_Children.SyncTaxonUpdation();
                    taxonButtonGroup_Excludes.SyncTaxonUpdation();
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.ChineseNameUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateRenameAndRerank();
                    taxonButtonGroup_Children.SyncTaxonUpdation();
                    taxonButtonGroup_Excludes.SyncTaxonUpdation();
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.RankUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateRenameAndRerank();
                    taxonButtonGroup_Children.SyncTaxonUpdation();
                    taxonButtonGroup_Excludes.SyncTaxonUpdation();
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.IsExtinctUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.IsUndetUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateVisibility();
                    break;

                case Common.EditOperation.BirthUpdated:
                case Common.EditOperation.ExtinctionUpdated:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.SynonymsUpdated:
                case Common.EditOperation.TagsUpdated:
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.DescriptionUpdated:
                    _UpdateVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.ParentChanged:
                    {
                        Taxon taxon = args[0] as Taxon;
                        Taxon oldParent = args[1] as Taxon;
                        Taxon newParent = args[1] as Taxon;

                        if (Common.CurrentTaxon.InheritFrom(taxon))
                        {
                            taxonTitle.SyncTaxonUpdation();
                            taxonButtonGroup_Children.SyncTaxonUpdation();
                            taxonButtonGroup_Excludes.SyncTaxonUpdation();
                        }

                        if (taxon == Common.CurrentTaxon)
                        {
                            _UpdateParents();
                        }

                        if (oldParent == Common.CurrentTaxon || newParent == Common.CurrentTaxon)
                        {
                            _UpdateChildrenWithVisibility();
                        }

                        _UpdateWarningMessage();
                    }
                    break;

                case Common.EditOperation.ChildrenReordered:
                    {
                        Taxon parent = args[0] as Taxon;

                        if (parent == Common.CurrentTaxon)
                        {
                            _UpdateChildren();
                        }
                    }
                    break;

                case Common.EditOperation.ChildrenAdded:
                    _UpdateChildrenWithVisibility();
                    _UpdateWarningMessage();
                    break;

                case Common.EditOperation.ChildrenRemoved:
                    {
                        Taxon parent = args[0] as Taxon;

                        if (parent == Common.CurrentTaxon)
                        {
                            _UpdateChildrenWithVisibility();
                        }

                        taxonTitle.SyncTaxonUpdation();
                        _UpdateExcludesWithVisibility();
                        _UpdateWarningMessage();
                    }
                    break;

                case Common.EditOperation.ExcludeByAdded:
                    {
                        Taxon taxon = args[0] as Taxon;
                        Taxon excludeBy = args[1] as Taxon;

                        if (taxon == Common.CurrentTaxon)
                        {
                            _UpdateExcludeByWithVisibility();
                        }

                        if (excludeBy == Common.CurrentTaxon)
                        {
                            taxonTitle.SyncTaxonUpdation();
                            _UpdateExcludesWithVisibility();
                        }
                    }
                    break;

                case Common.EditOperation.ExcludeByRemoved:
                    _UpdateExcludeByWithVisibility();
                    break;

                case Common.EditOperation.IncludeByAdded:
                    {
                        Taxon taxon = args[0] as Taxon;
                        Taxon includeBy = args[1] as Taxon;

                        if (taxon == Common.CurrentTaxon)
                        {
                            _UpdateIncludeByWithVisibility();
                        }

                        if (includeBy == Common.CurrentTaxon)
                        {
                            taxonTitle.SyncTaxonUpdation();
                            _UpdateIncludesWithVisibility();
                        }
                    }
                    break;

                case Common.EditOperation.IncludeByRemoved:
                    _UpdateIncludeByWithVisibility();
                    break;

                case Common.EditOperation.ExcludesReordered:
                    _UpdateExcludes();
                    break;

                case Common.EditOperation.ExcludesRemoved:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateExcludesWithVisibility();
                    break;

                case Common.EditOperation.IncludesReordered:
                    _UpdateIncludes();
                    break;

                case Common.EditOperation.IncludesRemoved:
                    taxonTitle.SyncTaxonUpdation();
                    _UpdateIncludesWithVisibility();
                    break;
            }
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.LoadFromCurrentTaxon();

            taxonTitle.Taxon = Common.CurrentTaxon;

            textBox_Parent.Clear();
            textBox_Children.Clear();

            _UpdateRenameAndRerank();
            _UpdateParents();
            _UpdateChildren();
            _UpdateExcludes();
            _UpdateExcludeBy();
            _UpdateIncludes();
            _UpdateIncludeBy();
            _UpdateVisibility();
            _UpdateWarningMessage();
        }

        #endregion
    }
}