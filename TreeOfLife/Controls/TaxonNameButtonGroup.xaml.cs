/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TaxonNameButtonGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonNameButtonGroup : UserControl
    {
        private class _Group
        {
            private string _Name; // 组名称。
            private ColorX _ThemeColor; // 主题颜色。
            private bool _DarkTheme = false; // 是否为暗色主题。

            private Grid _GroupPanel = new Grid();
            private Label _NameLabel = new Label();
            private List<TaxonNameButton> _Buttons = new List<TaxonNameButton>();

            public _Group(string name, ColorX themeColor, bool isDarkTheme)
            {
                _Name = name;
                _ThemeColor = themeColor;
                _DarkTheme = isDarkTheme;

                _GroupPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _GroupPanel.VerticalAlignment = VerticalAlignment.Top;
                _GroupPanel.ColumnDefinitions.Add(new ColumnDefinition());
                _GroupPanel.ColumnDefinitions.Add(new ColumnDefinition());

                _NameLabel.Padding = new Thickness(0);
                _NameLabel.Margin = new Thickness(0);
                _NameLabel.Content = _Name;
                _NameLabel.HorizontalContentAlignment = HorizontalAlignment.Center;
                _NameLabel.VerticalContentAlignment = VerticalAlignment.Center;
            }

            public string Name
            {
                get
                {
                    return _Name;
                }

                set
                {
                    _Name = value;

                    _NameLabel.Content = _Name;
                }
            }

            public ColorX ThemeColor
            {
                get
                {
                    return _ThemeColor;
                }

                set
                {
                    _ThemeColor = value;

                    UpdateColor();
                }
            }

            public bool IsDarkTheme
            {
                get
                {
                    return _DarkTheme;
                }

                set
                {
                    _DarkTheme = value;

                    UpdateColor();
                }
            }

            public Panel GroupPanel => _GroupPanel;

            public List<TaxonNameButton> Buttons => _Buttons;

            public void UpdateFont(FontFamily fontFamily, double fontSize)
            {
                _NameLabel.FontFamily = fontFamily;
                _NameLabel.FontSize = fontSize;
                _NameLabel.FontStyle = FontStyles.Normal;
                _NameLabel.FontWeight = FontWeights.Normal;

                foreach (var button in _Buttons)
                {
                    Taxon taxon = button.Taxon;

                    TaxonomicCategory category = taxon.Category;
                    bool basicPrimary = category.IsBasicPrimaryCategory();
                    bool bellowGenus = (category.IsPrimaryCategory() || category.IsSecondaryCategory()) && taxon.GetInheritedBasicPrimaryCategory() <= TaxonomicCategory.Genus;

                    button.FontFamily = fontFamily;
                    button.FontSize = fontSize;
                    button.FontStyle = (bellowGenus ? FontStyles.Italic : FontStyles.Normal);
                    button.FontWeight = (basicPrimary ? FontWeights.Bold : FontWeights.Normal);
                }
            }

            public void UpdateColor()
            {
                _NameLabel.SetForeColor(_DarkTheme ? Colors.Black : Colors.White);
                _NameLabel.SetBackColor(_ThemeColor.AtLightness_LAB(50).ToWpfColor());

                foreach (var button in _Buttons)
                {
                    button.IsDarkTheme = _DarkTheme;
                    button.ThemeColor = _ThemeColor;
                }
            }

            public void UpdateControls()
            {
                _GroupPanel.Children.Clear();
                _GroupPanel.RowDefinitions.Clear();

                for (int i = 0; i < _Buttons.Count; i++)
                {
                    TaxonNameButton button = _Buttons[i];

                    _GroupPanel.RowDefinitions.Add(new RowDefinition());
                    _GroupPanel.Children.Add(button);
                    Grid.SetColumn(button, 1);
                    Grid.SetRow(button, i);
                }

                _GroupPanel.Children.Add(_NameLabel);

                if (_Buttons.Count > 0)
                {
                    Grid.SetRowSpan(_NameLabel, _Buttons.Count);
                }
            }

            public void UpdateLayout(double groupNameWidth, Thickness groupPadding, double categoryNameWidth, double buttonHeight, Thickness buttonPadding)
            {
                if (_Buttons.Count > 0)
                {
                    buttonPadding.Right = 0;

                    if (groupNameWidth <= 0)
                    {
                        buttonPadding.Left = 0;
                    }

                    _NameLabel.HorizontalAlignment = HorizontalAlignment.Stretch;
                    _NameLabel.VerticalAlignment = VerticalAlignment.Stretch;

                    _GroupPanel.Margin = groupPadding;
                    _GroupPanel.Height = _Buttons.Count * (buttonHeight + buttonPadding.Vertical());
                    _GroupPanel.ColumnDefinitions[0].Width = new GridLength(groupNameWidth, GridUnitType.Pixel);

                    for (int i = 0; i < _Buttons.Count; i++)
                    {
                        TaxonNameButton button = _Buttons[i];

                        if (_Buttons.Count == 1) button.Margin = new Thickness(buttonPadding.Left, 0, buttonPadding.Right, 0);
                        else if (i == 0) button.Margin = new Thickness(buttonPadding.Left, 0, buttonPadding.Right, buttonPadding.Bottom);
                        else if (i == _Buttons.Count - 1) button.Margin = new Thickness(buttonPadding.Left, buttonPadding.Top, buttonPadding.Right, 0);
                        else button.Margin = buttonPadding;

                        button.HorizontalAlignment = HorizontalAlignment.Stretch;
                        button.VerticalAlignment = VerticalAlignment.Stretch;
                        button.CategoryNameWidth = categoryNameWidth;

                        _GroupPanel.RowDefinitions[i].Height = new GridLength(buttonHeight + buttonPadding.Vertical(), GridUnitType.Pixel);
                    }
                }
                else
                {
                    _GroupPanel.Margin = new Thickness(0);
                    _GroupPanel.Height = 0;
                }
            }
        }

        //

        private List<_Group> _Groups = new List<_Group>();

        private double _GroupNameWidth = 30; // 组名称宽度。
        private Thickness _GroupMargin = new Thickness(2); // 组外边距。
        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。
        private double _ButtonHeight = 22; // 按钮高度。
        private Thickness _ButtonMargin = new Thickness(4, 1, 0, 1); // 按钮外边距。
        private bool _DarkTheme = false; // 是否为暗色主题。

        private bool _Editing = false; // 是否正在编辑。

        //

        public TaxonNameButtonGroup()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateFont();
                _UpdateColor();
                _UpdateControls();
                _UpdateLayout();
            };
        }

        //

        private void _UpdateFont()
        {
            foreach (var group in _Groups)
            {
                group.UpdateFont(this.FontFamily, this.FontSize);
            }
        }

        private void _UpdateColor()
        {
            foreach (var group in _Groups)
            {
                group.IsDarkTheme = _DarkTheme;
                group.UpdateColor();
            }
        }

        private void _UpdateControls()
        {
            stackPanel_Groups.Children.Clear();

            foreach (var group in _Groups)
            {
                group.UpdateControls();

                stackPanel_Groups.Children.Add(group.GroupPanel);
            }
        }

        private void _UpdateLayout()
        {
            double height = _GroupMargin.Vertical() * _Groups.Count;

            foreach (var group in _Groups)
            {
                group.UpdateLayout(_GroupNameWidth, _GroupMargin, _CategoryNameWidth, _ButtonHeight, _ButtonMargin);
                height += group.GroupPanel.Height;
            }

            this.Height = (_Groups.Count > 0 ? height : 0);
        }

        //

        public double GroupNameWidth
        {
            get
            {
                return _GroupNameWidth;
            }

            set
            {
                _GroupNameWidth = value;

                if (!_Editing)
                {
                    _UpdateLayout();
                }
            }
        }

        public Thickness GroupMargin
        {
            get
            {
                return _GroupMargin;
            }

            set
            {
                _GroupMargin = value;

                if (!_Editing)
                {
                    _UpdateLayout();
                }
            }
        }

        public double CategoryNameWidth
        {
            get
            {
                return _CategoryNameWidth;
            }

            set
            {
                _CategoryNameWidth = value;

                if (!_Editing)
                {
                    _UpdateLayout();
                }
            }
        }

        public double ButtonHeight
        {
            get
            {
                return _ButtonHeight;
            }

            set
            {
                _ButtonHeight = value;

                if (!_Editing)
                {
                    _UpdateLayout();
                }
            }
        }

        public Thickness ButtonMargin
        {
            get
            {
                return _ButtonMargin;
            }

            set
            {
                _ButtonMargin = value;

                if (!_Editing)
                {
                    _UpdateLayout();
                }
            }
        }

        public bool IsDarkTheme
        {
            get
            {
                return _DarkTheme;
            }

            set
            {
                _DarkTheme = value;

                if (!_Editing)
                {
                    _UpdateColor();
                }
            }
        }

        //

        // 获取组数目。
        public int GroupCount()
        {
            return _Groups.Count;
        }

        // 获取指定组的按钮数目。
        public int ButtonCount(int groupIndex)
        {
            return _Groups[groupIndex].Buttons.Count;
        }

        // 添加一个组。
        public void AddGroup(string name, ColorX color, int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Insert(groupIndex, new _Group(name, color, _DarkTheme));
        }

        // 添加一个组。
        public void AddGroup(string name, ColorX color)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Add(new _Group(name, color, _DarkTheme));
        }

        // 删除一个组。
        public void RemoveGroup(int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.RemoveAt(groupIndex);
        }

        // 添加一个按钮。
        public void AddButton(TaxonNameButton button, int groupIndex, int buttonIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.Insert(buttonIndex, button);
        }

        // 添加一个按钮。
        public void AddButton(TaxonNameButton button, int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.Add(button);
        }

        // 删除一个按钮。
        public void RemoveButton(int groupIndex, int buttonIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.RemoveAt(buttonIndex);
        }

        // 删除所有组与按钮。
        public void Clear()
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Clear();
        }

        // 开始编辑。
        public void StartEditing()
        {
            if (!_Editing)
            {
                _Editing = true;
            }
        }

        // 完成编辑。
        public void FinishEditing()
        {
            if (_Editing)
            {
                stackPanel_Groups.Visibility = Visibility.Hidden;

                _UpdateFont();
                _UpdateColor();
                _UpdateControls();
                _UpdateLayout();

                stackPanel_Groups.Visibility = Visibility.Visible;

                _Editing = false;
            }
        }
    }
}