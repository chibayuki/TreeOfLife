/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1000.1000.M10.210130-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows;
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

        #region 页面切换

        public enum Pages
        {
            File,
            Evo,
            Search,
            About
        }

        private Pages? _CurrentPage = null;

        private Visibility _PageVisibility_File;
        private Visibility _PageVisibility_Evo;
        private Visibility _PageVisibility_Search;
        private Visibility _PageVisibility_About;

        // 更新可见性。
        private void _UpdateVisibility()
        {
            PageVisibility_File = (_CurrentPage.Value == Pages.File ? Visibility.Visible : Visibility.Collapsed);
            PageVisibility_Evo = (_CurrentPage.Value == Pages.Evo ? Visibility.Visible : Visibility.Collapsed);
            PageVisibility_Search = (_CurrentPage.Value == Pages.Search ? Visibility.Visible : Visibility.Collapsed);
            PageVisibility_About = (_CurrentPage.Value == Pages.About ? Visibility.Visible : Visibility.Collapsed);
        }

        public Pages? CurrentPage
        {
            get => _CurrentPage;

            set
            {
                _CurrentPage = value;

                _UpdateVisibility();
            }
        }

        public Visibility PageVisibility_File
        {
            get => _PageVisibility_File;

            set
            {
                _PageVisibility_File = value;

                NotifyPropertyChanged(nameof(PageVisibility_File));
            }
        }

        public Visibility PageVisibility_Evo
        {
            get => _PageVisibility_Evo;

            set
            {
                _PageVisibility_Evo = value;

                NotifyPropertyChanged(nameof(PageVisibility_Evo));
            }
        }

        public Visibility PageVisibility_Search
        {
            get => _PageVisibility_Search;

            set
            {
                _PageVisibility_Search = value;

                NotifyPropertyChanged(nameof(PageVisibility_Search));
            }
        }

        public Visibility PageVisibility_About
        {
            get => _PageVisibility_About;

            set
            {
                _PageVisibility_About = value;

                NotifyPropertyChanged(nameof(PageVisibility_About));
            }
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _Pages_BackGround;
        private Brush _PagesSide_ForeGround;
        private Brush _PagesSide_BackGround;

        private void _UpdateColors()
        {
            Pages_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(8, 8, 8) : Color.FromRgb(248, 248, 248));
            PagesSide_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(160, 160, 160) : Color.FromRgb(64, 64, 64));
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
                _Pages_BackGround = value;

                NotifyPropertyChanged(nameof(Pages_BackGround));
            }
        }

        public Brush PagesSide_ForeGround
        {
            get => _PagesSide_ForeGround;

            set
            {
                _PagesSide_ForeGround = value;

                NotifyPropertyChanged(nameof(PagesSide_ForeGround));
            }
        }

        public Brush PagesSide_BackGround
        {
            get => _PagesSide_BackGround;

            set
            {
                _PagesSide_BackGround = value;

                NotifyPropertyChanged(nameof(PagesSide_BackGround));
            }
        }

        #endregion
    }
}