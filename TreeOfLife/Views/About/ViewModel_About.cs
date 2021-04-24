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
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TreeOfLife.Views.About
{
    public class ViewModel_About
    {
        public ViewModel_About()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        public ImageSource AppLogo_256
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
    }
}