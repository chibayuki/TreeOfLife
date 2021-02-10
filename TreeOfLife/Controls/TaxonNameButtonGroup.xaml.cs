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

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    public class TaxonNameItem
    {
        public Taxon Taxon { get; set; } = null;
        public int Sign { get; set; } = 0;

        public bool Checked { get; set; } = false;

        public ContextMenu ContextMenu { get; set; } = null;

        public ColorX ThemeColor { get; set; } = ColorX.FromRGB(128, 128, 128);
    }

    /// <summary>
    /// TaxonNameButtonGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonNameButtonGroup : UserControl
    {
        private class _Group
        {
            private string _Name; // 组名称。

            private ColorX _ThemeColor; // 主题颜色。
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private Grid _GroupPanel = new Grid();
            private Label _NameLabel = new Label();
            private List<TaxonNameButton> _Buttons = new List<TaxonNameButton>();

            public _Group()
            {
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
                get => _Name;

                set
                {
                    _Name = value;

                    _NameLabel.Content = _Name;
                }
            }

            public ColorX ThemeColor
            {
                get => _ThemeColor;

                set
                {
                    _ThemeColor = value;

                    UpdateColor();
                }
            }

            public bool IsDarkTheme
            {
                get => _IsDarkTheme;

                set
                {
                    _IsDarkTheme = value;

                    UpdateTheme();
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
                _NameLabel.Foreground = (_IsDarkTheme ? Brushes.Black : Brushes.White);
                _NameLabel.Background = new SolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }

            public void UpdateTheme()
            {
                _NameLabel.Foreground = (_IsDarkTheme ? Brushes.Black : Brushes.White);
                _NameLabel.Background = new SolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());

                foreach (var button in _Buttons)
                {
                    button.IsDarkTheme = _IsDarkTheme;
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
                    _GroupPanel.Height = _Buttons.Count * (buttonHeight + (buttonPadding.Top + buttonPadding.Bottom));
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

                        _GroupPanel.RowDefinitions[i].Height = new GridLength(buttonHeight + (buttonPadding.Top + buttonPadding.Bottom), GridUnitType.Pixel);
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
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public TaxonNameButtonGroup()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateFont();
                _UpdateLayout();
            };

            stackPanel_Groups.AddHandler(UserControl.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TaxonNameButton source)
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                }
            }));
            stackPanel_Groups.AddHandler(UserControl.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is TaxonNameButton source)
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
        }

        //

        private void _UpdateFont()
        {
            foreach (var group in _Groups)
            {
                group.UpdateFont(this.FontFamily, this.FontSize);
            }
        }

        private void _UpdateTheme()
        {
            foreach (var group in _Groups)
            {
                group.IsDarkTheme = _IsDarkTheme;
            }
        }

        private void _UpdateLayout()
        {
            foreach (var group in _Groups)
            {
                group.UpdateLayout(_GroupNameWidth, _GroupMargin, _CategoryNameWidth, _ButtonHeight, _ButtonMargin);
            }
        }

        //

        public double GroupNameWidth
        {
            get => _GroupNameWidth;

            set
            {
                _GroupNameWidth = value;

                _UpdateLayout();
            }
        }

        public Thickness GroupMargin
        {
            get => _GroupMargin;

            set
            {
                _GroupMargin = value;

                _UpdateLayout();
            }
        }

        public double CategoryNameWidth
        {
            get => _CategoryNameWidth;

            set
            {
                _CategoryNameWidth = value;

                _UpdateLayout();
            }
        }

        public double ButtonHeight
        {
            get => _ButtonHeight;

            set
            {
                _ButtonHeight = value;

                _UpdateLayout();
            }
        }

        public Thickness ButtonMargin
        {
            get => _ButtonMargin;

            set
            {
                _ButtonMargin = value;

                _UpdateLayout();
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
        public int GetGroupCount()
        {
            return _Groups.Count;
        }

        // 获取指定组的按钮数目。
        public int GetButtonCount(int groupIndex)
        {
            return _Groups[groupIndex].Buttons.Count;
        }

        // 获取指定组的指定按钮关联的类群。
        public Taxon GetTaxon(int groupIndex, int buttonIndex)
        {
            return _Groups[groupIndex].Buttons[buttonIndex].Taxon;
        }

        public void Clear()
        {
            stackPanel_Groups.Children.Clear();

            _Groups.Clear();
        }

        private void _AddGroupsAndButtons()
        {
            foreach (var group in _Groups)
            {
                group.UpdateControls();

                stackPanel_Groups.Children.Add(group.GroupPanel);
            }
        }

        public void UpdateContent(IEnumerable<TaxonNameItem> items)
        {
            stackPanel_Groups.Children.Clear();

            _Groups.Clear();

            if (items != null && items.Any())
            {
                _GroupNameWidth = 0;

                _Groups.Add(new _Group());

                foreach (var item in items)
                {
                    _Groups[0].Buttons.Add(new TaxonNameButton()
                    {
                        Taxon = item.Taxon,
                        Sign = item.Sign,
                        Checked = item.Checked,
                        ContextMenu = item.ContextMenu,
                        ThemeColor = item.ThemeColor,
                        IsDarkTheme = _IsDarkTheme
                    }); ;
                }

                _AddGroupsAndButtons();
                _UpdateFont();
                _UpdateLayout();
            }
        }

        public void UpdateContent(string groupName, ColorX groupColor, IEnumerable<TaxonNameItem> items)
        {
            stackPanel_Groups.Children.Clear();

            _Groups.Clear();

            if (items != null && items.Any())
            {
                _Groups.Add(new _Group() { Name = groupName, ThemeColor = groupColor });

                foreach (var item in items)
                {
                    _Groups[0].Buttons.Add(new TaxonNameButton()
                    {
                        Taxon = item.Taxon,
                        Sign = item.Sign,
                        Checked = item.Checked,
                        ContextMenu = item.ContextMenu,
                        ThemeColor = item.ThemeColor,
                        IsDarkTheme = _IsDarkTheme
                    });
                }

                _AddGroupsAndButtons();
                _UpdateFont();
                _UpdateLayout();
            }
        }

        public void UpdateContent(IEnumerable<(string groupName, ColorX groupColor, IEnumerable<TaxonNameItem> items)> groups)
        {
            stackPanel_Groups.Children.Clear();

            _Groups.Clear();

            if (groups != null && groups.Any())
            {
                int groupIndex = 0;

                foreach (var group in groups)
                {
                    if (group.items != null && group.items.Any())
                    {
                        _Groups.Add(new _Group() { Name = group.groupName, ThemeColor = group.groupColor });

                        foreach (var item in group.items)
                        {
                            _Groups[groupIndex].Buttons.Add(new TaxonNameButton()
                            {
                                Taxon = item.Taxon,
                                Sign = item.Sign,
                                Checked = item.Checked,
                                ContextMenu = item.ContextMenu,
                                ThemeColor = item.ThemeColor,
                                IsDarkTheme = _IsDarkTheme
                            });
                        }

                        groupIndex++;
                    }
                }

                _AddGroupsAndButtons();
                _UpdateFont();
                _UpdateLayout();
            }
        }

        //

        public EventHandler<TaxonNameButton> MouseLeftButtonClick;

        public EventHandler<TaxonNameButton> MouseRightButtonClick;
    }
}