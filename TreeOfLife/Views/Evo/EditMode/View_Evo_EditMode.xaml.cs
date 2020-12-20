﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.608.1000.M6.201219-0000

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

using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Evo.EditMode
{
    /// <summary>
    /// View_Evo_EditMode.xaml 的交互逻辑
    /// </summary>
    public partial class View_Evo_EditMode : UserControl
    {
        private ContextMenu _ContextMenu_Parent;
        private ContextMenu _ContextMenu_Children;

        private Taxon _SelectedTaxon = null;

        //

        public View_Evo_EditMode()
        {
            InitializeComponent();

            //

            _InitContextMenus();

            //

            taxonNameButtonGroup_Children.GroupNameWidth = 0;
            taxonNameButtonGroup_Children.GroupMargin = new Thickness(0, 1, 0, 1);

            button_Back.Click += (s, e) => Common.ExitEditMode();

            categorySelector.MouseLeftButtonUp += (s, e) => ViewModel.Category = categorySelector.Category;

            button_AddParentUplevel.Click += Button_AddParentUplevel_Click;
            button_AddParentDownlevel.Click += Button_AddParentDownlevel_Click;
            button_AddChildren.Click += Button_AddChildren_Click;
        }

        private void _InitContextMenus()
        {
            MenuItem item_Parent_Select = new MenuItem() { Header = "选择" };
            item_Parent_Select.Click += (s, e) => _SelectedTaxon = Common.RightButtonTaxon;

            _ContextMenu_Parent = new ContextMenu();
            _ContextMenu_Parent.Items.Add(item_Parent_Select);

            MenuItem item_Children_Select = new MenuItem() { Header = "选择" };
            item_Children_Select.Click += (s, e) => _SelectedTaxon = Common.RightButtonTaxon;
            MenuItem item_Children_SetParent = new MenuItem() { Header = "继承选择的类群" };
            item_Children_SetParent.Click += (s, e) => { Common.RightButtonTaxon?.SetParent(_SelectedTaxon); _UpdateParents(); _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_MoveTop = new MenuItem() { Header = "移至最上" };
            item_Children_MoveTop.Click += (s, e) => { Common.RightButtonTaxon?.Parent.MoveChild(Common.RightButtonTaxon.Index, 0); _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_MoveUp = new MenuItem() { Header = "上移" };
            item_Children_MoveUp.Click += (s, e) => { Common.RightButtonTaxon?.Parent.SwapChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Index - 1); _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_MoveDown = new MenuItem() { Header = "下移" };
            item_Children_MoveDown.Click += (s, e) => { Common.RightButtonTaxon?.Parent.SwapChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Index + 1); _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_MoveBottom = new MenuItem() { Header = "移至最下" };
            item_Children_MoveBottom.Click += (s, e) => { Common.RightButtonTaxon?.Parent.MoveChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Parent.Children.Count - 1); _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_DeleteWithoutChildren = new MenuItem() { Header = "删除 (并且保留下级类群)" };
            item_Children_DeleteWithoutChildren.Click += (s, e) => { Common.RightButtonTaxon?.RemoveCurrent(false); if (_SelectedTaxon == Common.RightButtonTaxon) { _SelectedTaxon = null; } Common.RightButtonTaxon = null; _UpdateChildrenWithVisibility(); };
            MenuItem item_Children_DeleteWithinChildren = new MenuItem() { Header = "删除 (并且删除下级类群)" };
            item_Children_DeleteWithinChildren.Click += (s, e) => { Common.RightButtonTaxon?.RemoveCurrent(true); if (_SelectedTaxon == Common.RightButtonTaxon) { _SelectedTaxon = null; } Common.RightButtonTaxon = null; _UpdateChildrenWithVisibility(); };

            Action updateMenuItems_Children = () =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                item_Children_SetParent.IsEnabled = (_SelectedTaxon != null && _SelectedTaxon != rightButtonTaxon.Parent && !_SelectedTaxon.InheritFrom(rightButtonTaxon));
                item_Children_MoveTop.IsEnabled = item_Children_MoveUp.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index > 0);
                item_Children_MoveBottom.IsEnabled = item_Children_MoveDown.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index < rightButtonTaxon.Parent.Children.Count - 1);

                if (item_Children_SetParent.IsEnabled)
                {
                    if (_SelectedTaxon.IsAnonymous())
                    {
                        item_Children_SetParent.Header = "继承选择的类群 (未命名)";
                    }
                    else
                    {
                        string taxonName = _SelectedTaxon.LongName();

                        if (taxonName.Length > 16)
                        {
                            item_Children_SetParent.Header = string.Concat("继承选择的类群 (", taxonName.Substring(0, 16), "...)");
                        }
                        else
                        {
                            item_Children_SetParent.Header = string.Concat("继承选择的类群 (", taxonName, ")");
                        }
                    }
                }
                else
                {
                    item_Children_SetParent.Header = "继承选择的类群";
                }

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
            _ContextMenu_Children.Items.Add(item_Children_Select);
            _ContextMenu_Children.Items.Add(item_Children_SetParent);
            _ContextMenu_Children.Items.Add(item_Children_MoveTop);
            _ContextMenu_Children.Items.Add(item_Children_MoveUp);
            _ContextMenu_Children.Items.Add(item_Children_MoveDown);
            _ContextMenu_Children.Items.Add(item_Children_MoveBottom);
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithoutChildren);
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithinChildren);
            _ContextMenu_Children.DataContext = updateMenuItems_Children;
        }

        //

        private void Button_AddParentUplevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon parent = _CurrentTaxon.AddParentUplevel();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                parent.ParseCurrent(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            _UpdateParents();
        }

        private void Button_AddParentDownlevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon parent = _CurrentTaxon.AddParentDownlevel();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                parent.ParseCurrent(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            _UpdateChildrenWithVisibility();
        }

        private void Button_AddChildren_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox_Children.Text))
            {
                _CurrentTaxon.AddChild();
            }
            else
            {
                _CurrentTaxon.ParseChildren(textBox_Children.Text.Split(Environment.NewLine));

                textBox_Children.Clear();
            }

            //

            _UpdateChildrenWithVisibility();
        }

        //

        public ViewModel_Evo_EditMode ViewModel => this.DataContext as ViewModel_Evo_EditMode;

        //

        #region 类群

        private Taxon _CurrentTaxon { get; set; }

        // 更新父类群。
        private void _UpdateParents()
        {
            if (_CurrentTaxon.IsRoot)
            {
                taxonNameButtonGroup_Parents.StartEditing();
                taxonNameButtonGroup_Parents.Clear();
                taxonNameButtonGroup_Parents.FinishEditing();
            }
            else
            {
                var parents = _CurrentTaxon.GetSummaryParents(true);

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }
                else
                {
                    parents.Add(_CurrentTaxon.Parent);
                }

                Common.UpdateParents(taxonNameButtonGroup_Parents, parents, _ContextMenu_Parent);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            var children = _CurrentTaxon.Children;

            Common.UpdateChildren(taxonNameButtonGroup_Children, children, _ContextMenu_Children);

            grid_Children.Visibility = (_CurrentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新子类群及其可见性。
        private void _UpdateChildrenWithVisibility()
        {
            _UpdateChildren();

            grid_Children.Visibility = (_CurrentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            grid_Name.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_State.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Category.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Synonyms.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Tags.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Desc.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Parents.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_AddParent.Visibility = (_CurrentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Children.Visibility = (_CurrentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        public void SetTaxon(Taxon taxon)
        {
            _CurrentTaxon = taxon;

            ViewModel.UpdateFromTaxon(taxon);

            categorySelector.Category = ViewModel.Category;
            textBox_Parent.Clear();
            textBox_Children.Clear();

            _UpdateParents();
            _UpdateChildren();
            _UpdateVisibility();
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                categorySelector.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_Parents.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_Children.IsDarkTheme = _IsDarkTheme;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}