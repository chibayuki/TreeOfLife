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
using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views
{
    /// <summary>
    /// View_Search.xaml 的交互逻辑
    /// </summary>
    public partial class View_Search : UserControl
    {
        public View_Search()
        {
            InitializeComponent();

            //

            this.IsVisibleChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    _TrimSearchResult();
                }
            };

            textBox_Search.KeyUp += async (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    await _SearchAndUpdateResultAsync();

                    textBox_Search.SelectAll();
                }
                else if (e.Key == Key.Escape)
                {
                    ClearSearchResult();

                    textBox_Search.Focus();
                }
            };

            button_Search.Click += async (s, e) =>
            {
                await _SearchAndUpdateResultAsync();

                textBox_Search.Focus();
                textBox_Search.SelectAll();
            };

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                foreach (var result in _SearchResult)
                {
                    result.TaxonButtonGroup.IsDarkTheme = Theme.IsDarkTheme;
                }
            };
        }

        //

        public ViewModel_Search ViewModel => this.DataContext as ViewModel_Search;

        //

        private class _SearchResultItem
        {
            public string Title { get; set; }
            public List<TaxonItem> TaxonItems { get; set; }
            public TaxonButtonGroup TaxonButtonGroup { get; set; }
            public ContentControl TitleLabel { get; set; }
            public Button ExpandCollapsedButton { get; set; }
            public FrameworkElement Container { get; set; }
        }

        private List<_SearchResultItem> _SearchResult = new List<_SearchResultItem>();

        // 更新可见性。
        private void _UpdateVisibility()
        {
            grid_SearchResult_Empty.Visibility = _SearchResult.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_SearchResult.Visibility = _SearchResult.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 搜索并更新结果。
        private async Task _SearchAndUpdateResultAsync()
        {
            string keyWord = ViewModel.KeyWord;

            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _SearchResult.Clear();

                IReadOnlyDictionary<MatchLevel, IReadOnlyList<Taxon>> searchResult = Entrance.Root.SearchAndGroupByMatchLevel(keyWord);
                IReadOnlyList<Taxon> perfect = searchResult[MatchLevel.Perfect];
                IReadOnlyList<Taxon> high = searchResult[MatchLevel.High];
                IReadOnlyList<Taxon> low = searchResult[MatchLevel.Low];

                if (perfect.Count > 0)
                {
                    List<TaxonItem> items = new List<TaxonItem>();

                    foreach (var taxon in perfect)
                    {
                        items.Add(new TaxonItem() { Taxon = taxon });
                    }

                    _SearchResult.Add(new _SearchResultItem()
                    {
                        Title = "最相关结果",
                        TaxonItems = items
                    });
                }

                if (high.Count > 0)
                {
                    List<TaxonItem> items = new List<TaxonItem>();

                    foreach (var taxon in high)
                    {
                        items.Add(new TaxonItem() { Taxon = taxon });
                    }

                    _SearchResult.Add(new _SearchResultItem()
                    {
                        Title = "较相关结果",
                        TaxonItems = items
                    });
                }

                if (low.Count > 0)
                {
                    List<TaxonItem> items = new List<TaxonItem>();

                    foreach (var taxon in low)
                    {
                        items.Add(new TaxonItem() { Taxon = taxon });
                    }

                    _SearchResult.Add(new _SearchResultItem()
                    {
                        Title = "次相关结果",
                        TaxonItems = items
                    });
                }
            });
            AsyncMethod.Finish();

            stackPanel_SearchResult.Children.Clear();

            if (_SearchResult.Count > 0)
            {
                foreach (var result in _SearchResult)
                {
                    TaxonButtonGroup taxonButtonGroup = new TaxonButtonGroup()
                    {
                        IsDarkTheme = Theme.IsDarkTheme,
                        Margin = new Thickness(0, 10, 0, 0),
                        Visibility = Visibility.Collapsed
                    };
                    taxonButtonGroup.SetValue(Grid.RowProperty, 1);
                    taxonButtonGroup.MouseLeftButtonClick += (s, e) =>
                    {
                        if (e.Taxon.IsRoot)
                        {
                            MessageBox.Show("该类群已经被删除。");
                        }
                        else
                        {
                            ViewModel.ClickSearchResult(e.Taxon);
                        }
                    };

                    Label label = new Label() { Content = $"{result.Title} ({result.TaxonItems.Count})" };
                    label.SetValue(StyleProperty, Application.Current.Resources["VerticalTitleLabelStyle"]);

                    Button button = new Button()
                    {
                        Content = "展开",
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 0, 2),
                        Padding = new Thickness(6, 3, 6, 3)
                    };
                    button.SetValue(StyleProperty, Application.Current.Resources["TransparentButtonStyle"]);
                    button.Click += (s, e) =>
                    {
                        if (button.Content as string == "展开")
                        {
                            if (taxonButtonGroup.GetGroupCount() <= 0)
                            {
                                taxonButtonGroup.UpdateContent(result.TaxonItems);
                            }

                            taxonButtonGroup.Visibility = Visibility.Visible;

                            button.Content = "折叠";
                        }
                        else
                        {
                            taxonButtonGroup.Visibility = Visibility.Collapsed;

                            button.Content = "展开";
                        }
                    };

                    Grid grid = new Grid() { Margin = new Thickness(0, 25, 0, 0) };
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.Children.Add(label);
                    grid.Children.Add(button);
                    grid.Children.Add(taxonButtonGroup);

                    result.TaxonButtonGroup = taxonButtonGroup;
                    result.TitleLabel = label;
                    result.ExpandCollapsedButton = button;
                    result.Container = grid;

                    stackPanel_SearchResult.Children.Add(grid);
                }

                const int preferShowCount = 10;
                int count = 0;

                foreach (var result in _SearchResult)
                {
                    count += result.TaxonItems.Count;

                    if (count <= preferShowCount)
                    {
                        result.TaxonButtonGroup.UpdateContent(result.TaxonItems);

                        result.TaxonButtonGroup.Visibility = Visibility.Visible;

                        result.ExpandCollapsedButton.Content = "折叠";
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _UpdateVisibility();
        }

        // 裁剪搜索结果，去除已被删除的类群。
        private void _TrimSearchResult()
        {
            if (_SearchResult.Count > 0)
            {
                foreach (var result in _SearchResult)
                {
                    if (result.TaxonItems.RemoveAll((item) => item.Taxon.IsRoot) > 0)
                    {
                        if (result.TaxonItems.Count > 0)
                        {
                            result.TitleLabel.Content = $"{result.Title} ({result.TaxonItems.Count})";

                            if (result.TaxonButtonGroup.GetGroupCount() > 0)
                            {
                                result.TaxonButtonGroup.UpdateContent(result.TaxonItems);
                            }
                        }
                        else
                        {
                            stackPanel_SearchResult.Children.Remove(result.Container);
                        }
                    }
                }
            }

            _UpdateVisibility();
        }

        // 清空搜索结果。
        public void ClearSearchResult()
        {
            ViewModel.KeyWord = string.Empty;

            if (_SearchResult.Count > 0)
            {
                _SearchResult.Clear();

                stackPanel_SearchResult.Children.Clear();
            }

            _UpdateVisibility();
        }
    }
}