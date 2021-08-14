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

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TaxonNameButton.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonNameButton : UserControl
    {
        private Taxon _Taxon = null; // 类群。
        private int _Sign = 0; // "符号"，0：单系群继承关系的类群，-1：并系群排除的类群，+1：复系群包含的类群。

        private void _UpdateTaxon()
        {
            TaxonomicCategory category = _Taxon.Category;

            if (_Taxon is null || _Taxon.IsAnonymous())
            {
                textBlock_CategoryName.Text = string.Empty;
            }
            else
            {
                if (category.IsUnranked() || category.IsClade())
                {
                    if (_Taxon.IsParaphyly)
                    {
                        textBlock_CategoryName.Text = "并系群";
                    }
                    else if (_Taxon.IsPolyphyly)
                    {
                        textBlock_CategoryName.Text = "复系群";
                    }
                    else
                    {
                        if (category.IsUnranked())
                        {
                            textBlock_CategoryName.Text = string.Empty;
                        }
                        else
                        {
                            textBlock_CategoryName.Text = category.GetChineseName();
                        }
                    }
                }
                else
                {
                    textBlock_CategoryName.Text = category.GetChineseName();
                }
            }

            textBlock_TaxonName.Text = (_Taxon is null ? string.Empty : _Taxon.GetShortName());

            bool basicPrimary = category.IsBasicPrimaryCategory();
            bool bellowGenus = false;

            if (category.IsPrimaryOrSecondaryCategory())
            {
                TaxonomicCategory inheritedPrimaryCategory = _Taxon.GetInheritedPrimaryCategory();

                bellowGenus = inheritedPrimaryCategory.IsPrimaryCategory() && inheritedPrimaryCategory <= TaxonomicCategory.Genus;
            }

            textBlock_CategoryName.FontStyle = textBlock_TaxonName.FontStyle = (bellowGenus ? FontStyles.Italic : FontStyles.Normal);
            textBlock_CategoryName.FontWeight = textBlock_TaxonName.FontWeight = (basicPrimary ? FontWeights.Bold : FontWeights.Normal);
        }

        private void _UpdateSign()
        {
            grid_Positive.Visibility = (_Sign > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Negative.Visibility = (_Sign < 0 ? Visibility.Visible : Visibility.Collapsed);
        }

        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。

        private void _UpdateCategoryNameWidth()
        {
            border_CategoryName.Width = _CategoryNameWidth;
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            textBlock_CategoryName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            border_CategoryName.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));

            textBlock_TaxonName.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? 60 : 40) : 50));

            border_TaxonName.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? 10 : 90) : (_IsDarkTheme ? 3 : 97)));
            border_TaxonName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));
        }

        //

        public TaxonNameButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateSign();
                _UpdateCategoryNameWidth();
                _UpdateColor();
            };

            this.MouseEnter += (s, e) =>
            {
                _IsMouseOver = true;
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
                _UpdateColor();
            };
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;
                _ThemeColor = _Taxon.GetThemeColor();

                _UpdateTaxon();
                _UpdateColor();
            }
        }

        public int Sign
        {
            get => _Sign;

            set
            {
                _Sign = value;

                _UpdateSign();
            }
        }

        public double CategoryNameWidth
        {
            get => _CategoryNameWidth;

            set
            {
                _CategoryNameWidth = value;

                _UpdateCategoryNameWidth();
            }
        }

        public bool IsChecked
        {
            get => _IsChecked;

            set
            {
                _IsChecked = value;

                _UpdateColor();
            }
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColor();
            }
        }
    }
}