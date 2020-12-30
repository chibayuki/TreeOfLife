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

namespace TreeOfLife.Views.Tree
{
    /// <summary>
    /// View_Tree.xaml 的交互逻辑
    /// </summary>
    public partial class View_Tree : UserControl
    {
        public ViewModel_Tree ViewModel => this.DataContext as ViewModel_Tree;

        //

        public View_Tree()
        {
            InitializeComponent();
        }

        //

        #region 系统发生树（临时）

        public void UpdateTree()
        {
            ViewModel.UpdateTree();
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