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

using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TreeOfLife.Views.About
{
    public sealed class ViewModel_About : ViewModel
    {
        #region 关于信息

        public ImageSource AppLogo
        {
            get
            {
                BitmapImage bmp;

                try
                {
                    bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.StreamSource = new MemoryStream(Resource.AppLogo_256);
                    bmp.EndInit();
                }
                catch
                {
                    bmp = null;
                }

                return bmp;
            }
        }

        public string AppVersion => "版本: " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region 主题

        private bool _IsDarkTheme;

        private Brush _TextLabel_ForeGround;

        private void _UpdateColors()
        {
            TextLabel_ForeGround = Theme.GetSolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(64, 64, 64));
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

        public Brush TextLabel_ForeGround
        {
            get => _TextLabel_ForeGround;

            set
            {
                _TextLabel_ForeGround = value;

                NotifyPropertyChanged(nameof(TextLabel_ForeGround));
            }
        }

        #endregion
    }
}