/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1322.1000.M13.210925-1400

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
    /// <summary>
    /// View_File.xaml 的交互逻辑
    /// </summary>
    public partial class View_File : UserControl
    {
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

        private async void Button_Open_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.TrySaveAndCloseAsync())
            {
                ViewModel.UpdateFileInfo();

                bool? open = await ViewModel.OpenAsync();

                if (open is not null)
                {
                    if (open.Value)
                    {
                        ViewModel.UpdateFileInfo();
                        ViewModel.OpenDone();
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
            bool? save = await ViewModel.SaveAsync();

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
            bool? saveAs = await ViewModel.SaveAsAsync();

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
            if (await ViewModel.TrySaveAndCloseAsync())
            {
                ViewModel.UpdateFileInfo();
            }
        }

        //

        public ViewModel_File ViewModel => this.DataContext as ViewModel_File;
    }
}