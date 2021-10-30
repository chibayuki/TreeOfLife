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

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Geology.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    public partial class GeoChronSpan : UserControl
    {
        private class _GeoChronSymbol
        {
            private Border _Container = null;
            private TextBlock _SymbolText = null;

            private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateColor()
            {
                _Container.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));
                _SymbolText.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
            }

            //

            public _GeoChronSymbol()
            {
                _SymbolText = new TextBlock()
                {
                    FontSize = 11,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 2, 0, 2)
                };

                _Container = new Border()
                {
                    Child = _SymbolText,
                    CornerRadius = new CornerRadius(2),
                    Margin = new Thickness(0, 0, 2, 0)
                };
            }

            //

            public string SymbolText
            {
                get => _SymbolText.Text;
                set => _SymbolText.Text = value;
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

            public bool IsDarkTheme
            {
                get => _IsDarkTheme;

                set
                {
                    _IsDarkTheme = value;

                    _UpdateColor();
                }
            }

            public FrameworkElement Container => _Container;
        }

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

        private static readonly Dictionary<GeoChron, string> _GeoChronToSymbolTable = new Dictionary<GeoChron, string>()
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

        //

        private GeoChron _Birth = null;
        private GeoChron _Extinction = null;
        private Rank _Rank = Rank.Unranked;

        private Dictionary<GeoChron, _GeoChronSymbol> _GeoChronSymbolsTable = new Dictionary<GeoChron, _GeoChronSymbol>();

        private Border border_PreCambrianMainly_FullWidth = null;
        private Border border_PreCambrianMainly = null;
        private Border border_PhanerozoicMainly_FullWidth = null;
        private Border border_PhanerozoicMainly = null;

        private void _InitGraph()
        {
            grid_PreCambrianMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_PreCambrianMainly.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4) });
            grid_PreCambrianMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            grid_PhanerozoicMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            grid_PhanerozoicMainly.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4) });
            grid_PhanerozoicMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            // 网格与地质年代标签：

            GeoChron[] eons = new GeoChron[]
            {
                GeoChron.GetGeoChron(Eon.Hadean),
                GeoChron.GetGeoChron(Eon.Archean),
                GeoChron.GetGeoChron(Eon.Proterozoic),
                GeoChron.GetGeoChron(Eon.Phanerozoic)
            };

            for (int eonIndex = 0; eonIndex < eons.Length; eonIndex++)
            {
                GeoChron eon = eons[eonIndex];

                // "宙"标签
                _GeoChronSymbol symbol_Eon = new _GeoChronSymbol()
                {
                    SymbolText = eon.GetChineseName(),
                    ThemeColor = eon.GetThemeColor(),
                    IsDarkTheme = _IsDarkTheme
                };

                _GeoChronSymbolsTable.Add(eon, symbol_Eon);

                symbol_Eon.Container.SetValue(Grid.ColumnProperty, _GetColumnIndex(eon));
                symbol_Eon.Container.SetValue(Grid.ColumnSpanProperty, eonIndex < eons.Length - 1 ? _GetColumnIndex(eons[eonIndex + 1]) - _GetColumnIndex(eon) : _GetColumnIndex(GeoChron.Present) - _GetColumnIndex(eon) + 1);
                symbol_Eon.Container.SetValue(Grid.RowProperty, 2);

                grid_PreCambrianMainly.Children.Add(symbol_Eon.Container);

                if (eonIndex == eons.Length - 1)
                {
                    // "前寒武纪"标签
                    _GeoChronSymbol symbol_Period = new _GeoChronSymbol()
                    {
                        SymbolText = "PreꞒ",
                        ThemeColor = GeoChron.Empty.GetThemeColor(),
                        IsDarkTheme = _IsDarkTheme
                    };

                    _GeoChronSymbolsTable.Add(GeoChron.Empty, symbol_Period);

                    symbol_Period.Container.SetValue(Grid.ColumnProperty, 0);
                    symbol_Period.Container.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.GetGeoChron(Eon.Phanerozoic)));
                    symbol_Period.Container.SetValue(Grid.RowProperty, 2);

                    grid_PhanerozoicMainly.Children.Add(symbol_Period.Container);
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
                                                grid_PreCambrianMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                                grid_PhanerozoicMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                            }
                                        }
                                        else
                                        {
                                            periodColumnSpan++;

                                            grid_PreCambrianMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                            grid_PhanerozoicMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                        }
                                    }
                                }
                                else
                                {
                                    periodColumnSpan = 1;

                                    grid_PreCambrianMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(32, GridUnitType.Star) });
                                    grid_PhanerozoicMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                                }

                                if (eonIndex == eons.Length - 1)
                                {
                                    // "纪"标签
                                    _GeoChronSymbol symbol_Period = new _GeoChronSymbol()
                                    {
                                        SymbolText = _GeoChronToSymbolTable[period],
                                        ThemeColor = period.GetThemeColor(),
                                        IsDarkTheme = _IsDarkTheme
                                    };

                                    _GeoChronSymbolsTable.Add(period, symbol_Period);

                                    symbol_Period.Container.SetValue(Grid.ColumnProperty, _GetColumnIndex(period));
                                    symbol_Period.Container.SetValue(Grid.ColumnSpanProperty, periodColumnSpan);
                                    symbol_Period.Container.SetValue(Grid.RowProperty, 2);

                                    grid_PhanerozoicMainly.Children.Add(symbol_Period.Container);
                                }
                            }
                        }
                        else
                        {
                            grid_PreCambrianMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(64, GridUnitType.Star) });
                            grid_PhanerozoicMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                        }
                    }
                }
                else
                {
                    grid_PreCambrianMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(128, GridUnitType.Star) });
                    grid_PhanerozoicMainly.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                }
            }

            // 地质年代跨度条带：

            const double bandHeight_FullWidth = 6;
            const double bandHeight = 6;
            Thickness bandBorderThickness_FullWidth = new Thickness(0, 1, 0, 1);
            Thickness bandBorderThickness = new Thickness(1, 3, 1, 0);
            Thickness bandMargin_FullWidth = new Thickness(0, 0, 2, 0);
            Thickness bandMargin = new Thickness(0, 4, 2, 0);

            border_PreCambrianMainly_FullWidth = new Border()
            {
                Height = bandHeight_FullWidth,
                BorderThickness = bandBorderThickness_FullWidth,
                Margin = bandMargin_FullWidth,
                VerticalAlignment = VerticalAlignment.Top
            };
            border_PreCambrianMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly_FullWidth);

            border_PreCambrianMainly = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                Margin = bandMargin,
                VerticalAlignment = VerticalAlignment.Top
            };
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly);

            border_PhanerozoicMainly_FullWidth = new Border()
            {
                Height = bandHeight_FullWidth,
                BorderThickness = bandBorderThickness_FullWidth,
                Margin = bandMargin_FullWidth,
                VerticalAlignment = VerticalAlignment.Top
            };
            border_PhanerozoicMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            grid_PhanerozoicMainly.Children.Add(border_PhanerozoicMainly_FullWidth);

            border_PhanerozoicMainly = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                Margin = bandMargin,
                VerticalAlignment = VerticalAlignment.Top
            };
            grid_PhanerozoicMainly.Children.Add(border_PhanerozoicMainly);
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

            if (_Extinction is null || _Extinction.IsEmpty)
            {
                label_ExtinctionPrefix.Content = string.Empty;
                label_Extinction.Content = "?";
            }
            else if (_Extinction.IsPresent)
            {
                label_ExtinctionPrefix.Content = string.Empty;
                label_Extinction.Content = "至今";
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

        private void _UpdateGraph()
        {
            if ((_Birth is not null && !_Birth.IsEmpty && !_Birth.IsPresent) && (_Extinction is not null && !_Extinction.IsEmpty))
            {
                int columnIndex = _GetColumnIndex(_Birth);
                int columnSpan = Math.Max(1, _GetColumnIndex(_Extinction) - _GetColumnIndex(_Birth) + 1);

                if (_Birth < GeoChron.GetGeoChron(Eon.Phanerozoic))
                {
                    border_PreCambrianMainly.SetValue(Grid.ColumnProperty, columnIndex);
                    border_PreCambrianMainly.SetValue(Grid.ColumnSpanProperty, columnSpan);

                    grid_PreCambrianMainly.Visibility = Visibility.Visible;
                    grid_PhanerozoicMainly.Visibility = Visibility.Collapsed;
                }
                else
                {
                    border_PhanerozoicMainly.SetValue(Grid.ColumnProperty, columnIndex);
                    border_PhanerozoicMainly.SetValue(Grid.ColumnSpanProperty, columnSpan);

                    grid_PreCambrianMainly.Visibility = Visibility.Collapsed;
                    grid_PhanerozoicMainly.Visibility = Visibility.Visible;
                }

                grid_Graph.Visibility = Visibility.Visible;
            }
            else
            {
                grid_Graph.Visibility = Visibility.Collapsed;
            }
        }

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            foreach (var item in _GeoChronSymbolsTable)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }
        }

        private void _UpdateColor()
        {
            ColorX themeColor = _Rank.GetThemeColor();

            border_PreCambrianMainly_FullWidth.Background = border_PhanerozoicMainly_FullWidth.Background = Theme.GetSolidColorBrush(themeColor.AtLightness_HSL(_IsDarkTheme ? 15 : 85));
            border_PreCambrianMainly_FullWidth.BorderBrush = border_PhanerozoicMainly_FullWidth.BorderBrush = Theme.GetSolidColorBrush(themeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));
            border_PreCambrianMainly.BorderBrush = border_PhanerozoicMainly.BorderBrush = Theme.GetSolidColorBrush(themeColor.AtLightness_HSL(50));

            label_Birth.Foreground = label_BirthPrefix.Foreground = label_Extinction.Foreground = label_ExtinctionPrefix.Foreground = Theme.GetSolidColorBrush(themeColor.AtLightness_LAB(50));

            border_Span.BorderBrush = Theme.GetSolidColorBrush(themeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));
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

                _UpdateContent();
                _UpdateGraph();
            }
        }

        public GeoChron Extinction
        {
            get => _Extinction;

            set
            {
                _Extinction = value;

                _UpdateContent();
                _UpdateGraph();
            }
        }

        public Rank Rank
        {
            get => _Rank;

            set
            {
                _Rank = value;

                _UpdateColor();
            }
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateTheme();
                _UpdateColor();
            }
        }

        //

        public void Update(GeoChron birth, GeoChron extinction, Rank rank)
        {
            _Birth = birth;
            _Extinction = extinction;
            _Rank = rank;

            _UpdateContent();
            _UpdateGraph();
            _UpdateColor();
        }
    }
}