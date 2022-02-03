/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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

using System.Windows.Media;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI.Extensions
{
    // Com类库的扩展方法
    public static class ComExtension
    {
        public static Color ToWpfColor(this ColorX color) => Color.FromArgb((byte)(int)Math.Round(color.Alpha), (byte)(int)Math.Round(color.RgbRed), (byte)(int)Math.Round(color.RgbGreen), (byte)(int)Math.Round(color.RgbBlue));

        public static ColorX ToColorX(this Color color) => ColorX.FromRgb(color.A, color.R, color.G, color.B);
    }
}