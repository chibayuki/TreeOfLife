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

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.UI.Controls
{
    /// <summary>
    /// RankSelector.xaml 的交互逻辑
    /// </summary>
    public partial class RankSelector : UserControl
    {
        private Dictionary<Rank, RankButton> _BasicRankButtons = new Dictionary<Rank, RankButton>();
        private Dictionary<Rank, RankButton> _CategoriesButtons = new Dictionary<Rank, RankButton>();
        private Dictionary<Rank, Panel> _CategoriesPanels = new Dictionary<Rank, Panel>();

        //

        private void radioButton_PrimaryRank_Checked(object sender, RoutedEventArgs e) => Rank = Rank.Domain;
        private void radioButton_SecondaryRank_Checked(object sender, RoutedEventArgs e) => Rank = Rank.Tribe;
        private void radioButton_Clade_Checked(object sender, RoutedEventArgs e) => Rank = Rank.Clade;
        private void radioButton_Unranked_Checked(object sender, RoutedEventArgs e) => Rank = Rank.Unranked;

        private void _AddRadioButtonsEvents()
        {
            radioButton_PrimaryRank.Checked += radioButton_PrimaryRank_Checked;
            radioButton_SecondaryRank.Checked += radioButton_SecondaryRank_Checked;
            radioButton_Clade.Checked += radioButton_Clade_Checked;
            radioButton_Unranked.Checked += radioButton_Unranked_Checked;
        }

        private void _RemoveRadioButtonsEvents()
        {
            radioButton_PrimaryRank.Checked -= radioButton_PrimaryRank_Checked;
            radioButton_SecondaryRank.Checked -= radioButton_SecondaryRank_Checked;
            radioButton_Clade.Checked -= radioButton_Clade_Checked;
            radioButton_Unranked.Checked -= radioButton_Unranked_Checked;
        }

        //

        private void _InitBasicRankControls(Panel basicRankPanel, Rank basicRank, Panel categoriesPanel, IEnumerable<Rank> categories)
        {
            RankButton button_Basic = new RankButton()
            {
                Rank = basicRank,
                Margin = new Thickness(0, 6, 6, 0)
            };

            _BasicRankButtons.Add(basicRank, button_Basic);

            basicRankPanel.Children.Add(button_Basic);

            //

            WrapPanel wrapPanel = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Visibility = Visibility.Collapsed
            };

            _CategoriesPanels.Add(basicRank, wrapPanel);

            foreach (var rank in categories)
            {
                RankButton button = new RankButton()
                {
                    Rank = rank,
                    Margin = new Thickness(0, 6, 6, 0)
                };

                _CategoriesButtons.Add(rank, button);

                wrapPanel.Children.Add(button);
            }

            categoriesPanel.Children.Add(wrapPanel);
        }

        private void _InitRankControls()
        {
            foreach (var (basicRank, categories) in new[] {
                (Rank.Domain, new Rank[] {
                    Rank.Superdomain,
                    Rank.Domain }),
                (Rank.Kingdom, new Rank[] {
                    Rank.Superkingdom,
                    Rank.Kingdom,
                    Rank.Subkingdom,
                    Rank.Infrakingdom }),
                (Rank.Phylum, new Rank[] {
                    Rank.Superphylum,
                    Rank.Phylum,
                    Rank.Subphylum,
                    Rank.Infraphylum,
                    Rank.Parvphylum }),
                (Rank.Class, new Rank[] {
                    Rank.Megaclass,
                    Rank.Superclass,
                    Rank.Grandclass,
                    Rank.Hyperclass,
                    Rank.Class,
                    Rank.Subclass,
                    Rank.Infraclass,
                    Rank.Parvclass }),
                (Rank.Order, new Rank[] {
                    Rank.Gigaorder,
                    Rank.Megaorder,
                    Rank.Superorder,
                    Rank.Grandorder,
                    Rank.Hyperorder,
                    Rank.Order,
                    Rank.Nanorder,
                    Rank.Hypoorder,
                    Rank.Minorder,
                    Rank.Suborder,
                    Rank.Infraorder,
                    Rank.Parvorder }),
                (Rank.Family, new Rank[] {
                    Rank.Gigafamily,
                    Rank.Megafamily,
                    Rank.Superfamily,
                    Rank.Grandfamily,
                    Rank.Hyperfamily,
                    Rank.Epifamily,
                    Rank.Family,
                    Rank.Subfamily,
                    Rank.Infrafamily }),
                (Rank.Genus, new Rank[] {
                    Rank.Genus,
                    Rank.Subgenus,
                    Rank.Infragenus }),
                (Rank.Species, new Rank[] {
                    Rank.Superspecies,
                    Rank.Species,
                    Rank.Subspecies,
                    Rank.Variety,
                    Rank.Subvariety })
            })
            {
                _InitBasicRankControls(stackPanel_BasicPrimaryCategories, basicRank, grid_PrimaryCategories, categories);
            }

            foreach (var (basicRank, categories) in new[] {
                (Rank.Tribe, new Rank[] {
                    Rank.Supertribe,
                    Rank.Tribe,
                    Rank.Subtribe,
                    Rank.Infratribe }),
                (Rank.Cohort, new Rank[] {
                    Rank.Megacohort,
                    Rank.Supercohort,
                    Rank.Cohort,
                    Rank.Subcohort,
                    Rank.Infracohort }),
                (Rank.Section, new Rank[] {
                    Rank.Supersection,
                    Rank.Section,
                    Rank.Subsection,
                    Rank.Infrasection }),
                (Rank.Division, new Rank[] {
                    Rank.Superdivision,
                    Rank.Division,
                    Rank.Subdivision,
                    Rank.Infradivision }),
                (Rank.Series, new Rank[] {
                    Rank.Series,
                    Rank.Subseries }),
                (Rank.Form, new Rank[] {
                    Rank.Form,
                    Rank.Subform }),
                (Rank.Strain, new Rank[] {
                    Rank.Strain })
            })
            {
                _InitBasicRankControls(stackPanel_BasicSecondaryCategories, basicRank, grid_SecondaryCategories, categories);
            }

            //

            RankButton button = null;

            grid_Categories.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is RankButton source)
                {
                    button = source;
                }
            }));

            grid_Categories.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is RankButton source && source == button)
                {
                    Rank = source.Rank;
                    button = null;
                }
            }));
        }

        //

        private Rank _Rank = Rank.Unranked; // 当前选择的分类阶元。

        private void _UpdateRank(Rank oldRank, Rank newRank)
        {
            if (oldRank != newRank)
            {
                if (oldRank.IsPrimaryOrSecondaryRank() && newRank.IsPrimaryOrSecondaryRank())
                {
                    RankButton button;
                    Panel panel;

                    //

                    if (oldRank.IsPrimaryRank())
                    {
                        stackPanel_PrimaryRank.Visibility = Visibility.Collapsed;
                    }
                    else if (oldRank.IsSecondaryRank())
                    {
                        stackPanel_SecondaryRank.Visibility = Visibility.Collapsed;
                    }

                    if (newRank.IsPrimaryRank())
                    {
                        radioButton_PrimaryRank.IsChecked = true;
                        stackPanel_PrimaryRank.Visibility = Visibility.Visible;
                    }
                    else if (newRank.IsSecondaryRank())
                    {
                        radioButton_SecondaryRank.IsChecked = true;
                        stackPanel_SecondaryRank.Visibility = Visibility.Visible;
                    }

                    //

                    if (_BasicRankButtons.TryGetValue(oldRank.BasicRank(), out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (_BasicRankButtons.TryGetValue(newRank.BasicRank(), out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    if (_CategoriesButtons.TryGetValue(oldRank, out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (_CategoriesButtons.TryGetValue(newRank, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    if (_CategoriesPanels.TryGetValue(oldRank.BasicRank(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Collapsed;
                    }

                    if (_CategoriesPanels.TryGetValue(newRank.BasicRank(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Visible;
                    }
                }
                else if (oldRank.IsPrimaryOrSecondaryRank())
                {
                    if (oldRank.IsPrimaryRank())
                    {
                        stackPanel_PrimaryRank.Visibility = Visibility.Collapsed;
                    }
                    else if (oldRank.IsSecondaryRank())
                    {
                        stackPanel_SecondaryRank.Visibility = Visibility.Collapsed;
                    }

                    foreach (var item in _BasicRankButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }
                    }

                    foreach (var item in _CategoriesButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }
                    }

                    foreach (var item in _CategoriesPanels)
                    {
                        item.Value.Visibility = Visibility.Collapsed;
                    }

                    //

                    if (newRank.IsClade())
                    {
                        radioButton_Clade.IsChecked = true;
                    }
                    else if (newRank.IsUnranked())
                    {
                        radioButton_Unranked.IsChecked = true;
                    }
                }
                else if (newRank.IsPrimaryOrSecondaryRank())
                {
                    RankButton button;
                    Panel panel;

                    if (newRank.IsPrimaryRank())
                    {
                        radioButton_PrimaryRank.IsChecked = true;
                        stackPanel_PrimaryRank.Visibility = Visibility.Visible;
                    }
                    else if (newRank.IsSecondaryRank())
                    {
                        radioButton_SecondaryRank.IsChecked = true;
                        stackPanel_SecondaryRank.Visibility = Visibility.Visible;
                    }

                    if (_BasicRankButtons.TryGetValue(newRank.BasicRank(), out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    if (_CategoriesButtons.TryGetValue(newRank, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    if (_CategoriesPanels.TryGetValue(newRank.BasicRank(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (newRank.IsClade())
                    {
                        radioButton_Clade.IsChecked = true;
                    }
                    else if (newRank.IsUnranked())
                    {
                        radioButton_Unranked.IsChecked = true;
                    }
                }
            }
        }

        //

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            foreach (var item in _BasicRankButtons)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }

            foreach (var item in _CategoriesButtons)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }
        }

        //

        public RankSelector()
        {
            InitializeComponent();

            //

            _AddRadioButtonsEvents();

            _InitRankControls();
        }

        //

        public Rank Rank
        {
            get => _Rank;

            set
            {
                if (_Rank != value)
                {
                    _RemoveRadioButtonsEvents();
                    _UpdateRank(_Rank, value);
                    _AddRadioButtonsEvents();

                    _Rank = value;

                    RankChanged?.Invoke(this, _Rank);
                }
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

        public EventHandler<Rank> RankChanged;
    }
}