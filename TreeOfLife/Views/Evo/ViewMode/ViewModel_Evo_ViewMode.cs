﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.608.1000.M6.201219-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

using TreeOfLife.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Views.Evo.ViewMode
{
    public class ViewModel_Evo_ViewMode : INotifyPropertyChanged
    {
        public ViewModel_Evo_ViewMode()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 类群信息

        private Taxon _Taxon;

        private string _CategoryName;
        private string _TaxonName;

        private string[] _Synonyms;
        private string[] _Tags;

        private string _Desc;

        public Taxon Taxon => _Taxon;

        public string CategoryName
        {
            get => _CategoryName;

            set
            {
                if (_CategoryName != value)
                {
                    _CategoryName = value;

                    NotifyPropertyChanged(nameof(CategoryName));
                }
            }
        }

        public string TaxonName
        {
            get => _TaxonName;

            set
            {
                if (_TaxonName != value)
                {
                    _TaxonName = value;

                    NotifyPropertyChanged(nameof(TaxonName));
                }
            }
        }

        public string[] Synonyms => _Synonyms;

        public string[] Tags => _Tags;

        public string Desc
        {
            get => _Desc;

            set
            {
                if (_Desc != value)
                {
                    _Desc = value;

                    NotifyPropertyChanged(nameof(Desc));
                }
            }
        }

        public void UpdateFromTaxon(Taxon taxon)
        {
            _Taxon = taxon;

            TaxonColor = _Taxon.GetThemeColor();

            CategoryName = (_Taxon.IsRoot || _Taxon.IsAnonymous() ? string.Empty : _Taxon.Category.Name());
            TaxonName = (_Taxon.IsRoot ? string.Empty : _Taxon.ShortName('\n'));

            _Tags = _Taxon.Tags.ToArray();
            _Synonyms = _Taxon.Synonyms.ToArray();

            Desc = _Taxon.Description;
        }

        #endregion

        #region 主题

        private ColorX _TaxonColor;

        private bool _IsDarkTheme;

        private Brush _CategoryName_ForeGround;
        private Brush _CategoryName_BackGround;
        private Brush _TaxonName_ForeGround;
        private Brush _TaxonName_BackGround;
        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _Desc_BackGround;

        private void _UpdateColors()
        {
            CategoryName_ForeGround = new SolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);
            CategoryName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToWpfColor());
            TaxonName_ForeGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToWpfColor());
            TaxonName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToWpfColor());
            SubTitle_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(208, 208, 208) : Color.FromRgb(48, 48, 48));
            SubTitle_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(48, 48, 48) : Color.FromRgb(208, 208, 208));
            Desc_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
        }

        public ColorX TaxonColor
        {
            get => _TaxonColor;

            set
            {
                if (_TaxonColor != value)
                {
                    _TaxonColor = value;

                    _UpdateColors();
                }
            }
        }

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColors();
            }
        }

        public Brush CategoryName_ForeGround
        {
            get => _CategoryName_ForeGround;

            set
            {
                if (_CategoryName_ForeGround != value)
                {
                    _CategoryName_ForeGround = value;

                    NotifyPropertyChanged(nameof(CategoryName_ForeGround));
                }
            }
        }

        public Brush CategoryName_BackGround
        {
            get => _CategoryName_BackGround;

            set
            {
                if (_CategoryName_BackGround != value)
                {
                    _CategoryName_BackGround = value;

                    NotifyPropertyChanged(nameof(CategoryName_BackGround));
                }
            }
        }

        public Brush TaxonName_ForeGround
        {
            get => _TaxonName_ForeGround;

            set
            {
                if (_TaxonName_ForeGround != value)
                {
                    _TaxonName_ForeGround = value;

                    NotifyPropertyChanged(nameof(TaxonName_ForeGround));
                }
            }
        }

        public Brush TaxonName_BackGround
        {
            get => _TaxonName_BackGround;

            set
            {
                if (_TaxonName_BackGround != value)
                {
                    _TaxonName_BackGround = value;

                    NotifyPropertyChanged(nameof(TaxonName_BackGround));
                }
            }
        }

        public Brush SubTitle_ForeGround
        {
            get => _SubTitle_ForeGround;

            set
            {
                if (_SubTitle_ForeGround != value)
                {
                    _SubTitle_ForeGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_ForeGround));
                }
            }
        }

        public Brush SubTitle_BackGround
        {
            get => _SubTitle_BackGround;

            set
            {
                if (_SubTitle_BackGround != value)
                {
                    _SubTitle_BackGround = value;

                    NotifyPropertyChanged(nameof(SubTitle_BackGround));
                }
            }
        }

        public Brush Desc_BackGround
        {
            get => _Desc_BackGround;

            set
            {
                if (_Desc_BackGround != value)
                {
                    _Desc_BackGround = value;

                    NotifyPropertyChanged(nameof(Desc_BackGround));
                }
            }
        }

        #endregion
    }
}