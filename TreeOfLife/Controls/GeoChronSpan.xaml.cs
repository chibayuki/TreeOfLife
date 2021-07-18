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

        private Border border_PreCambrianMainly_FullWidth = null;
        private Border border_PreCambrianMainly = null;
        private Border border_PhanerozoicMainly_FullWidth = null;
        private Border border_PhanerozoicMainly = null;

        private void _InitGraph()
        {
            for (int i = 0; i < 2; i++)
            {
                grid_PreCambrianMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid_PhanerozoicMainly.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            // 网格与地质年代标签：

            const double fontSize = 11;
            const HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Center;
            const VerticalAlignment verticalContentAlignment = VerticalAlignment.Center;
            Thickness padding = new Thickness(0, 2, 0, 2);
            Thickness margin = new Thickness(0, 0, 2, 0);
            CornerRadius cornerRadius = new CornerRadius(2);
            Func<GeoChron, Brush> getForeground = (geoChron) => Common.GetSolidColorBrush(geoChron.GetThemeColor().AtLightness_LAB(60).ToWpfColor());
            Func<GeoChron, Brush> getBackground = (geoChron) => Common.GetSolidColorBrush(geoChron.GetThemeColor().AtLightness_HSL(90).ToWpfColor());

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
                TextBlock textBlock_Eon = new TextBlock()
                {
                    Text = eon.GetChineseName(),
                    Foreground = getForeground(eon),
                    FontSize = fontSize,
                    HorizontalAlignment = horizontalContentAlignment,
                    VerticalAlignment = verticalContentAlignment,
                    Margin = padding
                };

                Border border_Eon = new Border()
                {
                    Child = textBlock_Eon,
                    Background = getBackground(eon),
                    CornerRadius = cornerRadius,
                    Margin = margin
                };

                border_Eon.SetValue(Grid.ColumnProperty, _GetColumnIndex(eon));
                border_Eon.SetValue(Grid.ColumnSpanProperty, (eonIndex < eons.Length - 1 ? _GetColumnIndex(eons[eonIndex + 1]) - _GetColumnIndex(eon) : _GetColumnIndex(GeoChron.Present) - _GetColumnIndex(eon) + 1));

                grid_PreCambrianMainly.Children.Add(border_Eon);

                if (eonIndex == eons.Length - 1)
                {
                    // "前寒武纪"标签
                    TextBlock textBlock_Period = new TextBlock()
                    {
                        Text = "PreꞒ",
                        Foreground = getForeground(GeoChron.Empty),
                        FontSize = fontSize,
                        HorizontalAlignment = horizontalContentAlignment,
                        VerticalAlignment = verticalContentAlignment,
                        Margin = padding
                    };

                    Border border_Period = new Border()
                    {
                        Child = textBlock_Period,
                        Background = getBackground(GeoChron.Empty),
                        CornerRadius = cornerRadius,
                        Margin = margin
                    };

                    border_Period.SetValue(Grid.ColumnProperty, 0);
                    border_Period.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.GetGeoChron(Eon.Phanerozoic)));

                    grid_PhanerozoicMainly.Children.Add(border_Period);
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
                                    TextBlock textBlock_Period = new TextBlock()
                                    {
                                        Text = _GeoChronToSymbolTable[period],
                                        Foreground = getForeground(period),
                                        FontSize = fontSize,
                                        HorizontalAlignment = horizontalContentAlignment,
                                        VerticalAlignment = verticalContentAlignment,
                                        Margin = padding
                                    };

                                    Border border_Period = new Border()
                                    {
                                        Child = textBlock_Period,
                                        Background = getBackground(period),
                                        CornerRadius = cornerRadius,
                                        Margin = margin
                                    };

                                    border_Period.SetValue(Grid.ColumnProperty, _GetColumnIndex(period));
                                    border_Period.SetValue(Grid.ColumnSpanProperty, periodColumnSpan);

                                    grid_PhanerozoicMainly.Children.Add(border_Period);
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

            const double bandHeight = 6;
            CornerRadius bandCornerRadius = new CornerRadius(2);
            Thickness bandMargin = new Thickness(0, 4, 2, 0);
            Thickness bandBorderThickness = new Thickness(1);

            border_PreCambrianMainly_FullWidth = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                CornerRadius = bandCornerRadius,
                Margin = bandMargin
            };
            border_PreCambrianMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            border_PreCambrianMainly_FullWidth.SetValue(Grid.RowProperty, 1);
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly_FullWidth);

            border_PreCambrianMainly = new Border()
            {
                Height = bandHeight,
                CornerRadius = bandCornerRadius,
                Margin = bandMargin
            };
            border_PreCambrianMainly.SetValue(Grid.RowProperty, 1);
            grid_PreCambrianMainly.Children.Add(border_PreCambrianMainly);

            border_PhanerozoicMainly_FullWidth = new Border()
            {
                Height = bandHeight,
                BorderThickness = bandBorderThickness,
                CornerRadius = bandCornerRadius,
                Margin = bandMargin
            };
            border_PhanerozoicMainly_FullWidth.SetValue(Grid.ColumnSpanProperty, _GetColumnIndex(GeoChron.Present) + 1);
            border_PhanerozoicMainly_FullWidth.SetValue(Grid.RowProperty, 1);
            grid_PhanerozoicMainly.Children.Add(border_PhanerozoicMainly_FullWidth);

            border_PhanerozoicMainly = new Border()
            {
                Height = bandHeight,
                CornerRadius = bandCornerRadius,
                Margin = bandMargin
            };
            border_PhanerozoicMainly.SetValue(Grid.RowProperty, 1);
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

        private void _UpdateColor()
        {
            Brush brush_FullWidth = Common.GetSolidColorBrush(_Category.GetThemeColor().AtLightness_HSL(90).ToWpfColor());
            Brush brush = Common.GetSolidColorBrush(_Category.GetThemeColor().AtLightness_LAB(70).ToWpfColor());

            border_PreCambrianMainly_FullWidth.Background = brush_FullWidth;
            border_PreCambrianMainly_FullWidth.BorderBrush = brush;
            border_PreCambrianMainly.Background = brush;
            border_PhanerozoicMainly_FullWidth.Background = brush_FullWidth;
            border_PhanerozoicMainly_FullWidth.BorderBrush = brush;
            border_PhanerozoicMainly.Background = brush;
        }

        private void _Update()
        {
            _UpdateContent();
            _UpdateGraph();
            _UpdateColor();
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

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                _Category = value;

                _UpdateColor();
            }
        }

        public void Update(GeoChron birth, GeoChron extinction, TaxonomicCategory category)
        {
            _Birth = birth;
            _Extinction = extinction;
            _Category = category;

            _Update();
        }
    }
}