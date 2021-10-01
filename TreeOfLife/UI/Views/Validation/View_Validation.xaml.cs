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
using TreeOfLife.Core.Validation;
using TreeOfLife.Core.Validation.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.UI.Controls;

namespace TreeOfLife.UI.Views
{
    /// <summary>
    /// View_Validation.xaml 的交互逻辑
    /// </summary>
    public partial class View_Validation : UserControl
    {
        public View_Validation()
        {
            InitializeComponent();

            //

            this.IsVisibleChanged += (s, e) =>
            {
                if (this.IsVisible)
                {
                    _TrimValidationResult();
                }
            };

            button_Validate.Click += async (s, e) => await _ValidateAndUpdateResultAsync();

            //

            Theme.IsDarkThemeChanged += (s, e) =>
            {
                foreach (var result in _ValidateResult)
                {
                    result.Value.TaxonButtonGroup.IsDarkTheme = Theme.IsDarkTheme;
                }
            };
        }

        //

        public ViewModel_Validation ViewModel => this.DataContext as ViewModel_Validation;

        //

        private class _ValidateResultItem
        {
            public List<TaxonItem> TaxonItems { get; set; }
            public TaxonButtonGroup TaxonButtonGroup { get; set; }
            public FrameworkElement Container { get; set; }
            public ContentControl Title { get; set; }
        }

        private Dictionary<IValidator, _ValidateResultItem> _ValidateResult = new Dictionary<IValidator, _ValidateResultItem>();

        // 检查并更新结果。
        private async Task _ValidateAndUpdateResultAsync()
        {
            AsyncMethod.Start();
            await Task.Run(() =>
            {
                _ValidateResult.Clear();

                IReadOnlyDictionary<IValidator, IReadOnlyList<Taxon>> validateResult = Entrance.Root.ValidateChildrenAndGroupByValidator();

                foreach (var pair in validateResult)
                {
                    List<TaxonItem> items = new List<TaxonItem>();

                    foreach (var taxon in pair.Value)
                    {
                        items.Add(new TaxonItem() { Taxon = taxon });
                    }

                    _ValidateResult.Add(pair.Key, new _ValidateResultItem() { TaxonItems = items });
                }
            });
            AsyncMethod.Finish();

            stackPanel_ValidateResult.Children.Clear();

            if (_ValidateResult.Count > 0)
            {
                foreach (var result in _ValidateResult)
                {
                    TaxonButtonGroup taxonButtonGroup = new TaxonButtonGroup() { IsDarkTheme = Theme.IsDarkTheme };
                    taxonButtonGroup.SetValue(Grid.RowProperty, 2);
                    taxonButtonGroup.MouseLeftButtonClick += (s, e) =>
                    {
                        if (e.Taxon.IsRoot)
                        {
                            MessageBox.Show("该类群已经被删除。");
                        }
                        else
                        {
                            ViewModel.ClickValidateResult(e.Taxon);
                        }
                    };

                    Label label = new Label() { Content = $"{result.Key} ({result.Value.TaxonItems.Count})" };
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
                        if (taxonButtonGroup.GetGroupCount() > 0)
                        {
                            taxonButtonGroup.Clear();
                            button.Content = "展开";
                        }
                        else
                        {
                            taxonButtonGroup.UpdateContent(result.Value.TaxonItems);
                            button.Content = "折叠";
                        }
                    };

                    Grid title = new Grid();
                    title.Children.Add(label);
                    title.Children.Add(button);

                    Grid grid = new Grid() { Margin = new Thickness(0, 25, 0, 0) };
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(10) });
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.Children.Add(title);
                    grid.Children.Add(taxonButtonGroup);

                    result.Value.TaxonButtonGroup = taxonButtonGroup;
                    result.Value.Container = grid;
                    result.Value.Title = label;

                    stackPanel_ValidateResult.Children.Add(grid);
                }
            }
            else
            {
                stackPanel_ValidateResult.Children.Add(grid_SearchResult_Empty);
            }
        }

        // 裁剪检查结果，去除已被删除的类群。
        private void _TrimValidationResult()
        {
            if (_ValidateResult.Count > 0)
            {
                foreach (var result in _ValidateResult)
                {
                    if (result.Value.TaxonItems.RemoveAll((item) => item.Taxon.IsRoot) > 0)
                    {
                        if (result.Value.TaxonItems.Count > 0)
                        {
                            result.Value.Title.Content = $"{result.Key} ({result.Value.TaxonItems.Count})";

                            if (result.Value.TaxonButtonGroup.GetGroupCount() > 0)
                            {
                                result.Value.TaxonButtonGroup.UpdateContent(result.Value.TaxonItems);
                            }
                        }
                        else
                        {
                            stackPanel_ValidateResult.Children.Remove(result.Value.Container);
                        }
                    }
                }
            }
        }

        // 清空搜索结果。
        public void ClearValidationResult()
        {
            if (_ValidateResult.Count > 0)
            {
                _ValidateResult.Clear();

                stackPanel_ValidateResult.Children.Clear();
            }
        }
    }
}