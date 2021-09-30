﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    /// <summary>
    /// RankButton.xaml 的交互逻辑
    /// </summary>
    public partial class RankButton : UserControl
    {
        private Rank _Rank = Rank.Unranked; // 分类阶元。

        private void _UpdateRank()
        {
            textBlock_RankName.Text = _Rank.GetChineseName();
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor());

            border_RankName.Background = Theme.GetSolidColorBrush((_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());
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
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
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