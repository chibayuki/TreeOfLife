/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1000.1000.M10.210130-0000

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

        private double _CategoryNameWidth = 50; // 分类阶元名称宽度。

        private bool _Checked = false; // 是否处于已选择状态。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public TaxonNameButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateColor();
                _UpdateTaxon();
            };
        }

        //

        private void _UpdateTaxon()
        {
            label_CategoryName.Content = (_Taxon == null || _Taxon.IsAnonymous() ? string.Empty : _Taxon.Category.GetChineseName());
            label_TaxonName.Content = (_Taxon == null ? string.Empty : (_Sign == 0 ? _Taxon.GetShortName() : _Taxon.GetShortName() + (_Sign < 0 ? " [-]" : " [+]")));
        }

        private void _UpdateCategoryNameWidth()
        {
            label_CategoryName.Width = _CategoryNameWidth;
            label_TaxonName.Margin = new Thickness(_CategoryNameWidth, 0, 0, 0);
        }

        private Brush _CategoryNameForeground => SolidColorBrushes.GetBrush(_Checked ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

        private Brush _CategoryNameBackground => SolidColorBrushes.GetBrush((_Checked ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private Brush _TaxonNameForeground => SolidColorBrushes.GetBrush(_ThemeColor.AtLightness_LAB(_Checked ? (_IsDarkTheme ? 60 : 40) : 50).ToWpfColor());

        private Brush _TaxonNameBackground => SolidColorBrushes.GetBrush(_ThemeColor.AtLightness_HSL(_Checked ? (_IsDarkTheme ? 10 : 90) : (_IsDarkTheme ? 3 : 97)).ToWpfColor());

        private void _UpdateColor()
        {
            label_CategoryName.Foreground = _CategoryNameForeground;
            label_CategoryName.Background = _CategoryNameBackground;

            label_TaxonName.Foreground = _TaxonNameForeground;
            label_TaxonName.Background = _TaxonNameBackground;
            label_TaxonName.BorderBrush = _CategoryNameBackground;
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                _Taxon = value;

                _UpdateTaxon();
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