﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using TreeOfLife.Core;

namespace TreeOfLife.UI.Views
{
    public partial class View_File : UserControl
    {
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

        public Func<Task<bool?>> OpenAsync { get; set; }
        public Func<Task<bool?>> SaveAsync { get; set; }
        public Func<Task<bool?>> SaveAsAsync { get; set; }
        public Func<Task<bool>> TrySaveAndCloseAsync { get; set; }

        //

        #region 回调函数

        private async void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            if (await TrySaveAndCloseAsync())
            {
                ViewModel.UpdateFileInfo();

                bool? open = await OpenAsync();

                if (open is not null)
                {
                    if (open.Value)
                    {
                        ViewModel.UpdateFileInfo();

                        Common.CurrentTabPage = Common.TabPage.Evo;
                    }
                    else
                    {
                        MessageBox.Show("打开失败。", Entrance.AppName, MessageBoxButton.OK);
                    }
                }
            }
        }

        private async void Button_Save_Click(object sender, RoutedEventArgs e)
        {
            bool? save = await SaveAsync();

            if (save is not null)
            {
                if (save.Value)
                {
                    ViewModel.UpdateFileInfo();
                }
                else
                {
                    MessageBox.Show("保存失败。", Entrance.AppName, MessageBoxButton.OK);
                }
            }
        }

        private async void Button_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            bool? saveAs = await SaveAsAsync();

            if (saveAs is not null)
            {
                if (saveAs.Value)
                {
                    ViewModel.UpdateFileInfo();
                }
                else
                {
                    MessageBox.Show("保存失败。", Entrance.AppName, MessageBoxButton.OK);
                }
            }
        }

        private async void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            if (await TrySaveAndCloseAsync())
            {
                ViewModel.UpdateFileInfo();
            }
        }

        #endregion
    }
}