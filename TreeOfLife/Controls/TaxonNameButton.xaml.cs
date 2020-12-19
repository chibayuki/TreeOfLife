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
    public partial class TaxonNameButton : UserControl
    {
        private Taxon _Taxon = null; // 类群。

        private bool _Checked = false; // 是否处于已选择状态。

        private int _CategoryNameWidth = 50; // 分类阶元名称宽度。

        private ColorX _ThemeColor = ColorX.FromRGB(128, 128, 128); // 主题颜色。
        private bool _DarkTheme = false; // 是否为暗色主题。

        //

        public TaxonNameButton()
        {
            InitializeComponent();

            //

            this.Load += TaxonNameButton_Load;
            this.SizeChanged += TaxonNameButton_SizeChanged;
            this.FontChanged += TaxonNameButton_FontChanged;

            Label_CategoryName.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) { base.OnClick(e); } base.OnMouseClick(e); };
            Label_TaxonName.MouseClick += (s, e) => { if (e.Button == MouseButtons.Left) { base.OnClick(e); } base.OnMouseClick(e); };
        }

        //

        private void TaxonNameButton_Load(object sender, EventArgs e)
        {
            _UpdateFont();
            _UpdateColor();
            _SizeChanged();
            _UpdateTaxonInfo();
        }

        private void TaxonNameButton_SizeChanged(object sender, EventArgs e)
        {
            _SizeChanged();
        }

        private void TaxonNameButton_FontChanged(object sender, EventArgs e)
        {
            _UpdateFont();
        }

        //

        private void _SizeChanged()
        {
            Label_CategoryName.Size = new Size(_CategoryNameWidth - 1, Panel_Main.Height - 2);
            Label_CategoryName.Location = new Point(1, 1);

            Label_TaxonName.Size = new Size(Panel_Main.Width - _CategoryNameWidth - 1, Panel_Main.Height - 2);
            Label_TaxonName.Location = new Point(Label_CategoryName.Right, 1);
        }

        private void _UpdateFont()
        {
            Label_CategoryName.Font = this.Font;
            Label_TaxonName.Font = this.Font;
        }

        private void _UpdateColor()
        {
            Panel_Main.BackColor = _CategoryNameBackColor;

            Label_CategoryName.ForeColor = _CategoryNameForeColor;
            Label_CategoryName.BackColor = _CategoryNameBackColor;

            Label_TaxonName.ForeColor = _TaxonNameForeColor;
            Label_TaxonName.BackColor = _TaxonNameBackColor;
        }

        private void _UpdateTaxonInfo()
        {
            Label_CategoryName.Text = (_Taxon == null || _Taxon.IsAnonymous() ? string.Empty : _Taxon.Category.Name());
            Label_TaxonName.Text = (_Taxon == null ? string.Empty : _Taxon.ShortName());
        }

        //

        private Color _CategoryNameForeColor => (_Checked ? (_DarkTheme ? Color.Black : Color.White) : _ThemeColor.AtLightness_LAB(_DarkTheme ? 40 : 60).ToColor());

        private Color _CategoryNameBackColor => (_Checked ? _ThemeColor.AtLightness_LAB(_DarkTheme ? 30 : 70) : _ThemeColor.AtLightness_HSL(_DarkTheme ? 10 : 90)).ToColor();

        private Color _TaxonNameForeColor => _ThemeColor.AtLightness_LAB(_Checked ? (_DarkTheme ? 60 : 40) : 50).ToColor();

        private Color _TaxonNameBackColor => _ThemeColor.AtLightness_HSL(_Checked ? (_DarkTheme ? 10 : 90) : (_DarkTheme ? 3 : 97)).ToColor();

        //

        [Browsable(false)]
        public Taxon Taxon
        {
            get
            {
                return _Taxon;
            }

            set
            {
                _Taxon = value;

                _UpdateTaxonInfo();
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

        public int CategoryNameWidth
        {
            get
            {
                return _CategoryNameWidth;
            }

            set
            {
                _CategoryNameWidth = value;

                _SizeChanged();
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
