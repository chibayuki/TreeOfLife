/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1134.1000.M11.210518-2200

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
using TreeOfLife.Geology;
using TreeOfLife.Geology.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;
using TreeOfLife.Views;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// GeoChronSpan.xaml 的交互逻辑
    /// </summary>
    public partial class GeoChronSpan : UserControl
    {
        private GeoChron _Birth = null;
        private GeoChron _Extinction = null;
        private bool _IsExtinct = false;
        private TaxonomicCategory _Category = TaxonomicCategory.Unranked;

        //

        private static Dictionary<GeoChron, int> _GeoChronToColumnIndexTable = null;

        private static void _InitColumnIndexTable()
        {
            if (_GeoChronToColumnIndexTable is null)
            {
                _GeoChronToColumnIndexTable = new Dictionary<GeoChron, int>();

                int columnIndex = 0;

                GeoChron[] eons = new GeoChron[]
                {
                    GeoChron.GetGeoChron(Eon.Hadean),
                    GeoChron.GetGeoChron(Eon.Archean),
                    GeoChron.GetGeoChron(Eon.Proterozoic),
                    GeoChron.GetGeoChron(Eon.Phanerozoic)
                };

                for (int eonIndex = 0; eonIndex < eons.Length; eonIndex++)
                {
                    if (eonIndex > 0)
                    {
                        columnIndex++;
                    }

                    GeoChron eon = eons[eonIndex];

                    _GeoChronToColumnIndexTable.Add(eon, columnIndex);

                    if (eon.HasTimespanSubordinates)
                    {
                        for (int eraIndex = 0; eraIndex < eon.Subordinates.Count; eraIndex++)
                        {
                            if (eraIndex > 0)
                            {
                                columnIndex++;
                            }

                            GeoChron era = eon.Subordinates[eraIndex];

                            _GeoChronToColumnIndexTable.Add(era, columnIndex);

                            if (era.HasTimespanSubordinates)
                            {
                                for (int periodIndex = 0; periodIndex < era.Subordinates.Count; periodIndex++)
                                {
                                    if (periodIndex > 0)
                                    {
                                        columnIndex++;
                                    }

                                    GeoChron period = era.Subordinates[periodIndex];

                                    _GeoChronToColumnIndexTable.Add(period, columnIndex);

                                    if (period.HasTimespanSubordinates)
                                    {
                                        for (int epochIndex = 0; epochIndex < period.Subordinates.Count; epochIndex++)
                                        {
                                            if (epochIndex > 0)
                                            {
                                                columnIndex++;
                                            }

                                            GeoChron epoch = period.Subordinates[epochIndex];

                                            _GeoChronToColumnIndexTable.Add(epoch, columnIndex);

                                            if (epoch.HasTimespanSubordinates)
                                            {
                                                for (int ageIndex = 0; ageIndex < epoch.Subordinates.Count; ageIndex++)
                                                {
                                                    if (ageIndex > 0)
                                                    {
                                                        columnIndex++;
                                                    }

                                                    GeoChron age = epoch.Subordinates[ageIndex];

                                                    _GeoChronToColumnIndexTable.Add(age, columnIndex);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static int _GetColumnIndex(GeoChron geoChron)
        {
            _InitColumnIndexTable();

            return _GeoChronToColumnIndexTable[geoChron.IsTimespan ? geoChron : geoChron.Superior];
        }

        private static Dictionary<GeoChron, string> _GeoChronToSymbolTable = new Dictionary<GeoChron, string>()
        {
            { GeoChron.GetGeoChron(Period.Cambrian), "Ꞓ" },
            { GeoChron.GetGeoChron(Period.Ordovician), "O" },
            { GeoChron.GetGeoChron(Period.Silurian), "S" },
            { GeoChron.GetGeoChron(Period.Devonian), "D" },
            { GeoChron.GetGeoChron(Period.Carboniferous), "C" },
            { GeoChron.GetGeoChron(Period.Permian), "P" },

            { GeoChron.GetGeoChron(Period.Triassic), "T" },
            { GeoChron.GetGeoChron(Period.Jurassic), "J" },
            { GeoChron.GetGeoChron(Period.Cretaceous), "K" },

            { GeoChron.GetGeoChron(Period.Paleogene), "Pg" },
            { GeoChron.GetGeoChron(Period.Neogene), "N" },
            { GeoChron.GetGeoChron(Period.Quaternary), "Q" }
        };

        private Label label_PreCambrianAndPhanerozoic = null;
        private Label label_PhanerozoicOnly = null;

        private void _InitGraph()
        {
            GeoChron[] eons = new GeoChron[]
            {
                GeoChron.GetGeoChron(Eon.Hadean),
                GeoChron.GetGeoChron(Eon.Archean),
                GeoChron.GetGeoChron(Eon.Proterozoic),
                GeoChron.GetGeoChron(Eon.Phanerozoic)
            };

            int phanerozoicColumnIndex = _GetColumnIndex(GeoChron.GetGeoChron(Eon.Phanerozoic));
            int presentColumnIndex = _GetColumnIndex(GeoChron.Present);

            for (int i = 0; i < 5; i++)
            {
                grid_PreCambrianAndPhanerozoic.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(i == 0 ? 3 : 5) });
                grid_PhanerozoicOnly.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(i == 0 ? 3 : 5) });
            }

            for (int eonIndex = 0; eonIndex < eons.Length; eonIndex++)
            {
                GeoChron eon = eons[eonIndex];

                Label label_Eon = new Label()
                {
                    Content = eon.GetChineseName(),
                    Foreground = Common.GetSolidColorBrush(eon.GetThemeColor().AtLightness_LAB(60).ToWpfColor()),
                    Background = Common.GetSolidColorBrush(eon.GetThemeColor().AtLightness_HSL(90).ToWpfColor()),
                    FontSize = 11,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    VerticalContentAlignment = VerticalAlignment.Bottom,
                    Padding = new Thickness(0),
                    Margin = new Thickness(0, 0, 1, 0)
                };

                label_Eon.SetValue(Grid.ColumnProperty, _GetColumnIndex(eon));
                label_Eon.SetValue(Grid.ColumnSpanProperty, (eonIndex < eons.Length - 1 ? _GetColumnIndex(eons[eonIndex + 1]) - _GetColumnIndex(eon) : presentColumnIndex - _GetColumnIndex(eon) + 1));
                label_Eon.SetValue(Grid.RowProperty, 1);
                label_Eon.SetValue(Grid.RowSpanProperty, 4);

                grid_PreCambrianAndPhanerozoic.Children.Add(label_Eon);

                if (eonIndex == eons.Length - 1)
                {
                    Label label_Period = new Label()
                    {
                        Content = "PreꞒ",
                        Foreground = Common.GetSolidColorBrush(eons[^2].GetThemeColor().AtLightness_LAB(60).ToWpfColor()),
                        Background = Common.GetSolidColorBrush(eons[^2].GetThemeColor().AtLightness_HSL(90).ToWpfColor()),
                        FontSize = 11,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Bottom,
                        Padding = new Thickness(0),
                        Margin = new Thickness(0, 0, 1, 0)
                    };

                    label_Period.SetValue(Grid.ColumnProperty, 0);
                    label_Period.SetValue(Grid.ColumnSpanProperty, phanerozoicColumnIndex);
                    label_Period.SetValue(Grid.RowProperty, 1);
                    label_Period.SetValue(Grid.RowSpanProperty, 4);

                    grid_PhanerozoicOnly.Children.Add(label_Period);
                }

                if (eon.HasTimespanSubordinates)
                {
                    for (int eraIndex = 0; eraIndex < eon.Subordinates.Count; eraIndex++)
                    {
                        GeoChron era = eon.Subordinates[eraIndex];

                        if (era.HasTimespanSubordinates)
                        {
                            for (int periodIndex = 0; periodIndex < era.Subordinates.Count; periodIndex++)
                            {
                                GeoChron period = era.Subordinates[periodIndex];

                                int periodColumnSpan = 0;

                                if (period.HasTimespanSubordinates)
                                {
                                    for (int epochIndex = 0; epochIndex < period.Subordinates.Count; epochIndex++)
                                    {
                                        GeoChron epoch = period.Subordinates[epochIndex];

                                        if (epoch.HasTimespanSubordinates)
                                        {
                                            periodColumnSpan += epoch.Subordinates.Count;

                                            for (int ageIndex = 0; ageIndex < epoch.Subordinates.Count; ageIndex++)
                                            {
                                                GeoChron age = epoch.Subordinates[ageIndex];

                                                grid_PreCambrianAndPhanerozoic.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                                grid_PhanerozoicOnly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                            }
                                        }
                                        else
                                        {
                                            periodColumnSpan++;

                                            grid_PreCambrianAndPhanerozoic.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
                                            grid_PhanerozoicOnly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
                                        }
                                    }
                                }
                                else
                                {
                                    periodColumnSpan = 1;

                                    grid_PreCambrianAndPhanerozoic.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(16, GridUnitType.Star) });
                                    grid_PhanerozoicOnly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) });
                                }

                                if (eonIndex == eons.Length - 1)
                                {
                                    Label label_Period = new Label()
                                    {
                                        Content = _GeoChronToSymbolTable[period],
                                        Foreground = Common.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(60).ToWpfColor()),
                                        Background = Common.GetSolidColorBrush(period.GetThemeColor().AtLightness_HSL(90).ToWpfColor()),
                                        FontSize = 11,
                                        HorizontalContentAlignment = HorizontalAlignment.Center,
                                        VerticalContentAlignment = VerticalAlignment.Bottom,
                                        Padding = new Thickness(0),
                                        Margin = new Thickness(0, 0, 1, 0)
                                    };

                                    label_Period.SetValue(Grid.ColumnProperty, _GetColumnIndex(period));
                                    label_Period.SetValue(Grid.ColumnSpanProperty, periodColumnSpan);
                                    label_Period.SetValue(Grid.RowProperty, 1);
                                    label_Period.SetValue(Grid.RowSpanProperty, 4);

                                    grid_PhanerozoicOnly.Children.Add(label_Period);
                                }
                            }
                        }
                        else
                        {
                            grid_PreCambrianAndPhanerozoic.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Star) });
                            grid_PhanerozoicOnly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
                        }
                    }
                }
                else
                {
                    grid_PreCambrianAndPhanerozoic.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(64, GridUnitType.Star) });
                    grid_PhanerozoicOnly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
            }

            label_PreCambrianAndPhanerozoic = new Label() { Margin = new Thickness(0, 0, 1, 2) };
            label_PreCambrianAndPhanerozoic.SetValue(Grid.RowProperty, 0);
            label_PreCambrianAndPhanerozoic.SetValue(Grid.RowSpanProperty, 2);
            grid_PreCambrianAndPhanerozoic.Children.Add(label_PreCambrianAndPhanerozoic);

            label_PhanerozoicOnly = new Label();
            label_PhanerozoicOnly.SetValue(Grid.RowProperty, 0);
            label_PhanerozoicOnly.SetValue(Grid.RowSpanProperty, 2);
            grid_PhanerozoicOnly.Children.Add(label_PhanerozoicOnly);
        }

        //

        private void _UpdateContent()
        {
            if (_Birth is null || _Birth.IsEmpty)
            {
                label_BirthPrefix.Content = string.Empty;
                label_Birth.Content = "?";
            }
            else
            {
                GeoChron birth = _Birth;

                if (birth.IsTimepoint)
                {
                    if (birth.Superior is null)
                    {
                        label_Birth.Content = birth.GetChineseName();
                    }
                    else
                    {
                        label_Birth.Content = $"{birth.GetChineseName()} ({birth.Superior.GetChineseName()})";

                        birth = birth.Superior;
                    }
                }
                else
                {
                    label_Birth.Content = birth.GetChineseName();
                }

                if (birth.Superior is null)
                {
                    label_BirthPrefix.Content = string.Empty;
                }
                else
                {
                    GeoChron geoChron = birth.Superior;

                    string str = geoChron.GetChineseName();

                    while (geoChron.Superior is not null)
                    {
                        geoChron = geoChron.Superior;

                        str = $"{geoChron.GetChineseName()}·{str}";
                    }

                    label_BirthPrefix.Content = $"({str})";
                }
            }

            if (_IsExtinct)
            {
                if (_Extinction.IsEmpty)
                {
                    label_ExtinctionPrefix.Content = string.Empty;
                    label_Extinction.Content = "?";
                }
                else
                {
                    GeoChron extinction = _Extinction;

                    if (extinction.IsTimepoint)
                    {
                        if (extinction.Superior is null)
                        {
                            label_Extinction.Content = extinction.GetChineseName();
                        }
                        else
                        {
                            label_Extinction.Content = $"{extinction.GetChineseName()} ({extinction.Superior.GetChineseName()})";

                            extinction = extinction.Superior;
                        }
                    }
                    else
                    {
                        label_Extinction.Content = extinction.GetChineseName();
                    }

                    if (extinction.Superior is null)
                    {
                        label_ExtinctionPrefix.Content = string.Empty;
                    }
                    else
                    {
                        GeoChron geoChron = extinction.Superior;

                        string str = geoChron.GetChineseName();

                        while (geoChron.Superior is not null)
                        {
                            geoChron = geoChron.Superior;

                            str = $"{geoChron.GetChineseName()}·{str}";
                        }

                        label_ExtinctionPrefix.Content = $"({str})";
                    }
                }
            }
            else
            {
                label_ExtinctionPrefix.Content = string.Empty;
                label_Extinction.Content = "至今";
            }
        }

        private void _UpdateGraph()
        {
            if ((_Birth is not null && !_Birth.IsEmpty && !_Birth.IsPresent) && ((_IsExtinct && (_Extinction is not null && !_Extinction.IsEmpty && !_Extinction.IsPresent)) || !_IsExtinct))
            {
                if (_Birth < GeoChron.GetGeoChron(Eon.Phanerozoic))
                {
                    label_PreCambrianAndPhanerozoic.SetValue(Grid.ColumnProperty, _GetColumnIndex(_Birth));
                    label_PreCambrianAndPhanerozoic.SetValue(Grid.ColumnSpanProperty, (_IsExtinct ? _GetColumnIndex(_Extinction) : _GetColumnIndex(GeoChron.Present)) - _GetColumnIndex(_Birth) + 1);
                    label_PreCambrianAndPhanerozoic.Background = Common.GetSolidColorBrush(_Category.GetThemeColor().AtLightness_LAB(70).ToWpfColor());

                    grid_PreCambrianAndPhanerozoic.Visibility = Visibility.Visible;
                    grid_PhanerozoicOnly.Visibility = Visibility.Collapsed;
                }
                else
                {
                    label_PhanerozoicOnly.SetValue(Grid.ColumnProperty, _GetColumnIndex(_Birth));
                    label_PhanerozoicOnly.SetValue(Grid.ColumnSpanProperty, (_IsExtinct ? _GetColumnIndex(_Extinction) : _GetColumnIndex(GeoChron.Present)) - _GetColumnIndex(_Birth) + 1);
                    label_PhanerozoicOnly.Background = Common.GetSolidColorBrush(_Category.GetThemeColor().AtLightness_LAB(70).ToWpfColor());

                    grid_PreCambrianAndPhanerozoic.Visibility = Visibility.Collapsed;
                    grid_PhanerozoicOnly.Visibility = Visibility.Visible;
                }

                grid_Graph.Visibility = Visibility.Visible;
            }
            else
            {
                grid_Graph.Visibility = Visibility.Collapsed;
            }
        }

        private void _Update()
        {
            _UpdateContent();
            _UpdateGraph();
        }

        //

        public GeoChronSpan()
        {
            InitializeComponent();

            //

            _InitGraph();
        }

        //

        public GeoChron Birth
        {
            get => _Birth;

            set
            {
                _Birth = value;

                _Update();
            }
        }

        public GeoChron Extinction
        {
            get => _Extinction;

            set
            {
                _Extinction = value;

                _Update();
            }
        }

        public bool IsExtinct
        {
            get => _IsExtinct;

            set
            {
                _IsExtinct = value;

                _Update();
            }
        }

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                _Category = value;

                _Update();
            }
        }
    }
}