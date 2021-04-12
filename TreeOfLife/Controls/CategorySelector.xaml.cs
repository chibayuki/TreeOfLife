/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// CategorySelector.xaml 的交互逻辑
    /// </summary>
    public partial class CategorySelector : UserControl
    {
        private class _Group
        {
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private WrapPanel _GroupPanel = new WrapPanel();
            private List<CategoryNameButton> _Buttons = new List<CategoryNameButton>();

            private void _UpdateHeight(object sender, EventArgs e)
            {
                if (_Buttons.Count > 0)
                {
                    CategoryNameButton lastButton = _Buttons[^1];

                    _GroupPanel.Height = lastButton.TranslatePoint(new Point(lastButton.Height, lastButton.Height), _GroupPanel).Y + lastButton.Margin.Bottom;
                }
                else
                {
                    _GroupPanel.Height = 0;
                }
            }

            public _Group(bool isDarkTheme)
            {
                _IsDarkTheme = isDarkTheme;

                _GroupPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
                _GroupPanel.VerticalAlignment = VerticalAlignment.Top;
            }

            public bool IsDarkTheme
            {
                get => _IsDarkTheme;

                set
                {
                    _IsDarkTheme = value;

                    UpdateColor();
                }
            }

            public WrapPanel GroupPanel => _GroupPanel;

            public List<CategoryNameButton> Buttons => _Buttons;

            public void UpdateFont(FontFamily fontFamily, double fontSize, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch)
            {
                foreach (var button in _Buttons)
                {
                    button.FontFamily = fontFamily;
                    button.FontSize = fontSize;
                    button.FontStretch = fontStretch;
                    button.FontStyle = fontStyle;
                    button.FontWeight = fontWeight;
                }
            }

            public void UpdateColor()
            {
                foreach (var button in _Buttons)
                {
                    button.IsDarkTheme = _IsDarkTheme;
                }
            }

            public void UpdateControls()
            {
                _GroupPanel.SizeChanged -= _UpdateHeight;

                _GroupPanel.Children.Clear();

                foreach (var button in _Buttons)
                {
                    _GroupPanel.Children.Add(button);
                }

                _GroupPanel.SizeChanged += _UpdateHeight;
            }

            public void UpdateLayout(Thickness groupMargin, Thickness buttonMargin)
            {
                if (_Buttons.Count > 0)
                {
                    _GroupPanel.Margin = groupMargin;

                    foreach (var button in _Buttons)
                    {
                        button.Margin = buttonMargin;
                    }
                }
                else
                {
                    _GroupPanel.Margin = new Thickness(0);
                }
            }
        }

        //

        private CategoryNameButton _PrimaryButton = null;
        private CategoryNameButton _SecondaryButton = null;
        private CategoryNameButton _CladeButton = null;
        private CategoryNameButton _UnrankedButton = null;
        private _Group _Level1Group = null;
        private Dictionary<CategoryNameButton, _Group> _Level2Groups = new Dictionary<CategoryNameButton, _Group>();
        private Dictionary<CategoryNameButton, _Group> _Level3Groups = new Dictionary<CategoryNameButton, _Group>();

        private CategoryNameButton _Level1Button = null;
        private CategoryNameButton _Level2Button = null;
        private CategoryNameButton _Level3Button = null;
        private _Group _Level2Group = null;
        private _Group _Level3Group = null;

        private Thickness _ButtonMargin = new Thickness(3); // 按钮外边距。
        private Thickness _GroupMargin = new Thickness(0, 6, 0, 6); // 组外边距。

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 当前选择的分类阶元。

        //

        public CategorySelector()
        {
            InitializeComponent();

            //

            _InitLevel1();
            _InitLevel2();
            _InitLevel3();

            this.SizeChanged += (s, e) => _UpdateLayout();

            this.Loaded += (s, e) =>
            {
                _UpdateFont();
                _UpdateCategory();
                _UpdateLayout();
            };

            stackPanel_Levels.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is CategoryNameButton source)
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                }
            }));
            stackPanel_Levels.AddHandler(UIElement.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is CategoryNameButton source)
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
        }

        private void _InitLevel1()
        {
            _Level1Group = new _Group(_IsDarkTheme);

            _Level1Group.GroupPanel.Visibility = Visibility.Visible;

            _PrimaryButton = new CategoryNameButton()
            {
                CategoryName = "主要级别",
                ThemeColor = ColorX.FromRGB(128, 128, 128)
            };

            _PrimaryButton.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(TaxonomicCategory.Domain, e);

            _Level1Group.Buttons.Add(_PrimaryButton);

            _SecondaryButton = new CategoryNameButton()
            {
                CategoryName = "次要级别",
                ThemeColor = ColorX.FromRGB(128, 128, 128)
            };

            _SecondaryButton.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(TaxonomicCategory.Tribe, e);

            _Level1Group.Buttons.Add(_SecondaryButton);

            _CladeButton = new CategoryNameButton()
            {
                Category = TaxonomicCategory.Clade,
                ThemeColor = ColorX.FromRGB(128, 128, 128)
            };

            _CladeButton.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(TaxonomicCategory.Clade, e);

            _Level1Group.Buttons.Add(_CladeButton);

            _UnrankedButton = new CategoryNameButton()
            {
                Category = TaxonomicCategory.Unranked,
                ThemeColor = ColorX.FromRGB(128, 128, 128)
            };

            _UnrankedButton.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(TaxonomicCategory.Unranked, e);

            _Level1Group.Buttons.Add(_UnrankedButton);

            _Level1Group.UpdateControls();

            grid_Level1.Children.Add(_Level1Group.GroupPanel);
        }

        private void _InitLevel2()
        {
            _Group primaryGroup = new _Group(_IsDarkTheme);

            primaryGroup.GroupPanel.Visibility = Visibility.Collapsed;

            TaxonomicCategory[] primaryCategories =
            {
                TaxonomicCategory.Domain,
                TaxonomicCategory.Kingdom,
                TaxonomicCategory.Phylum,
                TaxonomicCategory.Class,
                TaxonomicCategory.Order,
                TaxonomicCategory.Family,
                TaxonomicCategory.Genus,
                TaxonomicCategory.Species
            };

            foreach (var category in primaryCategories)
            {
                CategoryNameButton button2 = new CategoryNameButton()
                {
                    Category = category,
                    ThemeColor = category.GetThemeColor()
                };

                primaryGroup.Buttons.Add(button2);

                button2.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(button2.Category.Value, e);
            }

            _Level2Groups.Add(_PrimaryButton, primaryGroup);

            primaryGroup.UpdateControls();

            grid_Level2.Children.Add(primaryGroup.GroupPanel);

            //

            _Group secondaryGroup = new _Group(_IsDarkTheme);

            secondaryGroup.GroupPanel.Visibility = Visibility.Collapsed;

            TaxonomicCategory[] secondaryCategories =
            {
                TaxonomicCategory.Tribe,
                TaxonomicCategory.Cohort,
                TaxonomicCategory.Section,
                TaxonomicCategory.Division,
                TaxonomicCategory.Series,
                TaxonomicCategory.Form,
                TaxonomicCategory.Strain
            };

            foreach (var category in secondaryCategories)
            {
                CategoryNameButton button2 = new CategoryNameButton()
                {
                    Category = category,
                    ThemeColor = category.GetThemeColor()
                };

                secondaryGroup.Buttons.Add(button2);

                button2.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(button2.Category.Value, e);
            }

            _Level2Groups.Add(_SecondaryButton, secondaryGroup);

            secondaryGroup.UpdateControls();

            grid_Level2.Children.Add(secondaryGroup.GroupPanel);
        }

        private void _InitLevel3()
        {
            foreach (var group2 in _Level2Groups.Values)
            {
                foreach (var button2 in group2.Buttons)
                {
                    _Group group3 = new _Group(_IsDarkTheme);

                    group3.GroupPanel.Visibility = Visibility.Collapsed;

                    TaxonomicCategory[] categories = null;

                    switch (button2.Category.Value)
                    {
                        case TaxonomicCategory.Domain:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superdomain,
                                TaxonomicCategory.Domain
                            };
                            break;

                        case TaxonomicCategory.Kingdom:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superkingdom,
                                TaxonomicCategory.Kingdom,
                                TaxonomicCategory.Subkingdom,
                                TaxonomicCategory.Infrakingdom
                            };
                            break;

                        case TaxonomicCategory.Phylum:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superphylum,
                                TaxonomicCategory.Phylum,
                                TaxonomicCategory.Subphylum,
                                TaxonomicCategory.Infraphylum,
                                TaxonomicCategory.Parvphylum
                            };
                            break;

                        case TaxonomicCategory.Class:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Megaclass,
                                TaxonomicCategory.Superclass,
                                TaxonomicCategory.Grandclass,
                                TaxonomicCategory.Hyperclass,
                                TaxonomicCategory.Class,
                                TaxonomicCategory.Subclass,
                                TaxonomicCategory.Infraclass,
                                TaxonomicCategory.Parvclass
                            };
                            break;

                        case TaxonomicCategory.Order:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Gigaorder,
                                TaxonomicCategory.Megaorder,
                                TaxonomicCategory.Superorder,
                                TaxonomicCategory.Grandorder,
                                TaxonomicCategory.Hyperorder,
                                TaxonomicCategory.Order,
                                TaxonomicCategory.Nanorder,
                                TaxonomicCategory.Hypoorder,
                                TaxonomicCategory.Minorder,
                                TaxonomicCategory.Suborder,
                                TaxonomicCategory.Infraorder,
                                TaxonomicCategory.Parvorder
                            };
                            break;

                        case TaxonomicCategory.Family:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Gigafamily,
                                TaxonomicCategory.Megafamily,
                                TaxonomicCategory.Superfamily,
                                TaxonomicCategory.Grandfamily,
                                TaxonomicCategory.Hyperfamily,
                                TaxonomicCategory.Epifamily,
                                TaxonomicCategory.Family,
                                TaxonomicCategory.Subfamily,
                                TaxonomicCategory.Infrafamily
                            };
                            break;

                        case TaxonomicCategory.Genus:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Genus,
                                TaxonomicCategory.Subgenus,
                                TaxonomicCategory.Infragenus
                            };
                            break;

                        case TaxonomicCategory.Species:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superspecies,
                                TaxonomicCategory.Species,
                                TaxonomicCategory.Subspecies,
                                TaxonomicCategory.Variety,
                                TaxonomicCategory.Subvariety
                            };
                            break;

                        //

                        case TaxonomicCategory.Tribe:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Supertribe,
                                TaxonomicCategory.Tribe,
                                TaxonomicCategory.Subtribe,
                                TaxonomicCategory.Infratribe
                            };
                            break;

                        case TaxonomicCategory.Cohort:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Megacohort,
                                TaxonomicCategory.Supercohort,
                                TaxonomicCategory.Cohort,
                                TaxonomicCategory.Subcohort,
                                TaxonomicCategory.Infracohort
                            };
                            break;

                        case TaxonomicCategory.Section:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Supersection,
                                TaxonomicCategory.Section,
                                TaxonomicCategory.Subsection,
                                TaxonomicCategory.Infrasection
                            };
                            break;

                        case TaxonomicCategory.Division:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Superdivision,
                                TaxonomicCategory.Division,
                                TaxonomicCategory.Subdivision,
                                TaxonomicCategory.Infradivision
                            };
                            break;

                        case TaxonomicCategory.Series:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Series,
                                TaxonomicCategory.Subseries
                            };
                            break;

                        case TaxonomicCategory.Form:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Form,
                                TaxonomicCategory.Subform
                            };
                            break;

                        case TaxonomicCategory.Strain:
                            categories = new TaxonomicCategory[]
                            {
                                TaxonomicCategory.Strain
                            };
                            break;
                    }

                    foreach (var category in categories)
                    {
                        CategoryNameButton button3 = new CategoryNameButton()
                        {
                            Category = category,
                            ThemeColor = category.GetThemeColor()
                        };

                        group3.Buttons.Add(button3);

                        button3.MouseLeftButtonUp += (s, e) => _UpdateCategoryAndOnMouseLeftButtonUp(button3.Category.Value, e);
                    }

                    _Level3Groups.Add(button2, group3);

                    group3.UpdateControls();

                    grid_Level3.Children.Add(group3.GroupPanel);
                }
            }
        }

        //

        private void _UpdateFont()
        {
            _Level1Group.UpdateFont(this.FontFamily, this.FontSize, this.FontStyle, this.FontWeight, this.FontStretch);

            foreach (var group in _Level2Groups.Values)
            {
                group.UpdateFont(this.FontFamily, this.FontSize, this.FontStyle, this.FontWeight, this.FontStretch);
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.UpdateFont(this.FontFamily, this.FontSize, this.FontStyle, this.FontWeight, this.FontStretch);
            }
        }

        private void _UpdateColor()
        {
            _Level1Group.IsDarkTheme = _IsDarkTheme;
            _Level1Group.UpdateColor();

            foreach (var group in _Level2Groups.Values)
            {
                group.IsDarkTheme = _IsDarkTheme;
                group.UpdateColor();
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.IsDarkTheme = _IsDarkTheme;
                group.UpdateColor();
            }
        }

        private void _UpdateLayout()
        {
            _Level1Group.UpdateLayout(new Thickness(_GroupMargin.Left, 0, _GroupMargin.Right, _GroupMargin.Bottom), _ButtonMargin);

            foreach (var group in _Level2Groups.Values)
            {
                group.UpdateLayout(_GroupMargin, _ButtonMargin);
            }

            foreach (var group in _Level3Groups.Values)
            {
                group.UpdateLayout(new Thickness(_GroupMargin.Left, _GroupMargin.Top, _GroupMargin.Right, 0), _ButtonMargin);
            }
        }

        private void _UpdateCategory()
        {
            _Level2Button = null;
            _Level3Button = null;

            if (_Category == TaxonomicCategory.Clade || _Category == TaxonomicCategory.Unranked)
            {
                if (_Category == TaxonomicCategory.Clade)
                {
                    _CurrentLevel1Button = _CladeButton;
                }
                else
                {
                    _CurrentLevel1Button = _UnrankedButton;
                }

                _CurrentLevel2Group = null;
                _CurrentLevel3Group = null;
            }
            else
            {
                if (_Category.IsPrimaryCategory())
                {
                    _CurrentLevel1Button = _PrimaryButton;
                    _CurrentLevel2Group = _Level2Groups[_PrimaryButton];
                }
                else
                {
                    _CurrentLevel1Button = _SecondaryButton;
                    _CurrentLevel2Group = _Level2Groups[_SecondaryButton];
                }

                //

                foreach (var button in _CurrentLevel2Group.Buttons)
                {
                    if (_Category.BasicCategory() == button.Category)
                    {
                        button.Checked = true;

                        _Level2Button = button;
                    }
                    else
                    {
                        button.Checked = false;
                    }
                }

                //

                _CurrentLevel3Group = _Level3Groups[_Level2Button];

                foreach (var button in _CurrentLevel3Group.Buttons)
                {
                    if (_Category == button.Category)
                    {
                        button.Checked = true;

                        _Level3Button = button;
                    }
                    else
                    {
                        button.Checked = false;
                    }
                }
            }

            //

            _UpdateLayout();
        }

        private void _UpdateCategoryAndOnMouseLeftButtonUp(TaxonomicCategory category, MouseButtonEventArgs e)
        {
            if (_Category != category)
            {
                _Category = category;

                _UpdateCategory();

                base.OnMouseLeftButtonUp(e);
            }
        }

        //

        private CategoryNameButton _CurrentLevel1Button
        {
            get => _Level1Button;

            set
            {
                if (_Level1Button != null)
                {
                    _Level1Button.Checked = false;
                }

                _Level1Button = value;

                if (_Level1Button != null)
                {
                    _Level1Button.Checked = true;
                }
            }
        }

        private CategoryNameButton _CurrentLevel2Button => _Level2Button;

        private CategoryNameButton _CurrentLevel3Button => _Level3Button;

        private _Group _CurrentLevel2Group
        {
            get => _Level2Group;

            set
            {
                if (_Level2Group != null)
                {
                    _Level2Group.GroupPanel.Visibility = Visibility.Collapsed;
                }

                _Level2Group = value;

                if (_Level2Group != null)
                {
                    _Level2Group.GroupPanel.Visibility = Visibility.Visible;

                    grid_Level2.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_Level2.Visibility = Visibility.Collapsed;
                }
            }
        }

        private _Group _CurrentLevel3Group
        {
            get => _Level3Group;

            set
            {
                if (_Level3Group != null)
                {
                    _Level3Group.GroupPanel.Visibility = Visibility.Collapsed;
                }

                _Level3Group = value;

                if (_Level3Group != null)
                {
                    _Level3Group.GroupPanel.Visibility = Visibility.Visible;

                    grid_Level3.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_Level3.Visibility = Visibility.Collapsed;
                }
            }
        }

        //

        public Thickness GroupMargin
        {
            get => _GroupMargin;

            set
            {
                _GroupMargin = value;

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

                _UpdateColor();
            }
        }

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                _Category = value;

                _UpdateCategory();
            }
        }

        //

        public EventHandler<CategoryNameButton> MouseLeftButtonClick;

        public EventHandler<CategoryNameButton> MouseRightButtonClick;
    }
}