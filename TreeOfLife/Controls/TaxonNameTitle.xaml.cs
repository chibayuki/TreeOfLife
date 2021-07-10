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
    /// TaxonNameTitle.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonNameTitle : UserControl
    {
        private TaxonomicCategory? _Category;
        private bool _IsParaphyly;
        private bool _IsPolyphyly;

        private void _UpdateCategory()
        {
            if (_Category is null)
            {
                textBlock_CategoryName.Text = string.Empty;
            }
            else
            {
                TaxonomicCategory category = _Category.Value;

                if (_IsParaphyly)
                {
                    textBlock_CategoryName.Text = (category.IsUnranked() || category.IsClade() ? "并系群" : category.GetChineseName() + "\n并系群");
                }
                else if (_IsPolyphyly)
                {
                    textBlock_CategoryName.Text = (category.IsUnranked() || category.IsClade() ? "复系群" : category.GetChineseName() + "\n复系群");
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
        }

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private Brush _CategoryNameForeground => Common.GetSolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);

        private Brush _CategoryNameBackground => Common.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToWpfColor());

        private Brush _TaxonNameForeground => Common.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToWpfColor());

        private Brush _TaxonNameBackground => Common.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToWpfColor());

        private Brush _BorderBrush => Common.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToWpfColor());

        private void _UpdateColor()
        {
            border_CategoryName.Background = _CategoryNameBackground;

            textBlock_CategoryName.Foreground = _CategoryNameForeground;

            border_TaxonName.Background = _TaxonNameBackground;
            border_TaxonName.BorderBrush = _BorderBrush;

            textBlock_TaxonName.Foreground = _TaxonNameForeground;
        }

        //

        public TaxonNameTitle()
        {
            InitializeComponent();
        }

        //

        public string TaxonName
        {
            get => textBlock_TaxonName.Text;
            set => textBlock_TaxonName.Text = value;
        }

        public TaxonomicCategory? Category
        {
            get => _Category;

            set
            {
                _Category = value;

                _UpdateCategory();
            }
        }

        public bool IsParaphyly
        {
            get => _IsParaphyly;

            set
            {
                _IsParaphyly = value;

                _UpdateCategory();
            }
        }

        public bool IsPolyphyly
        {
            get => _IsPolyphyly;

            set
            {
                _IsPolyphyly = value;

                _UpdateCategory();
            }
        }

        public ColorX ThemeColor
        {
            get => _ThemeColor;

            set
            {
                _ThemeColor = value;

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