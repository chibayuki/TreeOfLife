/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

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

using System.Windows.Media.Effects;

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    public partial class RankButton : UserControl
    {
        private static readonly DropShadowEffect _DropShadowEffect = new DropShadowEffect()
        {
            BlurRadius = 3,
            Direction = 315,
            Opacity = 0.3,
            ShadowDepth = 1
        };

        //

        private Rank _Rank = Rank.Unranked; // 分类阶元。

        private void _UpdateRank()
        {
            textBlock_RankName.Text = _Rank.GetChineseName();
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsIndirectlyChecked = false;
        private bool _IsMouseOver = false;

        private void _UpdateIsIndirectlyChecked() => path_IndirectlyChecked.Visibility = _IsIndirectlyChecked ? Visibility.Visible : Visibility.Collapsed;

        private void _UpdateEffect() => border_Background.Effect = !_IsChecked && _IsMouseOver ? _DropShadowEffect : null;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            border_Background.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 12.5 : 87.5));
            border_Background.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
            path_IndirectlyChecked.Fill = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor() : Colors.Transparent);
            textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
        }

        //

        public RankButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateColor();
                _UpdateRank();
            };

            this.MouseEnter += (s, e) =>
            {
                _IsMouseOver = true;
                _UpdateEffect();
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
                _UpdateEffect();
                _UpdateColor();
            };
        }

        //

        public Rank Rank
        {
            get => _Rank;

            set
            {
                _Rank = value;
                _ThemeColor = _Rank.GetThemeColor();

                _UpdateRank();
                _UpdateColor();
            }
        }

        public bool IsChecked
        {
            get => _IsChecked;

            set
            {
                _IsChecked = value;

                _UpdateEffect();
                _UpdateColor();
            }
        }

        public bool IsIndirectlyChecked
        {
            get => _IsIndirectlyChecked;

            set
            {
                _IsIndirectlyChecked = value;

                _UpdateIsIndirectlyChecked();
                _UpdateEffect();
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