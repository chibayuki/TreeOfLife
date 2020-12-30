/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.708.1000.M7.201230-2100

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

namespace TreeOfLife.Views.File
{
    /// <summary>
    /// View_File.xaml 的交互逻辑
    /// </summary>
    public partial class View_File : UserControl
    {
        private static readonly string _AppName = Assembly.GetExecutingAssembly().GetName().Name;

        //

        public ViewModel_File ViewModel => this.DataContext as ViewModel_File;

        //

        public View_File()
        {
            InitializeComponent();

            //

            this.Loaded += (s, e) => ViewModel.UpdateFileInfo();

            button_Open.Click += Button_Open_Click;
            button_Save.Click += Button_Save_Click;
            button_SaveAs.Click += Button_SaveAs_Click;
            button_Close.Click += Button_Close_Click;
        }

        //

        #region 回调函数

        private void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TrySaveAndClose())
            {
                ViewModel.UpdateFileInfo();

                bool? open = ViewModel.Open();

                if (open.HasValue)
                {
                    if (open.Value)
                    {
                        ViewModel.UpdateFileInfo();
                        ViewModel.OpenDone();
                    }
                    else
                    {
                        MessageBox.Show("打开失败。", _AppName, MessageBoxButton.OK);
                    }
                }
            }
        }

        private void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            bool? save = ViewModel.Save();

            if (save.HasValue)
            {
                if (save.Value)
                {
                    ViewModel.UpdateFileInfo();
                }
                else
                {
                    MessageBox.Show("保存失败。", _AppName, MessageBoxButton.OK);
                }
            }
        }

        private void Button_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            bool? saveAs = ViewModel.SaveAs();

            if (saveAs.HasValue)
            {
                if (saveAs.Value)
                {
                    ViewModel.UpdateFileInfo();
                }
                else
                {
                    MessageBox.Show("保存失败。", _AppName, MessageBoxButton.OK);
                }
            }
        }

        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.TrySaveAndClose())
            {
                ViewModel.UpdateFileInfo();
            }
        }

        #endregion

        #region 主题

        private bool _IsDarkTheme = false;

        public bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                ViewModel.IsDarkTheme = _IsDarkTheme;
            }
        }

        #endregion
    }
}