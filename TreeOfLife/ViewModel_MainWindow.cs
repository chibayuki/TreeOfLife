/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

namespace TreeOfLife
{
    public class ViewModel_MainWindow : INotifyPropertyChanged
    {
        public ViewModel_MainWindow()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 主题

        private bool _IsDarkTheme;

        private Brush _Pages_BackGround;
        private Brush _PagesSide_ForeGround;
        private Brush _PagesSide_BackGround;

        private void _UpdateColors()
        {
            Pages_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(8, 8, 8) : Color.FromRgb(248, 248, 248));
            PagesSide_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
            PagesSide_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(64, 64, 64) : Color.FromRgb(192, 192, 192));
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

        public Brush Pages_BackGround
        {
            get => _Pages_BackGround;

            set
            {
                if (_Pages_BackGround != value)
                {
                    _Pages_BackGround = value;

                    NotifyPropertyChanged(nameof(Pages_BackGround));
                }
            }
        }

        public Brush PagesSide_ForeGround
        {
            get => _PagesSide_ForeGround;

            set
            {
                if (_PagesSide_ForeGround != value)
                {
                    _PagesSide_ForeGround = value;

                    NotifyPropertyChanged(nameof(PagesSide_ForeGround));
                }
            }
        }

        public Brush PagesSide_BackGround
        {
            get => _PagesSide_BackGround;

            set
            {
                if (_PagesSide_BackGround != value)
                {
                    _PagesSide_BackGround = value;

                    NotifyPropertyChanged(nameof(PagesSide_BackGround));
                }
            }
        }

        #endregion
    }
}