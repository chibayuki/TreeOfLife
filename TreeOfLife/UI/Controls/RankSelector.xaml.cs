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
using TreeOfLife.Core.Taxonomy.Extensions;

namespace TreeOfLife.UI.Controls
{
    public partial class RankSelector : UserControl
    {
        private Dictionary<Rank, RankButton> _BasicRankButtons = new Dictionary<Rank, RankButton>();
        private Dictionary<Rank, RankButton> _RanksButtons = new Dictionary<Rank, RankButton>();
        private Dictionary<Rank, Panel> _RanksPanels = new Dictionary<Rank, Panel>();

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

        private void _InitBasicRankControls(Panel basicRankPanel, Rank basicRank, Panel RanksPanel, IEnumerable<Rank> Ranks)
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

            _RanksPanels.Add(basicRank, wrapPanel);

            foreach (var rank in Ranks)
            {
                RankButton button = new RankButton()
                {
                    Rank = rank,
                    Margin = new Thickness(0, 6, 6, 0)
                };

                _RanksButtons.Add(rank, button);

                wrapPanel.Children.Add(button);
            }

            RanksPanel.Children.Add(wrapPanel);
        }

        private void _InitRankControls()
        {
            foreach (var (basicRank, Ranks) in new[] {
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
                    Rank.Class,
                    Rank.Subclass,
                    Rank.Infraclass,
                    Rank.Parvclass }),
                (Rank.Order, new Rank[] {
                    Rank.Megaorder,
                    Rank.Superorder,
                    Rank.Grandorder,
                    Rank.Mirorder,
                    Rank.Order,
                    Rank.Suborder,
                    Rank.Infraorder,
                    Rank.Parvorder }),
                (Rank.Family, new Rank[] {
                    Rank.Superfamily,
                    Rank.Family,
                    Rank.Subfamily }),
                (Rank.Genus, new Rank[] {
                    Rank.Genus,
                    Rank.Subgenus }),
                (Rank.Species, new Rank[] {
                    Rank.Species,
                    Rank.Subspecies,
                    Rank.Variety,
                    Rank.Subvariety })
            })
            {
                _InitBasicRankControls(stackPanel_BasicPrimaryRanks, basicRank, grid_PrimaryRanks, Ranks);
            }

            foreach (var (basicRank, Ranks) in new[] {
                (Rank.Tribe, new Rank[] {
                    Rank.Tribe,
                    Rank.Subtribe }),
                (Rank.Cohort, new Rank[] {
                    Rank.Megacohort,
                    Rank.Supercohort,
                    Rank.Cohort,
                    Rank.Subcohort,
                    Rank.Infracohort }),
                (Rank.Section, new Rank[] {
                    Rank.Section,
                    Rank.Subsection }),
                (Rank.Division, new Rank[] {
                    Rank.Division,
                    Rank.Subdivision }),
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
                _InitBasicRankControls(stackPanel_BasicSecondaryRanks, basicRank, grid_SecondaryRanks, Ranks);
            }

            //

            RankButton button = null;

            grid_Ranks.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is RankButton source)
                {
                    button = source;
                }
            }));

            grid_Ranks.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is RankButton source && source == button)
                {
                    Rank = source.Rank;
                }

                button = null;
            }));
        }

        //

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
                        button.IsIndirectlyChecked = false;
                    }

                    if (_BasicRankButtons.TryGetValue(newRank.BasicRank(), out button) && button is not null)
                    {
                        button.IsIndirectlyChecked = true;
                    }

                    //

                    if (_RanksButtons.TryGetValue(oldRank, out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (_RanksButtons.TryGetValue(newRank, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    if (_RanksPanels.TryGetValue(oldRank.BasicRank(), out panel) && panel is not null)
                    {
                        panel.Visibility = Visibility.Collapsed;
                    }

                    if (_RanksPanels.TryGetValue(newRank.BasicRank(), out panel) && panel is not null)
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
                        if (item.Value.IsIndirectlyChecked)
                        {
                            item.Value.IsIndirectlyChecked = false;
                        }
                    }

                    foreach (var item in _RanksButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }
                    }

                    foreach (var item in _RanksPanels)
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
                        button.IsIndirectlyChecked = true;
                    }

                    if (_RanksButtons.TryGetValue(newRank, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    if (_RanksPanels.TryGetValue(newRank.BasicRank(), out panel) && panel is not null)
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

            foreach (var item in _RanksButtons)
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

        public static readonly DependencyProperty RankProperty = DependencyProperty.Register("Rank", typeof(Rank), typeof(RankSelector), new FrameworkPropertyMetadata(Rank.Unranked, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRankChanged));

        private static void OnRankChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RankSelector rankSelector)
            {
                Rank oldValue = e.OldValue as Rank? ?? Rank.Unranked;
                Rank newValue = e.NewValue as Rank? ?? Rank.Unranked;

                rankSelector._RemoveRadioButtonsEvents();
                rankSelector._UpdateRank(oldValue, newValue);
                rankSelector._AddRadioButtonsEvents();

                rankSelector.RankChanged?.Invoke(rankSelector, newValue);
            }
        }

        public Rank Rank
        {
            get => (Rank)GetValue(RankProperty);
            set => SetValue(RankProperty, value);
        }

        public EventHandler<Rank> RankChanged;
    }
}