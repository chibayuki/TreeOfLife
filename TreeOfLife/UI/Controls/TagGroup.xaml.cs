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

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    /// <summary>
    /// TagGroup.xaml 的交互逻辑
    /// </summary>
    public partial class TagGroup : UserControl
    {
        private class _Tag
        {
            private Border _Container = null;
            private TextBlock _TagText = null;

            private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128);
            private bool _IsDarkTheme = false; // 是否为暗色主题。

            private void _UpdateColor()
            {
                _TagText.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60));
                _Container.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));
            }

            //

            public _Tag(string text)
            {
                _TagText = new TextBlock()
                {
                    Text = text,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(6, 3, 6, 3)
                };

                _Container = new Border()
                {
                    CornerRadius = new CornerRadius(3),
                    Margin = new Thickness(0, 3, 6, 3),
                    Child = _TagText
                };
            }

            //

            public string TagText
            {
                get => _TagText.Text;
                set => _TagText.Text = value;
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

            public FrameworkElement Container => _Container;
        }

        //

        List<_Tag> _Tags = new List<_Tag>();

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            foreach (var tag in _Tags)
            {
                tag.IsDarkTheme = _IsDarkTheme;
                tag.ThemeColor = _ThemeColor;
            }
        }

        //

        public TagGroup()
        {
            InitializeComponent();
        }

        //

        //

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

        public void Clear()
        {
            wrapPanel_Tags.Children.Clear();

            _Tags.Clear();
        }

        public void UpdateContent(IEnumerable<string> tags)
        {
            Clear();

            if (tags is not null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    _Tags.Add(new _Tag(tag));
                }

                foreach (var tag in _Tags)
                {
                    wrapPanel_Tags.Children.Add(tag.Container);
                }

                _UpdateColor();
            }
        }
    }
}