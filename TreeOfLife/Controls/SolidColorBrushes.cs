/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1100.1000.M11.210405-0000

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;

namespace TreeOfLife.Controls
{
    // 用于获取和缓存单色 Brush。
    public static class SolidColorBrushes
    {
        private static Dictionary<Color, SolidColorBrush> _Brushes = new Dictionary<Color, SolidColorBrush>();

        public static SolidColorBrush GetBrush(Color color)
        {
            SolidColorBrush brush;

            if (!_Brushes.TryGetValue(color, out brush))
            {
                brush = new SolidColorBrush(color);

                _Brushes.Add(color, brush);
            }

            return brush;
        }
    }
}