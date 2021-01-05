/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.800.1000.M8.201231-0000

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

        private string _CategoryName;
        private string _TaxonName;

        private string[] _Synonyms;
        private string[] _Tags;

        private string _Desc;

        public string CategoryName
        {
            get => _CategoryName;

            set
            {
                _CategoryName = value;

                NotifyPropertyChanged(nameof(CategoryName));
            }
        }

        public string TaxonName
        {
            get => _TaxonName;

            set
            {
                _TaxonName = value;

                NotifyPropertyChanged(nameof(TaxonName));
            }
        }

        public string[] Synonyms => _Synonyms;

        public string[] Tags => _Tags;

        public string Desc
        {
            get => _Desc;

            set
            {
                _Desc = value;

                NotifyPropertyChanged(nameof(Desc));
            }
        }

        public void UpdateFromTaxon()
        {
            Taxon currentTaxon = Views.Common.CurrentTaxon;

            TaxonColor = currentTaxon.GetThemeColor();

            CategoryName = (currentTaxon.IsAnonymous() ? string.Empty : currentTaxon.Category.GetChineseName());
            TaxonName = currentTaxon.GetShortName('\n');

            _Tags = currentTaxon.Tags.ToArray();
            _Synonyms = currentTaxon.Synonyms.ToArray();

            Desc = currentTaxon.Description;
        }

        #endregion

        #region 主题

        private ColorX _TaxonColor;

        private bool _IsDarkTheme;

        private Brush _Button_ForeGround;
        private Brush _Button_BackGround;
        private Brush _CategoryName_ForeGround;
        private Brush _CategoryName_BackGround;
        private Brush _TaxonName_ForeGround;
        private Brush _TaxonName_BackGround;
        private Brush _SubTitle_ForeGround;
        private Brush _SubTitle_BackGround;
        private Brush _Desc_BackGround;

        private void _UpdateColors()
        {
            Button_ForeGround = Views.Common.Button_ForeGround;
            Button_BackGround = Views.Common.Button_BackGround;
            CategoryName_ForeGround = (_IsDarkTheme ? Brushes.Black : Brushes.White);
            CategoryName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToWpfColor());
            TaxonName_ForeGround = new SolidColorBrush(_TaxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToWpfColor());
            TaxonName_BackGround = new SolidColorBrush(_TaxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToWpfColor());
            SubTitle_ForeGround = Views.Common.SubTitle_ForeGround;
            SubTitle_BackGround = Views.Common.SubTitle_BackGround;
            Desc_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
        }

        public ColorX TaxonColor
        {
            get => _TaxonColor;

            set
            {
                _TaxonColor = value;

                _UpdateColors();
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

        public Brush Button_ForeGround
        {
            get => _Button_ForeGround;

            set
            {
                _Button_ForeGround = value;

                NotifyPropertyChanged(nameof(Button_ForeGround));
            }
        }

        public Brush Button_BackGround
        {
            get => _Button_BackGround;

            set
            {
                _Button_BackGround = value;

                NotifyPropertyChanged(nameof(Button_BackGround));
            }
        }

        public Brush CategoryName_ForeGround
        {
            get => _CategoryName_ForeGround;

            set
            {
                _CategoryName_ForeGround = value;

                NotifyPropertyChanged(nameof(CategoryName_ForeGround));
            }
        }

        public Brush CategoryName_BackGround
        {
            get => _CategoryName_BackGround;

            set
            {
                _CategoryName_BackGround = value;

                NotifyPropertyChanged(nameof(CategoryName_BackGround));
            }
        }

        public Brush TaxonName_ForeGround
        {
            get => _TaxonName_ForeGround;

            set
            {
                _TaxonName_ForeGround = value;

                NotifyPropertyChanged(nameof(TaxonName_ForeGround));
            }
        }

        public Brush TaxonName_BackGround
        {
            get => _TaxonName_BackGround;

            set
            {
                _TaxonName_BackGround = value;

                NotifyPropertyChanged(nameof(TaxonName_BackGround));
            }
        }

        public Brush SubTitle_ForeGround
        {
            get => _SubTitle_ForeGround;

            set
            {
                _SubTitle_ForeGround = value;

                NotifyPropertyChanged(nameof(SubTitle_ForeGround));
            }
        }

        public Brush SubTitle_BackGround
        {
            get => _SubTitle_BackGround;

            set
            {
                _SubTitle_BackGround = value;

                NotifyPropertyChanged(nameof(SubTitle_BackGround));
            }
        }

        public Brush Desc_BackGround
        {
            get => _Desc_BackGround;

            set
            {
                _Desc_BackGround = value;

                NotifyPropertyChanged(nameof(Desc_BackGround));
            }
        }

        #endregion
    }
}