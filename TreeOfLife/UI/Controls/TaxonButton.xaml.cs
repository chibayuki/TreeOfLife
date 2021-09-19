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

using TreeOfLife.Core.Extensions;
using TreeOfLife.Core.Taxonomy;
using TreeOfLife.Core.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Controls
{
    /// <summary>
    /// TaxonButton.xaml 的交互逻辑
    /// </summary>
    public partial class TaxonButton : UserControl
    {
        private Taxon _Taxon = null; // 类群。
        private int _Sign = 0; // "符号"，0：单系群继承关系的类群，-1：并系群排除的类群，+1：复系群包含的类群。

        private void _UpdateTaxon()
        {
            Rank rank = _Taxon.Rank;

            if (_Taxon is null || _Taxon.IsAnonymous)
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

            textBlock_TaxonName.Text = _Taxon is null ? string.Empty : _Taxon.GetShortName();

            bool basicPrimary = rank.IsBasicPrimaryRank();
            bool bellowGenus = false;

            if (rank.IsPrimaryOrSecondaryRank())
            {
                Rank inheritedPrimaryRank = _Taxon.GetInheritedPrimaryRank();

                bellowGenus = inheritedPrimaryRank.IsPrimaryRank() && inheritedPrimaryRank <= Rank.Genus;
            }

            textBlock_RankName.FontStyle = textBlock_TaxonName.FontStyle = bellowGenus ? FontStyles.Italic : FontStyles.Normal;
            textBlock_RankName.FontWeight = textBlock_TaxonName.FontWeight = basicPrimary ? FontWeights.Bold : FontWeights.Normal;
        }

        private void _UpdateSign()
        {
            grid_Positive.Visibility = _Sign > 0 ? Visibility.Visible : Visibility.Collapsed;
            grid_Negative.Visibility = _Sign < 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private double _RankNameWidth = 50; // 分类阶元名称宽度。

        private void _UpdateRankNameWidth()
        {
            border_RankName.Width = _RankNameWidth;
        }

        private bool _IsChecked = false; // 是否处于已选择状态。
        private bool _IsMouseOver = false;

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _IsDarkTheme = false; // 是否为暗色主题。

        private void _UpdateColor()
        {
            textBlock_RankName.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());

            border_RankName.Background = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));

            textBlock_TaxonName.Foreground = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_LAB(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? 60 : 40) : 50));

            border_TaxonName.Background = Theme.GetSolidColorBrush(_ThemeColor.AtLightness_HSL(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? 10 : 90) : (_IsDarkTheme ? 3 : 97)));
            border_TaxonName.BorderBrush = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? _ThemeColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90));
        }

        //

        public TaxonButton()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) =>
            {
                _UpdateTaxon();
                _UpdateSign();
                _UpdateRankNameWidth();
                _UpdateColor();
            };

            this.MouseEnter += (s, e) =>
            {
                _IsMouseOver = true;
                _UpdateColor();
            };

            this.MouseLeave += (s, e) =>
            {
                _IsMouseOver = false;
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

        public int Sign
        {
            get => _Sign;

            set
            {
                _Sign = value;

                _UpdateSign();
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