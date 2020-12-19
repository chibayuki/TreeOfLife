﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.608.1000.M6.201219-0000

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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TagGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TagGroup : UserControl
    {
        List<Tag> _TagLabels = new List<Tag>();

        private Thickness _TagMargin = new Thickness(2); // 标签外边距。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _DarkTheme = false; // 是否为暗色主题。

        //

        public TagGroup()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateFont();
                _UpdateColor();
            };
        }

        //

        private void _UpdateFont()
        {
            foreach (var tag in _TagLabels)
            {
                tag.FontFamily = this.FontFamily;
                tag.FontSize = this.FontSize;
                tag.FontStyle = this.FontStyle;
                tag.FontWeight = this.FontWeight;
            }
        }

        private void _UpdateColor()
        {
            foreach (var tag in _TagLabels)
            {
                tag.ThemeColor = _ThemeColor;
            }
        }

        private void _UpdateLayout()
        {
            if (_TagLabels.Count > 0)
            {
                foreach (var tagLabel in _TagLabels)
                {
                    tagLabel.Margin = _TagMargin;
                }
            }
        }

        //

        public Thickness TagMargin
        {
            get
            {
                return _TagMargin;
            }

            set
            {
                _TagMargin = value;

                _UpdateLayout();
            }
        }

        public ColorX ThemeColor
        {
            get
            {
                return _ThemeColor;
            }

            set
            {
                _ThemeColor = value;

                _UpdateColor();
            }
        }

        public bool IsDarkTheme
        {
            get
            {
                return _DarkTheme;
            }

            set
            {
                _DarkTheme = value;

                _UpdateColor();
            }
        }

        public string[] Tags
        {
            set
            {
                wrapPanel_Tags.Visibility = Visibility.Hidden;

                wrapPanel_Tags.Children.Clear();

                _TagLabels = new List<Tag>(value.Length);

                foreach (var tag in value)
                {
                    Tag tagLabel = new Tag
                    {
                        Text = tag
                    };

                    _TagLabels.Add(tagLabel);

                    wrapPanel_Tags.Children.Add(tagLabel);
                }

                _UpdateFont();
                _UpdateColor();
                _UpdateLayout();

                wrapPanel_Tags.Visibility = Visibility.Visible;
            }
        }
    }
}