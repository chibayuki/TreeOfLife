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

using System.Text.RegularExpressions;

using TreeOfLife.Geology;
using TreeOfLife.Geology.Extensions;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// GeoChronSelector.xaml 的交互逻辑
    /// </summary>
    public partial class GeoChronSelector : UserControl
    {
        private static readonly GeoChron _DefaultTimespan = GeoChron.GetGeoChron(Eon.Phanerozoic);
        private static readonly GeoChron _DefaultMaBP = GeoChron.CreateGeoChron(1.0);
        private static readonly GeoChron _DefaultCEYear = GeoChron.CreateGeoChron(1950);

        //

        private Dictionary<GeoChron, GeoChronNameButton> _GeoChronButtons = new Dictionary<GeoChron, GeoChronNameButton>();
        private Dictionary<GeoChron, IEnumerable<UIElement>> _PeriodElements = new Dictionary<GeoChron, IEnumerable<UIElement>>();

        //

        private void radioButton_Timespan_Checked(object sender, RoutedEventArgs e) => GeoChron = _DefaultTimespan;
        private void radioButton_MaBP_Checked(object sender, RoutedEventArgs e) => GeoChron = _DefaultMaBP;
        private void radioButton_CEYear_Checked(object sender, RoutedEventArgs e) => GeoChron = _DefaultCEYear;
        private void radioButton_Empty_Checked(object sender, RoutedEventArgs e) => GeoChron = GeoChron.Empty;

        private void _AddRadioButtonsEvents()
        {
            radioButton_Timespan.Checked += radioButton_Timespan_Checked;
            radioButton_MaBP.Checked += radioButton_MaBP_Checked;
            radioButton_CEYear.Checked += radioButton_CEYear_Checked;
            radioButton_Empty.Checked += radioButton_Empty_Checked;
        }

        private void _RemoveRadioButtonsEvents()
        {
            radioButton_Timespan.Checked -= radioButton_Timespan_Checked;
            radioButton_MaBP.Checked -= radioButton_MaBP_Checked;
            radioButton_CEYear.Checked -= radioButton_CEYear_Checked;
            radioButton_Empty.Checked -= radioButton_Empty_Checked;
        }

        //

        private bool _BeforeChrist = false;

        private bool BeforeChrist
        {
            get => _BeforeChrist;

            set
            {
                _BeforeChrist = value;

                button_ADBC.Content = (_BeforeChrist ? "公元前 (BC)" : "公元 (AD)");
            }
        }

        //

        private void _InitTimespanControls()
        {
            Thickness geoChronNameButtonMargin = new Thickness(0, 0, 1, 1);

            // 布局所有"宙"、"代"、"纪"，前三个"宙"共用一行布局，显生宙另起一行布局。
            foreach (var eons in new GeoChron[][] {
                new GeoChron[] {
                    GeoChron.GetGeoChron(Eon.Hadean),
                    GeoChron.GetGeoChron(Eon.Archean),
                    GeoChron.GetGeoChron(Eon.Proterozoic) },
                new GeoChron[] {
                    GeoChron.GetGeoChron(Eon.Phanerozoic) }})
            {
                // 布局表示"宙"、"代"、"纪"的按钮的 Grid。
                Grid grid_Eons = new Grid();

                if (eons.Length == 1)
                {
                    grid_Eons.Margin = new Thickness(0, 4, 0, 0);
                }

                // Grid 的行数与列数。
                const int eonRowCount = 4; // 从上到下依次是"宙"、"代"、"纪"、分割线
                int eonColumnCount = 0;

                // 表示"宙"、"代"、"纪"的按钮在 Grid 中的列索引与列跨度。
                Dictionary<int, int> eonColumnIndex = new Dictionary<int, int>();
                Dictionary<int, int> eonColumnSpan = new Dictionary<int, int>();

                // 先计算 Grid 的列数，以及表示"纪"的按钮、表示无下级的"宙"、"代"的按钮在 Grid 中的列索引。
                foreach (var eon in eons)
                {
                    if (eon.HasTimespanSubordinates)
                    {
                        foreach (var era in eon.Subordinates)
                        {
                            if (era.HasTimespanSubordinates)
                            {
                                foreach (var period in era.Subordinates)
                                {
                                    eonColumnIndex.Add(period.GetHashCode(), eonColumnCount);

                                    eonColumnCount++;
                                }
                            }
                            else
                            {
                                eonColumnIndex.Add(era.GetHashCode(), eonColumnCount);

                                eonColumnCount++;
                            }
                        }
                    }
                    else
                    {
                        eonColumnIndex.Add(eon.GetHashCode(), eonColumnCount);

                        eonColumnCount++;
                    }
                }

                // 再计算表示有下级的"宙"、"代"的按钮在 Grid 中的列索引，以及表示"宙"、"代"、"纪"的按钮在 Grid 中的列跨度。
                foreach (var eon in eons)
                {
                    if (eon.HasTimespanSubordinates)
                    {
                        int span = 0;

                        foreach (var era in eon.Subordinates)
                        {
                            if (era.HasTimespanSubordinates)
                            {
                                foreach (var period in era.Subordinates)
                                {
                                    eonColumnSpan.Add(period.GetHashCode(), 1);

                                    span++;
                                }

                                eonColumnIndex.Add(era.GetHashCode(), eonColumnIndex[era.Subordinates[0].GetHashCode()]);
                                eonColumnSpan.Add(era.GetHashCode(), era.Subordinates.Count);
                            }
                            else
                            {
                                eonColumnSpan.Add(era.GetHashCode(), 1);

                                span++;
                            }
                        }

                        eonColumnIndex.Add(eon.GetHashCode(), eonColumnIndex[eon.Subordinates[0].GetHashCode()]);
                        eonColumnSpan.Add(eon.GetHashCode(), span);
                    }
                    else
                    {
                        eonColumnSpan.Add(eon.GetHashCode(), 1);
                    }
                }

                for (int i = 0; i < eonRowCount; i++)
                {
                    grid_Eons.RowDefinitions.Add(new RowDefinition());
                }

                for (int i = 0; i < eonColumnCount; i++)
                {
                    grid_Eons.ColumnDefinitions.Add(new ColumnDefinition());
                }

                foreach (var eon in eons)
                {
                    GeoChronNameButton button_Eon = new GeoChronNameButton()
                    {
                        GeoChron = eon,
                        ThemeColor = eon.GetThemeColor(),
                        Margin = geoChronNameButtonMargin
                    };

                    button_Eon.SetValue(Grid.ColumnProperty, eonColumnIndex[eon.GetHashCode()]);
                    button_Eon.SetValue(Grid.ColumnSpanProperty, eonColumnSpan[eon.GetHashCode()]);

                    _GeoChronButtons.Add(eon, button_Eon);

                    grid_Eons.Children.Add(button_Eon);

                    if (eon.HasTimespanSubordinates)
                    {
                        foreach (var era in eon.Subordinates)
                        {
                            GeoChronNameButton button_Era = new GeoChronNameButton()
                            {
                                GeoChron = era,
                                ThemeColor = era.GetThemeColor(),
                                Margin = geoChronNameButtonMargin
                            };

                            button_Era.SetValue(Grid.ColumnProperty, eonColumnIndex[era.GetHashCode()]);
                            button_Era.SetValue(Grid.ColumnSpanProperty, eonColumnSpan[era.GetHashCode()]);
                            button_Era.SetValue(Grid.RowProperty, 1);

                            _GeoChronButtons.Add(era, button_Era);

                            grid_Eons.Children.Add(button_Era);

                            if (era.HasTimespanSubordinates)
                            {
                                foreach (var period in era.Subordinates)
                                {
                                    int periodColumnIndex = eonColumnIndex[period.GetHashCode()];
                                    int periodColumnSpan = eonColumnSpan[period.GetHashCode()];

                                    GeoChronNameButton button_Period = new GeoChronNameButton()
                                    {
                                        GeoChron = period,
                                        ThemeColor = period.GetThemeColor(),
                                        Margin = geoChronNameButtonMargin,
                                        Vertical = true
                                    };

                                    button_Period.SetValue(Grid.ColumnProperty, periodColumnIndex);
                                    button_Period.SetValue(Grid.ColumnSpanProperty, periodColumnSpan);
                                    button_Period.SetValue(Grid.RowProperty, 2);

                                    _GeoChronButtons.Add(period, button_Period);

                                    grid_Eons.Children.Add(button_Period);

                                    // 布局所有"世"、"期"。
                                    if (period.HasTimespanSubordinates)
                                    {
                                        List<UIElement> elements = new List<UIElement>();

                                        // "纪"下方的分割线
                                        Grid grid_Separator = new Grid()
                                        {
                                            Visibility = Visibility.Collapsed
                                        };

                                        grid_Separator.SetValue(Grid.ColumnProperty, periodColumnIndex);
                                        grid_Separator.SetValue(Grid.RowProperty, 3);

                                        grid_Separator.ColumnDefinitions.Add(new ColumnDefinition());
                                        grid_Separator.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10) });
                                        grid_Separator.ColumnDefinitions.Add(new ColumnDefinition());

                                        grid_Separator.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2) });
                                        grid_Separator.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(8) });
                                        grid_Separator.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2) });

                                        Polyline polyline = new Polyline()
                                        {
                                            Points = new PointCollection(new Point[]
                                            {
                                                new Point(0, 4.5),
                                                new Point(4, 0.5),
                                                new Point(8, 4.5)
                                            }),
                                            Stroke = Theme.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(50).AtOpacity(50)),
                                            HorizontalAlignment = HorizontalAlignment.Center,
                                            VerticalAlignment = VerticalAlignment.Top
                                        };

                                        polyline.SetValue(Grid.ColumnProperty, 1);
                                        polyline.SetValue(Grid.RowProperty, 1);

                                        Border border_InnerL = new Border()
                                        {
                                            BorderThickness = new Thickness(1),
                                            Height = 1,
                                            BorderBrush = Theme.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(50).AtOpacity(50)),
                                            VerticalAlignment = VerticalAlignment.Center,
                                            SnapsToDevicePixels = true
                                        };

                                        border_InnerL.SetValue(Grid.ColumnProperty, 0);
                                        border_InnerL.SetValue(Grid.RowProperty, 1);

                                        Border border_InnerR = new Border()
                                        {
                                            BorderThickness = new Thickness(1),
                                            Height = 1,
                                            BorderBrush = Theme.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(50).AtOpacity(50)),
                                            VerticalAlignment = VerticalAlignment.Center,
                                            SnapsToDevicePixels = true
                                        };

                                        border_InnerR.SetValue(Grid.ColumnProperty, 2);
                                        border_InnerR.SetValue(Grid.RowProperty, 1);

                                        grid_Separator.Children.Add(polyline);
                                        grid_Separator.Children.Add(border_InnerL);
                                        grid_Separator.Children.Add(border_InnerR);

                                        elements.Add(grid_Separator);

                                        grid_Eons.Children.Add(grid_Separator);

                                        if (periodColumnIndex > 0)
                                        {
                                            Border border_OuterL = new Border()
                                            {
                                                BorderThickness = new Thickness(1),
                                                Height = 1,
                                                BorderBrush = Theme.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(50).AtOpacity(50)),
                                                VerticalAlignment = VerticalAlignment.Center,
                                                SnapsToDevicePixels = true,
                                                Visibility = Visibility.Collapsed
                                            };

                                            border_OuterL.SetValue(Grid.ColumnProperty, 0);
                                            border_OuterL.SetValue(Grid.ColumnSpanProperty, periodColumnIndex);
                                            border_OuterL.SetValue(Grid.RowProperty, 3);

                                            elements.Add(border_OuterL);

                                            grid_Eons.Children.Add(border_OuterL);
                                        }

                                        if (periodColumnIndex + periodColumnSpan < eonColumnCount)
                                        {
                                            Border border_OuterR = new Border()
                                            {
                                                BorderThickness = new Thickness(1),
                                                Height = 1,
                                                BorderBrush = Theme.GetSolidColorBrush(period.GetThemeColor().AtLightness_LAB(50).AtOpacity(50)),
                                                VerticalAlignment = VerticalAlignment.Center,
                                                SnapsToDevicePixels = true,
                                                Visibility = Visibility.Collapsed
                                            };

                                            border_OuterR.SetValue(Grid.ColumnProperty, periodColumnIndex + periodColumnSpan);
                                            border_OuterR.SetValue(Grid.ColumnSpanProperty, eonColumnCount - periodColumnIndex - periodColumnSpan);
                                            border_OuterR.SetValue(Grid.RowProperty, 3);

                                            elements.Add(border_OuterR);

                                            grid_Eons.Children.Add(border_OuterR);
                                        }

                                        // 布局表示"世"、"期"的按钮的 Grid。
                                        Grid grid_Epochs = new Grid() { Visibility = Visibility.Collapsed };

                                        // Grid 的行数与列数。
                                        const int epochRowCount = 2;
                                        int epochColumnCount = 0;

                                        // 表示"世"、"期"的按钮在 Grid 中的列索引与列跨度。
                                        Dictionary<int, int> epochColumnIndex = new Dictionary<int, int>();
                                        Dictionary<int, int> epochColumnSpan = new Dictionary<int, int>();

                                        // 先计算 Grid 的列数，以及表示"期"的按钮、表示无下级的"世"的按钮在 Grid 中的列索引。
                                        foreach (var epoch in period.Subordinates)
                                        {
                                            if (epoch.HasTimespanSubordinates)
                                            {
                                                foreach (var age in epoch.Subordinates)
                                                {
                                                    epochColumnIndex.Add(age.GetHashCode(), epochColumnCount);

                                                    epochColumnCount++;
                                                }
                                            }
                                            else
                                            {
                                                epochColumnIndex.Add(epoch.GetHashCode(), epochColumnCount);

                                                epochColumnCount++;
                                            }
                                        }

                                        // 再计算表示有下级的"世"的按钮在 Grid 中的列索引，以及表示"世"、"期"的按钮在 Grid 中的列跨度。
                                        foreach (var epoch in period.Subordinates)
                                        {
                                            if (epoch.HasTimespanSubordinates)
                                            {
                                                foreach (var age in epoch.Subordinates)
                                                {
                                                    epochColumnSpan.Add(age.GetHashCode(), 1);
                                                }

                                                epochColumnIndex.Add(epoch.GetHashCode(), epochColumnIndex[epoch.Subordinates[0].GetHashCode()]);
                                                epochColumnSpan.Add(epoch.GetHashCode(), epoch.Subordinates.Count);
                                            }
                                            else
                                            {
                                                epochColumnSpan.Add(epoch.GetHashCode(), 1);
                                            }
                                        }

                                        for (int i = 0; i < epochRowCount; i++)
                                        {
                                            grid_Epochs.RowDefinitions.Add(new RowDefinition());
                                        }

                                        for (int i = 0; i < epochColumnCount; i++)
                                        {
                                            grid_Epochs.ColumnDefinitions.Add(new ColumnDefinition());
                                        }

                                        elements.Add(grid_Epochs);

                                        _PeriodElements.Add(period, elements);

                                        foreach (var epoch in period.Subordinates)
                                        {
                                            GeoChronNameButton button_Epoch = new GeoChronNameButton()
                                            {
                                                GeoChron = epoch,
                                                ThemeColor = epoch.GetThemeColor(),
                                                Margin = geoChronNameButtonMargin
                                            };

                                            button_Epoch.SetValue(Grid.ColumnProperty, epochColumnIndex[epoch.GetHashCode()]);
                                            button_Epoch.SetValue(Grid.ColumnSpanProperty, epochColumnSpan[epoch.GetHashCode()]);

                                            _GeoChronButtons.Add(epoch, button_Epoch);

                                            grid_Epochs.Children.Add(button_Epoch);

                                            if (epoch.HasTimespanSubordinates)
                                            {
                                                foreach (var age in epoch.Subordinates)
                                                {
                                                    GeoChronNameButton button_Age = new GeoChronNameButton()
                                                    {
                                                        GeoChron = age,
                                                        ThemeColor = age.GetThemeColor(),
                                                        Margin = geoChronNameButtonMargin,
                                                        Vertical = true
                                                    };

                                                    button_Age.SetValue(Grid.ColumnProperty, epochColumnIndex[age.GetHashCode()]);
                                                    button_Age.SetValue(Grid.ColumnSpanProperty, epochColumnSpan[age.GetHashCode()]);
                                                    button_Age.SetValue(Grid.RowProperty, 1);

                                                    _GeoChronButtons.Add(age, button_Age);

                                                    grid_Epochs.Children.Add(button_Age);
                                                }
                                            }
                                            else
                                            {
                                                button_Epoch.Vertical = true;
                                                button_Epoch.SetValue(Grid.RowSpanProperty, 2);
                                            }
                                        }

                                        grid_Periods.Children.Add(grid_Epochs);
                                    }
                                }
                            }
                            else
                            {
                                button_Era.Vertical = true;
                                button_Era.SetValue(Grid.RowSpanProperty, 2);
                            }
                        }
                    }
                    else
                    {
                        button_Eon.Vertical = true;
                        button_Eon.SetValue(Grid.RowSpanProperty, 3);
                    }
                }

                stackPanel_Eons.Children.Add(grid_Eons);
            }

            //

            GeoChronNameButton button = null;

            stackPanel_Timespan.AddHandler(UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is GeoChronNameButton source)
                {
                    button = source;
                }
            }));

            stackPanel_Timespan.AddHandler(UIElement.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is GeoChronNameButton source && source == button)
                {
                    GeoChron = source.GeoChron;
                    button = null;
                }
            }));
        }

        private void _InitMaBPControls()
        {
            textBox_MaBP.PreviewTextInput += (s, e) =>
            {
                TextBox textBox = s as TextBox;

                e.Handled = !new Regex("^([0-9]+)?.?([0-9]+)?$").IsMatch($"{textBox.Text[0..textBox.SelectionStart]}{e.Text}{textBox.Text[(textBox.SelectionStart + textBox.SelectionLength)..]}");
            };

            textBox_MaBP.TextChanged += (s, e) =>
            {
                if (double.TryParse((s as TextBox)?.Text, out double maBP))
                {
                    try
                    {
                        GeoChron geoChron = GeoChron.CreateGeoChron(maBP);

                        if (_GeoChron != geoChron)
                        {
                            _GeoChron = geoChron;

                            GeoChronChanged?.Invoke(this, _GeoChron);
                        }
                    }
                    catch { }
                }
            };
        }

        private void _InitCEYearControls()
        {
            Action updateByCEYear = () =>
            {
                if (int.TryParse(textBox_CEYear.Text, out int ceYear))
                {
                    if (BeforeChrist)
                    {
                        ceYear = -ceYear;
                    }

                    try
                    {
                        GeoChron geoChron = GeoChron.CreateGeoChron(ceYear);

                        if (_GeoChron != geoChron)
                        {
                            _GeoChron = geoChron;

                            GeoChronChanged?.Invoke(this, _GeoChron);
                        }
                    }
                    catch { }
                }
            };

            button_ADBC.Click += (s, e) =>
            {
                BeforeChrist = !BeforeChrist;

                updateByCEYear();

                textBox_CEYear.Focus();
                textBox_CEYear.SelectAll();
            };

            textBox_CEYear.PreviewTextInput += (s, e) => e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);

            textBox_CEYear.TextChanged += (s, e) => updateByCEYear();
        }

        //

        private GeoChron _GeoChron = GeoChron.Empty;

        private void _UpdateGeoChron(GeoChron oldGeoChron, GeoChron newGeoChron)
        {
            if (oldGeoChron != newGeoChron)
            {
                if (oldGeoChron.IsTimespan && newGeoChron.IsTimespan)
                {
                    GeoChronNameButton button;
                    IEnumerable<UIElement> elements;

                    //

                    // [0..4]=[Age..Eon]
                    GeoChron[] oldGeoChrons = { null, null, null, null, null };
                    GeoChron[] newGeoChrons = { null, null, null, null, null };

                    if (oldGeoChron is not null)
                    {
                        switch (oldGeoChron.Type)
                        {
                            case GeoChronType.Age: oldGeoChrons[0] = oldGeoChron; break;
                            case GeoChronType.Epoch: oldGeoChrons[1] = oldGeoChron; break;
                            case GeoChronType.Period: oldGeoChrons[2] = oldGeoChron; break;
                            case GeoChronType.Era: oldGeoChrons[3] = oldGeoChron; break;
                            case GeoChronType.Eon: oldGeoChrons[4] = oldGeoChron; break;
                        }
                    }

                    if (newGeoChron is not null)
                    {
                        switch (newGeoChron.Type)
                        {
                            case GeoChronType.Age: newGeoChrons[0] = newGeoChron; break;
                            case GeoChronType.Epoch: newGeoChrons[1] = newGeoChron; break;
                            case GeoChronType.Period: newGeoChrons[2] = newGeoChron; break;
                            case GeoChronType.Era: newGeoChrons[3] = newGeoChron; break;
                            case GeoChronType.Eon: newGeoChrons[4] = newGeoChron; break;
                        }
                    }

                    for (int i = 1; i < oldGeoChrons.Length; i++)
                    {
                        oldGeoChrons[i] ??= oldGeoChrons[i - 1]?.Superior;
                    }

                    for (int i = 1; i < newGeoChrons.Length; i++)
                    {
                        newGeoChrons[i] ??= newGeoChrons[i - 1]?.Superior;
                    }

                    GeoChron oldPeriod = oldGeoChrons[2];
                    GeoChron newPeriod = newGeoChrons[2];

                    //

                    if (oldGeoChron is not null && _GeoChronButtons.TryGetValue(oldGeoChron, out button) && button is not null)
                    {
                        button.IsChecked = false;
                    }

                    if (newGeoChron is not null && _GeoChronButtons.TryGetValue(newGeoChron, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    //

                    for (int i = 0; i < oldGeoChrons.Length; i++)
                    {
                        if (oldGeoChrons[i] is not null && oldGeoChrons[i] != oldGeoChron && _GeoChronButtons.TryGetValue(oldGeoChrons[i], out button) && button is not null)
                        {
                            button.IndirectlyChecked = false;
                        }
                    }

                    for (int i = 0; i < newGeoChrons.Length; i++)
                    {
                        if (newGeoChrons[i] is not null && newGeoChrons[i] != newGeoChron && _GeoChronButtons.TryGetValue(newGeoChrons[i], out button) && button is not null)
                        {
                            button.IndirectlyChecked = true;
                        }
                    }

                    //

                    if (oldPeriod is not null && _PeriodElements.TryGetValue(oldPeriod, out elements) && elements is not null)
                    {
                        foreach (var element in elements)
                        {
                            element.Visibility = Visibility.Collapsed;
                        }
                    }

                    if (newPeriod is not null && _PeriodElements.TryGetValue(newPeriod, out elements) && elements is not null)
                    {
                        foreach (var element in elements)
                        {
                            element.Visibility = Visibility.Visible;
                        }

                        grid_Periods.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid_Periods.Visibility = Visibility.Collapsed;
                    }
                }
                else if (oldGeoChron.IsTimespan)
                {
                    stackPanel_Timespan.Visibility = Visibility.Collapsed;

                    //

                    foreach (var item in _GeoChronButtons)
                    {
                        if (item.Value.IsChecked)
                        {
                            item.Value.IsChecked = false;
                        }

                        if (item.Value.IndirectlyChecked)
                        {
                            item.Value.IndirectlyChecked = false;
                        }
                    }

                    foreach (var item in _PeriodElements)
                    {
                        foreach (var element in item.Value)
                        {
                            element.Visibility = Visibility.Collapsed;
                        }
                    }

                    //

                    switch (newGeoChron.Type)
                    {
                        case GeoChronType.MaBP:
                            radioButton_MaBP.IsChecked = true;
                            stackPanel_MaBP.Visibility = Visibility.Visible;

                            textBox_MaBP.Text = newGeoChron.MaBP.ToString();
                            textBox_MaBP.Focus();
                            textBox_MaBP.SelectAll();
                            break;

                        case GeoChronType.CEYear:
                            radioButton_CEYear.IsChecked = true;
                            stackPanel_CEYear.Visibility = Visibility.Visible;

                            BeforeChrist = (newGeoChron.CEYear < 0);

                            textBox_CEYear.Text = Math.Abs(newGeoChron.CEYear.Value).ToString();
                            textBox_CEYear.Focus();
                            textBox_CEYear.SelectAll();
                            break;

                        case GeoChronType.Empty:
                            radioButton_Empty.IsChecked = true;
                            break;
                    }
                }
                else if (newGeoChron.IsTimespan)
                {
                    switch (oldGeoChron.Type)
                    {
                        case GeoChronType.MaBP:
                            stackPanel_MaBP.Visibility = Visibility.Collapsed;
                            break;

                        case GeoChronType.CEYear:
                            stackPanel_CEYear.Visibility = Visibility.Collapsed;
                            break;
                    }

                    radioButton_Timespan.IsChecked = true;

                    //

                    GeoChronNameButton button;
                    IEnumerable<UIElement> elements;

                    //

                    // [0..4]=[Age..Eon]
                    GeoChron[] newGeoChrons = { null, null, null, null, null };

                    if (newGeoChron is not null)
                    {
                        switch (newGeoChron.Type)
                        {
                            case GeoChronType.Age: newGeoChrons[0] = newGeoChron; break;
                            case GeoChronType.Epoch: newGeoChrons[1] = newGeoChron; break;
                            case GeoChronType.Period: newGeoChrons[2] = newGeoChron; break;
                            case GeoChronType.Era: newGeoChrons[3] = newGeoChron; break;
                            case GeoChronType.Eon: newGeoChrons[4] = newGeoChron; break;
                        }
                    }

                    for (int i = 1; i < newGeoChrons.Length; i++)
                    {
                        newGeoChrons[i] ??= newGeoChrons[i - 1]?.Superior;
                    }

                    GeoChron newPeriod = newGeoChrons[2];

                    //

                    if (newGeoChron is not null && _GeoChronButtons.TryGetValue(newGeoChron, out button) && button is not null)
                    {
                        button.IsChecked = true;
                    }

                    for (int i = 0; i < newGeoChrons.Length; i++)
                    {
                        if (newGeoChrons[i] is not null && newGeoChrons[i] != newGeoChron && _GeoChronButtons.TryGetValue(newGeoChrons[i], out button) && button is not null)
                        {
                            button.IndirectlyChecked = true;
                        }
                    }

                    if (newPeriod is not null && _PeriodElements.TryGetValue(newPeriod, out elements) && elements is not null)
                    {
                        foreach (var element in elements)
                        {
                            element.Visibility = Visibility.Visible;
                        }

                        grid_Periods.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        grid_Periods.Visibility = Visibility.Collapsed;
                    }

                    //

                    stackPanel_Timespan.Visibility = Visibility.Visible;
                }
                else
                {
                    switch (oldGeoChron.Type)
                    {
                        case GeoChronType.MaBP:
                            stackPanel_MaBP.Visibility = Visibility.Collapsed;
                            break;

                        case GeoChronType.CEYear:
                            stackPanel_CEYear.Visibility = Visibility.Collapsed;
                            break;
                    }

                    switch (newGeoChron.Type)
                    {
                        case GeoChronType.MaBP:
                            radioButton_MaBP.IsChecked = true;
                            stackPanel_MaBP.Visibility = Visibility.Visible;

                            textBox_MaBP.Text = newGeoChron.MaBP.ToString();
                            textBox_MaBP.Focus();
                            textBox_MaBP.SelectAll();
                            break;

                        case GeoChronType.CEYear:
                            radioButton_CEYear.IsChecked = true;
                            stackPanel_CEYear.Visibility = Visibility.Visible;

                            BeforeChrist = (newGeoChron.CEYear < 0);

                            textBox_CEYear.Text = Math.Abs(newGeoChron.CEYear.Value).ToString();
                            textBox_CEYear.Focus();
                            textBox_CEYear.SelectAll();
                            break;

                        case GeoChronType.Empty:
                            radioButton_Empty.IsChecked = true;
                            break;
                    }
                }
            }
        }

        //

        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateTheme()
        {
            foreach (var item in _GeoChronButtons)
            {
                item.Value.IsDarkTheme = _IsDarkTheme;
            }
        }

        //

        public GeoChronSelector()
        {
            InitializeComponent();

            //

            _AddRadioButtonsEvents();

            _InitTimespanControls();
            _InitMaBPControls();
            _InitCEYearControls();
        }

        //

        public GeoChron GeoChron
        {
            get => _GeoChron;

            set
            {
                if (_GeoChron != value)
                {
                    _RemoveRadioButtonsEvents();
                    _UpdateGeoChron(_GeoChron, value);
                    _AddRadioButtonsEvents();

                    _GeoChron = value;

                    GeoChronChanged?.Invoke(this, _GeoChron);
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

        public EventHandler<GeoChron> GeoChronChanged;
    }
}