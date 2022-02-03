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

using ColorX = Com.Chromatics.ColorX;
using Com.Chromatics.Extensions;

using TreeOfLife.Core.Geology;
using TreeOfLife.Core.Geology.Extensions;
using TreeOfLife.UI.Extensions;

namespace TreeOfLife.UI.Controls
{
    public partial class GeoChronButton : UserControl
    {
        private static readonly DropShadowEffect _DropShadowEffect = new DropShadowEffect()
        {
            BlurRadius = 3,
            Direction = 315,
            Opacity = 0.3,
            ShadowDepth = 1
        };

        //

        private GeoChron _GeoChron = null; // 地质年代。
        private string _GeoChronName = string.Empty; // 名称。

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsIndirectlyChecked = false;
        private bool _IsMouseOver = false;
        private bool _IsVertical = false;

        private void _UpdateIsIndirectlyChecked() => path_IndirectlyChecked.Visibility = _IsIndirectlyChecked ? Visibility.Visible : Visibility.Collapsed;

        private void _UpdateEffect() => border_Background.Effect = !_IsChecked && _IsMouseOver ? _DropShadowEffect : null;

        private void _UpdateGeoChron()
        {
            textBlock_GeoChronName.Text = _IsVertical ? string.Join(Environment.NewLine, _GeoChronName.ToCharArray()) : _GeoChronName;
        }

        private ColorX _ThemeColor = ColorX.FromRgb(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            border_Background.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLabLightness(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtHslLightness(_IsDarkTheme ? 12.5 : 87.5));
            border_Background.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _IsMouseOver ? _ThemeColor.AtLabLightness(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtHslLightness(_IsDarkTheme ? 20 : 80));
            path_IndirectlyChecked.Fill = Theme.GetSolidColorBrush(_IsChecked || _IsIndirectlyChecked || _IsMouseOver ? _ThemeColor.AtLabLightness(_IsDarkTheme ? 40 : 60).ToWpfColor() : Colors.Transparent);
            textBlock_GeoChronName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLabLightness(50).ToWpfColor());
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

        public bool IsVertical
        {
            get => _IsVertical;

            set
            {
                _IsVertical = value;

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