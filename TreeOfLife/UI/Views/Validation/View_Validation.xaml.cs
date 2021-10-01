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
                    result.TaxonButtonGroup.IsDarkTheme = Theme.IsDarkTheme;
                }
            };
        }

        //

        public ViewModel_Validation ViewModel => this.DataContext as ViewModel_Validation;

        //

        private class _ValidateResultItem
        {
            public string Title { get; set; }
            public List<TaxonItem> TaxonItems { get; set; }
            public TaxonButtonGroup TaxonButtonGroup { get; set; }
            public ContentControl TitleLabel { get; set; }
            public Button ExpandCollapsedButton { get; set; }
            public FrameworkElement Container { get; set; }
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
                            ViewModel.ClickValidateResult(e.Taxon);
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

                    stackPanel_ValidateResult.Children.Add(grid);
                }
            }

            _UpdateVisibility();
        }

        // 裁剪检查结果，去除已被删除的类群。
        private void _TrimValidationResult()
        {
            if (_ValidateResult.Count > 0)
            {
                foreach (var result in _ValidateResult)
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
                            stackPanel_ValidateResult.Children.Remove(result.Container);
                        }
                    }
                }
            }

            _UpdateVisibility();
        }

        // 清空搜索结果。
        public void ClearValidationResult()
        {
            if (_ValidateResult.Count > 0)
            {
                _ValidateResult.Clear();

                stackPanel_ValidateResult.Children.Clear();
            }

            _UpdateVisibility();
        }
    }
}