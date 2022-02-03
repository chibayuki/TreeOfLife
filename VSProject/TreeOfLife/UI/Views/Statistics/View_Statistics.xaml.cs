/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

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

using ColorX = Com.Chromatics.ColorX;
using Com.Chromatics.Extensions;

using TreeOfLife.Core;
using TreeOfLife.Core.Statistics.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

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
            private List<(Border border, TextBlock rank, TextBlock count)> _DetailText = null;

            private ColorX _ThemeColor = ColorX.FromRgb(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateTheme()
            {
                _TitleBorder.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLabLightness(50));
                _TitleText.Foreground = _IsDarkTheme ? Brushes.Black : Brushes.White;

                _TotalBorder.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtHslLightness(_IsDarkTheme ? 20 : 80));
                _TotalText.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLabLightness(50));

                Brush background = Theme.GetSolidColorBrush(_ThemeColor.AtHslLightness(_IsDarkTheme ? 12.5 : 87.5));
                Brush borderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtHslLightness(_IsDarkTheme ? 20 : 80));
                Brush foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLabLightness(50));

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

                //

                _TitleText = new TextBlock()
                {
                    Text = basicRank.IsUnranked() ? "未指定" : basicRank.IsClade() ? "未分级演化支" : basicRank.GetChineseName(),
                    Margin = new Thickness(9, 0, 9, 0),
                    VerticalAlignment = VerticalAlignment.Center
                };

                _TitleBorder = new Border()
                {
                    Child = _TitleText,
                    CornerRadius = new CornerRadius(3),
                    Margin = new Thickness(0, 0, 6, 0)
                };

                _Container.Children.Add(_TitleBorder);

                //

                _TotalText = new TextBlock()
                {
                    Text = $"总计:   {statisticsResult.TaxonCount}",
                    Margin = new Thickness(0, 4, 7, 2),
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                _TotalBorder = new Border()
                {
                    Child = _TotalText,
                    BorderThickness = new Thickness(0, 0, 0, 2)
                };

                if (basicRank.IsPrimaryOrSecondaryRank())
                {
                    StackPanel detailsStackPanel = new StackPanel();

                    int detailsCount = statisticsResult.Details.Count + 1; // 把"总计"放在最后一个
                    Grid grid = null;
                    const int maxColumnCount = 3;
                    int columnCount = 0;
                    int columnId = 0;

                    Thickness detailTextMargin = new Thickness(6, 3, 6, 3);

                    for (int i = 0; i < detailsCount; i++)
                    {
                        if (grid is null)
                        {
                            grid = new Grid() { Margin = new Thickness(0, i > 0 ? 3 : 0, 0, 0) };

                            // 避免"总计"单独放在一行
                            if (detailsCount - i == maxColumnCount + 1)
                            {
                                columnCount = maxColumnCount - 1;
                            }
                            else
                            {
                                columnCount = Math.Min(maxColumnCount, detailsCount - i);
                            }

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

                        if (i < detailsCount - 1)
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

                            detailBorder.SetValue(Grid.ColumnProperty, 2 * columnId);

                            _DetailText.Add((detailBorder, rankText, countText));

                            grid.Children.Add(detailBorder);
                        }
                        else
                        {
                            _TotalBorder.SetValue(Grid.ColumnProperty, 2 * columnId);

                            grid.Children.Add(_TotalBorder);
                        }

                        columnId++;

                        if (columnId >= columnCount)
                        {
                            detailsStackPanel.Children.Add(grid);

                            grid = null;
                            columnId = 0;
                        }
                    }

                    _Container.Children.Add(detailsStackPanel);
                }
                else
                {
                    _Container.Children.Add(_TotalBorder);
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
                for (int i = 0; i < _StatisticsResults.Count; i++)
                {
                    _StatisticsResultItem result = _StatisticsResults[i];

                    result.Container.Margin = new Thickness(0, i > 0 ? 6 : 0, 0, 0);

                    stackPanel_StatisticsResult.Children.Add(result.Container);
                }
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