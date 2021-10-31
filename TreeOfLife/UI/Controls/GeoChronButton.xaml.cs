/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Geology.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    public partial class GeoChronButton : UserControl
    {
        private GeoChron _GeoChron = null; // 地质年代。
        private string _GeoChronName = string.Empty; // 名称。

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsIndirectlyChecked = false;
        private bool _MouseOver = false;
        private bool _Vertical = false;

        private void _UpdateIsIndirectlyChecked() => path_IndirectlyChecked.Visibility = _IsIndirectlyChecked ? Visibility.Visible : Visibility.Collapsed;

        private void _UpdateGeoChron()
        {
            textBlock_GeoChronName.Text = _Vertical ? string.Join(Environment.NewLine, _GeoChronName.ToCharArray()) : _GeoChronName;
        }

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            border_GeoChronName.Background = Theme.GetSolidColorBrush(_IsChecked || _MouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 12.5 : 87.5));
            border_GeoChronName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _MouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
            path_IndirectlyChecked.Fill = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _MouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60).ToWpfColor() : Colors.Transparent);
            textBlock_GeoChronName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _MouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
        }

        //

        public GeoChronButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateColor();
                _UpdateGeoChron();
            };

            this.MouseEnter += (s, e) =>
            {
                _MouseOver = true;
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _MouseOver = false;
                _UpdateColor();
            };
        }

        //

        public string GeoChronName
        {
            get => _GeoChronName;

            set
            {
                _GeoChronName = value;

                _UpdateGeoChron();
            }
        }

        public GeoChron GeoChron
        {
            get => _GeoChron;

            set
            {
                _GeoChron = value;

                if (_GeoChron is not null)
                {
                    _GeoChronName = _GeoChron.GetChineseName();

                    _UpdateGeoChron();
                }
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

        public bool IsIndirectlyChecked
        {
            get => _IsIndirectlyChecked;

            set
            {
                _IsIndirectlyChecked = value;

                _UpdateIsIndirectlyChecked();
                _UpdateColor();
            }
        }

        public bool Vertical
        {
            get => _Vertical;

            set
            {
                _Vertical = value;

                _UpdateGeoChron();
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