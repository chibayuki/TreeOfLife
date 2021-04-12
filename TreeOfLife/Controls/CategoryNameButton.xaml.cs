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

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// CategoryNameButton.xaml 的交互逻辑
    /// </summary>
    public partial class CategoryNameButton : UserControl
    {
        private TaxonomicCategory? _Category = null; // 分类阶元。
        private string _CategoryName = string.Empty; // 名称。

        private bool _Checked = false; // 是否处于已选择状态。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

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
        }

        //

        private Brush _CategoryNameForeground => SolidColorBrushes.GetBrush(_Checked ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

        private Brush _CategoryNameBackground => SolidColorBrushes.GetBrush((_Checked ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private void _UpdateColor()
        {
            border_CategoryName.Background = _CategoryNameBackground;

            label_CategoryName.Foreground = _CategoryNameForeground;
        }

        private void _UpdateCategory()
        {
            label_CategoryName.Content = _CategoryName;
        }

        //

        public string CategoryName
        {
            get => _CategoryName;

            set
            {
                _CategoryName = value;

                _UpdateCategory();
            }
        }

        public TaxonomicCategory? Category
        {
            get => _Category;

            set
            {
                _Category = value;

                if (_Category != null)
                {
                    _CategoryName = _Category.Value.GetChineseName();

                    _UpdateCategory();
                }
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