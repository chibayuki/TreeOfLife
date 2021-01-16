/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.900.1000.M9.210112-0000

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

            taxonNameButtonGroup_SearchResult.GroupNameWidth = 0;
            taxonNameButtonGroup_SearchResult.GroupMargin = new Thickness(0, 1, 0, 1);

            //

            button_Search.Click += (s, e) => _SearchAndUpdateResult();
        }

        //

        #region 搜索

        private void _SearchAndUpdateResult()
        {
            taxonNameButtonGroup_SearchResult.StartEditing();
            taxonNameButtonGroup_SearchResult.Clear();

            IReadOnlyList<Taxon> searchResult;

#if !DEBUG
            try
#endif
            {
                searchResult = Phylogenesis.Root.Search(ViewModel.KeyWord.Trim());
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

                    taxonNameButtonGroup_SearchResult.AddGroup(string.Empty, taxon.GetThemeColor());

                    TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };

                    button.MouseLeftButtonUp += (s, e) =>
                    {
                        if (taxon.IsRoot)
                        {
                            MessageBox.Show("该类群已经被删除。");
                        }
                        else
                        {
                            Common.SetCurrentTaxon(taxon);
                            ViewModel.ClickSearchResult();
                        }
                    };

                    taxonNameButtonGroup_SearchResult.AddButton(button, i);
                }
            }

            taxonNameButtonGroup_SearchResult.FinishEditing();
        }

        public void ClearSearchResult()
        {
            ViewModel.KeyWord = string.Empty;

            if (taxonNameButtonGroup_SearchResult.GetGroupCount() > 0)
            {
                taxonNameButtonGroup_SearchResult.StartEditing();
                taxonNameButtonGroup_SearchResult.Clear();
                taxonNameButtonGroup_SearchResult.FinishEditing();
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

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}