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

using TreeOfLife.Controls;
using TreeOfLife.Phylogeny;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

namespace TreeOfLife.Views.Search
{
    /// <summary>
    /// View_Search.xaml 的交互逻辑
    /// </summary>
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
                    _TrimSearchResult();
                }
            };

            textBox_Search.KeyUp += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    _SearchAndUpdateResult();

                    textBox_Search.SelectAll();
                }
                else if (e.Key == Key.Escape)
                {
                    ClearSearchResult();

                    textBox_Search.Focus();
                }
            };

            button_Search.Click += (s, e) =>
            {
                _SearchAndUpdateResult();

                textBox_Search.Focus();
                textBox_Search.SelectAll();
            };

            EventHandler<TaxonNameButton> taxonNameButtonGroup_SearchResult_MouseLeftButtonClick = (s, e) =>
            {
                if (e.Taxon.IsRoot)
                {
                    MessageBox.Show("该类群已经被删除。");
                }
                else
                {
                    Common.SetCurrentTaxon(e.Taxon);
                    ViewModel.ClickSearchResult();
                }
            };

            taxonNameButtonGroup_SearchResult_Perfect.MouseLeftButtonClick += taxonNameButtonGroup_SearchResult_MouseLeftButtonClick;
            taxonNameButtonGroup_SearchResult_High.MouseLeftButtonClick += taxonNameButtonGroup_SearchResult_MouseLeftButtonClick;
            taxonNameButtonGroup_SearchResult_Low.MouseLeftButtonClick += taxonNameButtonGroup_SearchResult_MouseLeftButtonClick;

            button_ExpandHigh.Click += (s, e) =>
            {
                taxonNameButtonGroup_SearchResult_High.UpdateContent(_SearchResult_High);

                button_ExpandHigh.Visibility = Visibility.Collapsed;
            };

            button_ExpandLow.Click += (s, e) =>
            {
                taxonNameButtonGroup_SearchResult_Low.UpdateContent(_SearchResult_Low);

                button_ExpandLow.Visibility = Visibility.Collapsed;
            };
        }

        //

        #region 搜索

        private List<TaxonNameItem> _SearchResult_Perfect = new List<TaxonNameItem>();
        private List<TaxonNameItem> _SearchResult_High = new List<TaxonNameItem>();
        private List<TaxonNameItem> _SearchResult_Low = new List<TaxonNameItem>();

        // 更新可见性。
        private void _UpdateVisibility()
        {
            grid_SearchResult_Empty.Visibility = (_SearchResult_Perfect.Count <= 0 && _SearchResult_High.Count <= 0 && _SearchResult_Low.Count <= 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_SearchResult_Perfect.Visibility = (_SearchResult_Perfect.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_SearchResult_High.Visibility = (_SearchResult_High.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_SearchResult_Low.Visibility = (_SearchResult_Low.Count > 0 ? Visibility.Visible : Visibility.Collapsed);
        }

        // 搜索并更新结果。
        private void _SearchAndUpdateResult()
        {
            _SearchResult_Perfect.Clear();
            _SearchResult_High.Clear();
            _SearchResult_Low.Clear();

            IReadOnlyList<(Taxon taxon, TaxonSearchExtension.MatchLevel matchLevel)> searchResult;

#if !DEBUG
            try
#endif
            {
                searchResult = Phylogenesis.Root.Search(ViewModel.KeyWord);
            }
#if !DEBUG
            catch
            {
                searchResult = null;
            }
#endif

            if (searchResult is not null)
            {
                for (int i = 0; i < searchResult.Count; i++)
                {
                    (Taxon taxon, TaxonSearchExtension.MatchLevel matchLevel) = searchResult[i];

                    if (matchLevel == TaxonSearchExtension.MatchLevel.Perfect)
                    {
                        _SearchResult_Perfect.Add(new TaxonNameItem() { Taxon = taxon });
                    }
                    else if (matchLevel == TaxonSearchExtension.MatchLevel.High)
                    {
                        _SearchResult_High.Add(new TaxonNameItem() { Taxon = taxon });
                    }
                    else
                    {
                        _SearchResult_Low.Add(new TaxonNameItem() { Taxon = taxon });
                    }
                }
            }

            taxonNameButtonGroup_SearchResult_Perfect.UpdateContent(_SearchResult_Perfect);

            const int preferShowCount = 10;
            int perfectCount = _SearchResult_Perfect.Count;
            int highCount = _SearchResult_High.Count;
            int lowCount = _SearchResult_Low.Count;

            if (perfectCount <= 0 || perfectCount + highCount <= preferShowCount)
            {
                taxonNameButtonGroup_SearchResult_High.UpdateContent(_SearchResult_High);

                button_ExpandHigh.Visibility = Visibility.Collapsed;
            }
            else
            {
                taxonNameButtonGroup_SearchResult_High.Clear();

                button_ExpandHigh.Visibility = Visibility.Visible;
            }

            if (perfectCount + highCount <= 0 || perfectCount + highCount + lowCount <= preferShowCount)
            {
                taxonNameButtonGroup_SearchResult_Low.UpdateContent(_SearchResult_Low);

                button_ExpandLow.Visibility = Visibility.Collapsed;
            }
            else
            {
                taxonNameButtonGroup_SearchResult_Low.Clear();

                button_ExpandLow.Visibility = Visibility.Visible;
            }

            _UpdateVisibility();
        }

        // 裁剪搜索结果，去除已被删除的类群。
        private void _TrimSearchResult()
        {
            if (_SearchResult_Perfect.Count > 0 && _SearchResult_Perfect.RemoveAll((item) => item.Taxon.IsRoot) > 0)
            {
                taxonNameButtonGroup_SearchResult_Perfect.UpdateContent(_SearchResult_Perfect);
            }

            if (_SearchResult_High.Count > 0 && _SearchResult_High.RemoveAll((item) => item.Taxon.IsRoot) > 0)
            {
                taxonNameButtonGroup_SearchResult_High.UpdateContent(_SearchResult_High);
            }

            if (_SearchResult_Low.Count > 0 && _SearchResult_Low.RemoveAll((item) => item.Taxon.IsRoot) > 0)
            {
                taxonNameButtonGroup_SearchResult_Low.UpdateContent(_SearchResult_Low);
            }

            _UpdateVisibility();
        }

        // 清空搜索结果。
        public void ClearSearchResult()
        {
            ViewModel.KeyWord = string.Empty;

            if (_SearchResult_Perfect.Count > 0)
            {
                _SearchResult_Perfect.Clear();

                taxonNameButtonGroup_SearchResult_Perfect.Clear();
            }

            if (_SearchResult_High.Count > 0)
            {
                _SearchResult_High.Clear();

                taxonNameButtonGroup_SearchResult_High.Clear();
            }

            if (_SearchResult_Low.Count > 0)
            {
                _SearchResult_Low.Clear();

                taxonNameButtonGroup_SearchResult_Low.Clear();
            }

            _UpdateVisibility();
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                taxonNameButtonGroup_SearchResult_Perfect.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_SearchResult_High.IsDarkTheme = _IsDarkTheme;
                taxonNameButtonGroup_SearchResult_Low.IsDarkTheme = _IsDarkTheme;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}