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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    public partial class TaxonTitle : UserControl
    {
        private Rank? _Rank = null;
        private bool _IsParaphyly = false;
        private bool _IsPolyphyly = false;

        private void _UpdateRank()
        {
            if (_Rank is null)
            {
                textBlock_RankName.Text = string.Empty;
            }
            else
            {
                Rank rank = _Rank.Value;

                if (_IsParaphyly)
                {
                    textBlock_RankName.Text = rank.IsUnranked() || rank.IsClade() ? "并系群" : rank.GetChineseName() + "\n并系群";
                }
                else if (_IsPolyphyly)
                {
                    textBlock_RankName.Text = rank.IsUnranked() || rank.IsClade() ? "复系群" : rank.GetChineseName() + "\n复系群";
                }
                else
                {
                    if (rank.IsUnranked())
                    {
                        textBlock_RankName.Text = string.Empty;
                    }
                    else
                    {
                        textBlock_RankName.Text = rank.GetChineseName();
                    }
                }
            }

            border_Split.Visibility = !string.IsNullOrEmpty(textBlock_RankName.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool _ShowUnderline = true;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            textBlock_TaxonName.Foreground = textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(50));
            border_Split.BorderBrush = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 30 : 70));
        }

        //

        public TaxonTitle()
        {
            InitializeComponent();
        }

        //

        public string TaxonName
        {
            get => textBlock_TaxonName.Text;
            set => textBlock_TaxonName.Text = value;
        }

        public Rank? Rank
        {
            get => _Rank;

            set
            {
                _Rank = value;

                _UpdateRank();
            }
        }

        public bool IsParaphyly
        {
            get => _IsParaphyly;

            set
            {
                _IsParaphyly = value;

                _UpdateRank();
            }
        }

        public bool IsPolyphyly
        {
            get => _IsPolyphyly;

            set
            {
                _IsPolyphyly = value;

                _UpdateRank();
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