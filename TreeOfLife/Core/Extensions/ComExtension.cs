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

using System.Windows.Media;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.Core.Extensions
{
    // Com.dll 扩展（临时）
    public static class ComExtension
    {
        public static Color ToWpfColor(this ColorX colorX) => Color.FromArgb((byte)(int)Math.Round(colorX.Alpha), (byte)(int)Math.Round(colorX.Red), (byte)(int)Math.Round(colorX.Green), (byte)(int)Math.Round(colorX.Blue));
    }
}