/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

TreeOfLife
Version 1.0.617.1000.M6.201225-2240

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TreeOfLife.Extensions
{
    // WPF 扩展（临时）
    public static class WpfExtension
    {
        public static double Horizontal(this Thickness thickness)
        {
            return thickness.Left + thickness.Right;
        }

        public static double Vertical(this Thickness thickness)
        {
            return thickness.Top + thickness.Bottom;
        }

        //

        public static void SetForeColor(this Control control, Color color)
        {
            control.Foreground = new SolidColorBrush(color);
        }

        public static void SetBackColor(this Control control, Color color)
        {
            control.Background = new SolidColorBrush(color);
        }
    }
}