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

using TreeOfLife.Taxonomy;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    // 树控件的节点信息。
    public class TreeNodeItem
    {
        public TreeNodeItem Parent { get; set; } = null;
        public List<TreeNodeItem> Children { get; } = new List<TreeNodeItem>();

        public Taxon Taxon { get; set; } = null;
        public int Sign { get; set; } = 0;

        public bool IsRoot { get; set; } = false;
        public bool IsFinal { get; set; } = false;
        public bool IsFirst { get; set; } = false;
        public bool IsLast { get; set; } = false;
        public bool ShowButton { get; set; } = false;
        public bool Checked { get; set; } = false;

        public ContextMenu ContextMenu { get; set; } = null;

        public ColorX ThemeColor { get; set; } = ColorX.FromRGB(128, 128, 128);
    }

    /// <summary>
    /// Tree.xaml 的交互逻辑
    /// </summary>
    public partial class Tree : UserControl
    {
        private class _TreeNode
        {
            public _TreeNode(TreeNodeItem node)
            {
                Button = new TreeNodeButton()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,

                    Taxon = node.Taxon,
                    Sign = node.Sign,

                    IsRoot = node.IsRoot,
                    IsFinal = node.IsFinal,
                    IsFirst = node.IsFirst,
                    IsLast = node.IsLast,
                    ShowButton = node.ShowButton,
                    Checked = node.Checked,

                    ContextMenu = node.ContextMenu,

                    ThemeColor = node.ThemeColor
                };
            }

            //

            public _TreeNode Parent { get; set; } = null;
            public List<_TreeNode> Children { get; } = new List<_TreeNode>();

            public TreeNodeButton Button { get; set; } = null;

            //

            public static _TreeNode BuildTree(TreeNodeItem node)
            {
                if (node == null)
                {
                    throw new ArgumentNullException();
                }

                //

                _TreeNode _Node = new _TreeNode(node);

                foreach (var child in node.Children)
                {
                    _TreeNode _Child = BuildTree(child);

                    _Node.Children.Add(_Child);
                    _Child.Parent = _Node;
                }

                return _Node;
            }
        }

        private _TreeNode _RootNode = null; // 子树的根节点。

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public Tree()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTreeNodeFont(_RootNode);
                _UpdateTreeNodeTheme(_RootNode);
            };

            stackPanel_Tree.AddHandler(UserControl.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TreeNodeButton source)
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                }
            }));
            stackPanel_Tree.AddHandler(UserControl.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TreeNodeButton source)
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
        }

        //

        private void _UpdateTreeNodeFont(_TreeNode node)
        {
            if (node != null)
            {
                node.Button.FontFamily = this.FontFamily;
                node.Button.FontSize = this.FontSize;
                node.Button.FontStyle = this.FontStyle;
                node.Button.FontWeight = this.FontWeight;
                node.Button.FontStretch = this.FontStretch;

                if (node.Children != null)
                {
                    foreach (var child in node.Children)
                    {
                        _UpdateTreeNodeFont(child);
                    }
                }
            }
        }

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

        //

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateTreeNodeTheme(_RootNode);
            }
        }

        //

        public void Clear()
        {
            stackPanel_Tree.Children.Clear();

            _RootNode = null;
        }

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

        public void UpdateContent(TreeNodeItem rootNode)
        {
            stackPanel_Tree.Children.Clear();

            if (rootNode == null)
            {
                _RootNode = null;
            }
            else
            {
                _RootNode = _TreeNode.BuildTree(rootNode);

                _AddTreeNodeButton(stackPanel_Tree, _RootNode);
            }
        }

        //

        public EventHandler<TreeNodeButton> MouseLeftButtonClick;

        public EventHandler<TreeNodeButton> MouseRightButtonClick;
    }
}