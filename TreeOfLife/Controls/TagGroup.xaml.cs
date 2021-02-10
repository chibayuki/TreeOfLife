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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TagGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TagGroup : UserControl
    {
        List<Tag> _TagLabels = new List<Tag>();

        private Thickness _TagMargin = new Thickness(4); // 标签外边距。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

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

            wrapPanel_Tags.AddHandler(UserControl.MouseLeftButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is Tag source)
                {
                    MouseLeftButtonClick?.Invoke(this, source);
                }
            }));
            wrapPanel_Tags.AddHandler(UserControl.MouseRightButtonUpEvent, new RoutedEventHandler((s, e) =>
            {
                if (e.Source is Tag source)
                {
                    MouseRightButtonClick?.Invoke(this, source);
                }
            }));
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
                tag.FontStretch = this.FontStretch;
            }
        }

        private void _UpdateColor()
        {
            foreach (var tag in _TagLabels)
            {
                tag.IsDarkTheme = _IsDarkTheme;
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
            get => _TagMargin;

            set
            {
                _TagMargin = value;

                _UpdateLayout();
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

        public string[] Tags
        {
            set
            {
                wrapPanel_Tags.Visibility = Visibility.Hidden;

                wrapPanel_Tags.Children.Clear();

                _TagLabels = new List<Tag>(value.Length);

                foreach (var tag in value)
                {
                    Tag tagLabel = new Tag() { Text = tag };

                    _TagLabels.Add(tagLabel);

                    wrapPanel_Tags.Children.Add(tagLabel);
                }

                _UpdateFont();
                _UpdateColor();
                _UpdateLayout();

                wrapPanel_Tags.Visibility = Visibility.Visible;
            }
        }

        //

        public EventHandler<Tag> MouseLeftButtonClick;

        public EventHandler<Tag> MouseRightButtonClick;
    }
}