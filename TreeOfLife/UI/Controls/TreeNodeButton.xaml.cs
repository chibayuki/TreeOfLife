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

using System.Windows.Media.Effects;

using TreeOfLife.Core.Search.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;
using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    public partial class TreeNodeButton : UserControl
    {
        private static readonly DropShadowEffect _DropShadowEffect = new DropShadowEffect()
        {
            BlurRadius = 3,
            Direction = 315,
            Opacity = 0.3,
            ShadowDepth = 1
        };

        //

        private Taxon _Taxon = null; // 类群。
        private bool _IsRef = false; // 是否为引用，false：单系群继承关系的类群，true：并系群排除的类群或复系群包含的类群。

        private void _UpdateIsUndet()
        {
            if (!_IsRef && _Taxon.IsUndet)
            {
                grid_Single_Det.Visibility = grid_First_Det.Visibility = grid_Last_Det.Visibility = grid_Normal_Det.Visibility = Visibility.Collapsed;
                grid_Single_Undet.Visibility = grid_First_Undet.Visibility = grid_Last_Undet.Visibility = grid_Normal_Undet.Visibility = Visibility.Visible;
            }
            else
            {
                grid_Single_Det.Visibility = grid_First_Det.Visibility = grid_Last_Det.Visibility = grid_Normal_Det.Visibility = Visibility.Visible;
                grid_Single_Undet.Visibility = grid_First_Undet.Visibility = grid_Last_Undet.Visibility = grid_Normal_Undet.Visibility = Visibility.Collapsed;
            }

            grid_Undet.Visibility = !_IsRef && _Taxon.IsNamed && _Taxon.IsUndet ? Visibility.Visible : Visibility.Collapsed;
        }

        private void _UpdateTaxon()
        {
            if (_Taxon is not null)
            {
                textBlock_TaxonName.Text = _Taxon.GetLongName();

                Rank rank = _Taxon.Rank;

                bool basicPrimary = rank.IsBasicPrimaryRank();
                bool bellowGenus = false;

                if (rank.IsPrimaryOrSecondaryRank())
                {
                    Rank inheritedPrimaryRank = _Taxon.GetInheritedPrimaryRank();

                    bellowGenus = inheritedPrimaryRank.IsPrimaryRank() && inheritedPrimaryRank <= Rank.Genus;
                }

                textBlock_TaxonName.FontStyle = bellowGenus ? FontStyles.Italic : FontStyles.Normal;
                textBlock_TaxonName.FontWeight = basicPrimary ? FontWeights.Bold : FontWeights.Normal;

                //

                _UpdateIsUndet();

                grid_Ex.Visibility = _Taxon.IsNamed && _Taxon.IsExtinct ? Visibility.Visible : Visibility.Collapsed;
                grid_Paraphyly.Visibility = _Taxon.IsNamed && _Taxon.IsParaphyly ? Visibility.Visible : Visibility.Collapsed;
                grid_Polyphyly.Visibility = _Taxon.IsNamed && _Taxon.IsPolyphyly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _UpdateIsRef()
        {
            _UpdateIsUndet();

            grid_Ref.Visibility = _IsRef ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool _IsRoot = false;
        private bool _IsFinal = false;
        private bool _IsFirst = false;
        private bool _IsLast = false;
        private bool _ShowButton = false;

        private enum _NodeEntranceType
        {
            Single,
            First,
            Last,
            Normal
        }

        private void _SetEntranceType(_NodeEntranceType entranceType)
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

        private void _UpdateAttributes()
        {
            if (_IsRoot)
            {
                if (_ShowButton)
                {
                    _SetEntranceType(_NodeEntranceType.Single);

                    grid_LeftPart.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_LeftPart.Visibility = Visibility.Collapsed;
                }

                grid_RightPart.Visibility = !_IsFinal ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                if (_IsFirst && _IsLast) _SetEntranceType(_NodeEntranceType.Single);
                else if (_IsFirst) _SetEntranceType(_NodeEntranceType.First);
                else if (_IsLast) _SetEntranceType(_NodeEntranceType.Last);
                else _SetEntranceType(_NodeEntranceType.Normal);

                grid_LeftPart.Visibility = Visibility.Visible;
                grid_RightPart.Visibility = !_IsFinal && _ShowButton ? Visibility.Visible : Visibility.Collapsed;
            }

            if (_ShowButton && _Taxon is not null)
            {
                grid_MiddlePart.Visibility = Visibility.Visible;
            }
            else
            {
                grid_MiddlePart.Visibility = Visibility.Collapsed;
            }
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private void _UpdateEffect() => border_Background.Effect = !_IsChecked && _IsMouseOver ? _DropShadowEffect : null;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            border_Background.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 5 : 95));
            border_Background.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
            textBlock_TaxonName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            textBlock_Undet.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            border_Ex_Part1.BorderBrush = border_Ex_Part2.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Paraphyly_Part1.Stroke = path_Paraphyly_Part2.Stroke = path_Paraphyly_Part3.Stroke = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Polyphyly.Stroke = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Ref_Part1.Stroke = path_Ref_Part2.Fill = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
        }

        //

        public TreeNodeButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateIsRef();
                _UpdateAttributes();
                _UpdateColor();
            };

            grid_MiddlePart.MouseEnter += (s, e) =>
            {
                _IsMouseOver = true;
                _UpdateEffect();
                _UpdateColor();
            };

            grid_MiddlePart.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
                _UpdateEffect();
                _UpdateColor();
            };
        }

        //

        internal bool VerifyMousePosition()
        {
            Point p = Mouse.GetPosition(grid_MiddlePart);

            return p.X >= 0 && p.X < grid_MiddlePart.ActualWidth && p.Y >= 0 && p.Y < grid_MiddlePart.ActualHeight;
        }

        //

        public Taxon Taxon
        {
            get => _Taxon;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                //

                _Taxon = value;
                _ThemeColor = _Taxon.GetThemeColor();

                _UpdateTaxon();
                _UpdateColor();
            }
        }

        public bool IsRef
        {
            get => _IsRef;

            set
            {
                _IsRef = value;

                _UpdateIsRef();
            }
        }

        public bool IsRoot
        {
            get => _IsRoot;

            set
            {
                _IsRoot = value;

                _UpdateAttributes();
            }
        }

        public bool IsFinal
        {
            get => _IsFinal;

            set
            {
                _IsFinal = value;

                _UpdateAttributes();
            }
        }

        public bool IsFirst
        {
            get => _IsFirst;

            set
            {
                _IsFirst = value;

                _UpdateAttributes();
            }
        }

        public bool IsLast
        {
            get => _IsLast;

            set
            {
                _IsLast = value;

                _UpdateAttributes();
            }
        }

        public bool ShowButton
        {
            get => _ShowButton;

            set
            {
                _ShowButton = value;

                _UpdateAttributes();
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

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColor();
            }
        }

        //

        public void SyncTaxonUpdation()
        {
            _ThemeColor = _Taxon.GetThemeColor();

            _UpdateTaxon();
            _UpdateColor();
        }
    }
}