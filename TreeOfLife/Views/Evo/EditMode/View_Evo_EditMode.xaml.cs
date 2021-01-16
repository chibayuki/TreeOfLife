/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.900.1000.M9.210112-0000

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

        //

        public ViewModel_Evo_EditMode ViewModel => this.DataContext as ViewModel_Evo_EditMode;

        //

        public View_Evo_EditMode()
        {
            InitializeComponent();

            //

            _InitContextMenus();

            //

            taxonNameButtonGroup_Children.GroupNameWidth = 0;
            taxonNameButtonGroup_Children.GroupMargin = new Thickness(0, 1, 0, 1);

            button_Back.Click += (s, e) => Views.Common.ExitEditMode();

            categorySelector.MouseLeftButtonUp += CategorySelector_MouseLeftButtonUp;

            button_AddParentUplevel.Click += Button_AddParentUplevel_Click;
            button_AddParentDownlevel.Click += Button_AddParentDownlevel_Click;
            button_AddChildren.Click += Button_AddChildren_Click;
        }

        private void _InitContextMenus()
        {
            Thickness menuItemPadding = new Thickness(3, 6, 3, 6);
            Thickness menuItemMargin = new Thickness(0, 3, 0, 3);

            MenuItem item_Parent_Select = new MenuItem() { Header = "选择" };
            item_Parent_Select.Padding = menuItemPadding;
            item_Parent_Select.Margin = menuItemMargin;
            item_Parent_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            _ContextMenu_Parent = new ContextMenu();
            _ContextMenu_Parent.Items.Add(item_Parent_Select);

            MenuItem item_Children_Select = new MenuItem() { Header = "选择" };
            item_Children_Select.Padding = menuItemPadding;
            item_Children_Select.Margin = menuItemMargin;
            item_Children_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_Children_SetParent = new MenuItem() { Header = "继承选择的类群" };
            item_Children_SetParent.Padding = menuItemPadding;
            item_Children_SetParent.Margin = menuItemMargin;
            item_Children_SetParent.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.SetParent(Common.SelectedTaxon);
                _UpdateParents();
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveTop = new MenuItem() { Header = "移至最上" };
            item_Children_MoveTop.Padding = menuItemPadding;
            item_Children_MoveTop.Margin = menuItemMargin;
            item_Children_MoveTop.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.Parent.MoveChild(Common.RightButtonTaxon.Index, 0);
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveUp = new MenuItem() { Header = "上移" };
            item_Children_MoveUp.Padding = menuItemPadding;
            item_Children_MoveUp.Margin = menuItemMargin;
            item_Children_MoveUp.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.Parent.SwapChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Index - 1);
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveDown = new MenuItem() { Header = "下移" };
            item_Children_MoveDown.Padding = menuItemPadding;
            item_Children_MoveDown.Margin = menuItemMargin;
            item_Children_MoveDown.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.Parent.SwapChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Index + 1);
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveBottom = new MenuItem() { Header = "移至最下" };
            item_Children_MoveBottom.Padding = menuItemPadding;
            item_Children_MoveBottom.Margin = menuItemMargin;
            item_Children_MoveBottom.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.Parent.MoveChild(Common.RightButtonTaxon.Index, Common.RightButtonTaxon.Parent.Children.Count - 1);
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_DeleteWithoutChildren = new MenuItem() { Header = "删除 (并且保留下级类群)" };
            item_Children_DeleteWithoutChildren.Padding = menuItemPadding;
            item_Children_DeleteWithoutChildren.Margin = menuItemMargin;
            item_Children_DeleteWithoutChildren.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.RemoveCurrent(false);
                if (Common.SelectedTaxon == Common.RightButtonTaxon) Common.SelectedTaxon = null;
                Common.RightButtonTaxon = null;
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            MenuItem item_Children_DeleteWithinChildren = new MenuItem() { Header = "删除 (并且删除下级类群)" };
            item_Children_DeleteWithinChildren.Padding = menuItemPadding;
            item_Children_DeleteWithinChildren.Margin = menuItemMargin;
            item_Children_DeleteWithinChildren.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.RemoveCurrent(true);
                if (Common.SelectedTaxon == Common.RightButtonTaxon) Common.SelectedTaxon = null;
                Common.RightButtonTaxon = null;
                _UpdateChildrenWithVisibility();
                Views.Common.UpdateTree();
            };

            Action updateMenuItems_Children = () =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                item_Children_SetParent.IsEnabled = rightButtonTaxon.CanSetParent(Common.SelectedTaxon);
                item_Children_MoveTop.IsEnabled = item_Children_MoveUp.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index > 0);
                item_Children_MoveBottom.IsEnabled = item_Children_MoveDown.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index < rightButtonTaxon.Parent.Children.Count - 1);

                if (Common.SelectedTaxon == null)
                {
                    item_Parent_Select.Header = item_Children_Select.Header = "选择";
                }
                else
                {
                    if (Common.SelectedTaxon.IsAnonymous())
                    {
                        item_Parent_Select.Header = item_Children_Select.Header = "选择 (已选择: \"(未命名)\")";
                    }
                    else
                    {
                        string taxonName = Common.SelectedTaxon.GetLongName();

                        if (taxonName.Length > 32)
                        {
                            item_Parent_Select.Header = item_Children_Select.Header = string.Concat("选择 (已选择：\"", taxonName[0..32], "...\")");
                        }
                        else
                        {
                            item_Parent_Select.Header = item_Children_Select.Header = string.Concat("选择 (已选择：\"", taxonName, "\")");
                        }
                    }
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
            _ContextMenu_Children.Items.Add(new Separator());
            _ContextMenu_Children.Items.Add(item_Children_MoveTop);
            _ContextMenu_Children.Items.Add(item_Children_MoveUp);
            _ContextMenu_Children.Items.Add(item_Children_MoveDown);
            _ContextMenu_Children.Items.Add(item_Children_MoveBottom);
            _ContextMenu_Children.Items.Add(new Separator());
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithoutChildren);
            _ContextMenu_Children.Items.Add(item_Children_DeleteWithinChildren);
            _ContextMenu_Children.DataContext = updateMenuItems_Children;
        }

        //

        #region 回调函数

        private void CategorySelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Category = categorySelector.Category;

            //

            // 更新类群的中文名
            if (!string.IsNullOrEmpty(ViewModel.ChsName))
            {
                string chsNameWithoutCategory = TaxonomicCategoryChineseExtension.SplitChineseName(ViewModel.ChsName).headPart;

                if (!string.IsNullOrEmpty(chsNameWithoutCategory))
                {
                    if (ViewModel.Category.IsClade())
                    {
                        ViewModel.ChsName = chsNameWithoutCategory + "类";
                    }
                    else if (ViewModel.Category.IsPrimaryCategory() || ViewModel.Category.IsSecondaryCategory())
                    {
                        ViewModel.ChsName = chsNameWithoutCategory + ViewModel.Category.GetChineseName();
                    }
                    else
                    {
                        ViewModel.ChsName = chsNameWithoutCategory;
                    }
                }
            }
        }

        private void Button_AddParentUplevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon parent = Views.Common.CurrentTaxon.AddParentUplevel();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                parent.ParseCurrent(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            _UpdateParents();

            Views.Common.UpdateTree();
        }

        private void Button_AddParentDownlevel_Click(object sender, RoutedEventArgs e)
        {
            Taxon parent = Views.Common.CurrentTaxon.AddParentDownlevel();

            if (!string.IsNullOrEmpty(textBox_Parent.Text))
            {
                parent.ParseCurrent(textBox_Parent.Text);

                textBox_Parent.Clear();
            }

            //

            _UpdateChildrenWithVisibility();

            Views.Common.UpdateTree();
        }

        private void Button_AddChildren_Click(object sender, RoutedEventArgs e)
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (string.IsNullOrEmpty(textBox_Children.Text))
            {
                currentTaxon.AddChild();
            }
            else
            {
                currentTaxon.ParseChildren(textBox_Children.Text.Split(Environment.NewLine));

                textBox_Children.Clear();
            }

            //

            _UpdateChildrenWithVisibility();

            Views.Common.UpdateTree();
        }

        #endregion

        #region 类群

        // 更新父类群。
        private void _UpdateParents()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            if (currentTaxon.IsRoot)
            {
                taxonNameButtonGroup_Parents.StartEditing();
                taxonNameButtonGroup_Parents.Clear();
                taxonNameButtonGroup_Parents.FinishEditing();
            }
            else
            {
                var parents = currentTaxon.GetSummaryParents(true);

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }
                else
                {
                    parents.Add(currentTaxon.Parent);
                }

                Common.UpdateParents(taxonNameButtonGroup_Parents, parents, _ContextMenu_Parent);
            }
        }

        // 更新子类群。
        private void _UpdateChildren()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            var children = currentTaxon.Children;

            Common.UpdateChildren(taxonNameButtonGroup_Children, children, _ContextMenu_Children);

            grid_Children.Visibility = (currentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新子类群及其可见性。
        private void _UpdateChildrenWithVisibility()
        {
            _UpdateChildren();

            grid_Children.Visibility = (Views.Common.CurrentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新可见性。
        private void _UpdateVisibility()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            grid_Name.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_State.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Category.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Synonyms.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Tags.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Desc.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Parents.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_AddParent.Visibility = (currentTaxon.IsRoot ? Visibility.Collapsed : Visibility.Visible);
            grid_Children.Visibility = (currentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.UpdateFromTaxon();

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