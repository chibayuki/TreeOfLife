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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Reflection;

using TreeOfLife.Phylogeny;

namespace TreeOfLife.Views.File
{
    /// <summary>
    /// View_File.xaml 的交互逻辑
    /// </summary>
    public partial class View_File : UserControl
    {
        private static readonly string _AppName = Assembly.GetExecutingAssembly().GetName().Name;

        //

        public View_File()
        {
            InitializeComponent();

            //

            button_Open.Click += Button_Open_Click;
            button_Save.Click += Button_Save_Click;
            button_SaveAs.Click += Button_SaveAs_Click;
            button_Close.Click += Button_Close_Click;
        }

        //

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TrySaveAndClose())
            {
                ViewModel.CreationTime = DateTime.MinValue;
                ViewModel.ModificationTime = DateTime.MinValue;

                if (ViewModel.Open())
                {
                    ViewModel.CreationTime = Phylogenesis.CreationTime;
                    ViewModel.ModificationTime = Phylogenesis.ModificationTime;

                    ViewModel.OpenDone();
                }
                else
                {
                    MessageBox.Show("打开失败。", _AppName, MessageBoxButton.OK);
                }
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Save())
            {
                ViewModel.ModificationTime = Phylogenesis.ModificationTime;
            }
            else
            {
                MessageBox.Show("保存失败。", _AppName, MessageBoxButton.OK);
            }
        }

        private void Button_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SaveAs())
            {
                ViewModel.CreationTime = Phylogenesis.CreationTime;
                ViewModel.ModificationTime = Phylogenesis.ModificationTime;
            }
            else
            {
                MessageBox.Show("保存失败。", _AppName, MessageBoxButton.OK);
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TrySaveAndClose())
            {
                ViewModel.CreationTime = DateTime.MinValue;
                ViewModel.ModificationTime = DateTime.MinValue;
            }
        }

        //

        public ViewModel_File ViewModel => this.DataContext as ViewModel_File;

        //

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                if (_IsDarkTheme != value)
                {
                    _IsDarkTheme = value;

                    ViewModel.IsDarkTheme = _IsDarkTheme;
                }
            }
        }

        #endregion
    }
}