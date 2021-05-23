/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1134.1000.M11.210518-2200

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
using TreeOfLife.Views;

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

        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。

        private bool _Checked = false; // 是否处于已选择状态。
        private bool _MouseOver = false;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public TaxonNameButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateFont();
                _UpdateColor();
            };

            this.MouseEnter += (s, e) =>
            {
                _MouseOver = true;
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _MouseOver = false;
                _UpdateColor();
            };
        }

        //

        private void _UpdateCategoryNameWidth()
        {
            border_CategoryName.Width = _CategoryNameWidth;
        }

        private void _UpdateTaxon()
        {
            if (_Taxon == null || _Taxon.IsAnonymous())
            {
                textBlock_CategoryName.Text = string.Empty;
            }
            else
            {
                TaxonomicCategory category = _Taxon.Category;

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
                        textBlock_CategoryName.Text = category.GetChineseName();
                    }
                }
                else
                {
                    textBlock_CategoryName.Text = category.GetChineseName();
                }
            }

            textBlock_TaxonName.Text = (_Taxon == null ? string.Empty : _Taxon.GetShortName());

            grid_Positive.Visibility = (_Sign > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Negative.Visibility = (_Sign < 0 ? Visibility.Visible : Visibility.Collapsed);
        }

        private void _UpdateFont()
        {
            TaxonomicCategory category = _Taxon.Category;

            bool basicPrimary = category.IsBasicPrimaryCategory();
            bool bellowGenus = (category.IsPrimaryCategory() || category.IsSecondaryCategory()) && _Taxon.GetInheritedBasicPrimaryCategory() <= TaxonomicCategory.Genus;

            textBlock_CategoryName.FontStyle = textBlock_TaxonName.FontStyle = (bellowGenus ? FontStyles.Italic : FontStyles.Normal);
            textBlock_CategoryName.FontWeight = textBlock_TaxonName.FontWeight = (basicPrimary ? FontWeights.Bold : FontWeights.Normal);
        }

        private Brush _CategoryNameForeground => Common.GetSolidColorBrush(_Checked || _MouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

        private Brush _CategoryNameBackground => Common.GetSolidColorBrush((_Checked || _MouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private Brush _TaxonNameForeground => Common.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_Checked || _MouseOver ? (_IsDarkTheme ? 60 : 40) : 50).ToWpfColor());

        private Brush _TaxonNameBackground => Common.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_Checked || _MouseOver ? (_IsDarkTheme ? 10 : 90) : (_IsDarkTheme ? 3 : 97)).ToWpfColor());

        private Brush _BorderBrush => Common.GetSolidColorBrush((_Checked || _MouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private void _UpdateColor()
        {
            _ThemeColor = _Taxon.GetThemeColor();

            border_CategoryName.Background = _CategoryNameBackground;
            border_CategoryName.BorderBrush = _BorderBrush;

            textBlock_CategoryName.Foreground = _CategoryNameForeground;

            border_TaxonName.Background = _TaxonNameBackground;
            border_TaxonName.BorderBrush = _BorderBrush;

            textBlock_TaxonName.Foreground = _TaxonNameForeground;
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;

                _UpdateTaxon();
                _UpdateFont();
                _UpdateColor();
            }
        }

        public int Sign
        {
            get => _Sign;

            set
            {
                _Sign = value;

                _UpdateTaxon();
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

        public bool Checked
        {
            get => _Checked;

            set
            {
                _Checked = value;

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