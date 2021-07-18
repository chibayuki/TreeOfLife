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
using TreeOfLife.Views;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// CategoryNameButton.xaml 的交互逻辑
    /// </summary>
    public partial class CategoryNameButton : UserControl
    {
        private TaxonomicCategory _Category = TaxonomicCategory.Unranked; // 分类阶元。

        private void _UpdateCategory()
        {
            textBlock_CategoryName.Text = _Category.GetChineseName();
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private Brush _CategoryNameForeground => Common.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

        private Brush _CategoryNameBackground => Common.GetSolidColorBrush((_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private void _UpdateColor()
        {
            border_CategoryName.Background = _CategoryNameBackground;

            textBlock_CategoryName.Foreground = _CategoryNameForeground;
        }

        //

        public CategoryNameButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateColor();
                _UpdateCategory();
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

        public TaxonomicCategory Category
        {
            get => _Category;

            set
            {
                _Category = value;
                _ThemeColor = _Category.GetThemeColor();

                _UpdateCategory();
                _UpdateColor();
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