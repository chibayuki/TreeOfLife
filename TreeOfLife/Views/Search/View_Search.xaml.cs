/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

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

            button_Search.Click += (s, e) => _SearchAndUpdateResult();

            taxonNameButtonGroup_SearchResult.MouseLeftButtonClick += (s, e) =>
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
        }

        //

        #region 搜索

        private List<TaxonNameItem> _SearchResult = new List<TaxonNameItem>();

        // 搜索并更新结果。
        private void _SearchAndUpdateResult()
        {
            _SearchResult.Clear();

            IReadOnlyList<Taxon> searchResult;

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

            if (searchResult != null)
            {
                for (int i = 0; i < searchResult.Count; i++)
                {
                    Taxon taxon = searchResult[i];

                    _SearchResult.Add(new TaxonNameItem() { Taxon = taxon });
                }
            }

            taxonNameButtonGroup_SearchResult.UpdateContent(_SearchResult);
        }

        // 裁剪搜索结果，去除已被删除的类群。
        private void _TrimSearchResult()
        {
            if (_SearchResult.Count > 0)
            {
                int i = 0;

                while (i < _SearchResult.Count)
                {
                    if (_SearchResult[i].Taxon.IsRoot)
                    {
                        _SearchResult.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }

                taxonNameButtonGroup_SearchResult.UpdateContent(_SearchResult);
            }
        }

        // 清空搜索结果。
        public void ClearSearchResult()
        {
            ViewModel.KeyWord = string.Empty;

            if (_SearchResult.Count > 0)
            {
                _SearchResult.Clear();

                taxonNameButtonGroup_SearchResult.Clear();
            }
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

                taxonNameButtonGroup_SearchResult.IsDarkTheme = _IsDarkTheme;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}