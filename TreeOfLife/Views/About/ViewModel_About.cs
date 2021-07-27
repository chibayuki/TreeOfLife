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
    }
}