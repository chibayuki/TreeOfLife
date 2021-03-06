﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1000.1000.M10.210130-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Extensions
{
    // Com.dll 扩展（临时）
    public static class ComExtension
    {
        public static Color ToWpfColor(this ColorX colorX)
        {
            return Color.FromArgb((byte)(int)Math.Round(colorX.Alpha), (byte)(int)Math.Round(colorX.Red), (byte)(int)Math.Round(colorX.Green), (byte)(int)Math.Round(colorX.Blue));
        }
    }
}