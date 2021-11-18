/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TreeOfLife.Core.Taxonomy;

namespace TreeOfLife.UI.Controls
{
    // 树控件的节点信息。
    public sealed class TreeNodeItem
    {
        public TreeNodeItem Parent { get; set; } = null;
        public List<TreeNodeItem> Children { get; } = new List<TreeNodeItem>();

        public Taxon Taxon { get; set; } = null;
        public bool IsRef { get; set; } = false;

        public bool IsRoot { get; set; } = false;
        public bool IsFinal { get; set; } = false;
        public bool IsFirst { get; set; } = false;
        public bool IsLast { get; set; } = false;
        public bool ShowButton { get; set; } = false;

        public bool IsChecked { get; set; } = false;

        public IEnumerable<(DependencyProperty dp, object value)> Properties { get; set; } = null;
    }

    public partial class Tree : UserControl
    {
        private class _TreeNode
        {
            private StackPanel _Container = null;
            private TreeNodeButton _Button = null;
            private List<_TreeNode> _Children = null;

            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateTheme()
            {
                _Button.IsDarkTheme = _IsDarkTheme;

                foreach (var child in _Children)
                {
                    child.IsDarkTheme = _IsDarkTheme;
                }
            }

            //

            public _TreeNode(TreeNodeItem item)
            {
                if (item is null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Container = new StackPanel() { Orientation = Orientation.Horizontal };

                _Button = new TreeNodeButton()
                {
                    VerticalAlignment = VerticalAlignment.Stretch,

                    Taxon = item.Taxon,
                    IsRef = item.IsRef,

                    IsRoot = item.IsRoot,
                    IsFinal = item.IsFinal,
                    IsFirst = item.IsFirst,
                    IsLast = item.IsLast,
                    ShowButton = item.ShowButton,

                    IsChecked = item.IsChecked
                };

                var properties = item.Properties;

                if (properties is not null)
                {
                    foreach (var (dp, value) in properties)
                    {
                        _Button.SetValue(dp, value);
                    }
                }

                _Container.Children.Add(_Button);

                StackPanel childrenContainer = new StackPanel() { Orientation = Orientation.Vertical };

                _Children = new List<_TreeNode>();

                foreach (var child in item.Children)
                {
                    _TreeNode treeNode = new _TreeNode(child);

                    _Children.Add(treeNode);

                    childrenContainer.Children.Add(treeNode.Container);
                }

                _Container.Children.Add(childrenContainer);
            }

            //

            public bool IsDarkTheme
            {
                get => _IsDarkTheme;

                set
                {
                    _IsDarkTheme = value;

                    _UpdateTheme();
                }
            }

            public FrameworkElement Container => _Container;

            //

            public void UpdateContent()
            {
                _Button.UpdateContent();

                foreach (var child in _Children)
                {
                    child.UpdateContent();
                }
            }
        }

        //

        private _TreeNode _RootNode = null; // 子树的根节点。

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            if (_RootNode is not null)
            {
                _RootNode.IsDarkTheme = _IsDarkTheme;
            }
        }

        //

        public Tree()
        {
            InitializeComponent();

            //

            TreeNodeButton button = null;

            stackPanel_Tree.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TreeNodeButton source && source.VerifyMousePosition())
                {
                    button = source;
                }
            }));

            stackPanel_Tree.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TreeNodeButton source && source == button && source.VerifyMousePosition())
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                }

                button = null;
            }));

            // 不检查是否曾按下右键，因为右键菜单也不检查
            stackPanel_Tree.AddHandler(UIElement.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TreeNodeButton source && source.VerifyMousePosition())
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
        }

        //

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateTheme();
            }
        }

        //

        public void Clear()
        {
            stackPanel_Tree.Children.Clear();

            _RootNode = null;
        }

        public void UpdateContent(TreeNodeItem rootNode)
        {
            Clear();

            if (rootNode is not null)
            {
                _RootNode = new _TreeNode(rootNode);

                stackPanel_Tree.Children.Add(_RootNode.Container);

                _UpdateTheme();
            }
        }

        public void UpdateContent()
        {
            if (_RootNode is not null)
            {
                _RootNode.UpdateContent();
            }
        }

        //

        public EventHandler<TreeNodeButton> MouseLeftButtonClick;

        public EventHandler<TreeNodeButton> MouseRightButtonClick;
    }
}