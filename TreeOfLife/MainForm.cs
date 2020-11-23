/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
Copyright © 2020 chibayuki@foxmail.com

生命树 (TreeOfLife)
Version 1.0.305.1000.M4.201120-0000

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

using ColorManipulation = Com.ColorManipulation;
using ColorX = Com.ColorX;
using FormManager = Com.WinForm.FormManager;
using Theme = Com.WinForm.Theme;

namespace TreeOfLife
{
    internal partial class MainForm : Form
    {
        private bool _IsDarkTheme;

        #region 窗口定义

        private FormManager Me;

        public FormManager FormManager
        {
            get
            {
                return Me;
            }
        }

        private void _Ctor(FormManager owner)
        {
            InitializeComponent();

            //

            if (owner != null)
            {
                Me = new FormManager(this, owner);
            }
            else
            {
                Me = new FormManager(this);
            }

            //

            FormDefine();
        }

        public MainForm()
        {
            _Ctor(null);
        }

        public MainForm(FormManager owner)
        {
            _Ctor(owner);
        }

        private void FormDefine()
        {
            Me.Caption = Application.ProductName;
            Me.ShowCaptionBarColor = true;
            Me.EnableCaptionBarTransparent = true;
            Me.Theme = Theme.White;
            Me.ThemeColor = Color.FromArgb(128, 128, 128);

            Me.Loading += Me_Loading;
            Me.Loaded += Me_Loaded;
            Me.Closing += Me_Closing;
            Me.Closed += Me_Closed;
            Me.Resize += Me_Resize;
            Me.SizeChanged += Me_SizeChanged;
            Me.ThemeChanged += Me_ThemeChanged;
            Me.ThemeColorChanged += Me_ThemeChanged;
        }

        #endregion

        #region 窗口事件回调

        private void Me_Loading(object sender, EventArgs e)
        {
            Phylogenesis.Open(_FileName);
        }

        private void Me_Loaded(object sender, EventArgs e)
        {
            Me.OnThemeChanged();
            Me.OnSizeChanged();

            //

            _AddEditModeEvents();

            //

            TaxonNameButtonGroup_ViewMode_Parent.AutoSize = true;
            TaxonNameButtonGroup_ViewMode_Children.AutoSize = true;

            TaxonNameButtonGroup_EditMode_Parent.AutoSize = true;
            TaxonNameButtonGroup_EditMode_Children.AutoSize = true;

            //

            CategorySelector_EditMode_Category.SizeChanged += (_s, _e) => _UpdateEditModeLayout();

            //

            //_SetCurrentTaxon(Phylogenesis.Root);

            Taxon taxon = Phylogenesis.Root;
            while (taxon.Children.Count > 0)
            {
                if (taxon.ChineseName == "合弓纲")
                {
                    break;
                }

                taxon = taxon.Children[0];
            }
            _SetCurrentTaxon(taxon);

            //

            Panel_Main.Visible = true;
        }

        private void Me_Closing(object sender, EventArgs e)
        {
            Phylogenesis.Save();
        }

        private void Me_Closed(object sender, EventArgs e)
        {

        }

        private void Me_Resize(object sender, EventArgs e)
        {

        }

        private void Me_SizeChanged(object sender, EventArgs e)
        {
            Me.OnResize();
        }

        private void Me_ThemeChanged(object sender, EventArgs e)
        {
            this.BackColor = Me.RecommendColors.FormBackground.ToColor();

            //

            _IsDarkTheme = (Me.Theme == Theme.DarkGray || Me.Theme == Theme.Black);

            //

            Panel_TaxonInfo.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            //

            Label_ViewMode_Synonym.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Synonym.BackColor = Me.RecommendColors.Background.ToColor();

            Label_ViewMode_Synonym_Value.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_ViewMode_Tag.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Tag.BackColor = Me.RecommendColors.Background.ToColor();

            Label_ViewMode_Tag_Value.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_ViewMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Desc.BackColor = Me.RecommendColors.Background.ToColor();

            Label_ViewMode_Desc_Value.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_ViewMode_Parent.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Parent.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_ViewMode_Parent.IsDarkTheme = _IsDarkTheme;

            Label_ViewMode_Children.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Children.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_ViewMode_Children.IsDarkTheme = _IsDarkTheme;

            //

            Label_EditMode_Name.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Name.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Name.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Name.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_ChsName.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_ChsName.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_ChsName.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_ChsName.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_State.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_State.BackColor = Me.RecommendColors.Background.ToColor();

            CheckBox_EditMode_EX.ForeColor = Me.RecommendColors.Text.ToColor();
            CheckBox_EditMode_Doubt.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_EditMode_Category.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Category.BackColor = Me.RecommendColors.Background.ToColor();

            CategorySelector_EditMode_Category.IsDarkTheme = _IsDarkTheme;

            Label_EditMode_Synonym.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Synonym.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Synonym.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Synonym.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Tag.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Tag.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Tag.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Tag.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Desc.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Desc.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_ParseCurrent.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_ParseCurrent.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_ParseCurrent.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_ParseCurrent.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_ParseChildren.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_ParseChildren.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_ParseChildren.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_ParseChildren.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Parent.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Parent.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_EditMode_Parent.IsDarkTheme = _IsDarkTheme;

            Label_EditMode_Children.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Children.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_EditMode_Children.IsDarkTheme = _IsDarkTheme;
        }

        #endregion

        #region 类群信息页面

        private const string _FileName = @".\Phylogenesis.json";

        private bool _EditMode = false; // 是否为编辑模式。

        // 进入/退出编辑模式。
        private void _SetMode(bool editMode)
        {
            _EditMode = editMode;

            Panel_TaxonInfo_EditMode.Visible = _EditMode;
            Panel_TaxonInfo_ViewMode.Visible = !_EditMode;

            //

            _UpdateCurrentTaxonInfo();
        }

        private Taxon _CurrentTaxon = null; // 当前选择的类群。

        // 设置当前选择的类群。
        private void _SetCurrentTaxon(Taxon taxon)
        {
            if (_CurrentTaxon != taxon)
            {
                _CurrentTaxon = taxon;

                _UpdateCurrentTaxonInfo();
            }
        }

        // 更新父类群控件。
        private void _UpdateParents(IReadOnlyList<Taxon> parents, TaxonNameButtonGroup control)
        {
            control.StartEditing();
            control.Clear();

            int groupIndex = 0;
            TaxonomicCategory categoryOfGroup = TaxonomicCategory.Unranked;

            for (int i = 0; i < parents.Count; i++)
            {
                Taxon taxon = parents[i];

                if (i == 0)
                {
                    categoryOfGroup = taxon.Category.BasicCategory();

                    control.AddGroup(((categoryOfGroup.IsPrimaryCategory() || categoryOfGroup.IsSecondaryCategory()) ? categoryOfGroup.Name() : string.Empty), taxon.GetThemeColor());
                }
                else
                {
                    TaxonomicCategory basicCategory = taxon.GetInheritedBasicCategory();

                    if (categoryOfGroup != basicCategory)
                    {
                        categoryOfGroup = basicCategory;

                        control.AddGroup(categoryOfGroup.Name(), taxon.GetThemeColor());

                        groupIndex++;
                    }
                }

                TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };
                button.Click += (s, e) => _SetCurrentTaxon(taxon);

                if (taxon == _CurrentTaxon)
                {
                    button.Checked = true;
                }

                control.AddButton(button, groupIndex);
            }

            control.FinishEditing();
        }

        // 更新子类群控件。
        private void _UpdateChildren(IReadOnlyList<Taxon> children, TaxonNameButtonGroup control)
        {
            control.StartEditing();
            control.Clear();

            for (int i = 0; i < children.Count; i++)
            {
                Taxon taxon = children[i];

                control.AddGroup(string.Empty, taxon.GetThemeColor());

                TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };
                button.Click += (s, e) => _SetCurrentTaxon(taxon);

                control.AddButton(button, i);
            }

            control.FinishEditing();
        }

        // 更新当前类群的所有信息。
        private void _UpdateCurrentTaxonInfo()
        {
            if (_EditMode)
            {
                _UpdateEditModeParents();
                _UpdateEditModeChildren();

                _UpdateEditModeLayout();

                //

                _RemoveEditModeEvents();

                _UpdateEditModeInfo();

                _AddEditModeEvents();
            }
            else
            {
                _UpdateViewModeInfo();
                _UpdateViewModeParents();
                _UpdateViewModeChildren();

                _UpdateViewModeLayout();
            }
        }

        #region 查看模式

        // 更新所有信息。
        private void _UpdateViewModeInfo()
        {
            if (!_CurrentTaxon.IsRoot)
            {
                Label_ViewMode_CategoryName.Text = (_CurrentTaxon.IsAnonymous() ? string.Empty : _CurrentTaxon.Category.Name());
                Label_ViewMode_TaxonName.Text = _CurrentTaxon.ShortName('\n');

                ColorX taxonColor = _CurrentTaxon.GetThemeColor();

                Label_ViewMode_CategoryName.ForeColor = (_IsDarkTheme ? Color.Black : Color.White);
                Label_ViewMode_CategoryName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

                Label_ViewMode_TaxonName.ForeColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToColor();
                Label_ViewMode_TaxonName.BackColor = taxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToColor();

                //

                if (_CurrentTaxon.Synonym.Count > 0)
                {
                    StringBuilder synonym = new StringBuilder();

                    foreach (var item in _CurrentTaxon.Synonym)
                    {
                        synonym.AppendLine(item);
                    }

                    Label_ViewMode_Synonym_Value.Text = synonym.ToString();
                }

                //

                if (_CurrentTaxon.Tag.Count > 0)
                {
                    StringBuilder tag = new StringBuilder();

                    foreach (var item in _CurrentTaxon.Tag)
                    {
                        tag.Append(item);
                        tag.Append("  ");
                    }

                    Label_ViewMode_Tag_Value.Text = tag.ToString();
                }

                //

                Label_ViewMode_Desc_Value.Text = _CurrentTaxon.Description;
            }
        }

        // 更新父类群。
        private void _UpdateViewModeParents()
        {
            if (_CurrentTaxon.IsRoot)
            {
                TaxonNameButtonGroup_ViewMode_Parent.StartEditing();
                TaxonNameButtonGroup_ViewMode_Parent.Clear();
                TaxonNameButtonGroup_ViewMode_Parent.FinishEditing();
            }
            else
            {
                var parents = _CurrentTaxon.GetSummaryParents(_EditMode);

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }
                else
                {
                    parents.Add(_CurrentTaxon.Parent);
                }

                parents.Add(_CurrentTaxon);

                _UpdateParents(parents, TaxonNameButtonGroup_ViewMode_Parent);
            }
        }

        // 更新子类群。
        private void _UpdateViewModeChildren()
        {
            var children = _CurrentTaxon.GetNamedChildren(true);

            if (children.Count > 0)
            {
                _UpdateChildren(children, TaxonNameButtonGroup_ViewMode_Children);
            }
        }

        // 更新布局。
        private void _UpdateViewModeLayout()
        {
            Panel_ViewMode_Title.Height = (_CurrentTaxon.IsRoot ? 0 : Label_ViewMode_TaxonName.Bottom);

            Label_ViewMode_Tag.Height = Label_ViewMode_Tag_Value.Height;
            Panel_ViewMode_Tag.Height = (_CurrentTaxon.Tag.Count <= 0 ? 0 : Label_ViewMode_Tag_Value.Bottom);
            Panel_ViewMode_Tag.Top = Panel_ViewMode_Title.Bottom;

            Panel_ViewMode_Parent.Height = (_CurrentTaxon.IsRoot ? 0 : TaxonNameButtonGroup_ViewMode_Parent.Bottom);
            Panel_ViewMode_Parent.Top = Panel_ViewMode_Tag.Bottom;

            Panel_ViewMode_Children.Height = (_CurrentTaxon.Children.Count <= 0 ? 0 : TaxonNameButtonGroup_ViewMode_Children.Bottom);
            Panel_ViewMode_Children.Top = Panel_ViewMode_Parent.Bottom;

            Label_ViewMode_Synonym.Height = Label_ViewMode_Synonym_Value.Height;
            Panel_ViewMode_Synonym.Height = (_CurrentTaxon.Synonym.Count <= 0 ? 0 : Label_ViewMode_Synonym_Value.Bottom);
            Panel_ViewMode_Synonym.Top = Panel_ViewMode_Children.Bottom;

            Label_ViewMode_Desc.Height = Label_ViewMode_Desc_Value.Height;
            Panel_ViewMode_Desc.Height = (string.IsNullOrWhiteSpace(_CurrentTaxon.Description) ? 0 : Label_ViewMode_Desc_Value.Bottom);
            Panel_ViewMode_Desc.Top = Panel_ViewMode_Synonym.Bottom;

            Button_EnterEditMode.Top = Panel_ViewMode_Desc.Bottom + 25;
        }

        private void Button_EnterEditMode_Click(object sender, EventArgs e)
        {
            _SetMode(true);
        }

        #endregion

        #region 编辑模式

        // 更新所有信息。
        private void _UpdateEditModeInfo()
        {
            TextBox_EditMode_Name.Text = _CurrentTaxon.BotanicalName;
            TextBox_EditMode_ChsName.Text = _CurrentTaxon.ChineseName;
            CheckBox_EditMode_EX.Checked = _CurrentTaxon.IsExtinct;
            CheckBox_EditMode_Doubt.Checked = _CurrentTaxon.InDoubt;
            CategorySelector_EditMode_Category.Category = _CurrentTaxon.Category;
            TextBox_EditMode_Synonym.Lines = _CurrentTaxon.Synonym.ToArray();
            TextBox_EditMode_Tag.Lines = _CurrentTaxon.Tag.ToArray();
            TextBox_EditMode_Desc.Text = _CurrentTaxon.Description;
            TextBox_EditMode_ParseCurrent.Text = string.Empty;
            TextBox_EditMode_ParseChildren.Text = string.Empty;
        }

        // 更新父类群。
        private void _UpdateEditModeParents()
        {
            if (_CurrentTaxon.IsRoot)
            {
                TaxonNameButtonGroup_EditMode_Parent.StartEditing();
                TaxonNameButtonGroup_EditMode_Parent.Clear();
                TaxonNameButtonGroup_EditMode_Parent.FinishEditing();
            }
            else
            {
                var parents = _CurrentTaxon.GetSummaryParents(_EditMode);

                if (parents.Count > 0)
                {
                    parents.Reverse();
                }
                else
                {
                    parents.Add(_CurrentTaxon.Parent);
                }

                _UpdateParents(parents, TaxonNameButtonGroup_EditMode_Parent);
            }
        }

        // 更新子类群。
        private void _UpdateEditModeChildren()
        {
            var children = _CurrentTaxon.Children;

            if (children.Count > 0)
            {
                _UpdateChildren(children, TaxonNameButtonGroup_EditMode_Children);
            }
        }

        // 更新布局。
        private void _UpdateEditModeLayout()
        {
            Panel_EditMode_TaxonName.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_ChsName.Bottom);

            Panel_EditMode_State.Height = (_CurrentTaxon.IsRoot ? 0 : CheckBox_EditMode_Doubt.Bottom);
            Panel_EditMode_State.Top = Panel_EditMode_TaxonName.Bottom;

            Label_EditMode_Category.Height = CategorySelector_EditMode_Category.Height;
            Panel_EditMode_Category.Height = (_CurrentTaxon.IsRoot ? 0 : CategorySelector_EditMode_Category.Bottom);
            Panel_EditMode_Category.Top = Panel_EditMode_State.Bottom;

            Panel_EditMode_Synonym.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Synonym.Bottom);
            Panel_EditMode_Synonym.Top = Panel_EditMode_Category.Bottom;

            Panel_EditMode_Tag.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Tag.Bottom);
            Panel_EditMode_Tag.Top = Panel_EditMode_Synonym.Bottom;

            Panel_EditMode_Desc.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Desc.Bottom);
            Panel_EditMode_Desc.Top = Panel_EditMode_Tag.Bottom;

            Panel_EditMode_ParseCurrent.Height = 0;// (_CurrentTaxon.IsRoot ? 0 : Button_EditMode_ParseCurrent.Bottom);
            Panel_EditMode_ParseCurrent.Top = Panel_EditMode_Desc.Bottom;

            Panel_EditMode_ParseChildren.Top = Panel_EditMode_ParseCurrent.Bottom;

            Panel_EditMode_Parent.Height = (_CurrentTaxon.IsRoot ? 0 : TaxonNameButtonGroup_EditMode_Parent.Bottom);
            Panel_EditMode_Parent.Top = Panel_EditMode_ParseChildren.Bottom;

            Panel_EditMode_Children.Height = (_CurrentTaxon.Children.Count <= 0 ? 0 : TaxonNameButtonGroup_EditMode_Children.Bottom);
            Panel_EditMode_Children.Top = Panel_EditMode_Parent.Bottom;

            Button_EnterViewMode.Top = Panel_EditMode_Children.Bottom + 25;
        }

        // 添加订阅事件。
        private void _AddEditModeEvents()
        {
            TextBox_EditMode_Name.TextChanged += TextBox_EditMode_Name_TextChanged;
            TextBox_EditMode_ChsName.TextChanged += TextBox_EditMode_ChsName_TextChanged;
            CheckBox_EditMode_EX.CheckedChanged += CheckBox_EditMode_EX_CheckedChanged;
            CheckBox_EditMode_Doubt.CheckedChanged += CheckBox_EditMode_Doubt_CheckedChanged;
            CategorySelector_EditMode_Category.Click += CategorySelector_EditMode_Category_Click;
            TextBox_EditMode_Synonym.TextChanged += TextBox_EditMode_Synonym_TextChanged;
            TextBox_EditMode_Tag.TextChanged += TextBox_EditMode_Tag_TextChanged;
            TextBox_EditMode_Desc.TextChanged += TextBox_EditMode_Desc_TextChanged;
            Button_EditMode_ParseCurrent.Click += Button_EditMode_ParseCurrent_Click;
            Button_EditMode_ParseChildren.Click += Button_EditMode_ParseChildren_Click;
        }

        // 删除订阅事件。
        private void _RemoveEditModeEvents()
        {
            TextBox_EditMode_Name.TextChanged -= TextBox_EditMode_Name_TextChanged;
            TextBox_EditMode_ChsName.TextChanged -= TextBox_EditMode_ChsName_TextChanged;
            CheckBox_EditMode_EX.CheckedChanged -= CheckBox_EditMode_EX_CheckedChanged;
            CheckBox_EditMode_Doubt.CheckedChanged -= CheckBox_EditMode_Doubt_CheckedChanged;
            CategorySelector_EditMode_Category.Click -= CategorySelector_EditMode_Category_Click;
            TextBox_EditMode_Synonym.TextChanged -= TextBox_EditMode_Synonym_TextChanged;
            TextBox_EditMode_Tag.TextChanged -= TextBox_EditMode_Tag_TextChanged;
            TextBox_EditMode_Desc.TextChanged -= TextBox_EditMode_Desc_TextChanged;
            Button_EditMode_ParseCurrent.Click -= Button_EditMode_ParseCurrent_Click;
            Button_EditMode_ParseChildren.Click -= Button_EditMode_ParseChildren_Click;
        }

        private void TextBox_EditMode_Name_TextChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.BotanicalName = TextBox_EditMode_Name.Text.Trim();
        }

        private void TextBox_EditMode_ChsName_TextChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.ChineseName = TextBox_EditMode_ChsName.Text.Trim();
        }

        private void CheckBox_EditMode_EX_CheckedChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.IsExtinct = CheckBox_EditMode_EX.Checked;
        }

        private void CheckBox_EditMode_Doubt_CheckedChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.InDoubt = CheckBox_EditMode_Doubt.Checked;
        }

        private void CategorySelector_EditMode_Category_Click(object sender, EventArgs e)
        {
            _CurrentTaxon.Category = CategorySelector_EditMode_Category.Category;
        }

        private void TextBox_EditMode_Synonym_TextChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.Synonym.Clear();

            foreach (string synonym in TextBox_EditMode_Synonym.Lines)
            {
                _CurrentTaxon.Synonym.Add(synonym.Trim());
            }
        }

        private void TextBox_EditMode_Tag_TextChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.Tag.Clear();

            foreach (string tag in TextBox_EditMode_Tag.Lines)
            {
                _CurrentTaxon.Tag.Add(tag.Trim());
            }
        }

        private void TextBox_EditMode_Desc_TextChanged(object sender, EventArgs e)
        {
            _CurrentTaxon.Description = TextBox_EditMode_Desc.Text.Trim();
        }

        private void Button_EditMode_ParseCurrent_Click(object sender, EventArgs e)
        {
            _CurrentTaxon.ParseCurrent(TextBox_EditMode_ParseCurrent.Text);

            TextBox_EditMode_ParseCurrent.Clear();
        }

        private void Button_EditMode_ParseChildren_Click(object sender, EventArgs e)
        {
            _CurrentTaxon.ParseChildren(TextBox_EditMode_ParseChildren.Lines);

            TextBox_EditMode_ParseChildren.Clear();

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void Button_EnterViewMode_Click(object sender, EventArgs e)
        {
            _SetMode(false);
        }

        #endregion

        #endregion
    }
}