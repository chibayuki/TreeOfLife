/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2022 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;

namespace TreeOfLife.UI.Views
{
    public partial class View_About : UserControl
    {
        public ViewModel_About ViewModel => this.DataContext as ViewModel_About;

        //

        public View_About()
        {
            InitializeComponent();

            //

            BitmapImage appLogo_Light = new BitmapImage();
            appLogo_Light.BeginInit();
            appLogo_Light.StreamSource = new MemoryStream(Resource.AppLogo_256);
            appLogo_Light.EndInit();

            BitmapImage appLogo_Dark = new BitmapImage();
            appLogo_Dark.BeginInit();
            appLogo_Dark.StreamSource = new MemoryStream(Resource.AppLogo_256_Dark);
            appLogo_Dark.EndInit();

            Theme.IsDarkThemeChanged += (s, e) => ViewModel.AppLogo = Theme.IsDarkTheme ? appLogo_Dark : appLogo_Light;
        }
    }
}