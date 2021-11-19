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
    public partial class TaxonTitle : UserControl
    {
        private class _GeoChronSymbol
        {
            private Border _Container = null;
            private TextBlock _SymbolText = null;

            private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateColor()
            {
                _Container.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 12.5 : 87.5));
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
                    Margin = new Thickness(0, 0, 2, 0),
                    SnapsToDevicePixels = true
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

        private Dictionary<GeoChron, _GeoChronSymbol> _GeoChronSymbolsTable = new Dictionary<GeoChron, _GeoChronSymbol>();

        private Border border_PreCambrianMainly_FullWidth = null;
        private Border border_PreCambrianMainly = null;
        private Border border_PhanerozoicMainly_FullWidth = null;
        private Border border_PhanerozoicMainly = null;

        private void _InitGeoChronGraph()
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

            border_Underline.Height = bandHeight_FullWidth;
            border_Underline.BorderThickness = bandBorderThickness_FullWidth;
            border_Underline.Margin = bandMargin_FullWidth;

            border_PreCambrianMainly_FullWidth = new Border()
            {
                Height = bandHeight_FullWidth,
                BorderThickness = bandBorderThickness_FullWidth,
                Margin = bandMargin_FullWidth,
                VerticalAlignment = VerticalAlignment.Top,
                SnapsToDevicePixels = true
            };
            border_PreCambrianMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly_FullWidth);

            border_PreCambrianMainly = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                Margin = bandMargin,
                VerticalAlignment = VerticalAlignment.Top,
                SnapsToDevicePixels = true
            };
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly);

            border_PhanerozoicMainly_FullWidth = new Border()
            {
                Height = bandHeight_FullWidth,
                BorderThickness = bandBorderThickness_FullWidth,
                Margin = bandMargin_FullWidth,
                VerticalAlignment = VerticalAlignment.Top,
                SnapsToDevicePixels = true
            };
            border_PhanerozoicMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            grid_PhanerozoicMainly.Children.Add(border_PhanerozoicMainly_FullWidth);

            border_PhanerozoicMainly = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                Margin = bandMargin,
                VerticalAlignment = VerticalAlignment.Top,
                SnapsToDevicePixels = true
            };
            grid_PhanerozoicMainly.Children.Add(border_PhanerozoicMainly);
        }

        //

        private Taxon _Taxon = null; // 类群。

        private void _UpdateTaxon()
        {
            if (_Taxon is not null)
            {
                Rank rank = _Taxon.Rank;

                if (_Taxon.IsAnonymous)
                {
                    textBlock_RankName.Text = string.Empty;
                }
                else
                {
                    if (_Taxon.IsParaphyly)
                    {
                        textBlock_RankName.Text = rank.IsUnranked() || rank.IsClade() ? "并系群" : rank.GetChineseName() + "\n并系群";
                    }
                    else if (_Taxon.IsPolyphyly)
                    {
                        textBlock_RankName.Text = rank.IsUnranked() || rank.IsClade() ? "复系群" : rank.GetChineseName() + "\n复系群";
                    }
                    else
                    {
                        if (rank.IsUnranked())
                        {
                            textBlock_RankName.Text = string.Empty;
                        }
                        else
                        {
                            textBlock_RankName.Text = rank.GetChineseName();
                        }
                    }
                }

                textBlock_TaxonName.Text = _Taxon.GetShortName();

                //

                border_RankName.Visibility = !string.IsNullOrEmpty(textBlock_RankName.Text) ? Visibility.Visible : Visibility.Collapsed;

                grid_Undet.Visibility = _Taxon.IsNamed && _Taxon.IsUndet ? Visibility.Visible : Visibility.Collapsed;
                grid_Ex.Visibility = _Taxon.IsNamed && _Taxon.IsExtinct ? Visibility.Visible : Visibility.Collapsed;
                grid_Paraphyly.Visibility = _Taxon.IsNamed && _Taxon.IsParaphyly ? Visibility.Visible : Visibility.Collapsed;
                grid_Polyphyly.Visibility = _Taxon.IsNamed && _Taxon.IsPolyphyly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _UpdateGeoChronText()
        {
            GeoChron birth = _Taxon.Birth;
            GeoChron extinction = _Taxon.IsExtinct ? _Taxon.Extinction : GeoChron.Present;

            if (_Taxon.IsAnonymous || (birth.IsEmpty && (extinction.IsEmpty || extinction.IsPresent)))
            {
                grid_Text.Visibility = Visibility.Collapsed;
            }
            else
            {
                grid_Text.Visibility = Visibility.Visible;

                if (birth.IsEmpty)
                {
                    label_BirthPrefix.Content = string.Empty;
                    label_Birth.Content = "?";
                }
                else
                {
                    GeoChron birthTimespan = birth;

                    if (birthTimespan.IsTimepoint)
                    {
                        if (birthTimespan.Superior is null)
                        {
                            label_Birth.Content = birthTimespan.GetChineseName();
                        }
                        else
                        {
                            label_Birth.Content = $"{birthTimespan.GetChineseName()} ({birthTimespan.Superior.GetChineseName()})";

                            birthTimespan = birthTimespan.Superior;
                        }
                    }
                    else
                    {
                        label_Birth.Content = birthTimespan.GetChineseName();
                    }

                    if (birthTimespan.Superior is null)
                    {
                        label_BirthPrefix.Content = string.Empty;
                    }
                    else
                    {
                        GeoChron geoChron = birthTimespan.Superior;

                        string str = geoChron.GetChineseName();

                        while (geoChron.Superior is not null)
                        {
                            geoChron = geoChron.Superior;

                            str = $"{geoChron.GetChineseName()}·{str}";
                        }

                        label_BirthPrefix.Content = $"({str})";
                    }
                }

                if (extinction.IsEmpty)
                {
                    label_ExtinctionPrefix.Content = string.Empty;
                    label_Extinction.Content = "?";
                }
                else if (extinction.IsPresent)
                {
                    label_ExtinctionPrefix.Content = string.Empty;
                    label_Extinction.Content = "至今";
                }
                else
                {
                    GeoChron extinctionTimespan = extinction;

                    if (extinctionTimespan.IsTimepoint)
                    {
                        if (extinctionTimespan.Superior is null)
                        {
                            label_Extinction.Content = extinctionTimespan.GetChineseName();
                        }
                        else
                        {
                            label_Extinction.Content = $"{extinctionTimespan.GetChineseName()} ({extinctionTimespan.Superior.GetChineseName()})";

                            extinctionTimespan = extinctionTimespan.Superior;
                        }
                    }
                    else
                    {
                        label_Extinction.Content = extinctionTimespan.GetChineseName();
                    }

                    if (extinctionTimespan.Superior is null)
                    {
                        label_ExtinctionPrefix.Content = string.Empty;
                    }
                    else
                    {
                        GeoChron geoChron = extinctionTimespan.Superior;

                        string str = geoChron.GetChineseName();

                        while (geoChron.Superior is not null)
                        {
                            geoChron = geoChron.Superior;

                            str = $"{geoChron.GetChineseName()}·{str}";
                        }

                        label_ExtinctionPrefix.Content = $"({str})";
                    }
                }

                label_BirthPrefix.Visibility = !string.IsNullOrWhiteSpace(label_BirthPrefix.Content as string) ? Visibility.Visible : Visibility.Collapsed;
                label_ExtinctionPrefix.Visibility = !string.IsNullOrWhiteSpace(label_ExtinctionPrefix.Content as string) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _UpdateGeoChronGraph()
        {
            GeoChron birth = _Taxon.Birth;
            GeoChron extinction = _Taxon.IsExtinct ? _Taxon.Extinction : GeoChron.Present;

            if (_Taxon.IsAnonymous || ((birth.IsEmpty || birth.IsPresent) || extinction.IsEmpty || !(birth <= extinction)))
            {
                border_Underline.Visibility = Visibility.Visible;

                grid_PreCambrianMainly.Visibility = Visibility.Collapsed;
                grid_PhanerozoicMainly.Visibility = Visibility.Collapsed;
            }
            else
            {
                border_Underline.Visibility = Visibility.Collapsed;

                int columnIndex = _GetColumnIndex(birth);
                int columnSpan = Math.Max(1, _GetColumnIndex(extinction) - _GetColumnIndex(birth) + 1);

                if (birth < GeoChron.GetGeoChron(Eon.Phanerozoic))
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
            }
        }

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
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
            textBlock_TaxonName.Foreground = textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
            border_RankName.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));

            textBlock_Undet.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());
            border_Ex_Part1.BorderBrush = border_Ex_Part2.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Paraphyly_Part1.Stroke = path_Paraphyly_Part2.Stroke = path_Paraphyly_Part3.Stroke = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Polyphyly.Stroke = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50).ToWpfColor());

            border_Underline.Background = border_PreCambrianMainly_FullWidth.Background = border_PhanerozoicMainly_FullWidth.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 15 : 85));
            border_Underline.BorderBrush = border_PreCambrianMainly_FullWidth.BorderBrush = border_PhanerozoicMainly_FullWidth.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));
            border_PreCambrianMainly.BorderBrush = border_PhanerozoicMainly.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));

            label_Birth.Foreground = label_BirthPrefix.Foreground = label_Extinction.Foreground = label_ExtinctionPrefix.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
            border_GeoChron.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));
        }

        //

        public TaxonTitle()
        {
            InitializeComponent();

            //

            _InitGeoChronGraph();

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateColor();
            };
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;
                _ThemeColor = _Taxon.GetThemeColor();

                _UpdateTaxon();
                _UpdateGeoChronText();
                _UpdateGeoChronGraph();
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

        public void SyncTaxonUpdation()
        {
            _ThemeColor = _Taxon.GetThemeColor();

            _UpdateTaxon();
            _UpdateGeoChronText();
            _UpdateGeoChronGraph();
            _UpdateColor();
        }
    }
}