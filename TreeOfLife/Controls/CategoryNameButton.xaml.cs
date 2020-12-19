/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.415.1000.M5.201204-2200

This file is part of "生命树" (TreeOfLife)

"生命树" (TreeOfLife) is released under the GPLv3 license
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ColorX = Com.ColorX;

namespace TreeOfLife
{
    public partial class CategoryNameButton : UserControl
    {
        private TaxonomicCategory? _Category = null; // 分类阶元。
        private string _CategoryName = string.Empty; // 名称。

        private bool _Checked = false; // 是否处于已选择状态。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _DarkTheme = false; // 是否为暗色主题。

        //

        public CategoryNameButton()
        {
            InitializeComponent();

            //

            this.Load += TaxonNameButton_Load;
            this.AutoSizeChanged += CategoryNameButton_AutoSizeChanged;
            this.FontChanged += TaxonNameButton_FontChanged;

            Label_CategoryName.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) { base.OnClick(e); } base.OnMouseClick(e); };
        }

        //

        private void TaxonNameButton_Load(object sender, EventArgs e)
        {
            _FontChanged();
            _UpdateColor();
            _UpdateCategoryInfo();
            _AutoSizeChanged();
        }

        private void CategoryNameButton_AutoSizeChanged(object sender, EventArgs e)
        {
            _AutoSizeChanged();
        }

        private void TaxonNameButton_FontChanged(object sender, EventArgs e)
        {
            _FontChanged();
        }

        //

        private void _AutoSizeChanged()
        {
            if (this.AutoSize)
            {
                Label_CategoryName.Dock = DockStyle.None;
                Label_CategoryName.AutoSize = true;
                Label_CategoryName.MinimumSize = this.MinimumSize;
                Label_CategoryName.MaximumSize = this.MaximumSize;

                _AutoSize();
            }
            else
            {
                Label_CategoryName.AutoSize = false;
                Label_CategoryName.Dock = DockStyle.Fill;
            }
        }

        private void _AutoSize()
        {
            this.Size = Label_CategoryName.Size;
        }

        private void _FontChanged()
        {
            Label_CategoryName.Font = this.Font;
        }

        private void _UpdateColor()
        {
            Label_CategoryName.ForeColor = _CategoryNameForeColor;
            Label_CategoryName.BackColor = _CategoryNameBackColor;
        }

        private void _UpdateCategoryInfo()
        {
            Label_CategoryName.Text = _CategoryName;
        }

        //

        private Color _CategoryNameForeColor => (_Checked ? (_DarkTheme ? Color.Black : Color.White) : _ThemeColor.AtLightness_LAB(_DarkTheme ? 40 : 60).ToColor());

        private Color _CategoryNameBackColor => (_Checked ? _ThemeColor.AtLightness_LAB(_DarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_DarkTheme ? 10 : 90)).ToColor();

        //

        public string CategoryName
        {
            get
            {
                return _CategoryName;
            }

            set
            {
                _CategoryName = value;

                _UpdateCategoryInfo();
            }
        }

        [Browsable(false)]
        public TaxonomicCategory? Category
        {
            get
            {
                return _Category;
            }

            set
            {
                _Category = value;

                if (_Category.HasValue)
                {
                    _CategoryName = _Category.Value.Name();

                    _UpdateCategoryInfo();
                }
            }
        }

        public bool Checked
        {
            get
            {
                return _Checked;
            }

            set
            {
                _Checked = value;

                _UpdateColor();
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
    }
}
