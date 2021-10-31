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
            private StackPanel _Container = null;
            private TextBlock _TitleText = null;
            private TextBlock _TotalCountText = null;
            private Border _TitleBorder = null;
            private List<(Border border, TextBlock rank, TextBlock count)> _DetailText = null;

            private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateTheme()
            {
                _TitleBorder.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
                _TitleText.Foreground = _TotalCountText.Foreground = _IsDarkTheme ? Brushes.Black : Brushes.White;

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

                _Container = new StackPanel();

                Thickness titleTextMargin = new Thickness(10, 4, 10, 4);

                _TitleText = new TextBlock()
                {
                    Text = basicRank.IsUnranked() ? "未指定" : basicRank.IsClade() ? "未分级演化支" : basicRank.GetChineseName(),
                    Margin = titleTextMargin
                };

                _TotalCountText = new TextBlock()
                {
                    Text = statisticsResult.TaxonCount.ToString(),
                    Margin = titleTextMargin,
                    HorizontalAlignment = HorizontalAlignment.Right
                };

                Grid titleGrid = new Grid();
                titleGrid.Children.Add(_TitleText);
                titleGrid.Children.Add(_TotalCountText);

                _TitleBorder = new Border()
                {
                    Child = titleGrid,
                    CornerRadius = new CornerRadius(3)
                };

                _Container.Children.Add(_TitleBorder);

                _DetailText = new List<(Border border, TextBlock rank, TextBlock count)>();

                if (basicRank.IsPrimaryOrSecondaryRank())
                {
                    Thickness detailTextMargin = new Thickness(9, 3, 9, 3);

                    foreach (var detail in statisticsResult.Details)
                    {
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

                        Grid detailGrid = new Grid();
                        detailGrid.Children.Add(rankText);
                        detailGrid.Children.Add(countText);

                        Border detailBorder = new Border()
                        {
                            Child = detailGrid,
                            CornerRadius = new CornerRadius(3),
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(0, 3, 0, 0),
                            SnapsToDevicePixels = true
                        };

                        _DetailText.Add((detailBorder, rankText, countText));

                        _Container.Children.Add(detailBorder);
                    }
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
            StatisticsResult statisticsResult = null;

            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _StatisticsResults.Clear();

                statisticsResult = Entrance.Root.Statistics();
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
                foreach (var result in _StatisticsResults)
                {
                    result.Container.Margin = new Thickness(0, 25, 0, 0);

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