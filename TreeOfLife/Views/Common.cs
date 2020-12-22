﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
using System.Windows.Input;

using System.Windows.Media;

namespace TreeOfLife.Views
{
    public static class Common
    {
        #region 主题

        private static bool _IsDarkTheme;

        private static void _UpdateColors()
        {
            Button_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(128, 128, 128) : Color.FromRgb(64, 64, 64));
            Button_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(32, 32, 32) : Color.FromRgb(224, 224, 224));
            SubTitle_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(48, 48, 48));
            SubTitle_BackGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(48, 48, 48) : Color.FromRgb(208, 208, 208));
            TextBox_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(48, 48, 48));
            TextBox_BackGround = new SolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);
            TextBox_Selection = new SolidColorBrush(Color.FromRgb(0, 120, 215));
            TextBox_SelectionText = new SolidColorBrush(_IsDarkTheme ? Colors.Black : Colors.White);
            CheckBox_ForeGround = new SolidColorBrush(_IsDarkTheme ? Color.FromRgb(192, 192, 192) : Color.FromRgb(48, 48, 48));
        }

        public static bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColors();
            }
        }

        public static Brush Button_ForeGround { get; set; }

        public static Brush Button_BackGround { get; set; }

        public static Brush SubTitle_ForeGround { get; set; }

        public static Brush SubTitle_BackGround { get; set; }

        public static Brush TextBox_ForeGround { get; set; }

        public static Brush TextBox_BackGround { get; set; }

        public static Brush TextBox_Selection { get; set; }

        public static Brush TextBox_SelectionText { get; set; }

        public static Brush CheckBox_ForeGround { get; set; }

        #endregion
    }
}