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
        public ViewModel_Evo_EditMode ViewModel => this.DataContext as ViewModel_Evo_EditMode;

        //

        public View_Evo_EditMode()
        {
            InitializeComponent();

            //

            _InitContextMenus();

            //

            label_CategoryName.ContextMenu = _ContextMenu_Current;
            label_CategoryName.MouseRightButtonUp += (s, e) =>
            {
                Common.RightButtonTaxon = Views.Common.CurrentTaxon;
                (label_CategoryName.ContextMenu.DataContext as Action)?.Invoke();
            };

            label_TaxonName.ContextMenu = _ContextMenu_Current;
            label_TaxonName.MouseRightButtonUp += (s, e) =>
            {
                Common.RightButtonTaxon = Views.Common.CurrentTaxon;
                (label_TaxonName.ContextMenu.DataContext as Action)?.Invoke();
            };

            taxonNameButtonGroup_Children.GroupNameWidth = 0;
            taxonNameButtonGroup_Children.GroupMargin = new Thickness(0, 1, 0, 1);

            taxonNameButtonGroup_Excludes.GroupNameWidth = 0;
            taxonNameButtonGroup_Excludes.GroupMargin = new Thickness(0, 1, 0, 1);

            taxonNameButtonGroup_ExcludeBy.GroupNameWidth = 0;
            taxonNameButtonGroup_ExcludeBy.GroupMargin = new Thickness(0, 1, 0, 1);

            taxonNameButtonGroup_Includes.GroupNameWidth = 0;
            taxonNameButtonGroup_Includes.GroupMargin = new Thickness(0, 1, 0, 1);

            taxonNameButtonGroup_IncludeBy.GroupNameWidth = 0;
            taxonNameButtonGroup_IncludeBy.GroupMargin = new Thickness(0, 1, 0, 1);

            button_Back.Click += (s, e) => Views.Common.ExitEditMode();

            categorySelector.MouseLeftButtonUp += CategorySelector_MouseLeftButtonUp;
            button_Rename.Click += Button_Rename_Click;
            grid_Rename.Visibility = Visibility.Collapsed;

            button_AddParentUplevel.Click += Button_AddParentUplevel_Click;
            button_AddParentDownlevel.Click += Button_AddParentDownlevel_Click;
            button_AddChildren.Click += Button_AddChildren_Click;
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
            if (selected != null && select != null)
            {
                Taxon selectedTaxon = Common.SelectedTaxon;

                selected.IsEnabled = false;
                select.IsEnabled = (selectedTaxon != Common.RightButtonTaxon);

                if (selectedTaxon == null)
                {
                    selected.Visibility = Visibility.Collapsed;
                }
                else
                {
                    selected.Visibility = Visibility.Visible;

                    if (selectedTaxon.IsAnonymous())
                    {
                        selected.Header = "已选择: \"(未命名)\"";
                    }
                    else
                    {
                        string taxonName = selectedTaxon.GetLongName();

                        if (taxonName.Length > 32)
                        {
                            selected.Header = string.Concat("已选择：\"", taxonName[0..32], "...\"");
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
            Thickness menuItemPadding = new Thickness(3, 6, 3, 6);
            Thickness menuItemMargin = new Thickness(0, 3, 0, 3);

            Thickness selectedPadding = new Thickness(3, 1, 3, 1);
            Brush selectedBackground = new SolidColorBrush(Color.FromRgb(224, 224, 224));

            //

            MenuItem item_Current_Selected = new MenuItem()
            {
                Padding = selectedPadding,
                Background = selectedBackground
            };

            MenuItem item_Current_Select = new MenuItem()
            {
                Header = "选择",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Current_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_Current_SetParent = new MenuItem()
            {
                Header = "继承选择的类群",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Current_SetParent.Click += (s, e) =>
            {
                Views.Common.CurrentTaxon.SetParent(Common.SelectedTaxon);

                _UpdateParents();

                Views.Common.UpdateTree();
            };

            MenuItem item_Current_ExcludeBy = new MenuItem()
            {
                Header = "排除自选择的类群（并系群）",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Current_ExcludeBy.Click += (s, e) =>
            {
                Common.SelectedTaxon?.AddExclude(Common.RightButtonTaxon);

                _UpdateExcludeByWithVisibility();
            };

            MenuItem item_Current_IncludeBy = new MenuItem()
            {
                Header = "包含至选择的类群（复系群）",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Current_IncludeBy.Click += (s, e) =>
            {
                Common.SelectedTaxon?.AddInclude(Common.RightButtonTaxon);

                _UpdateIncludeByWithVisibility();
            };

            Action updateMenuItems_Current = () =>
            {
                _UpdateSelectOfMenuItem(item_Current_Selected, item_Current_Select);

                Taxon currentTaxon = Views.Common.CurrentTaxon;
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

            MenuItem item_Parent_Selected = new MenuItem()
            {
                Padding = selectedPadding,
                Background = selectedBackground
            };

            MenuItem item_Parent_Select = new MenuItem()
            {
                Header = "选择",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Parent_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            Action updateMenuItems_Parent = () => _UpdateSelectOfMenuItem(item_Parent_Selected, item_Parent_Select);

            _ContextMenu_Parent = new ContextMenu();
            _ContextMenu_Parent.Items.Add(item_Parent_Selected);
            _ContextMenu_Parent.Items.Add(item_Parent_Select);
            _ContextMenu_Parent.DataContext = updateMenuItems_Parent;

            //

            MenuItem item_Children_Selected = new MenuItem()
            {
                Padding = selectedPadding,
                Background = selectedBackground
            };

            MenuItem item_Children_Select = new MenuItem()
            {
                Header = "选择",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_Children_SetParent = new MenuItem()
            {
                Header = "继承选择的类群",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_SetParent.Click += (s, e) =>
            {
                Common.RightButtonTaxon?.SetParent(Common.SelectedTaxon);

                _UpdateParents();
                _UpdateChildrenWithVisibility();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_ExcludeBy = new MenuItem()
            {
                Header = "排除自选择的类群（并系群）",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_ExcludeBy.Click += (s, e) =>
            {
                Taxon selectedTaxon = Common.SelectedTaxon;

                selectedTaxon?.AddExclude(Common.RightButtonTaxon);

                ViewModel.UpdateTitle();

                if (selectedTaxon == Views.Common.CurrentTaxon)
                {
                    _UpdateExcludesWithVisibility();
                }
            };

            MenuItem item_Children_IncludeBy = new MenuItem()
            {
                Header = "包含至选择的类群（复系群）",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_IncludeBy.Click += (s, e) =>
            {
                Taxon selectedTaxon = Common.SelectedTaxon;

                selectedTaxon?.AddInclude(Common.RightButtonTaxon);

                ViewModel.UpdateTitle();

                if (selectedTaxon == Views.Common.CurrentTaxon)
                {
                    _UpdateIncludesWithVisibility();
                }
            };

            MenuItem item_Children_MoveTop = new MenuItem()
            {
                Header = "移至最上",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_MoveTop.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.Parent.MoveChild(rightButtonTaxon.Index, 0);

                _UpdateChildren();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveUp = new MenuItem()
            {
                Header = "上移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_MoveUp.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.Parent.SwapChild(rightButtonTaxon.Index, rightButtonTaxon.Index - 1);

                _UpdateChildren();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveDown = new MenuItem()
            {
                Header = "下移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_MoveDown.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.Parent.SwapChild(rightButtonTaxon.Index, rightButtonTaxon.Index + 1);

                _UpdateChildren();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_MoveBottom = new MenuItem()
            {
                Header = "移至最下",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_MoveBottom.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.Parent.MoveChild(rightButtonTaxon.Index, rightButtonTaxon.Parent.Children.Count - 1);

                _UpdateChildren();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_DeleteWithoutChildren = new MenuItem()
            {
                Header = "删除 (并且保留下级类群)",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_DeleteWithoutChildren.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.RemoveCurrent(false);

                if (Common.SelectedTaxon == rightButtonTaxon)
                {
                    Common.SelectedTaxon = null;
                }

                Common.RightButtonTaxon = null;

                _UpdateChildrenWithVisibility();

                Views.Common.UpdateTree();
            };

            MenuItem item_Children_DeleteWithinChildren = new MenuItem()
            {
                Header = "删除 (并且删除下级类群)",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Children_DeleteWithinChildren.Click += (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                rightButtonTaxon?.RemoveCurrent(true);

                if (Common.SelectedTaxon == rightButtonTaxon)
                {
                    Common.SelectedTaxon = null;
                }

                Common.RightButtonTaxon = null;

                _UpdateChildrenWithVisibility();

                Views.Common.UpdateTree();
            };

            Action updateMenuItems_Children = () =>
            {
                _UpdateSelectOfMenuItem(item_Children_Selected, item_Children_Select);

                Taxon rightButtonTaxon = Common.RightButtonTaxon;
                Taxon selectedTaxon = Common.SelectedTaxon;

                item_Children_SetParent.IsEnabled = rightButtonTaxon.CanSetParent(selectedTaxon);
                item_Children_ExcludeBy.IsEnabled = selectedTaxon?.CanAddExclude(rightButtonTaxon) ?? false;
                item_Children_IncludeBy.IsEnabled = selectedTaxon?.CanAddInclude(rightButtonTaxon) ?? false;

                item_Children_MoveTop.IsEnabled = item_Children_MoveUp.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index > 0);
                item_Children_MoveBottom.IsEnabled = item_Children_MoveDown.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index < rightButtonTaxon.Parent.Children.Count - 1);

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

            MenuItem item_Excludes_MoveTop = new MenuItem()
            {
                Header = "移至最上",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Excludes_MoveTop.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                currentTaxon.MoveExclude(currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon), 0);

                _UpdateExcludes();
            };

            MenuItem item_Excludes_MoveUp = new MenuItem()
            {
                Header = "上移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Excludes_MoveUp.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon);

                currentTaxon.SwapExclude(index, index - 1);

                _UpdateExcludes();
            };

            MenuItem item_Excludes_MoveDown = new MenuItem()
            {
                Header = "下移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Excludes_MoveDown.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon);

                currentTaxon.SwapExclude(index, index + 1);

                _UpdateExcludes();
            };

            MenuItem item_Excludes_MoveBottom = new MenuItem()
            {
                Header = "移至最下",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Excludes_MoveBottom.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                currentTaxon.MoveExclude(currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon), currentTaxon.Excludes.Count - 1);

                _UpdateExcludes();
            };

            MenuItem item_Excludes_Remove = new MenuItem()
            {
                Header = "解除排除关系",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Excludes_Remove.Click += (s, e) =>
            {
                Views.Common.CurrentTaxon.RemoveExclude(Common.RightButtonTaxon);

                ViewModel.UpdateTitle();

                _UpdateExcludesWithVisibility();
            };

            Action updateMenuItems_Excludes = () =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfExclude(Common.RightButtonTaxon);

                item_Excludes_MoveTop.IsEnabled = item_Excludes_MoveUp.IsEnabled = (index > 0);
                item_Excludes_MoveBottom.IsEnabled = item_Excludes_MoveDown.IsEnabled = (index < currentTaxon.Excludes.Count - 1);
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

            MenuItem item_ExcludeBy_Remove = new MenuItem()
            {
                Header = "解除排除关系",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_ExcludeBy_Remove.Click += (s, e) =>
            {
                Common.RightButtonTaxon.RemoveExclude(Views.Common.CurrentTaxon);

                _UpdateExcludeByWithVisibility();
            };

            _ContextMenu_ExcludeBy = new ContextMenu();
            _ContextMenu_ExcludeBy.Items.Add(item_ExcludeBy_Remove);

            //

            MenuItem item_Includes_MoveTop = new MenuItem()
            {
                Header = "移至最上",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Includes_MoveTop.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                currentTaxon.MoveInclude(currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon), 0);

                _UpdateIncludes();
            };

            MenuItem item_Includes_MoveUp = new MenuItem()
            {
                Header = "上移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Includes_MoveUp.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon);

                currentTaxon.SwapInclude(index, index - 1);

                _UpdateIncludes();
            };

            MenuItem item_Includes_MoveDown = new MenuItem()
            {
                Header = "下移",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Includes_MoveDown.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon);

                currentTaxon.SwapInclude(index, index + 1);

                _UpdateIncludes();
            };

            MenuItem item_Includes_MoveBottom = new MenuItem()
            {
                Header = "移至最下",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Includes_MoveBottom.Click += (s, e) =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                currentTaxon.MoveInclude(currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon), currentTaxon.Includes.Count - 1);

                _UpdateIncludes();
            };

            MenuItem item_Includes_Remove = new MenuItem()
            {
                Header = "解除包含关系",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_Includes_Remove.Click += (s, e) =>
            {
                Views.Common.CurrentTaxon.RemoveInclude(Common.RightButtonTaxon);

                ViewModel.UpdateTitle();

                _UpdateIncludesWithVisibility();
            };

            Action updateMenuItems_Includes = () =>
            {
                Taxon currentTaxon = Views.Common.CurrentTaxon;

                int index = currentTaxon.GetIndexOfInclude(Common.RightButtonTaxon);

                item_Includes_MoveTop.IsEnabled = item_Includes_MoveUp.IsEnabled = (index > 0);
                item_Includes_MoveBottom.IsEnabled = item_Includes_MoveDown.IsEnabled = (index < currentTaxon.Includes.Count - 1);
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

            MenuItem item_IncludeBy_Remove = new MenuItem()
            {
                Header = "解除包含关系",
                Padding = menuItemPadding,
                Margin = menuItemMargin
            };

            item_IncludeBy_Remove.Click += (s, e) =>
            {
                Common.RightButtonTaxon.RemoveInclude(Views.Common.CurrentTaxon);

                _UpdateIncludeByWithVisibility();
            };

            _ContextMenu_IncludeBy = new ContextMenu();
            _ContextMenu_IncludeBy.Items.Add(item_IncludeBy_Remove);
        }

        //

        #region 回调函数

        private string _ChsRename = string.Empty; // 可更新的中文名。

        private void CategorySelector_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ViewModel.Category = categorySelector.Category;

            //

            _ChsRename = ViewModel.ChsName;

            if (!string.IsNullOrEmpty(ViewModel.ChsName))
            {
                string chsNameWithoutCategory = TaxonomicCategoryChineseExtension.SplitChineseName(ViewModel.ChsName).headPart;

                if (!string.IsNullOrEmpty(chsNameWithoutCategory))
                {
                    if (ViewModel.Category.IsClade())
                    {
                        _ChsRename = chsNameWithoutCategory + "类";
                    }
                    else if (ViewModel.Category.IsPrimaryCategory() || ViewModel.Category.IsSecondaryCategory())
                    {
                        _ChsRename = chsNameWithoutCategory + ViewModel.Category.GetChineseName();
                    }
                    else
                    {
                        _ChsRename = chsNameWithoutCategory;
                    }
                }
            }

            if (_ChsRename != ViewModel.ChsName)
            {
                label_Rename.Content = "更新中文名为: " + _ChsRename;

                grid_Rename.Visibility = Visibility.Visible;
            }
            else
            {
                grid_Rename.Visibility = Visibility.Collapsed;
            }
        }

        private void Button_Rename_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ChsName = _ChsRename;

            grid_Rename.Visibility = Visibility.Collapsed;
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
        }

        // 更新子类群及其可见性。
        private void _UpdateChildrenWithVisibility()
        {
            _UpdateChildren();

            grid_Children.Visibility = (Views.Common.CurrentTaxon.IsFinal ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新 Excludes。
        private void _UpdateExcludes()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            var excludes = currentTaxon.Excludes;

            Common.UpdateChildren(taxonNameButtonGroup_Excludes, excludes, _ContextMenu_Excludes);
        }

        // 更新 Excludes 及其可见性。
        private void _UpdateExcludesWithVisibility()
        {
            _UpdateExcludes();

            grid_Excludes.Visibility = (Views.Common.CurrentTaxon.Excludes.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新 ExcludeBy。
        private void _UpdateExcludeBy()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            var excludeBy = currentTaxon.ExcludeBy;

            Common.UpdateChildren(taxonNameButtonGroup_ExcludeBy, excludeBy, _ContextMenu_ExcludeBy);
        }

        // 更新 ExcludeBy 及其可见性。
        private void _UpdateExcludeByWithVisibility()
        {
            _UpdateExcludeBy();

            grid_ExcludeBy.Visibility = (Views.Common.CurrentTaxon.ExcludeBy.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新 Includes。
        private void _UpdateIncludes()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            var Includes = currentTaxon.Includes;

            Common.UpdateChildren(taxonNameButtonGroup_Includes, Includes, _ContextMenu_Includes);
        }

        // 更新 Includes 及其可见性。
        private void _UpdateIncludesWithVisibility()
        {
            _UpdateIncludes();

            grid_Includes.Visibility = (Views.Common.CurrentTaxon.Includes.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        // 更新 IncludeBy。
        private void _UpdateIncludeBy()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            var IncludeBy = currentTaxon.IncludeBy;

            Common.UpdateChildren(taxonNameButtonGroup_IncludeBy, IncludeBy, _ContextMenu_IncludeBy);
        }

        // 更新 IncludeBy 及其可见性。
        private void _UpdateIncludeByWithVisibility()
        {
            _UpdateIncludeBy();

            grid_IncludeBy.Visibility = (Views.Common.CurrentTaxon.IncludeBy.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
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
            grid_Excludes.Visibility = (currentTaxon.Excludes.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
            grid_ExcludeBy.Visibility = (currentTaxon.ExcludeBy.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
            grid_Includes.Visibility = (currentTaxon.Includes.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
            grid_IncludeBy.Visibility = (currentTaxon.IncludeBy.Count <= 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        public void UpdateCurrentTaxonInfo()
        {
            ViewModel.UpdateFromTaxon();

            categorySelector.Category = ViewModel.Category;
            grid_Rename.Visibility = Visibility.Collapsed;

            textBox_Parent.Clear();
            textBox_Children.Clear();

            _UpdateParents();
            _UpdateChildren();
            _UpdateExcludes();
            _UpdateExcludeBy();
            _UpdateIncludes();
            _UpdateIncludeBy();
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