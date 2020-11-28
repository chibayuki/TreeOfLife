/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.319.1000.M4.201128 -0000

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ColorX = Com.ColorX;

namespace TreeOfLife.Controls
{
    internal partial class TagGroup : UserControl
    {
        List<Label> _Tags = new List<Label>();

        private Size _MinTagSize = new Size(32, 22); // 标签最小大小。
        private Padding _TagPadding = new Padding(2, 2, 2, 2); // 标签外边距。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _DarkTheme = false; // 是否为暗色主题。

        //

        public TagGroup()
        {
            InitializeComponent();

            //

            this.Load += TagGroup_Load;
            this.SizeChanged += TagGroup_SizeChanged;
            this.AutoSizeChanged += TagGroup_AutoSizeChanged;
            this.FontChanged += TagGroup_FontChanged;
        }

        //

        private void TagGroup_Load(object sender, EventArgs e)
        {
            _UpdateFont();
            _UpdateColor();
            _UpdateLayout();
        }

        private void TagGroup_SizeChanged(object sender, EventArgs e)
        {
            _UpdateLayout();
        }

        private void TagGroup_AutoSizeChanged(object sender, EventArgs e)
        {
            _AutoSizeChanged();
        }

        private void TagGroup_FontChanged(object sender, EventArgs e)
        {
            _UpdateFont();
        }

        //

        private void _AutoSizeChanged()
        {
            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        private void _AutoSize()
        {
            this.Height = (_Tags.Count <= 0 ? 0 : _Tags[_Tags.Count - 1].Bottom);
        }

        private void _UpdateFont()
        {
            foreach (var tag in _Tags)
            {
                tag.Font = this.Font;
            }
        }

        private void _UpdateColor()
        {
            foreach (var tag in _Tags)
            {
                tag.ForeColor = _TagForeColor;
                tag.BackColor = _TagBackColor;
            }
        }

        private void _UpdateLayout()
        {
            if (_Tags.Count > 0)
            {
                for (int i = 0; i < _Tags.Count; i++)
                {
                    _Tags[i].MinimumSize = _MinTagSize;

                    if (i > 0)
                    {
                        if (_Tags[i - 1].Right + _TagPadding.Vertical + _Tags[i].Width > this.Width)
                        {
                            _Tags[i].Location = new Point(0, _Tags[i - 1].Bottom + _TagPadding.Vertical);
                        }
                        else
                        {
                            _Tags[i].Location = new Point(_Tags[i - 1].Right + _TagPadding.Horizontal, _Tags[i - 1].Top);
                        }
                    }
                    else
                    {
                        _Tags[i].Location = new Point(0, 0);
                    }
                }
            }

            //

            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        //

        private Color _TagForeColor => _ThemeColor.AtLightness_LAB(_DarkTheme ? 40 : 60).ToColor();

        private Color _TagBackColor => _ThemeColor.AtLightness_HSL(_DarkTheme ? 10 : 90).ToColor();

        //

        public Size MinTagSize
        {
            get
            {
                return _MinTagSize;
            }

            set
            {
                _MinTagSize = value;

                _UpdateLayout();
            }
        }

        public Padding TagPadding
        {
            get
            {
                return _TagPadding;
            }

            set
            {
                _TagPadding = value;

                _UpdateLayout();
            }
        }

        [Browsable(false)]
        public ColorX ThemeColor
        {
            get
            {
                return _ThemeColor;
            }

            set
            {
                _ThemeColor = value;

                _UpdateColor();
            }
        }

        public bool IsDarkTheme
        {
            get
            {
                return _DarkTheme;
            }

            set
            {
                _DarkTheme = value;

                _UpdateColor();
            }
        }

        [Browsable(false)]
        public string[] Tags
        {
            set
            {
                Panel_Main.Visible = false;

                Panel_Main.Controls.Clear();

                _Tags = new List<Label>(value.Length);

                for (int i = 0; i < value.Length; i++)
                {
                    Label tag = new Label();
                    tag.MinimumSize = _MinTagSize;
                    tag.AutoSize = true;
                    tag.TextAlign = ContentAlignment.MiddleCenter;
                    tag.Text = value[i];

                    _Tags.Add(tag);

                    Panel_Main.Controls.Add(tag);
                }

                _UpdateFont();
                _UpdateColor();
                _UpdateLayout();

                Panel_Main.Visible = true;
            }
        }
    }
}