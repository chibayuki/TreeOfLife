/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1132.1000.M11.210516-1800

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Windows.Media;

namespace TreeOfLife.Views.Tree
{
    public class ViewModel_Tree : INotifyPropertyChanged
    {
        public ViewModel_Tree()
        {
        }

        //

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //

        #region 主题

        private bool _IsDarkTheme;

        private Brush _Tree_BackGround;

        private void _UpdateColors()
        {
            Tree_BackGround = (_IsDarkTheme ? Brushes.Black : Brushes.White);
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

        public Brush Tree_BackGround
        {
            get => _Tree_BackGround;

            set
            {
                _Tree_BackGround = value;

                NotifyPropertyChanged(nameof(Tree_BackGround));
            }
        }

        #endregion
    }
}