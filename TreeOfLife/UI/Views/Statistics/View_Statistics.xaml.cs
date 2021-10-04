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

        public ViewModel_Validation ViewModel => this.DataContext as ViewModel_Validation;

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
                _TitleText.Foreground = _TotalCountText.Foreground = _IsDarkTheme ? Brushes.Black : Brushes.White;
                _TitleBorder.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));

                Brush foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
                Brush background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));

                foreach ((Border border, TextBlock rank, TextBlock count) in _DetailText)
                {
                    rank.Foreground = count.Foreground = foreground;
                    border.Background = background;
                }
            }

            //

            public _StatisticsResultItem(StatisticsResultOfBasicRank statisticsResult)
            {
                Rank basicRank = statisticsResult.BasicRank;

                _ThemeColor = basicRank.GetThemeColor();

                _Container = new StackPanel();

                _TitleText = new TextBlock()
                {
                    Text = basicRank.IsUnranked() ? "未指定" : basicRank.IsClade() ? "未分级演化支" : basicRank.GetChineseName(),
                    Margin = new Thickness(12, 4, 12, 4)
                };

                _TotalCountText = new TextBlock()
                {
                    Text = statisticsResult.TaxonCount.ToString(),
                    Margin = new Thickness(12, 4, 12, 4),
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
                    foreach (var detail in statisticsResult.Details)
                    {
                        TextBlock rankText = new TextBlock()
                        {
                            Text = detail.Rank.GetChineseName(),
                            Margin = new Thickness(12, 4, 12, 4)
                        };

                        TextBlock countText = new TextBlock()
                        {
                            Text = detail.TaxonCount.ToString(),
                            Margin = new Thickness(12, 4, 12, 4),
                            HorizontalAlignment = HorizontalAlignment.Right
                        };

                        Grid detailGrid = new Grid();
                        detailGrid.Children.Add(rankText);
                        detailGrid.Children.Add(countText);

                        Border border = new Border()
                        {
                            Child = detailGrid,
                            CornerRadius = new CornerRadius(3),
                            Margin = new Thickness(0, 3, 0, 0)
                        };

                        _DetailText.Add((border, rankText, countText));

                        _Container.Children.Add(border);
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