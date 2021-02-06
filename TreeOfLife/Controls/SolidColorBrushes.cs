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