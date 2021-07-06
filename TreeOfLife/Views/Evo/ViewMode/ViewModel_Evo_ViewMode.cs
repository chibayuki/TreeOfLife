/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1134.1000.M11.210518-2200

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

using TreeOfLife.Geology;
using TreeOfLife.Geology.Extensions;
using TreeOfLife.Taxonomy;
using TreeOfLife.Taxonomy.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Views.Evo.ViewMode
{
    public sealed class ViewModel_Evo_ViewMode : INotifyPropertyChanged
    {
        public ViewModel_Evo_ViewMode()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 类群信息

        private string _Birth_Prefix;
        private string _Birth;
        private string _Extinction_Prefix;
        private string _Extinction;

        private string _Desc;

        public string Birth_Prefix
        {
            get => _Birth_Prefix;

            set
            {
                _Birth_Prefix = value;

                NotifyPropertyChanged(nameof(Birth_Prefix));
            }
        }

        public string Birth
        {
            get => _Birth;

            set
            {
                _Birth = value;

                NotifyPropertyChanged(nameof(Birth));
            }
        }

        public string Extinction_Prefix
        {
            get => _Extinction_Prefix;

            set
            {
                _Extinction_Prefix = value;

                NotifyPropertyChanged(nameof(Extinction_Prefix));
            }
        }

        public string Extinction
        {
            get => _Extinction;

            set
            {
                _Extinction = value;

                NotifyPropertyChanged(nameof(Extinction));
            }
        }

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

            if (currentTaxon.Birth.IsEmpty)
            {
                Birth_Prefix = string.Empty;
                Birth = "?";
            }
            else
            {
                GeoChron birth = currentTaxon.Birth;

                if (birth.IsTimepoint)
                {
                    if (birth.Superior is null)
                    {
                        Birth = birth.GetChineseName();
                    }
                    else
                    {
                        Birth = $"{birth.GetChineseName()} ({birth.Superior.GetChineseName()})";

                        birth = birth.Superior;
                    }
                }
                else
                {
                    Birth = birth.GetChineseName();
                }

                if (birth.Superior is null)
                {
                    Birth_Prefix = string.Empty;
                }
                else
                {
                    GeoChron geoChron = birth.Superior;

                    string str = geoChron.GetChineseName();

                    while (geoChron.Superior is not null)
                    {
                        geoChron = geoChron.Superior;

                        str = $"{geoChron.GetChineseName()}·{str}";
                    }

                    Birth_Prefix = $"({str})";
                }
            }

            if (currentTaxon.IsExtinct)
            {
                if (currentTaxon.Extinction.IsEmpty)
                {
                    Extinction_Prefix = string.Empty;
                    Extinction = "?";
                }
                else
                {
                    GeoChron extinction = currentTaxon.Extinction;

                    if (extinction.IsTimepoint)
                    {
                        if (extinction.Superior is null)
                        {
                            Extinction = extinction.GetChineseName();
                        }
                        else
                        {
                            Extinction = $"{extinction.GetChineseName()} ({extinction.Superior.GetChineseName()})";

                            extinction = extinction.Superior;
                        }
                    }
                    else
                    {
                        Extinction = extinction.GetChineseName();
                    }

                    if (extinction.Superior is null)
                    {
                        Extinction_Prefix = string.Empty;
                    }
                    else
                    {
                        GeoChron geoChron = extinction.Superior;

                        string str = geoChron.GetChineseName();

                        while (geoChron.Superior is not null)
                        {
                            geoChron = geoChron.Superior;

                            str = $"{geoChron.GetChineseName()}·{str}";
                        }

                        Extinction_Prefix = $"({str})";
                    }
                }
            }
            else
            {
                Extinction_Prefix = string.Empty;
                Extinction = "至今";
            }

            Desc = currentTaxon.Description;
        }

        #endregion

        #region 主题

        private ColorX _TaxonColor;

        private bool _IsDarkTheme;

        private Brush _Desc_BackGround;

        private void _UpdateColors()
        {
            Desc_BackGround = Views.Common.GetSolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
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