﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Views;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    public sealed class TaxonNameItem
    {
        public Taxon Taxon { get; set; } = null;
        public int Sign { get; set; } = 0;

        public bool IsChecked { get; set; } = false;

        public IEnumerable<(DependencyProperty dp, object value)> Properties { get; set; } = null;
    }

    /// <summary>
    /// TaxonNameButtonGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonNameButtonGroup : UserControl
    {
        private class _Group
        {
            private DockPanel _Container = null;
            private Border _NameBorder = null;
            private TextBlock _NameText = null;
            private StackPanel _ButtonsContainer = null;
            private List<TaxonNameButton> _Buttons = null;

            private double _CategoryNameWidth = 50; // 分类阶元名称宽度。
            private double _ButtonMarginHeight = 3; // 按钮边距高度。

            private void _UpdateCategoryNameWidth()
            {
                foreach (var button in _Buttons)
                {
                    button.CategoryNameWidth = _CategoryNameWidth;
                }
            }

            private void _UpdateButtonMarginHeight()
            {
                for (int i = 0; i < _Buttons.Count; i++)
                {
                    _Buttons[i].Margin = new Thickness(0, (i > 0 ? _ButtonMarginHeight : 0), 0, 0);
                }
            }

            private ColorX _GroupNameColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateGroupNameColor()
            {
                _NameText.Foreground = (_IsDarkTheme ? Brushes.Black : Brushes.White);
                _NameBorder.Background = Common.GetSolidColorBrush(_GroupNameColor.AtLightness_LAB(50).ToWpfColor());
            }

            private void _UpdateTheme()
            {
                _UpdateGroupNameColor();

                foreach (var button in _Buttons)
                {
                    button.IsDarkTheme = _IsDarkTheme;
                }
            }

            //

            public _Group(IEnumerable<TaxonNameItem> items, string groupName = null)
            {
                if (items is null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Container = new DockPanel();

                _NameText = new TextBlock()
                {
                    Text = groupName,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                _NameBorder = new Border()
                {
                    CornerRadius = new CornerRadius(3),
                    Margin = new Thickness(0, 0, 6, 0),
                    Child = _NameText
                };

                _Container.Children.Add(_NameBorder);

                _Buttons = new List<TaxonNameButton>();

                foreach (var item in items)
                {
                    TaxonNameButton button = new TaxonNameButton()
                    {
                        Taxon = item.Taxon,
                        Sign = item.Sign,
                        IsChecked = item.IsChecked
                    };

                    var properties = item.Properties;

                    if (properties is not null)
                    {
                        foreach (var property in properties)
                        {
                            button.SetValue(property.dp, property.value);
                        }
                    }

                    _Buttons.Add(button);
                }

                _UpdateCategoryNameWidth();
                _UpdateButtonMarginHeight();
                _UpdateTheme();

                _ButtonsContainer = new StackPanel() { Orientation = Orientation.Vertical };

                foreach (var button in _Buttons)
                {
                    _ButtonsContainer.Children.Add(button);
                }

                _Container.Children.Add(_ButtonsContainer);
            }

            //

            public bool ShowGroupName
            {
                get => _NameBorder.Visibility == Visibility.Visible;
                set => _NameBorder.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
            }

            public string GroupName
            {
                get => _NameText.Text;
                set => _NameText.Text = value;
            }

            public double GroupNameWidth
            {
                get => _NameBorder.Width;
                set => _NameBorder.Width = value;
            }

            public double GroupNameMarginWidth
            {
                get => _NameBorder.Margin.Right;
                set => _NameBorder.Margin = new Thickness(0, 0, value, 0);
            }

            public double CategoryNameWidth
            {
                get => _CategoryNameWidth;

                set
                {
                    _CategoryNameWidth = value;

                    _UpdateCategoryNameWidth();
                }
            }

            public double ButtonMarginHeight
            {
                get => _ButtonMarginHeight;

                set
                {
                    _ButtonMarginHeight = value;

                    _UpdateButtonMarginHeight();
                }
            }

            public ColorX GroupNameColor
            {
                get => _GroupNameColor;

                set
                {
                    _GroupNameColor = value;

                    _UpdateGroupNameColor();
                }
            }

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

            public int ButtonsCount => _Buttons.Count;

            public Taxon GetTaxon(int buttonIndex) => _Buttons[buttonIndex].Taxon;
        }

        //

        private List<_Group> _Groups = new List<_Group>();

        private double _GroupNameWidth = 30; // 组名称宽度。
        private double _GroupMarginHeight = 6; // 组外边距高度。
        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。
        private double _ButtonMarginHeight = 3; // 按钮外边距高度。

        private void _UpdateGroupNameWidth()
        {
            foreach (var group in _Groups)
            {
                group.GroupNameWidth = _GroupNameWidth;
            }
        }

        private void _UpdateGroupMarginHeight()
        {
            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].Container.Margin = new Thickness(0, (i > 0 ? _GroupMarginHeight : 0), 0, 0);
            }
        }

        private void _UpdateCategoryNameWidth()
        {
            foreach (var group in _Groups)
            {
                group.CategoryNameWidth = _CategoryNameWidth;
            }
        }

        private void _UpdateButtonMarginHeight()
        {
            foreach (var group in _Groups)
            {
                group.ButtonMarginHeight = _ButtonMarginHeight;
            }
        }

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            foreach (var group in _Groups)
            {
                group.IsDarkTheme = _IsDarkTheme;
            }
        }

        //

        public TaxonNameButtonGroup()
        {
            InitializeComponent();

            //

            TaxonNameButton button = null;

            stackPanel_Groups.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TaxonNameButton source)
                {
                    button = source;
                }
            }));

            stackPanel_Groups.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TaxonNameButton source && source == button)
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                    button = null;
                }
            }));

            // 不检查是否曾按下右键，因为右键菜单也不检查
            stackPanel_Groups.AddHandler(UIElement.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TaxonNameButton source)
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
        }

        //

        public double GroupNameWidth
        {
            get => _GroupNameWidth;

            set
            {
                _GroupNameWidth = value;

                _UpdateGroupNameWidth();
            }
        }

        public double GroupMarginHeight
        {
            get => _GroupMarginHeight;

            set
            {
                _GroupMarginHeight = value;

                _UpdateGroupMarginHeight();
            }
        }

        public double CategoryNameWidth
        {
            get => _CategoryNameWidth;

            set
            {
                _CategoryNameWidth = value;

                _UpdateCategoryNameWidth();
            }
        }

        public double ButtonMarginHeight
        {
            get => _ButtonMarginHeight;

            set
            {
                _ButtonMarginHeight = value;

                _UpdateButtonMarginHeight();
            }
        }

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

        // 获取组数目。
        public int GetGroupCount() => _Groups.Count;

        // 获取指定组的按钮数目。
        public int GetButtonCount(int groupIndex) => _Groups[groupIndex].ButtonsCount;

        // 获取指定组的指定按钮关联的类群。
        public Taxon GetTaxon(int groupIndex, int buttonIndex) => _Groups[groupIndex].GetTaxon(buttonIndex);

        public void Clear()
        {
            stackPanel_Groups.Children.Clear();

            _Groups.Clear();
        }

        private void _UIAddGroups()
        {
            foreach (var group in _Groups)
            {
                stackPanel_Groups.Children.Add(group.Container);
            }

            _UpdateGroupNameWidth();
            _UpdateGroupMarginHeight();
            _UpdateCategoryNameWidth();
            _UpdateButtonMarginHeight();
            _UpdateTheme();
        }

        public void UpdateContent(IEnumerable<TaxonNameItem> items)
        {
            Clear();

            if (items is not null && items.Any())
            {
                _Groups.Add(new _Group(items) { ShowGroupName = false });

                _UIAddGroups();
            }
        }

        public void UpdateContent(double groupNameWidth, IEnumerable<(string groupName, ColorX groupColor, IEnumerable<TaxonNameItem> items)> groups)
        {
            Clear();

            if (groups is not null && groups.Any())
            {
                _GroupNameWidth = groupNameWidth;

                foreach (var group in groups)
                {
                    if (group.items is not null && group.items.Any())
                    {
                        _Groups.Add(new _Group(group.items, group.groupName) { GroupNameColor = group.groupColor });
                    }
                }

                _UIAddGroups();
            }
        }

        //

        public EventHandler<TaxonNameButton> MouseLeftButtonClick;

        public EventHandler<TaxonNameButton> MouseRightButtonClick;
    }
}