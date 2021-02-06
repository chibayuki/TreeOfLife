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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Controls;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Tree
{
    /// <summary>
    /// View_Tree.xaml 的交互逻辑
    /// </summary>
    public partial class View_Tree : UserControl
    {
        public ViewModel_Tree ViewModel => this.DataContext as ViewModel_Tree;

        //

        public View_Tree()
        {
            InitializeComponent();
        }

        //

        #region 系统发生树

        private class _TreeNode
        {
            public _TreeNode Parent { get; set; } = null;
            public List<_TreeNode> Children { get; } = new List<_TreeNode>();

            public Taxon Taxon { get; set; } = null;
            public int Sign { get; set; } = 0;
            public TreeNodeButton Button { get; set; } = null;

            public double Width => Button?.Width ?? double.NaN;
            public double Height => Button?.Height ?? double.NaN;
            public double Left { get; set; } = 0;
            public double Top { get; set; } = 0;
            public double Right => Left + Width;
            public double Bottom => Top + Height;
        }

        private _TreeNode _SubTreeRoot = null; // 子树的根节点。

        private int _ParentLevels = 1; // 向上追溯的具名父类群的层数。
        private int _ChildrenLevels = 3; // 向下追溯的具名子类群的层数。
        private int _SiblingLevels = 0; // 旁系群向下追溯的具名子类群的层数。

        // 生成一个子树节点。
        private _TreeNode _GenerateTreeNode(Taxon taxon)
        {
            return new _TreeNode() { Taxon = taxon, Button = new TreeNodeButton() { Taxon = taxon, VerticalAlignment = VerticalAlignment.Stretch } };
        }

        // 构造子树的并系群部分。
        private void _BuildSubTreeForExcludes(_TreeNode node, Taxon child)
        {
            if (node == null || child == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var exclude in child.Excludes)
            {
                if (exclude.IsNamed())
                {
                    _TreeNode excludeNode = _GenerateTreeNode(exclude);
                    excludeNode.Sign = -1;

                    node.Children.Add(excludeNode);
                    excludeNode.Parent = node;
                }
                else
                {
                    foreach (var item in exclude.GetNamedChildren(true))
                    {
                        _TreeNode excludeNode = _GenerateTreeNode(item);
                        excludeNode.Sign = -1;

                        node.Children.Add(excludeNode);
                        excludeNode.Parent = node;
                    }
                }
            }
        }

        // 构造子树的复系群部分。
        private void _BuildSubTreeForIncludes(_TreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var include in node.Taxon.Includes)
            {
                if (include.IsNamed())
                {
                    _TreeNode includeNode = _GenerateTreeNode(include);
                    includeNode.Sign = +1;

                    node.Children.Add(includeNode);
                    includeNode.Parent = node;
                }
                else
                {
                    foreach (var item in include.GetNamedChildren(true))
                    {
                        _TreeNode includeNode = _GenerateTreeNode(item);
                        includeNode.Sign = +1;

                        node.Children.Add(includeNode);
                        includeNode.Parent = node;
                    }
                }
            }
        }

        private int _CurrentChildrenDepth; // 当前递归深度。

        // 构造子树的子类群部分。
        private void _BuildSubTreeForChildren(_TreeNode node)
        {
            if (node == null)
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
                    _TreeNode childNode = _GenerateTreeNode(child);

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
        private void _BuildSubTreeForSiblings(_TreeNode node)
        {
            if (node == null)
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
                    _TreeNode childNode = _GenerateTreeNode(child);

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
        private void _BuildSubTree(_TreeNode node)
        {
            if (node == null)
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
                    _TreeNode childNode = _GenerateTreeNode(child);

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
        private void _UpdateTreeNodeAttr(_TreeNode node)
        {
            if (node != null)
            {
                node.Button.Sign = node.Sign;

                node.Button.IsRoot = (node.Parent == null);
                node.Button.IsFinal = (node.Children.Count <= 0);

                if (node.Button.IsRoot)
                {
                    node.Button.IsFirst = true;
                    node.Button.IsLast = true;
                }
                else
                {
                    node.Button.IsFirst = (node.Parent.Children.IndexOf(node) <= 0);
                    node.Button.IsLast = (node.Parent.Children.IndexOf(node) >= node.Parent.Children.Count - 1);
                }

                node.Button.ShowButton = ((Common.EditMode ?? false) ? true : node.Taxon.IsNamed());

                node.Button.Checked = (node.Taxon == Common.CurrentTaxon);
                node.Button.ThemeColor = node.Taxon.GetThemeColor();
                node.Button.IsDarkTheme = _IsDarkTheme;

                node.Button.MouseLeftButtonUp += (s, e) => Common.SetCurrentTaxon(node.Taxon);

                if (node.Children != null)
                {
                    foreach (var child in node.Children)
                    {
                        _UpdateTreeNodeAttr(child);
                    }
                }
            }
        }

        // 将所有节点添加到容器控件。
        private static void _AddTreeNodeButton(Panel panel, _TreeNode node)
        {
            if (node != null)
            {
                StackPanel stackPanelH = new StackPanel() { Orientation = Orientation.Horizontal };
                stackPanelH.Children.Add(node.Button);
                panel.Children.Add(stackPanelH);

                if (node.Children != null)
                {
                    StackPanel stackPanelV = new StackPanel() { Orientation = Orientation.Vertical };
                    stackPanelH.Children.Add(stackPanelV);

                    foreach (var child in node.Children)
                    {
                        _AddTreeNodeButton(stackPanelV, child);
                    }
                }
            }
        }

        private Taxon _NamedTaxon = null; // 当前类群或其最近的具名上级类群。

        public void UpdateSubTree()
        {
            Taxon currentTaxon = Common.CurrentTaxon;

            if (!currentTaxon.IsRoot && currentTaxon.IsAnonymous())
            {
                _NamedTaxon = currentTaxon.GetNamedParent();

                if (_NamedTaxon == null)
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

                if (parent == null)
                {
                    parent = _NamedTaxon.Root;

                    break;
                }
            }

            stackPanel_SubTree.Children.Clear();

            _SubTreeRoot = _GenerateTreeNode(parent);

            _BuildSubTree(_SubTreeRoot);

            //

            _UpdateTreeNodeAttr(_SubTreeRoot);
            _AddTreeNodeButton(stackPanel_SubTree, _SubTreeRoot);
        }

        #endregion

        #region 主题

        private void _UpdateTreeNodeTheme(_TreeNode node)
        {
            if (node != null)
            {
                node.Button.IsDarkTheme = _IsDarkTheme;

                if (node.Children != null)
                {
                    foreach (var child in node.Children)
                    {
                        _UpdateTreeNodeTheme(child);
                    }
                }
            }
        }

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateTreeNodeTheme(_SubTreeRoot);

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}