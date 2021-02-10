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

            private Label _NameLabel = new Label();
            private List<TaxonNameButton> _Buttons = new List<TaxonNameButton>();

            public _Group()
            {
                _NameLabel.Padding = new Thickness(0);
                _NameLabel.Margin = new Thickness(0);
                _NameLabel.Content = _Name;
                _NameLabel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _NameLabel.VerticalAlignment = VerticalAlignment.Stretch;
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

                    _UpdateColor();
                }
            }

            public Label NameLabel => _NameLabel;

            public List<TaxonNameButton> Buttons => _Buttons;

            private void _UpdateColor()
            {
                _NameLabel.Foreground = (_IsDarkTheme ? Brushes.Black : Brushes.White);
                _NameLabel.Background = new SolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }
        }

        //

        private List<_Group> _Groups = new List<_Group>();

        private double _GroupNameWidth = 30; // 组名称宽度。
        private Thickness _GroupMargin = new Thickness(2); // 组外边距。
        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。
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
                group.NameLabel.FontFamily = this.FontFamily;
                group.NameLabel.FontSize = this.FontSize;
                group.NameLabel.FontStyle = FontStyles.Normal;
                group.NameLabel.FontWeight = FontWeights.Normal;
                group.NameLabel.FontStretch = this.FontStretch;

                foreach (var button in group.Buttons)
                {
                    Taxon taxon = button.Taxon;

                    TaxonomicCategory category = taxon.Category;
                    bool basicPrimary = category.IsBasicPrimaryCategory();
                    bool bellowGenus = (category.IsPrimaryCategory() || category.IsSecondaryCategory()) && taxon.GetInheritedBasicPrimaryCategory() <= TaxonomicCategory.Genus;

                    button.FontFamily = this.FontFamily;
                    button.FontSize = this.FontSize;
                    button.FontStyle = (bellowGenus ? FontStyles.Italic : FontStyles.Normal);
                    button.FontWeight = (basicPrimary ? FontWeights.Bold : FontWeights.Normal);
                    button.FontStretch = this.FontStretch;
                }
            }
        }

        private void _UpdateTheme()
        {
            foreach (var group in _Groups)
            {
                group.NameLabel.Foreground = (_IsDarkTheme ? Brushes.Black : Brushes.White);
                group.NameLabel.Background = new SolidColorBrush(group.ThemeColor.AtLightness_LAB(50).ToWpfColor());

                foreach (var button in group.Buttons)
                {
                    button.IsDarkTheme = _IsDarkTheme;
                }
            }
        }

        private void _UpdateLayout()
        {
            foreach (var group in _Groups)
            {
                Thickness groupMargin = _GroupMargin;

                groupMargin.Right = 0;

                if (_GroupNameWidth <= 0)
                {
                    groupMargin.Left = 0;
                }
                else
                {
                    group.NameLabel.Margin = new Thickness(0, 0, _ButtonMargin.Left, 0);
                }

                group.NameLabel.Width = _GroupNameWidth;

                ((Panel)group.NameLabel.Parent).Margin = groupMargin;

                for (int i = 0; i < group.Buttons.Count; i++)
                {
                    Thickness buttonMargin = _ButtonMargin;

                    buttonMargin.Left = 0;
                    buttonMargin.Right = 0;

                    if (group.Buttons.Count == 1)
                    {
                        buttonMargin.Top = 0;
                        buttonMargin.Bottom = 0;
                    }
                    else if (i == 0)
                    {
                        buttonMargin.Top = 0;
                    }
                    else if (i == group.Buttons.Count - 1)
                    {
                        buttonMargin.Bottom = 0;
                    }

                    TaxonNameButton button = group.Buttons[i];

                    button.Margin = buttonMargin;
                    button.CategoryNameWidth = _CategoryNameWidth;
                }
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
                DockPanel dockPanel = new DockPanel();
                StackPanel stackPanelV = new StackPanel() { Orientation = Orientation.Vertical };

                dockPanel.Children.Add(group.NameLabel);
                dockPanel.Children.Add(stackPanelV);

                foreach (var button in group.Buttons)
                {
                    stackPanelV.Children.Add(button);
                }

                stackPanel_Groups.Children.Add(dockPanel);
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
                    });
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