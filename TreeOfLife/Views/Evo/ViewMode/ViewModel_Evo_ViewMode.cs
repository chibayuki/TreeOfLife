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

        private string _Desc;

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