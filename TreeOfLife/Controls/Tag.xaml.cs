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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// Tag.xaml 的交互逻辑
    /// </summary>
    public partial class Tag : UserControl
    {
        private string _Text = string.Empty; // 文本。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public Tag()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                label_Tag.FontFamily = this.FontFamily;
                label_Tag.FontSize = this.FontSize;
                label_Tag.FontStyle = this.FontStyle;
                label_Tag.FontWeight = this.FontWeight;
                label_Tag.MinWidth = this.MinWidth;
                label_Tag.MaxWidth = this.MaxWidth;

                _UpdateColor();
                _UpdateText();
            };

            label_Tag.MouseUp += (s, e) => base.OnMouseUp(e);
            label_Tag.MouseLeftButtonUp += (s, e) => base.OnMouseLeftButtonUp(e);
            label_Tag.MouseRightButtonUp += (s, e) => base.OnMouseRightButtonUp(e);
        }

        //

        private Brush _Foreground => SolidColorBrushes.GetBrush(_ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

        private Brush _Background => SolidColorBrushes.GetBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToWpfColor());

        private void _UpdateColor()
        {
            label_Tag.Foreground = _Foreground;
            border.Background = _Background;
        }

        private void _UpdateText()
        {
            label_Tag.Content = _Text;
        }

        //

        public string Text
        {
            get => _Text;

            set
            {
                _Text = value;

                _UpdateText();
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