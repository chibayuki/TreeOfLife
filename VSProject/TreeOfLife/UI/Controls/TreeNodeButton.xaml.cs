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

        private ExSymbol exSymbol = null;
        private ParaphylySymbol paraphylySymbol = null;
        private PolyphylySymbol polyphylySymbol = null;
        private RefSymbol refSymbol = null;

        private SingleLine singleLine = null;
        private UndetSingleLine undetSingleLine = null;
        private FirstLine firstLine = null;
        private UndetFirstLine undetFirstLine = null;
        private LastLine lastLine = null;
        private UndetLastLine undetLastLine = null;
        private NormalLine normalLine = null;
        private UndetNormalLine undetNormalLine = null;

        private SingleLine rightLine = null;

        private enum _EntranceTypes
        {
            None = -1,
            Single,
            First,
            Last,
            Normal
        }

        private _EntranceTypes _EntranceType = _EntranceTypes.None;

        private void _UpdateEntranceLineVisibility()
        {
            bool isUndet = _Taxon.IsNamed && (_IsRef || _Taxon.IsUndet);
            bool isSingle = _EntranceType == _EntranceTypes.Single;
            bool isFirst = _EntranceType == _EntranceTypes.First;
            bool isLast = _EntranceType == _EntranceTypes.Last;
            bool isNormal = _EntranceType == _EntranceTypes.Normal;

            if (!isUndet && isSingle)
            {
                if (singleLine is null)
                {
                    singleLine = new SingleLine();
                    grid_LeftPart.Children.Add(singleLine);
                }
                else
                {
                    singleLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (singleLine is not null)
                {
                    singleLine.Visibility = Visibility.Collapsed;
                }
            }

            if (isUndet && isSingle)
            {
                if (undetSingleLine is null)
                {
                    undetSingleLine = new UndetSingleLine();
                    grid_LeftPart.Children.Add(undetSingleLine);
                }
                else
                {
                    undetSingleLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (undetSingleLine is not null)
                {
                    undetSingleLine.Visibility = Visibility.Collapsed;
                }
            }

            if (!isUndet && isFirst)
            {
                if (firstLine is null)
                {
                    firstLine = new FirstLine();
                    grid_LeftPart.Children.Add(firstLine);
                }
                else
                {
                    firstLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (firstLine is not null)
                {
                    firstLine.Visibility = Visibility.Collapsed;
                }
            }

            if (isUndet && isFirst)
            {
                if (undetFirstLine is null)
                {
                    undetFirstLine = new UndetFirstLine();
                    grid_LeftPart.Children.Add(undetFirstLine);
                }
                else
                {
                    undetFirstLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (undetFirstLine is not null)
                {
                    undetFirstLine.Visibility = Visibility.Collapsed;
                }
            }

            if (!isUndet && isLast)
            {
                if (lastLine is null)
                {
                    lastLine = new LastLine();
                    grid_LeftPart.Children.Add(lastLine);
                }
                else
                {
                    lastLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (lastLine is not null)
                {
                    lastLine.Visibility = Visibility.Collapsed;
                }
            }

            if (isUndet && isLast)
            {
                if (undetLastLine is null)
                {
                    undetLastLine = new UndetLastLine();
                    grid_LeftPart.Children.Add(undetLastLine);
                }
                else
                {
                    undetLastLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (undetLastLine is not null)
                {
                    undetLastLine.Visibility = Visibility.Collapsed;
                }
            }

            if (!isUndet && isNormal)
            {
                if (normalLine is null)
                {
                    normalLine = new NormalLine();
                    grid_LeftPart.Children.Add(normalLine);
                }
                else
                {
                    normalLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (normalLine is not null)
                {
                    normalLine.Visibility = Visibility.Collapsed;
                }
            }

            if (isUndet && isNormal)
            {
                if (undetNormalLine is null)
                {
                    undetNormalLine = new UndetNormalLine();
                    grid_LeftPart.Children.Add(undetNormalLine);
                }
                else
                {
                    undetNormalLine.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (undetNormalLine is not null)
                {
                    undetNormalLine.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void _UpdateEntranceType(_EntranceTypes entranceType)
        {
            _EntranceType = entranceType;

            _UpdateEntranceLineVisibility();
        }

        private void _UpdateIsUndet()
        {
            _UpdateEntranceLineVisibility();

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

                if (_Taxon.IsNamed && _Taxon.IsExtinct)
                {
                    if (exSymbol is null)
                    {
                        exSymbol = new ExSymbol() { Margin = new Thickness(0, 0, 3, 0) };
                        panel_State.Children.Add(exSymbol);
                    }
                    else
                    {
                        exSymbol.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (exSymbol is not null)
                    {
                        exSymbol.Visibility = Visibility.Collapsed;
                    }
                }

                if (_Taxon.IsNamed && _Taxon.IsParaphyly)
                {
                    if (paraphylySymbol is null)
                    {
                        paraphylySymbol = new ParaphylySymbol()
                        {
                            Margin = new Thickness(3, 0, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        grid_PolyOrPara.Children.Add(paraphylySymbol);
                    }
                    else
                    {
                        paraphylySymbol.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (paraphylySymbol is not null)
                    {
                        paraphylySymbol.Visibility = Visibility.Collapsed;
                    }
                }

                if (_Taxon.IsNamed && _Taxon.IsPolyphyly)
                {
                    if (polyphylySymbol is null)
                    {
                        polyphylySymbol = new PolyphylySymbol()
                        {
                            Margin = new Thickness(3, 0, 0, 0),
                            VerticalAlignment = VerticalAlignment.Top
                        };
                        grid_PolyOrPara.Children.Add(polyphylySymbol);
                    }
                    else
                    {
                        polyphylySymbol.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (polyphylySymbol is not null)
                    {
                        polyphylySymbol.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void _UpdateIsRef()
        {
            _UpdateIsUndet();

            if (_IsRef)
            {
                if (refSymbol is null)
                {
                    refSymbol = new RefSymbol() { Margin = new Thickness(6, 0, 0, 0) };
                    grid_Ref.Children.Add(refSymbol);
                }
                else
                {
                    refSymbol.Visibility = Visibility.Visible;
                }
            }
            else
            {
                if (refSymbol is not null)
                {
                    refSymbol.Visibility = Visibility.Collapsed;
                }
            }
        }

        private bool _IsRoot = false;
        private bool _IsFinal = false;
        private bool _IsFirst = false;
        private bool _IsLast = false;
        private bool _ShowButton = false;

        private void _UpdateAttributes()
        {
            if (_IsRoot)
            {
                if (_ShowButton)
                {
                    _UpdateEntranceType(_EntranceTypes.Single);

                    grid_LeftPart.Visibility = Visibility.Visible;
                }
                else
                {
                    grid_LeftPart.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                if (_IsFirst && _IsLast) _UpdateEntranceType(_EntranceTypes.Single);
                else if (_IsFirst) _UpdateEntranceType(_EntranceTypes.First);
                else if (_IsLast) _UpdateEntranceType(_EntranceTypes.Last);
                else _UpdateEntranceType(_EntranceTypes.Normal);

                grid_LeftPart.Visibility = Visibility.Visible;
            }

            if ((_IsRoot && !_IsFinal) || (!_IsFinal && _ShowButton))
            {
                if (rightLine is null)
                {
                    rightLine = new SingleLine();
                    grid_RightPart.Children.Add(rightLine);
                }

                grid_RightPart.Visibility = Visibility.Visible;
            }
            else
            {
                grid_RightPart.Visibility = Visibility.Collapsed;
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

            if (exSymbol is not null)
            {
                exSymbol.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }

            if (paraphylySymbol is not null)
            {
                paraphylySymbol.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }

            if (polyphylySymbol is not null)
            {
                polyphylySymbol.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }

            if (refSymbol is not null)
            {
                refSymbol.Foreground = Theme.GetSolidColorBrush(_IsChecked || _IsMouseOver ? (_IsDarkTheme ? Colors.Black : Colors.White) : _ThemeColor.AtLightness_LAB(50).ToWpfColor());
            }
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