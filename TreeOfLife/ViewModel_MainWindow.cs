/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

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

using TreeOfLife.Extensions;

using ColorX = Com.Chromatics.ColorX;

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

        private bool _PageButtonEnabled_File;
        private bool _PageButtonEnabled_Evo;
        private bool _PageButtonEnabled_Search;
        private bool _PageButtonEnabled_About;

        private Visibility _PageVisibility_File;
        private Visibility _PageVisibility_Evo;
        private Visibility _PageVisibility_Search;
        private Visibility _PageVisibility_About;

        // 更新可见性。
        private void _UpdateVisibility()
        {
            PageButtonEnabled_File = !(_CurrentPage.Value == Pages.File);
            PageButtonEnabled_Evo = !(_CurrentPage.Value == Pages.Evo);
            PageButtonEnabled_Search = !(_CurrentPage.Value == Pages.Search);
            PageButtonEnabled_About = !(_CurrentPage.Value == Pages.About);

            PageVisibility_File = (PageButtonEnabled_File ? Visibility.Collapsed : Visibility.Visible);
            PageVisibility_Evo = (PageButtonEnabled_Evo ? Visibility.Collapsed : Visibility.Visible);
            PageVisibility_Search = (PageButtonEnabled_Search ? Visibility.Collapsed : Visibility.Visible);
            PageVisibility_About = (PageButtonEnabled_About ? Visibility.Collapsed : Visibility.Visible);
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

        public bool PageButtonEnabled_File
        {
            get => _PageButtonEnabled_File;

            set
            {
                _PageButtonEnabled_File = value;

                NotifyPropertyChanged(nameof(PageButtonEnabled_File));
            }
        }

        public bool PageButtonEnabled_Evo
        {
            get => _PageButtonEnabled_Evo;

            set
            {
                _PageButtonEnabled_Evo = value;

                NotifyPropertyChanged(nameof(PageButtonEnabled_Evo));
            }
        }

        public bool PageButtonEnabled_Search
        {
            get => _PageButtonEnabled_Search;

            set
            {
                _PageButtonEnabled_Search = value;

                NotifyPropertyChanged(nameof(PageButtonEnabled_Search));
            }
        }

        public bool PageButtonEnabled_About
        {
            get => _PageButtonEnabled_About;

            set
            {
                _PageButtonEnabled_About = value;

                NotifyPropertyChanged(nameof(PageButtonEnabled_About));
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