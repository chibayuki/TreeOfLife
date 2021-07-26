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

        private void _UpdateTaxon()
        {
            if (_Taxon is not null)
            {
                textBlock_TaxonName.Text = _Taxon.GetLongName();

                TaxonomicCategory category = _Taxon.Category;

                bool basicPrimary = category.IsBasicPrimaryCategory();
                bool bellowGenus = category.IsPrimaryOrSecondaryCategory() && _Taxon.GetInheritedBasicPrimaryCategory() <= TaxonomicCategory.Genus;

                textBlock_TaxonName.FontStyle = (bellowGenus ? FontStyles.Italic : FontStyles.Normal);
                textBlock_TaxonName.FontWeight = (basicPrimary ? FontWeights.Bold : FontWeights.Normal);
            }
        }

        private void _UpdateSign()
        {
            grid_Positive.Visibility = (_Sign > 0 ? Visibility.Visible : Visibility.Collapsed);
            grid_Negative.Visibility = (_Sign < 0 ? Visibility.Visible : Visibility.Collapsed);
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

                grid_RightPart.Visibility = (!_IsFinal ? Visibility.Visible : Visibility.Collapsed);
            }
            else
            {
                if (_IsFirst && _IsLast) _SetEntranceType(_NodeEntranceType.Single);
                else if (_IsFirst) _SetEntranceType(_NodeEntranceType.First);
                else if (_IsLast) _SetEntranceType(_NodeEntranceType.Last);
                else _SetEntranceType(_NodeEntranceType.Normal);

                grid_LeftPart.Visibility = Visibility.Visible;
                grid_RightPart.Visibility = (!_IsFinal && _ShowButton ? Visibility.Visible : Visibility.Collapsed);
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

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            textBlock_TaxonName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            border_TaxonName.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 3 : 97));
            border_TaxonName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));
        }

        //

        public TreeNodeButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateSign();
                _UpdateAttributes();
                _UpdateColor();
            };

            border_TaxonName.MouseEnter += (s, e) =>
            {
                _IsMouseOver = true;
                _UpdateColor();
            };

            border_TaxonName.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
                _UpdateColor();
            };
        }

        //

        internal bool VerifyMousePosition()
        {
            Point p = Mouse.GetPosition(border_TaxonName);

            return (p.X >= 0 && p.X < border_TaxonName.ActualWidth && p.Y >= 0 && p.Y < border_TaxonName.ActualHeight);
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

        public int Sign
        {
            get => _Sign;

            set
            {
                _Sign = value;

                _UpdateSign();
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