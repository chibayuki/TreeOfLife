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

using TreeOfLife.Core;
using TreeOfLife.Core.Statistics.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Views
{
    public partial class View_Statistics : UserControl
    {
        public ViewModel_Validation ViewModel => this.DataContext as ViewModel_Validation;

        //

        public View_Statistics()
        {
            InitializeComponent();

            //

            button_Statistics.Click += async (s, e) => await _ValidateAndUpdateResultAsync();

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                foreach (var result in _StatisticsResults)
                {
                    result.IsDarkTheme = Theme.IsDarkTheme;
                }
            };
        }

        //

        private class _StatisticsResultItem
        {
            private DockPanel _Container = null;
            private Border _TitleBorder = null;
            private TextBlock _TitleText = null;
            private Border _TotalBorder = null;
            private TextBlock _TotalText = null;
            private TextBlock _TotalCountText = null;
            private List<(Border border, TextBlock rank, TextBlock count)> _DetailText = null;

            private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateTheme()
            {
                _TitleBorder.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
                _TitleText.Foreground = _IsDarkTheme ? Brushes.Black : Brushes.White;

                _TotalBorder.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
                _TotalText.Foreground = _TotalCountText.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));

                Brush background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 12.5 : 87.5));
                Brush borderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
                Brush foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));

                foreach ((Border border, TextBlock rank, TextBlock count) in _DetailText)
                {
                    border.Background = background;
                    border.BorderBrush = borderBrush;
                    rank.Foreground = count.Foreground = foreground;
                }
            }

            //

            public _StatisticsResultItem(StatisticsResultOfBasicRank statisticsResult)
            {
                Rank basicRank = statisticsResult.BasicRank;

                _ThemeColor = basicRank.GetThemeColor();

                _Container = new DockPanel();
                _DetailText = new List<(Border border, TextBlock rank, TextBlock count)>();

                if (basicRank.IsPrimaryOrSecondaryRank())
                {
                    _TitleText = new TextBlock()
                    {
                        Text = basicRank.GetChineseName(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    _TitleBorder = new Border()
                    {
                        Child = _TitleText,
                        CornerRadius = new CornerRadius(3),
                        Width = 30,
                        Margin = new Thickness(0, 0, 6, 0)
                    };

                    _Container.Children.Add(_TitleBorder);

                    //

                    StackPanel detailsStackPanel = new StackPanel();

                    int detailsCount = statisticsResult.Details.Count;
                    int columnCount = Math.Min(3, detailsCount);
                    Grid grid = null;

                    Thickness detailTextMargin = new Thickness(6, 3, 6, 3);

                    for (int i = 0; i < detailsCount; i++)
                    {
                        StatisticsResultOfRank detail = statisticsResult.Details[i];

                        TextBlock rankText = new TextBlock()
                        {
                            Text = detail.Rank.GetChineseName(),
                            Margin = detailTextMargin
                        };

                        TextBlock countText = new TextBlock()
                        {
                            Text = detail.TaxonCount.ToString(),
                            Margin = detailTextMargin,
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        if (detail.Rank.IsBasicPrimaryRank())
                        {
                            rankText.FontWeight = countText.FontWeight = FontWeights.Bold;
                        }

                        Grid detailGrid = new Grid();
                        detailGrid.Children.Add(rankText);
                        detailGrid.Children.Add(countText);

                        Border detailBorder = new Border()
                        {
                            Child = detailGrid,
                            CornerRadius = new CornerRadius(3),
                            BorderThickness = new Thickness(1),
                            SnapsToDevicePixels = true
                        };

                        detailBorder.SetValue(Grid.ColumnProperty, 2 * (i % columnCount));

                        _DetailText.Add((detailBorder, rankText, countText));

                        if (i % columnCount == 0)
                        {
                            grid = new Grid() { Margin = new Thickness(0, i > 0 ? 3 : 0, 0, 0) };

                            if (columnCount > 1)
                            {
                                grid.ColumnDefinitions.Add(new ColumnDefinition());

                                for (int c = 1; c < columnCount; c++)
                                {
                                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(3) });
                                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                                }
                            }
                        }

                        grid.Children.Add(detailBorder);

                        if (i % columnCount == columnCount - 1 || i == detailsCount - 1)
                        {
                            detailsStackPanel.Children.Add(grid);
                        }
                    }

                    //

                    _TotalText = new TextBlock()
                    {
                        Text = "总计:",
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    _TotalCountText = new TextBlock() { Text = statisticsResult.TaxonCount.ToString() };

                    StackPanel totalCountStackPanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 4, 7, 4),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    totalCountStackPanel.Children.Add(_TotalText);
                    totalCountStackPanel.Children.Add(_TotalCountText);

                    _TotalBorder = new Border()
                    {
                        Child = totalCountStackPanel,
                        BorderThickness = new Thickness(0, 0, 0, 2),
                        Margin = new Thickness(0, 3, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    detailsStackPanel.Children.Add(_TotalBorder);

                    _Container.Children.Add(detailsStackPanel);
                }
                else
                {
                    Grid grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(6) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    _TitleText = new TextBlock()
                    {
                        Text = basicRank.IsUnranked() ? "未指定" : basicRank.IsClade() ? "未分级演化支" : basicRank.GetChineseName(),
                        Margin = new Thickness(9, 0, 9, 0),
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    _TitleBorder = new Border()
                    {
                        Child = _TitleText,
                        CornerRadius = new CornerRadius(3)
                    };

                    grid.Children.Add(_TitleBorder);

                    //

                    _TotalText = new TextBlock()
                    {
                        Text = "总计:",
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    _TotalCountText = new TextBlock() { Text = statisticsResult.TaxonCount.ToString() };

                    StackPanel totalCountStackPanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 4, 7, 4),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    totalCountStackPanel.Children.Add(_TotalText);
                    totalCountStackPanel.Children.Add(_TotalCountText);

                    _TotalBorder = new Border()
                    {
                        Child = totalCountStackPanel,
                        BorderThickness = new Thickness(0, 0, 0, 2),
                        Margin = new Thickness(0, 3, 0, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    _TotalBorder.SetValue(Grid.ColumnProperty, 2);

                    grid.Children.Add(_TotalBorder);

                    _Container.Children.Add(grid);
                }

                _UpdateTheme();
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

            public FrameworkElement Container => _Container;
        }

        private List<_StatisticsResultItem> _StatisticsResults = new List<_StatisticsResultItem>();

        // 更新可见性。
        private void _UpdateVisibility()
        {
            stackPanel_StatisticsResult.Visibility = _StatisticsResults.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 统计并更新结果。
        private async Task _ValidateAndUpdateResultAsync()
        {
            Taxon taxon = radioButton_Root.IsChecked ?? false ? Entrance.Root : Common.CurrentTaxon;
            StatisticsResult statisticsResult = null;

            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _StatisticsResults.Clear();

                statisticsResult = taxon.Statistics();
            });
            AsyncMethod.Finish();

            label_TaxonCount.Content = statisticsResult.TaxonCount.ToString();
            label_NodeCount.Content = statisticsResult.NodeCount.ToString();

            foreach (var detail in statisticsResult.Details)
            {
                _StatisticsResults.Add(new _StatisticsResultItem(detail) { IsDarkTheme = Theme.IsDarkTheme });
            }

            stackPanel_StatisticsResult.Children.Clear();

            if (_StatisticsResults.Count > 0)
            {
                StackPanel stackPanel = new StackPanel() { Margin = new Thickness(0, 25, 0, 0) };

                for (int i = 0; i < _StatisticsResults.Count; i++)
                {
                    _StatisticsResultItem result = _StatisticsResults[i];

                    result.Container.Margin = new Thickness(0, i > 0 ? 6 : 0, 0, 0);

                    stackPanel.Children.Add(result.Container);
                }

                stackPanel_StatisticsResult.Children.Add(stackPanel);
            }

            _UpdateVisibility();
        }

        // 清空统计结果。
        public void ClearStatisticsResult()
        {
            label_TaxonCount.Content = label_NodeCount.Content = "0";

            if (_StatisticsResults.Count > 0)
            {
                stackPanel_StatisticsResult.Children.Clear();

                _StatisticsResults.Clear();
            }

            _UpdateVisibility();
        }
    }
}