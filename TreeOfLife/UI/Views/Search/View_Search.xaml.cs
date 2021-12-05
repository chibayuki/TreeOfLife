/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

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

using TreeOfLife.Core;
using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views
{
    public partial class View_Search : UserControl
    {
        public ViewModel_Search ViewModel => this.DataContext as ViewModel_Search;

        //

        public View_Search()
        {
            InitializeComponent();

            //

            this.IsVisibleChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    _TrimAndSync();
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

            button_Search.GotFocus += (s, e) => textBox_Search.Focus();

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

        private class _SearchResultItem
        {
            public string Title { get; set; }
            public List<TaxonItem> TaxonItems { get; set; }
            public CollapsibleTaxonButtonGroup TaxonButtonGroup { get; set; }
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
            Taxon taxon = radioButton_Root.IsChecked ?? false ? Entrance.Root : Common.CurrentTaxon;
            string keyWord = ViewModel.KeyWord;

            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _SearchResult.Clear();

                IReadOnlyDictionary<MatchLevel, IReadOnlyList<Taxon>> searchResult = taxon.SearchAndGroupByMatchLevel(keyWord);
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
                    CollapsibleTaxonButtonGroup taxonButtonGroup = new CollapsibleTaxonButtonGroup()
                    {
                        Title = result.Title,
                        IsDarkTheme = Theme.IsDarkTheme,
                        Margin = new Thickness(0, 25, 0, 0),
                    };

                    taxonButtonGroup.UpdateContent(result.TaxonItems);

                    taxonButtonGroup.MouseLeftButtonClick += (s, e) =>
                    {
                        if (e.Taxon.IsRoot)
                        {
                            MessageBox.Show("该类群已经被删除。");
                        }
                        else
                        {
                            Common.CurrentTabPage = Common.TabPage.Evo;
                            Common.CurrentTaxon = e.Taxon;
                        }
                    };

                    result.TaxonButtonGroup = taxonButtonGroup;

                    stackPanel_SearchResult.Children.Add(taxonButtonGroup);
                }

                const int preferShowCount = 10;
                int count = 0;

                foreach (var result in _SearchResult)
                {
                    count += result.TaxonItems.Count;

                    if (count <= preferShowCount)
                    {
                        result.TaxonButtonGroup.Expanded = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _UpdateVisibility();
        }

        // 去除已被删除的类群，同步类群更新。
        private void _TrimAndSync()
        {
            if (_SearchResult.Count > 0)
            {
                foreach (var result in _SearchResult)
                {
                    if (result.TaxonItems.RemoveAll((item) => item.Taxon.IsRoot) > 0)
                    {
                        if (result.TaxonItems.Count > 0)
                        {
                            result.TaxonButtonGroup.UpdateContent(result.TaxonItems);
                        }
                        else
                        {
                            stackPanel_SearchResult.Children.Remove(result.TaxonButtonGroup);

                            result.TaxonButtonGroup.Clear();
                        }
                    }
                    else
                    {
                        result.TaxonButtonGroup.SyncTaxonUpdation();
                    }
                }

                _SearchResult.RemoveAll((item) => item.TaxonItems.Count <= 0);
            }

            _UpdateVisibility();
        }

        // 清空搜索结果。
        public void ClearSearchResult()
        {
            ViewModel.KeyWord = string.Empty;

            if (_SearchResult.Count > 0)
            {
                stackPanel_SearchResult.Children.Clear();

                _SearchResult.Clear();
            }

            _UpdateVisibility();
        }
    }
}