﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.Windows.Media;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Views.Tree
{
    public sealed class ViewModel_Tree : ViewModel
    {
        #region 主题

        private bool _IsDarkTheme;

        private Brush _Tree_BackGround;

        private void _UpdateColors()
        {
            Tree_BackGround = Theme.GetSolidColorBrush(_IsDarkTheme ? ColorX.FromHSL(0, 0, 2) : ColorX.FromHSL(0, 0, 98));
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