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
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Controls
{
    /// <summary>
    /// TreeNodeButton.xaml 的交互逻辑
    /// </summary>
    public partial class TreeNodeButton : UserControl
    {
        private Taxon _Taxon = null; // 类群。
        private int _Sign = 0; // "符号"，0：单系群继承关系的类群，-1：并系群排除的类群，+1：复系群包含的类群。

        private bool _IsRoot = false;
        private bool _IsFinal = false;
        private bool _IsFirst = false;
        private bool _IsLast = false;
        private bool _ShowButton = false;

        private bool _Checked = false; // 是否处于已选择状态。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        //

        public TreeNodeButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                label_TaxonName.FontFamily = this.FontFamily;
                label_TaxonName.FontSize = this.FontSize;
                label_TaxonName.FontStyle = this.FontStyle;
                label_TaxonName.FontWeight = this.FontWeight;

                _UpdateColor();
                _UpdateTaxon();
            };

            label_TaxonName.MouseUp += (s, e) => base.OnMouseUp(e);
            label_TaxonName.MouseLeftButtonUp += (s, e) => base.OnMouseLeftButtonUp(e);
            label_TaxonName.MouseRightButtonUp += (s, e) => base.OnMouseRightButtonUp(e);
        }

        //

        private Brush _Border => new SolidColorBrush((_Checked ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90)).ToWpfColor());

        private Brush _Foreground => new SolidColorBrush(_Checked ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

        private Brush _Background => new SolidColorBrush((_Checked ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 3 : 97)).ToWpfColor());

        private void _UpdateColor()
        {
            label_TaxonName.Foreground = _Foreground;
            label_TaxonName.Background = _Background;
            label_TaxonName.BorderBrush = _Border;
        }

        private enum _NodeEntranceType
        {
            Single,
            First,
            Last,
            Normal
        }

        private void _UpdateEntrance(_NodeEntranceType entranceType)
        {
            switch (entranceType)
            {
                case _NodeEntranceType.Single:
                    grid_Single.Visibility = Visibility.Visible;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Collapsed;
                    break;

                case _NodeEntranceType.First:
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Visible;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Collapsed;
                    break;
                case _NodeEntranceType.Last:
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Visible;
                    grid_Normal.Visibility = Visibility.Collapsed;
                    break;

                case _NodeEntranceType.Normal:
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Visible;
                    break;

                default:
                    grid_Single.Visibility = Visibility.Collapsed;
                    grid_First.Visibility = Visibility.Collapsed;
                    grid_Last.Visibility = Visibility.Collapsed;
                    grid_Normal.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void _UpdateTaxon()
        {
            if (_IsRoot)
            {
                if (_ShowButton)
                {
                    _UpdateEntrance(_NodeEntranceType.Single);

                    grid_LeftPart.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_LeftPart.Visibility = Visibility.Collapsed;
                }

                grid_RightPart.Visibility = (_IsFinal ? Visibility.Collapsed : Visibility.Visible);
            }
            else
            {
                if (_IsFirst && _IsLast)
                {
                    _UpdateEntrance(_NodeEntranceType.Single);
                }
                else if (_IsFirst)
                {
                    _UpdateEntrance(_NodeEntranceType.First);
                }
                else if (_IsLast)
                {
                    _UpdateEntrance(_NodeEntranceType.Last);
                }
                else
                {
                    _UpdateEntrance(_NodeEntranceType.Normal);
                }

                grid_LeftPart.Visibility = Visibility.Visible;
                grid_RightPart.Visibility = (_IsFinal || !_ShowButton ? Visibility.Collapsed : Visibility.Visible);
            }

            if (_ShowButton && _Taxon != null)
            {
                label_TaxonName.Content = (_Sign == 0 ? _Taxon.GetLongName() : _Taxon.GetLongName() + (_Sign < 0 ? " [-]" : " [+]"));

                grid_MiddlePart.Visibility = Visibility.Visible;
            }
            else
            {
                grid_MiddlePart.Visibility = Visibility.Collapsed;
            }
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;

                _UpdateTaxon();
            }
        }

        public int Sign
        {
            get
            {
                return _Sign;
            }

            set
            {
                _Sign = value;

                _UpdateTaxon();
            }
        }

        public bool IsRoot
        {
            get => _IsRoot;

            set
            {
                _IsRoot = value;

                _UpdateTaxon();
            }
        }

        public bool IsFinal
        {
            get => _IsFinal;

            set
            {
                _IsFinal = value;

                _UpdateTaxon();
            }
        }

        public bool IsFirst
        {
            get => _IsFirst;

            set
            {
                _IsFirst = value;

                _UpdateTaxon();
            }
        }

        public bool IsLast
        {
            get => _IsLast;

            set
            {
                _IsLast = value;

                _UpdateTaxon();
            }
        }

        public bool ShowButton
        {
            get => _ShowButton;

            set
            {
                _ShowButton = value;

                _UpdateTaxon();
            }
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }

            set
            {
                _Checked = value;

                _UpdateColor();
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
                return _IsDarkTheme;
            }

            set
            {
                _IsDarkTheme = value;

                _UpdateColor();
            }
        }
    }
}