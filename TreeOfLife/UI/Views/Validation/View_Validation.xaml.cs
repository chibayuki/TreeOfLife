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
using TreeOfLife.Core.Validation;
using TreeOfLife.Core.Validation.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views
{
    public partial class View_Validation : UserControl
    {
        public ViewModel_Validation ViewModel => this.DataContext as ViewModel_Validation;

        //

        public View_Validation()
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

            button_Validate.Click += async (s, e) => await _ValidateAndUpdateResultAsync();

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                foreach (var result in _ValidateResult)
                {
                    result.TaxonButtonGroup.IsDarkTheme = Theme.IsDarkTheme;
                }
            };
        }

        //

        private class _ValidateResultItem
        {
            public string Title { get; set; }
            public List<TaxonItem> TaxonItems { get; set; }
            public CollapsibleTaxonButtonGroup TaxonButtonGroup { get; set; }
        }

        private List<_ValidateResultItem> _ValidateResult = new List<_ValidateResultItem>();

        // 更新可见性。
        private void _UpdateVisibility()
        {
            grid_ValidateResult_Empty.Visibility = _ValidateResult.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;
            stackPanel_ValidateResult.Visibility = _ValidateResult.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        // 检查并更新结果。
        private async Task _ValidateAndUpdateResultAsync()
        {
            Taxon taxon = radioButton_Root.IsChecked ?? false ? Entrance.Root : Common.CurrentTaxon;

            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _ValidateResult.Clear();

                IReadOnlyDictionary<IValidator, IReadOnlyCollection<Taxon>> validateResult = taxon.ValidateChildrenAndGroupByValidator();

                foreach (var pair in validateResult)
                {
                    List<TaxonItem> items = new List<TaxonItem>();

                    foreach (var taxon in pair.Value)
                    {
                        items.Add(new TaxonItem() { Taxon = taxon });
                    }

                    _ValidateResult.Add(new _ValidateResultItem()
                    {
                        Title = pair.Key.ToString(),
                        TaxonItems = items
                    });
                }
            });
            AsyncMethod.Finish();

            stackPanel_ValidateResult.Children.Clear();

            if (_ValidateResult.Count > 0)
            {
                foreach (var result in _ValidateResult)
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
                            Common.IsEditMode = true;
                            Common.CurrentTaxon = e.Taxon;
                        }
                    };

                    result.TaxonButtonGroup = taxonButtonGroup;

                    stackPanel_ValidateResult.Children.Add(taxonButtonGroup);
                }
            }

            _UpdateVisibility();
        }

        // 去除已被删除的类群，同步类群更新。
        private void _TrimAndSync()
        {
            if (_ValidateResult.Count > 0)
            {
                foreach (var result in _ValidateResult)
                {
                    if (result.TaxonItems.RemoveAll((item) => item.Taxon.IsRoot) > 0)
                    {
                        if (result.TaxonItems.Count > 0)
                        {
                            result.TaxonButtonGroup.UpdateContent(result.TaxonItems);
                        }
                        else
                        {
                            stackPanel_ValidateResult.Children.Remove(result.TaxonButtonGroup);

                            result.TaxonButtonGroup.Clear();
                        }
                    }
                    else
                    {
                        result.TaxonButtonGroup.SyncTaxonUpdation();
                    }
                }

                _ValidateResult.RemoveAll((item) => item.TaxonItems.Count <= 0);
            }

            _UpdateVisibility();
        }

        // 清空搜索结果。
        public void ClearValidateResult()
        {
            if (_ValidateResult.Count > 0)
            {
                stackPanel_ValidateResult.Children.Clear();

                _ValidateResult.Clear();
            }

            _UpdateVisibility();
        }
    }
}