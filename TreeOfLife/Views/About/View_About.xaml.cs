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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreeOfLife.Views.About
{
    /// <summary>
    /// View_About.xaml 的交互逻辑
    /// </summary>
    public partial class View_About : UserControl
    {
        public View_About()
        {
            InitializeComponent();
        }

        //

        public ViewModel_About ViewModel => this.DataContext as ViewModel_About;
    }
}