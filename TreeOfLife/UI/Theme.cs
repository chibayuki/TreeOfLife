/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2021 chibayuki@foxmail.com

TreeOfLife
Version 1.0.1470.1000.M14.211205-1900

This file is part of TreeOfLife
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Media;

using TreeOfLife.UI.Extensions;

using ColorX = Com.Chromatics.ColorX;

namespace TreeOfLife.UI
{
    // 主题。
    public static class Theme
    {
        private static Dictionary<Color, SolidColorBrush> _SolidColorBrushes = new Dictionary<Color, SolidColorBrush>();

        // 用于获取和缓存单色 Brush。
        public static SolidColorBrush GetSolidColorBrush(Color color)
        {
            if (!_SolidColorBrushes.TryGetValue(color, out SolidColorBrush brush))
            {
                brush = new SolidColorBrush(color);

                _SolidColorBrushes.Add(color, brush);
            }

            return brush;
        }

        public static SolidColorBrush GetSolidColorBrush(ColorX color) => GetSolidColorBrush(color.ToWpfColor());

        //

        private static ColorX _ThemeColor = ColorX.FromHSL(215, 50, 50); // 主题颜色。
        private static bool _IsDarkTheme = false; // 是否为暗色主题。

        private static void _UpdateColor()
        {
            ResourceDictionary resourceDictionary = Application.Current.Resources;

            if (_IsDarkTheme)
            {
                // ButtonStyle.xaml

                resourceDictionary["Button.Static.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 10));
                resourceDictionary["Button.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["Button.Static.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));

                resourceDictionary["Button.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["Button.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["Button.MouseOver.Foreground"] = Brushes.Black;

                resourceDictionary["Button.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["Button.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["Button.Pressed.Foreground"] = Brushes.Black;

                resourceDictionary["Button.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["Button.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["Button.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 60));

                resourceDictionary["TabPageButton.Static.Background"] = Brushes.Transparent;
                resourceDictionary["TabPageButton.Static.Foreground"] = Brushes.Black;

                resourceDictionary["TabPageButton.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));
                resourceDictionary["TabPageButton.MouseOver.Foreground"] = Brushes.Black;

                resourceDictionary["TabPageButton.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["TabPageButton.Pressed.Foreground"] = Brushes.Black;

                resourceDictionary["TabPageButton.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 5));
                resourceDictionary["TabPageButton.Disabled.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));

                resourceDictionary["TransparentButton.Static.Foreground"] = Brushes.Gray;
                resourceDictionary["TransparentButton.MouseOver.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["TransparentButton.Pressed.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["TransparentButton.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 50, 50));

                // CheckBoxStyle.xaml

                resourceDictionary["CheckBox.Static.Background"] = Brushes.Black;
                resourceDictionary["CheckBox.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["CheckBox.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));
                resourceDictionary["CheckBox.Static.Glyph"] = Brushes.Black;

                resourceDictionary["CheckBox.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["CheckBox.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["CheckBox.MouseOver.Glyph"] = Brushes.Black;

                resourceDictionary["CheckBox.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["CheckBox.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["CheckBox.Pressed.Glyph"] = Brushes.Black;

                resourceDictionary["CheckBox.Checked.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["CheckBox.Checked.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["CheckBox.Checked.Glyph"] = Brushes.Black;

                resourceDictionary["CheckBox.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["CheckBox.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["CheckBox.Disabled.Glyph"] = Brushes.Gray;

                // ContextMenuStyle.xaml

                resourceDictionary["ContextMenu.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 10));
                resourceDictionary["ContextMenu.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));

                resourceDictionary["Separator.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));

                resourceDictionary["MenuItem.Static.Background"] = Brushes.Transparent;
                resourceDictionary["MenuItem.Static.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));

                resourceDictionary["MenuItem.Highlighted.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["MenuItem.Highlighted.Foreground"] = Brushes.Black;

                resourceDictionary["MenuItem.Disabled.Background"] = Brushes.Transparent;
                resourceDictionary["MenuItem.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 60));

                // LabelStyle.xaml

                resourceDictionary["Label.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));

                resourceDictionary["TitleLabel.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 10));
                resourceDictionary["TitleLabel.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));
                resourceDictionary["TitleLabel.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));

                // RadioButtonStyle.xaml

                resourceDictionary["RadioButton.Static.Background"] = Brushes.Black;
                resourceDictionary["RadioButton.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["RadioButton.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));
                resourceDictionary["RadioButton.Static.Glyph"] = Brushes.Black;

                resourceDictionary["RadioButton.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["RadioButton.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["RadioButton.MouseOver.Glyph"] = Brushes.Black;

                resourceDictionary["RadioButton.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["RadioButton.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["RadioButton.Pressed.Glyph"] = Brushes.Black;

                resourceDictionary["RadioButton.Checked.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["RadioButton.Checked.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["RadioButton.Checked.Glyph"] = Brushes.Black;

                resourceDictionary["RadioButton.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["RadioButton.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["RadioButton.Disabled.Glyph"] = Brushes.Gray;

                // ScrollBarStyle.xaml

                resourceDictionary["ScrollBar.Static.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(15));
                resourceDictionary["ScrollBar.Static.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["ScrollBar.Static.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));

                resourceDictionary["ScrollBar.MouseOver.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["ScrollBar.MouseOver.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));

                resourceDictionary["ScrollBar.Pressed.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["ScrollBar.Pressed.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));

                resourceDictionary["ScrollBar.Disabled.Thumb"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));

                // TextBoxStyle.xaml

                resourceDictionary["TextBox.Static.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 0, 50));
                resourceDictionary["TextBox.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 20));
                resourceDictionary["TextBox.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));

                resourceDictionary["TextBox.MouseOver.Background"] = Brushes.Black;
                resourceDictionary["TextBox.MouseOver.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));
                resourceDictionary["TextBox.MouseOver.BorderBrush.Underline"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));
                resourceDictionary["TextBox.MouseOver.Foreground"] = Brushes.White;

                resourceDictionary["TextBox.Focused.Background"] = Brushes.Black;
                resourceDictionary["TextBox.Focused.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 40));
                resourceDictionary["TextBox.Focused.BorderBrush.Underline"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["TextBox.Focused.Foreground"] = Brushes.White;

                // Misc.xaml

                resourceDictionary["TabPage.Header.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 6));
                resourceDictionary["TabPage.Body.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 4));
                resourceDictionary["TabPage.Side.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));

                resourceDictionary["Tree.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 2));
                resourceDictionary["TreeNode.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));

                resourceDictionary["WarningMessage.Background"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 10));
                resourceDictionary["WarningMessage.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 50));
                resourceDictionary["WarningMessage.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 70));
            }
            else
            {
                // ButtonStyle.xaml

                resourceDictionary["Button.Static.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 90));
                resourceDictionary["Button.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["Button.Static.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));

                resourceDictionary["Button.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["Button.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["Button.MouseOver.Foreground"] = Brushes.White;

                resourceDictionary["Button.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["Button.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["Button.Pressed.Foreground"] = Brushes.White;

                resourceDictionary["Button.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["Button.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["Button.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 40));

                resourceDictionary["TabPageButton.Static.Background"] = Brushes.Transparent;
                resourceDictionary["TabPageButton.Static.Foreground"] = Brushes.White;

                resourceDictionary["TabPageButton.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["TabPageButton.MouseOver.Foreground"] = Brushes.White;

                resourceDictionary["TabPageButton.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));
                resourceDictionary["TabPageButton.Pressed.Foreground"] = Brushes.White;

                resourceDictionary["TabPageButton.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 95));
                resourceDictionary["TabPageButton.Disabled.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));

                resourceDictionary["TransparentButton.Static.Foreground"] = Brushes.Gray;
                resourceDictionary["TransparentButton.MouseOver.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["TransparentButton.Pressed.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["TransparentButton.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 50, 50));

                // CheckBoxStyle.xaml

                resourceDictionary["CheckBox.Static.Background"] = Brushes.White;
                resourceDictionary["CheckBox.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["CheckBox.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));
                resourceDictionary["CheckBox.Static.Glyph"] = Brushes.White;

                resourceDictionary["CheckBox.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["CheckBox.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["CheckBox.MouseOver.Glyph"] = Brushes.White;

                resourceDictionary["CheckBox.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["CheckBox.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["CheckBox.Pressed.Glyph"] = Brushes.White;

                resourceDictionary["CheckBox.Checked.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["CheckBox.Checked.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["CheckBox.Checked.Glyph"] = Brushes.White;

                resourceDictionary["CheckBox.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["CheckBox.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["CheckBox.Disabled.Glyph"] = Brushes.Gray;

                // ContextMenuStyle.xaml

                resourceDictionary["ContextMenu.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 90));
                resourceDictionary["ContextMenu.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));

                resourceDictionary["Separator.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));

                resourceDictionary["MenuItem.Static.Background"] = Brushes.Transparent;
                resourceDictionary["MenuItem.Static.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));

                resourceDictionary["MenuItem.Highlighted.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["MenuItem.Highlighted.Foreground"] = Brushes.White;

                resourceDictionary["MenuItem.Disabled.Background"] = Brushes.Transparent;
                resourceDictionary["MenuItem.Disabled.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 40));

                // LabelStyle.xaml

                resourceDictionary["Label.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));

                resourceDictionary["TitleLabel.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 90));
                resourceDictionary["TitleLabel.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));
                resourceDictionary["TitleLabel.Foreground"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(30));

                // RadioButtonStyle.xaml

                resourceDictionary["RadioButton.Static.Background"] = Brushes.White;
                resourceDictionary["RadioButton.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["RadioButton.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));
                resourceDictionary["RadioButton.Static.Glyph"] = Brushes.White;

                resourceDictionary["RadioButton.MouseOver.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["RadioButton.MouseOver.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["RadioButton.MouseOver.Glyph"] = Brushes.White;

                resourceDictionary["RadioButton.Pressed.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["RadioButton.Pressed.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["RadioButton.Pressed.Glyph"] = Brushes.White;

                resourceDictionary["RadioButton.Checked.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["RadioButton.Checked.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["RadioButton.Checked.Glyph"] = Brushes.White;

                resourceDictionary["RadioButton.Disabled.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["RadioButton.Disabled.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["RadioButton.Disabled.Glyph"] = Brushes.Gray;

                // ScrollBarStyle.xaml

                resourceDictionary["ScrollBar.Static.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(85));
                resourceDictionary["ScrollBar.Static.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));
                resourceDictionary["ScrollBar.Static.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(50));

                resourceDictionary["ScrollBar.MouseOver.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["ScrollBar.MouseOver.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));

                resourceDictionary["ScrollBar.Pressed.Glyph"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));
                resourceDictionary["ScrollBar.Pressed.Thumb"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));

                resourceDictionary["ScrollBar.Disabled.Glyph"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));
                resourceDictionary["ScrollBar.Disabled.Thumb"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));

                // TextBoxStyle.xaml

                resourceDictionary["TextBox.Static.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 100, 50));
                resourceDictionary["TextBox.Static.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 80));
                resourceDictionary["TextBox.Static.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 30));

                resourceDictionary["TextBox.MouseOver.Background"] = Brushes.White;
                resourceDictionary["TextBox.MouseOver.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 70));
                resourceDictionary["TextBox.MouseOver.BorderBrush.Underline"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));
                resourceDictionary["TextBox.MouseOver.Foreground"] = Brushes.Black;

                resourceDictionary["TextBox.Focused.Background"] = Brushes.White;
                resourceDictionary["TextBox.Focused.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 60));
                resourceDictionary["TextBox.Focused.BorderBrush.Underline"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(60));
                resourceDictionary["TextBox.Focused.Foreground"] = Brushes.Black;

                // Misc.xaml

                resourceDictionary["TabPage.Header.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 94));
                resourceDictionary["TabPage.Body.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 96));
                resourceDictionary["TabPage.Side.Background"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(40));

                resourceDictionary["Tree.Background"] = GetSolidColorBrush(ColorX.FromHSL(0, 0, 98));
                resourceDictionary["TreeNode.BorderBrush"] = GetSolidColorBrush(_ThemeColor.AtLightness_HSL(70));

                resourceDictionary["WarningMessage.Background"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 90));
                resourceDictionary["WarningMessage.BorderBrush"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 50));
                resourceDictionary["WarningMessage.Foreground"] = GetSolidColorBrush(ColorX.FromHSL(5, 80, 30));
            }
        }

        public static ColorX ThemeColor
        {
            get => _ThemeColor;

            set
            {
                _ThemeColor = value;

                _UpdateColor();

                ThemeColorChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static bool IsDarkTheme
        {
            get => _IsDarkTheme;

            set
            {
                _IsDarkTheme = value;

                _UpdateColor();

                IsDarkThemeChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        //

        public static EventHandler ThemeColorChanged;

        public static EventHandler IsDarkThemeChanged;
    }
}