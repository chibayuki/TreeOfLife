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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Core;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views.Tree
{
    /// <summary>
    /// View_Tree.xaml 的交互逻辑
    /// </summary>
    public partial class View_Tree : UserControl
    {
        public View_Tree()
        {
            InitializeComponent();

            //

            _InitContextMenus();

            //

            tree.MouseLeftButtonClick += (s, e) => Common.SetCurrentTaxon(e.Taxon);
            tree.MouseRightButtonClick += (s, e) =>
            {
                if (Common.EditMode ?? false)
                {
                    Common.RightButtonTaxon = e.Taxon;
                    (_ContextMenu.DataContext as Action)?.Invoke();
                }
            };

            //

            Theme.IsDarkThemeChanged += (s, e) => tree.IsDarkTheme = Theme.IsDarkTheme;
        }

        private ContextMenu _ContextMenu;

        private void _UpdateSelectOfMenuItem(MenuItem selected, MenuItem select)
        {
            if (selected is not null && select is not null)
            {
                Taxon selectedTaxon = Common.SelectedTaxon;

                selected.IsEnabled = false;
                select.IsEnabled = (selectedTaxon != Common.RightButtonTaxon);

                if (selectedTaxon is null)
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
            MenuItem item_Selected = new MenuItem();

            MenuItem item_Select = new MenuItem() { Header = "选择" };

            item_Select.Click += (s, e) => Common.SelectedTaxon = Common.RightButtonTaxon;

            MenuItem item_SetParent = new MenuItem() { Header = "继承选择的类群" };

            item_SetParent.Click += async (s, e) =>
            {
                await Common.RightButtonTaxon.SetParentAsync(Common.SelectedTaxon);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_ExcludeBy = new MenuItem() { Header = "排除自选择的类群（并系群）" };

            item_ExcludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddExcludeAsync(Common.RightButtonTaxon);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_IncludeBy = new MenuItem() { Header = "包含至选择的类群（复系群）" };

            item_IncludeBy.Click += async (s, e) =>
            {
                await Common.SelectedTaxon.AddIncludeAsync(Common.RightButtonTaxon);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_MoveTop = new MenuItem() { Header = "移至最上" };

            item_MoveTop.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.MoveChildAsync(rightButtonTaxon.Index, 0);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_MoveUp = new MenuItem() { Header = "上移" };

            item_MoveUp.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.SwapChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Index - 1);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_MoveDown = new MenuItem() { Header = "下移" };

            item_MoveDown.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.SwapChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Index + 1);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_MoveBottom = new MenuItem() { Header = "移至最下" };

            item_MoveBottom.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                await rightButtonTaxon.Parent.MoveChildAsync(rightButtonTaxon.Index, rightButtonTaxon.Parent.Children.Count - 1);

                Common.ApplyToTaxon();
                Common.UpdateCurrentTaxonInfo();

                UpdateSubTree();
            };

            MenuItem item_DeleteWithoutChildren = new MenuItem() { Header = "删除 (并且保留下级类群)" };

            item_DeleteWithoutChildren.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                // 如果当前类群是要删除的类群，删除后需跳转至被删除类群的父类群
                if (Common.CurrentTaxon == rightButtonTaxon)
                {
                    Taxon taxon = rightButtonTaxon.Parent;

                    await rightButtonTaxon.RemoveCurrentAsync(false);

                    if (Common.SelectedTaxon == rightButtonTaxon)
                    {
                        Common.SelectedTaxon = null;
                    }

                    Common.RightButtonTaxon = null;

                    Common.SetCurrentTaxon(taxon);
                }
                else
                {
                    await rightButtonTaxon.RemoveCurrentAsync(false);

                    if (Common.SelectedTaxon == rightButtonTaxon)
                    {
                        Common.SelectedTaxon = null;
                    }

                    Common.RightButtonTaxon = null;

                    Common.ApplyToTaxon();
                    Common.UpdateCurrentTaxonInfo();

                    UpdateSubTree();
                }
            };

            MenuItem item_DeleteWithinChildren = new MenuItem() { Header = "删除 (并且删除下级类群)" };

            item_DeleteWithinChildren.Click += async (s, e) =>
            {
                Taxon rightButtonTaxon = Common.RightButtonTaxon;

                // 如果当前类群继承自要删除的类群，删除后需跳转至被删除类群的父类群
                if (Common.CurrentTaxon.InheritFrom(rightButtonTaxon))
                {
                    Taxon taxon = rightButtonTaxon.Parent;

                    await rightButtonTaxon.RemoveCurrentAsync(true);

                    if (Common.SelectedTaxon == rightButtonTaxon)
                    {
                        Common.SelectedTaxon = null;
                    }

                    Common.RightButtonTaxon = null;

                    Common.SetCurrentTaxon(taxon);
                }
                else
                {
                    await rightButtonTaxon.RemoveCurrentAsync(true);

                    if (Common.SelectedTaxon == rightButtonTaxon)
                    {
                        Common.SelectedTaxon = null;
                    }

                    Common.RightButtonTaxon = null;

                    Common.ApplyToTaxon();
                    Common.UpdateCurrentTaxonInfo();

                    UpdateSubTree();
                }
            };

            Action updateMenuItems = () =>
            {
                _UpdateSelectOfMenuItem(item_Selected, item_Select);

                Taxon rightButtonTaxon = Common.RightButtonTaxon;
                Taxon selectedTaxon = Common.SelectedTaxon;

                item_SetParent.IsEnabled = rightButtonTaxon.CanSetParent(selectedTaxon);
                item_ExcludeBy.IsEnabled = selectedTaxon?.CanAddExclude(rightButtonTaxon) ?? false;
                item_IncludeBy.IsEnabled = selectedTaxon?.CanAddInclude(rightButtonTaxon) ?? false;

                item_MoveTop.IsEnabled = item_MoveUp.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index > 0);
                item_MoveBottom.IsEnabled = item_MoveDown.IsEnabled = (!rightButtonTaxon.IsRoot && rightButtonTaxon.Index < rightButtonTaxon.Parent.Children.Count - 1);

                if (rightButtonTaxon.IsRoot)
                {
                    item_DeleteWithoutChildren.Visibility = Visibility.Collapsed;

                    item_DeleteWithinChildren.IsEnabled = false;
                    item_DeleteWithinChildren.Header = "删除";
                }
                else
                {
                    item_DeleteWithinChildren.IsEnabled = true;

                    if (rightButtonTaxon.IsFinal)
                    {
                        item_DeleteWithoutChildren.Visibility = Visibility.Collapsed;

                        item_DeleteWithinChildren.Header = "删除";
                    }
                    else
                    {
                        item_DeleteWithoutChildren.Visibility = Visibility.Visible;

                        item_DeleteWithinChildren.Header = "删除 (并且删除下级类群)";
                    }
                }
            };

            _ContextMenu = new ContextMenu();
            _ContextMenu.Items.Add(item_Selected);
            _ContextMenu.Items.Add(item_Select);
            _ContextMenu.Items.Add(new Separator());
            _ContextMenu.Items.Add(item_SetParent);
            _ContextMenu.Items.Add(item_ExcludeBy);
            _ContextMenu.Items.Add(item_IncludeBy);
            _ContextMenu.Items.Add(new Separator());
            _ContextMenu.Items.Add(item_MoveTop);
            _ContextMenu.Items.Add(item_MoveUp);
            _ContextMenu.Items.Add(item_MoveDown);
            _ContextMenu.Items.Add(item_MoveBottom);
            _ContextMenu.Items.Add(new Separator());
            _ContextMenu.Items.Add(item_DeleteWithoutChildren);
            _ContextMenu.Items.Add(item_DeleteWithinChildren);
            _ContextMenu.DataContext = updateMenuItems;
        }

        //

        public ViewModel_Tree ViewModel => this.DataContext as ViewModel_Tree;

        //

        private TreeNodeItem _SubTreeRoot = null; // 子树的根节点。

        private int _ParentLevels = 1; // 向上追溯的具名父类群的层数。
        private int _ChildrenLevels = 3; // 向下追溯的具名子类群的层数。
        private int _SiblingLevels = 0; // 旁系群向下追溯的具名子类群的层数。

        // 构造子树的并系群部分。
        private void _BuildSubTreeForExcludes(TreeNodeItem node, Taxon child)
        {
            if (node is null || child is null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var exclude in child.Excludes)
            {
                if (exclude.IsNamed())
                {
                    TreeNodeItem excludeNode = new TreeNodeItem() { Taxon = exclude };
                    excludeNode.Sign = -1;

                    node.Children.Add(excludeNode);
                    excludeNode.Parent = node;
                }
                else
                {
                    foreach (var item in exclude.GetNamedChildren(true))
                    {
                        TreeNodeItem excludeNode = new TreeNodeItem() { Taxon = item };
                        excludeNode.Sign = -1;

                        node.Children.Add(excludeNode);
                        excludeNode.Parent = node;
                    }
                }
            }
        }

        // 构造子树的复系群部分。
        private void _BuildSubTreeForIncludes(TreeNodeItem node)
        {
            if (node is null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var include in node.Taxon.Includes)
            {
                if (include.IsNamed())
                {
                    TreeNodeItem includeNode = new TreeNodeItem() { Taxon = include };
                    includeNode.Sign = +1;

                    node.Children.Add(includeNode);
                    includeNode.Parent = node;
                }
                else
                {
                    foreach (var item in include.GetNamedChildren(true))
                    {
                        TreeNodeItem includeNode = new TreeNodeItem() { Taxon = item };
                        includeNode.Sign = +1;

                        node.Children.Add(includeNode);
                        includeNode.Parent = node;
                    }
                }
            }
        }

        private int _CurrentChildrenDepth; // 当前递归深度。

        // 构造子树的子类群部分。
        private void _BuildSubTreeForChildren(TreeNodeItem node)
        {
            if (node is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (node.Taxon.IsRoot || node.Taxon.IsNamed())
            {
                _CurrentChildrenDepth++;
            }

            if (_CurrentChildrenDepth < _ChildrenLevels)
            {
                foreach (var child in node.Taxon.Children)
                {
                    TreeNodeItem childNode = new TreeNodeItem() { Taxon = child };

                    node.Children.Add(childNode);
                    childNode.Parent = node;

                    _BuildSubTreeForChildren(childNode);

                    if (!(Common.EditMode ?? false))
                    {
                        // 若子类群是并系群，添加并系群排除的类群
                        _BuildSubTreeForExcludes(node, child);
                    }
                }

                if (!(Common.EditMode ?? false))
                {
                    // 若当前类群是复系群，添加复系群包含的类群
                    _BuildSubTreeForIncludes(node);
                }
            }

            if (node.Taxon.IsRoot || node.Taxon.IsNamed())
            {
                _CurrentChildrenDepth--;
            }
        }

        private int _CurrentSiblingsDepth; // 当前递归深度。

        // 构造子树的旁系群部分。
        private void _BuildSubTreeForSiblings(TreeNodeItem node)
        {
            if (node is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (node.Taxon.IsRoot || node.Taxon.IsNamed())
            {
                _CurrentSiblingsDepth++;
            }

            if (_CurrentSiblingsDepth < _SiblingLevels)
            {
                foreach (var child in node.Taxon.Children)
                {
                    TreeNodeItem childNode = new TreeNodeItem() { Taxon = child };

                    node.Children.Add(childNode);
                    childNode.Parent = node;

                    _BuildSubTreeForSiblings(childNode);

                    if (!(Common.EditMode ?? false))
                    {
                        // 若子类群是并系群，添加并系群排除的类群
                        _BuildSubTreeForExcludes(node, child);
                    }
                }

                if (!(Common.EditMode ?? false))
                {
                    // 若当前类群是复系群，添加复系群包含的类群
                    _BuildSubTreeForIncludes(node);
                }
            }

            if (node.Taxon.IsRoot || node.Taxon.IsNamed())
            {
                _CurrentSiblingsDepth--;
            }
        }

        // 构造子树。
        private void _BuildSubTree(TreeNodeItem node)
        {
            if (node is null)
            {
                throw new ArgumentNullException();
            }

            //

            if (node.Taxon == _NamedTaxon)
            {
                _CurrentChildrenDepth = -1;

                _BuildSubTreeForChildren(node);
            }
            else if (node.Taxon.IsNamed() && !_NamedTaxon.InheritFrom(node.Taxon))
            {
                _CurrentSiblingsDepth = -1;

                _BuildSubTreeForSiblings(node);
            }
            else
            {
                foreach (var child in node.Taxon.Children)
                {
                    TreeNodeItem childNode = new TreeNodeItem() { Taxon = child };

                    node.Children.Add(childNode);
                    childNode.Parent = node;

                    _BuildSubTree(childNode);

                    if (!(Common.EditMode ?? false))
                    {
                        // 若子类群是并系群，添加并系群排除的类群
                        _BuildSubTreeForExcludes(node, child);
                    }
                }

                if (!(Common.EditMode ?? false))
                {
                    // 若当前类群是复系群，添加复系群包含的类群
                    _BuildSubTreeForIncludes(node);
                }
            }
        }

        // 更新所有节点的属性。
        private void _UpdateTreeNodeAttr(TreeNodeItem node)
        {
            if (node is not null)
            {
                node.Sign = node.Sign;

                node.IsRoot = (node.Parent is null);
                node.IsFinal = (node.Children.Count <= 0);

                if (node.IsRoot)
                {
                    node.IsFirst = true;
                    node.IsLast = true;
                }
                else
                {
                    node.IsFirst = (node.Parent.Children.IndexOf(node) <= 0);
                    node.IsLast = (node.Parent.Children.IndexOf(node) >= node.Parent.Children.Count - 1);
                }

                node.ShowButton = ((Common.EditMode ?? false) ? true : node.Taxon.IsNamed());
                node.IsChecked = (node.Taxon == Common.CurrentTaxon);

                if (Common.EditMode ?? false)
                {
                    node.Properties = new (DependencyProperty, object)[] { (FrameworkElement.ContextMenuProperty, _ContextMenu) };
                }

                if (node.Children is not null)
                {
                    foreach (var child in node.Children)
                    {
                        _UpdateTreeNodeAttr(child);
                    }
                }
            }
        }

        private Taxon _NamedTaxon = null; // 当前类群或其最近的具名上级类群。

        // 更新子树。
        public void UpdateSubTree()
        {
            if (Entrance.Root.IsFinal)
            {
                _NamedTaxon = Entrance.Root;

                _SubTreeRoot = new TreeNodeItem() { Taxon = _NamedTaxon };
            }
            else
            {
                Taxon currentTaxon = Common.CurrentTaxon;

                if (!currentTaxon.IsRoot && currentTaxon.IsAnonymous())
                {
                    _NamedTaxon = currentTaxon.GetNamedParent();

                    if (_NamedTaxon is null)
                    {
                        _NamedTaxon = currentTaxon.Root;
                    }
                }
                else
                {
                    _NamedTaxon = currentTaxon;
                }

                Taxon parent = _NamedTaxon;

                for (int i = 0; i < _ParentLevels; i++)
                {
                    parent = parent.GetNamedParent();

                    if (parent is null)
                    {
                        parent = _NamedTaxon.Root;

                        break;
                    }
                }

                _SubTreeRoot = new TreeNodeItem() { Taxon = parent };

                _BuildSubTree(_SubTreeRoot);
            }

            //

            _UpdateTreeNodeAttr(_SubTreeRoot);

            tree.UpdateContent(_SubTreeRoot);
        }
    }
}