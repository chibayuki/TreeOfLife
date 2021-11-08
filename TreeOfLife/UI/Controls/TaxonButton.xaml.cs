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
    public partial class TaxonButton : UserControl
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

        private void _UpdateTaxon()
        {
            if (_Taxon is not null)
            {
                Rank rank = _Taxon.Rank;

                if (_Taxon.IsAnonymous)
                {
                    textBlock_RankName.Text = string.Empty;
                }
                else
                {
                    if (rank.IsUnranked() || rank.IsClade())
                    {
                        if (_Taxon.IsParaphyly)
                        {
                            textBlock_RankName.Text = "并系群";
                        }
                        else if (_Taxon.IsPolyphyly)
                        {
                            textBlock_RankName.Text = "复系群";
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
                    else
                    {
                        textBlock_RankName.Text = rank.GetChineseName();
                    }
                }

                textBlock_TaxonName.Text = _Taxon.GetShortName();

                bool basicPrimary = rank.IsBasicPrimaryRank();
                bool bellowGenus = false;

                if (rank.IsPrimaryOrSecondaryRank())
                {
                    Rank inheritedPrimaryRank = _Taxon.GetInheritedPrimaryRank();

                    bellowGenus = inheritedPrimaryRank.IsPrimaryRank() && inheritedPrimaryRank <= Rank.Genus;
                }

                textBlock_RankName.FontStyle = textBlock_TaxonName.FontStyle = bellowGenus ? FontStyles.Italic : FontStyles.Normal;
                textBlock_RankName.FontWeight = textBlock_TaxonName.FontWeight = basicPrimary ? FontWeights.Bold : FontWeights.Normal;

                //

                grid_Undet.Visibility = _Taxon.IsUnsure ? Visibility.Visible : Visibility.Collapsed;
                grid_Ex.Visibility = _Taxon.IsExtinct ? Visibility.Visible : Visibility.Collapsed;
                grid_Paraphyly.Visibility = _Taxon.IsParaphyly ? Visibility.Visible : Visibility.Collapsed;
                grid_Polyphyly.Visibility = _Taxon.IsPolyphyly ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void _UpdateIsRef() => grid_Ref.Visibility = _IsRef ? Visibility.Visible : Visibility.Collapsed;

        private double _RankNameWidth = 50; // 分类阶元名称宽度。

        private void _UpdateRankNameWidth() => border_RankName.Width = _RankNameWidth;

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private void _UpdateEffect() => this.Effect = !_IsChecked && _IsMouseOver ? _DropShadowEffect : null;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            border_TaxonName.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 5 : 95));
            border_TaxonName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
            textBlock_TaxonName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            border_RankName.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 12.5 : 87.5));
            border_RankName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 40 : 60) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 20 : 80));
            textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            textBlock_Undet.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            border_Ex_Part1.BorderBrush = border_Ex_Part2.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Paraphyly_Part1.Stroke = path_Paraphyly_Part2.Stroke = path_Paraphyly_Part3.Stroke = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Polyphyly.Stroke = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            path_Ref_Part1.Stroke = path_Ref_Part2.Fill = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
        }

        //

        public TaxonButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateIsRef();
                _UpdateRankNameWidth();
                _UpdateColor();
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

        public double RankNameWidth
        {
            get => _RankNameWidth;

            set
            {
                _RankNameWidth = value;

                _UpdateRankNameWidth();
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
    }
}