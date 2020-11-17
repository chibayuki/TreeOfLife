/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.200.1000.M3.201111-0000

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
    internal partial class TaxonNameButtonGroup : UserControl
    {
        private class _Group
        {
            private string _Name; // 组名称。
            private ColorX _ThemeColor; // 主题颜色。
            private bool _DarkTheme = false; // 是否为暗色主题。

            private Panel _GroupPanel = new Panel();
            private Label _NameLabel = new Label();
            private List<TaxonNameButton> _Buttons = new List<TaxonNameButton>();

            public _Group(string name, ColorX themeColor, bool isDarkTheme)
            {
                _Name = name;
                _ThemeColor = themeColor;
                _DarkTheme = isDarkTheme;

                _NameLabel.Text = _Name;
                _NameLabel.TextAlign = ContentAlignment.MiddleCenter;
                _NameLabel.AutoEllipsis = true;
                _NameLabel.Location = new Point(0, 0);
            }

            public string Name
            {
                get
                {
                    return _Name;
                }

                set
                {
                    _Name = value;

                    _NameLabel.Text = _Name;
                }
            }

            public ColorX ThemeColor
            {
                get
                {
                    return _ThemeColor;
                }

                set
                {
                    _ThemeColor = value;

                    UpdateColor();
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

                    UpdateColor();
                }
            }

            public Panel GroupPanel => _GroupPanel;

            public List<TaxonNameButton> Buttons => _Buttons;

            public void UpdateFont(Font groupName, Font anyCategory, Font basicPrimaryCategory, Font anyCategoryBellowGenus, Font basicPrimaryCategoryBellowGenus)
            {
                _NameLabel.Font = groupName;

                for (int i = 0; i < _Buttons.Count; i++)
                {
                    Taxon taxon = _Buttons[i].Taxon;

                    TaxonomicCategory category = taxon.Category;
                    bool basicPrimary = category.IsBasicPrimaryCategory();
                    bool bellowGenus = (category.IsPrimaryCategory() || category.IsSecondaryCategory()) && taxon.GetInheritedBasicPrimaryCategory() <= TaxonomicCategory.Genus;

                    Font font = (bellowGenus ? (basicPrimary ? basicPrimaryCategoryBellowGenus : anyCategoryBellowGenus) : (basicPrimary ? basicPrimaryCategory : anyCategory));

                    _Buttons[i].Font = font;
                }
            }

            public void UpdateColor()
            {
                _NameLabel.ForeColor = (_DarkTheme ? Color.Black : Color.White);
                _NameLabel.BackColor = _ThemeColor.AtLightness_LAB(50).ToColor();

                for (int i = 0; i < _Buttons.Count; i++)
                {
                    _Buttons[i].IsDarkTheme = _DarkTheme;
                    _Buttons[i].ThemeColor = _ThemeColor;
                }
            }

            public void UpdateLayout(int groupWidth, int groupNameWidth, int categoryNameWidth, int buttonHeight, Padding buttonPadding)
            {
                if (_Buttons.Count > 0)
                {
                    _GroupPanel.Size = new Size(groupWidth, _Buttons.Count * buttonHeight + (_Buttons.Count - 1) * buttonPadding.Vertical);
                    _NameLabel.Size = new Size(groupNameWidth, _GroupPanel.Height);

                    for (int i = 0; i < _Buttons.Count; i++)
                    {
                        _Buttons[i].CategoryNameWidth = categoryNameWidth;
                        _Buttons[i].Size = new Size(_GroupPanel.Width - _NameLabel.Right - buttonPadding.Horizontal, buttonHeight);
                        _Buttons[i].Location = new Point(_NameLabel.Right + buttonPadding.Left, (i > 0 ? _Buttons[i - 1].Bottom + buttonPadding.Vertical : 0));
                    }
                }
                else
                {
                    _GroupPanel.Size = new Size(groupWidth, 0);
                }
            }

            public void UpdateControls()
            {
                _GroupPanel.Controls.Clear();

                _GroupPanel.Controls.Add(_NameLabel);

                for (int i = 0; i < _Buttons.Count; i++)
                {
                    _GroupPanel.Controls.Add(_Buttons[i]);
                }
            }
        }

        private List<_Group> _Groups = new List<_Group>();

        private int _GroupNameWidth = 30; // 组名称宽度。
        private int _CategoryNameWidth = 50; // 分类阶元名称宽度。
        private int _ButtonHeight = 22; // 按钮高度。
        private Padding _ButtonPadding = new Padding(4, 1, 0, 1); // 按钮外边距。
        private Padding _GroupPadding = new Padding(0, 2, 0, 2); // 组外边距。
        private bool _DarkTheme = false; // 是否为暗色主题。

        private bool _Editing = false; // 是否正在编辑。

        public TaxonNameButtonGroup()
        {
            InitializeComponent();

            this.Load += TaxonNameGroup_Load;
            this.SizeChanged += TaxonNameGroup_SizeChanged;
            this.AutoSizeChanged += TaxonNameGroup_AutoSizeChanged;
            this.FontChanged += TaxonNameGroup_FontChanged;
        }

        private void TaxonNameGroup_Load(object sender, EventArgs e)
        {
            _UpdateGroupFont();
            _UpdateGroupColor();
            _UpdateGroupControls();
            _UpdateGroupLayout();
        }

        private void TaxonNameGroup_SizeChanged(object sender, EventArgs e)
        {
            if (!_Editing)
            {
                _UpdateGroupLayout();
            }
        }

        private void TaxonNameGroup_AutoSizeChanged(object sender, EventArgs e)
        {
            if (!_Editing)
            {
                _AutoSizeChanged();
            }
        }

        private void TaxonNameGroup_FontChanged(object sender, EventArgs e)
        {
            if (!_Editing)
            {
                _UpdateGroupFont();
            }
        }

        private void _AutoSizeChanged()
        {
            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        private void _UpdateGroupFont()
        {
            FontFamily family = this.Font.FontFamily;
            float emSize = this.Font.Size;
            GraphicsUnit unit = GraphicsUnit.Point;
            byte gdiCharSet = (byte)134;

            Font groupName = new Font(family, emSize, FontStyle.Regular, unit, gdiCharSet);
            Font anyCategory = new Font(family, emSize, FontStyle.Regular, unit, gdiCharSet);
            Font basicPrimaryCategory = new Font(family, emSize, FontStyle.Bold, unit, gdiCharSet);
            Font anyCategoryBellowGenus = new Font(family, emSize, FontStyle.Italic, unit, gdiCharSet);
            Font basicPrimaryCategoryBellowGenus = new Font(family, emSize, FontStyle.Bold | FontStyle.Italic, unit, gdiCharSet);

            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].UpdateFont(groupName, anyCategory, basicPrimaryCategory, anyCategoryBellowGenus, basicPrimaryCategoryBellowGenus);
            }
        }

        private void _UpdateGroupColor()
        {
            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].IsDarkTheme = _DarkTheme;
                _Groups[i].UpdateColor();
            }
        }

        private void _UpdateGroupControls()
        {
            Panel_Main.Controls.Clear();

            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].UpdateControls();

                Panel_Main.Controls.Add(_Groups[i].GroupPanel);
            }
        }

        private void _UpdateGroupLayout()
        {
            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].UpdateLayout(this.Width - _GroupPadding.Horizontal, _GroupNameWidth, _CategoryNameWidth, _ButtonHeight, _ButtonPadding);
            }

            for (int i = 0; i < _Groups.Count; i++)
            {
                _Groups[i].GroupPanel.Location = new Point(_GroupPadding.Left, (i > 0 ? _Groups[i - 1].GroupPanel.Bottom + _GroupPadding.Vertical : 0));
            }

            if (this.AutoSize)
            {
                _AutoSize();
            }
        }

        private void _AutoSize()
        {
            this.Height = (_Groups.Count > 0 ? _Groups[_Groups.Count - 1].GroupPanel.Bottom : 0);
        }

        public int GroupNameWidth
        {
            get
            {
                return _GroupNameWidth;
            }

            set
            {
                _GroupNameWidth = value;

                if (!_Editing)
                {
                    _UpdateGroupLayout();
                }
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

                if (!_Editing)
                {
                    _UpdateGroupLayout();
                }
            }
        }

        public int ButtonHeight
        {
            get
            {
                return _ButtonHeight;
            }

            set
            {
                _ButtonHeight = value;

                if (!_Editing)
                {
                    _UpdateGroupLayout();
                }
            }
        }

        public Padding ButtonPadding
        {
            get
            {
                return _ButtonPadding;
            }

            set
            {
                _ButtonPadding = value;

                if (!_Editing)
                {
                    _UpdateGroupLayout();
                }
            }
        }

        public Padding GroupPadding
        {
            get
            {
                return _GroupPadding;
            }

            set
            {
                _GroupPadding = value;

                if (!_Editing)
                {
                    _UpdateGroupLayout();
                }
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

                if (!_Editing)
                {
                    _UpdateGroupColor();
                }
            }
        }

        // 获取组数目。
        public int GroupCount()
        {
            return _Groups.Count;
        }

        // 获取指定组的按钮数目。
        public int ButtonCount(int groupIndex)
        {
            return _Groups[groupIndex].Buttons.Count;
        }

        // 添加一个组。
        public void AddGroup(string name, ColorX color, int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Insert(groupIndex, new _Group(name, color, _DarkTheme));
        }

        // 添加一个组。
        public void AddGroup(string name, ColorX color)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Add(new _Group(name, color, _DarkTheme));
        }

        // 删除一个组。
        public void RemoveGroup(int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.RemoveAt(groupIndex);
        }

        // 添加一个按钮。
        public void AddButton(TaxonNameButton button, int groupIndex, int buttonIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.Insert(buttonIndex, button);
        }

        // 添加一个按钮。
        public void AddButton(TaxonNameButton button, int groupIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.Add(button);
        }

        // 删除一个按钮。
        public void RemoveButton(int groupIndex, int buttonIndex)
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups[groupIndex].Buttons.RemoveAt(buttonIndex);
        }

        // 删除所有组与按钮。
        public void Clear()
        {
            if (!_Editing)
            {
                throw new InvalidOperationException();
            }

            //

            _Groups.Clear();
        }

        // 开始编辑。
        public void StartEditing()
        {
            if (!_Editing)
            {
                _Editing = true;
            }
        }

        // 完成编辑。
        public void FinishEditing()
        {
            if (_Editing)
            {
                Panel_Main.Visible = false;

                _UpdateGroupFont();
                _UpdateGroupColor();
                _UpdateGroupControls();
                _UpdateGroupLayout();

                Panel_Main.Visible = true;

                _Editing = false;
            }
        }
    }
}