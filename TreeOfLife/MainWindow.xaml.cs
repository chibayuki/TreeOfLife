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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ColorX = Com.ColorX;
using FormManager = Com.WinForm.FormManager;
using Theme = Com.WinForm.Theme;

namespace TreeOfLife
{
    public partial class MainForm : Form
    {
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

            Me.Loaded += Me_Loaded;
            Me.ThemeChanged += Me_ThemeChanged;
            Me.ThemeColorChanged += Me_ThemeChanged;

            Me.CloseVerification = Me_CloseVerification;

            //

            Button_TabPage_File.Click += (s, e) => _SelectTabPage(TabPages.File);
            Button_TabPage_TaxonInfo.Click += (s, e) => _SelectTabPage(TabPages.TaxonInfo);
            Button_TabPage_Search.Click += (s, e) => _SelectTabPage(TabPages.Search);
            Button_TabPage_About.Click += (s, e) => _SelectTabPage(TabPages.About);

            //

            ToolStripMenuItem_File_Open.Click += ToolStripMenuItem_File_Open_Click;
            ToolStripMenuItem_File_Save.Click += ToolStripMenuItem_File_Save_Click;
            ToolStripMenuItem_File_SaveAs.Click += ToolStripMenuItem_File_SaveAs_Click;
            ToolStripMenuItem_File_Close.Click += ToolStripMenuItem_File_Close_Click;

            ToolStripMenuItem_Parents_Select.Click += ToolStripMenuItem_Parents_Select_Click;

            ToolStripMenuItem_Children_Select.Click += ToolStripMenuItem_Children_Select_Click;
            ToolStripMenuItem_Children_SetParent.Click += ToolStripMenuItem_Children_SetParent_Click;
            ToolStripMenuItem_Children_MoveTop.Click += ToolStripMenuItem_Children_MoveTop_Click;
            ToolStripMenuItem_Children_MoveUp.Click += ToolStripMenuItem_Children_MoveUp_Click;
            ToolStripMenuItem_Children_MoveDown.Click += ToolStripMenuItem_Children_MoveDown_Click;
            ToolStripMenuItem_Children_MoveBottom.Click += ToolStripMenuItem_Children_MoveBottom_Click;
            ToolStripMenuItem_Children_DeleteWithoutChildren.Click += ToolStripMenuItem_Children_DeleteWithoutChildren_Click;
            ToolStripMenuItem_Children_DeleteWithinChildren.Click += ToolStripMenuItem_Children_DeleteWithinChildren_Click;

            CategorySelector_EditMode_Category.SizeChanged += (_s, _e) => _UpdateEditModeLayout();

            Button_EditMode_AddParentUplevel.Click += Button_EditMode_AddParentUplevel_Click;
            Button_EditMode_AddParentDownlevel.Click += Button_EditMode_AddParentDownlevel_Click;
            Button_EditMode_AddChildren.Click += Button_EditMode_AddChildren_Click;
        }

        #endregion

        #region 窗口事件回调

        private void Me_Loaded(object sender, EventArgs e)
        {
            Me.OnThemeChanged();
            Me.OnSizeChanged();

            //

            TagGroup_Synonyms.AutoSize = true;
            TagGroup_ViewMode_Tags.AutoSize = true;

            TaxonNameButtonGroup_ViewMode_Parents.AutoSize = true;
            TaxonNameButtonGroup_ViewMode_Children.AutoSize = true;

            TaxonNameButtonGroup_EditMode_Parents.AutoSize = true;
            TaxonNameButtonGroup_EditMode_Children.AutoSize = true;

            //

            _SetCurrentTaxon(Phylogenesis.Root);
            _SetMode(false);

            //

            _SelectTabPage(TabPages.File);

            //

            Panel_Main.Visible = true;
        }

        private bool _IsDarkTheme;

        private void Me_ThemeChanged(object sender, EventArgs e)
        {
            this.BackColor = Me.RecommendColors.FormBackground.ToColor();

            //

            _IsDarkTheme = (Me.Theme == Theme.DarkGray || Me.Theme == Theme.Black);

            //

            Panel_TabPage_Pages.BackColor = Me.RecommendColors.Background_DEC.ToColor();
            Panel_TabPage_Buttons.BackColor = Me.RecommendColors.Main.ToColor();

            //

            Label_ViewMode_Synonyms.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Synonyms.BackColor = Me.RecommendColors.Background.ToColor();

            TagGroup_Synonyms.IsDarkTheme = _IsDarkTheme;

            Label_ViewMode_Tags.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Tags.BackColor = Me.RecommendColors.Background.ToColor();

            TagGroup_ViewMode_Tags.IsDarkTheme = _IsDarkTheme;

            Label_ViewMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Desc.BackColor = Me.RecommendColors.Background.ToColor();

            Label_ViewMode_Desc_Value.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_ViewMode_Parents.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_ViewMode_Parents.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_ViewMode_Parents.IsDarkTheme = _IsDarkTheme;

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
            CheckBox_EditMode_Unsure.ForeColor = Me.RecommendColors.Text.ToColor();

            Label_EditMode_Category.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Category.BackColor = Me.RecommendColors.Background.ToColor();

            CategorySelector_EditMode_Category.IsDarkTheme = _IsDarkTheme;

            Label_EditMode_Synonyms.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Synonyms.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Synonyms.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Synonyms.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Tags.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Tags.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Tags.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Tags.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Desc.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_Desc.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_Desc.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_AddParent.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_AddParent.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_AddParent.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_AddParent.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_AddChildren.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_AddChildren.BackColor = Me.RecommendColors.Background.ToColor();

            TextBox_EditMode_AddChildren.ForeColor = Me.RecommendColors.Text.ToColor();
            TextBox_EditMode_AddChildren.BackColor = Me.RecommendColors.Background_DEC.ToColor();

            Label_EditMode_Parents.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Parents.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_EditMode_Parents.IsDarkTheme = _IsDarkTheme;

            Label_EditMode_Children.ForeColor = Me.RecommendColors.Text.ToColor();
            Label_EditMode_Children.BackColor = Me.RecommendColors.Background.ToColor();

            TaxonNameButtonGroup_EditMode_Children.IsDarkTheme = _IsDarkTheme;
        }

        private bool Me_CloseVerification(EventArgs e)
        {
            return _TrySaveAndClose();
        }

        #endregion

        #region 选项卡

        private enum TabPages
        {
            None = -1,
            File,
            TaxonInfo,
            Search,
            About
        }

        private TabPages _TabPages = TabPages.None;

        private void _SelectTabPage(TabPages tabPage)
        {
            if (_TabPages != tabPage)
            {
                _TabPages = tabPage;

                Panel_File.Visible = (_TabPages == TabPages.File);
                Panel_TaxonInfo.Visible = (_TabPages == TabPages.TaxonInfo);
                Panel_Search.Visible = (_TabPages == TabPages.Search);
                Panel_About.Visible = (_TabPages == TabPages.About);
            }
        }

        #endregion

        #region 文件选项卡

        private bool _Open()
        {
            bool result = true;

            DialogResult dr = OpenFileDialog_Open.ShowDialog();

            if (dr == DialogResult.OK)
            {
                result = Phylogenesis.Open(OpenFileDialog_Open.FileName);
            }

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _SetMode(false);
                _UpdatePhylogeneticTree();
            }

            return result;
        }

        private bool _Save()
        {
            bool result = true;

            if (_EditMode)
            {
                _ApplyEditModeInfo();
            }

            if (!string.IsNullOrEmpty(Phylogenesis.FileName))
            {
                result = Phylogenesis.Save();
            }
            else
            {
                DialogResult dr = SaveFileDialog_SaveAs.ShowDialog();

                if (dr == DialogResult.OK)
                {
                    result = Phylogenesis.SaveAs(SaveFileDialog_SaveAs.FileName);
                }
            }

            return result;
        }

        private bool _SaveAs()
        {
            bool result = true;

            DialogResult dr = SaveFileDialog_SaveAs.ShowDialog();

            if (dr == DialogResult.OK)
            {
                if (_EditMode)
                {
                    _ApplyEditModeInfo();
                }

                result = Phylogenesis.SaveAs(SaveFileDialog_SaveAs.FileName);
            }

            return result;
        }

        private bool _Close()
        {
            bool result = true;

            result = Phylogenesis.Close();

            if (result)
            {
                _SetCurrentTaxon(Phylogenesis.Root);
                _SetMode(false);
                _UpdatePhylogeneticTree();
            }

            return result;
        }

        private bool _TrySaveAndClose()
        {
            if (string.IsNullOrEmpty(Phylogenesis.FileName) && Phylogenesis.IsEmpty)
            {
                return true;
            }
            else
            {
                DialogResult dr = MessageBox.Show("是否保存?", Application.ProductName, MessageBoxButtons.YesNoCancel);

                switch (dr)
                {
                    case DialogResult.Cancel: return false;

                    case DialogResult.Yes:
                        if (_Save())
                        {
                            if (_Close())
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("关闭失败。", Application.ProductName, MessageBoxButtons.OK);

                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("保存失败。", Application.ProductName, MessageBoxButtons.OK);

                            return false;
                        }

                    case DialogResult.No:
                        if (_Close())
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("关闭失败。", Application.ProductName, MessageBoxButtons.OK);

                            return false;
                        }

                    default: return false;
                }
            }
        }

        private void ToolStripMenuItem_File_Open_Click(object sender, EventArgs e)
        {
            if (_TrySaveAndClose())
            {
                if (_Open())
                {
                    _SelectTabPage(TabPages.TaxonInfo);
                }
                else
                {
                    MessageBox.Show("打开失败。", Application.ProductName, MessageBoxButtons.OK);
                }
            }
        }

        private void ToolStripMenuItem_File_Save_Click(object sender, EventArgs e)
        {
            if (!_Save())
            {
                MessageBox.Show("保存失败。", Application.ProductName, MessageBoxButtons.OK);
            }
        }

        private void ToolStripMenuItem_File_SaveAs_Click(object sender, EventArgs e)
        {
            if (!_SaveAs())
            {
                MessageBox.Show("保存失败。", Application.ProductName, MessageBoxButtons.OK);
            }
        }

        private void ToolStripMenuItem_File_Close_Click(object sender, EventArgs e)
        {
            _TrySaveAndClose();
        }

        #endregion

        #region 类群信息选项卡

        private bool _EditMode = false; // 是否为编辑模式。

        // 进入/退出编辑模式。
        private void _SetMode(bool editMode)
        {
            if (_EditMode != editMode)
            {
                _EditMode = editMode;

                Panel_TaxonInfo_EditMode.Visible = _EditMode;
                Panel_TaxonInfo_ViewMode.Visible = !_EditMode;

                //

                _UpdateCurrentTaxonInfo();

                //

                if (!_EditMode)
                {
                    _UpdatePhylogeneticTree();
                }
            }
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
        private void _UpdateParents(IReadOnlyList<Taxon> parents, bool editMode)
        {
            TaxonNameButtonGroup control = (editMode ? TaxonNameButtonGroup_EditMode_Parents : TaxonNameButtonGroup_ViewMode_Parents);
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

                if (editMode)
                {
                    button.Click += (s, e) => { _ApplyEditModeInfo(); _SetCurrentTaxon(taxon); };

                    button.MouseClick += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            _ShowMenuItem_Parents(button.Taxon);
                        }
                    };
                }
                else
                {
                    button.Click += (s, e) => _SetCurrentTaxon(taxon);
                }

                if (taxon == _CurrentTaxon)
                {
                    button.Checked = true;
                }

                control.AddButton(button, groupIndex);
            }

            control.FinishEditing();
        }

        // 更新子类群控件。
        private void _UpdateChildren(IReadOnlyList<Taxon> children, bool editMode)
        {
            TaxonNameButtonGroup control = (editMode ? TaxonNameButtonGroup_EditMode_Children : TaxonNameButtonGroup_ViewMode_Children);
            control.StartEditing();
            control.Clear();

            for (int i = 0; i < children.Count; i++)
            {
                Taxon taxon = children[i];

                control.AddGroup(string.Empty, taxon.GetThemeColor());

                TaxonNameButton button = new TaxonNameButton() { Taxon = taxon };

                if (editMode)
                {
                    button.Click += (s, e) => { _ApplyEditModeInfo(); _SetCurrentTaxon(taxon); };

                    button.MouseClick += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            _ShowMenuItem_Children(button.Taxon);
                        }
                    };
                }
                else
                {
                    button.Click += (s, e) => _SetCurrentTaxon(taxon);
                }

                control.AddButton(button, i);
            }

            control.FinishEditing();
        }

        // 更新当前类群的所有信息。
        private void _UpdateCurrentTaxonInfo()
        {
            if (_EditMode)
            {
                _EditModeRemoveEvent();

                _UpdateEditModeInfo();
                _UpdateEditModeParents();
                _UpdateEditModeChildren();

                _UpdateEditModeLayout();

                _EditModeAddEvent();
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
                ColorX taxonColor = _CurrentTaxon.GetThemeColor();

                //

                Label_ViewMode_CategoryName.Text = (_CurrentTaxon.IsAnonymous() ? string.Empty : _CurrentTaxon.Category.Name());
                Label_ViewMode_TaxonName.Text = _CurrentTaxon.ShortName('\n');

                Panel_ViewMode_TaxonName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

                Label_ViewMode_CategoryName.ForeColor = (_IsDarkTheme ? Color.Black : Color.White);
                Label_ViewMode_CategoryName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

                Label_ViewMode_TaxonName.ForeColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToColor();
                Label_ViewMode_TaxonName.BackColor = taxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToColor();

                //

                TagGroup_Synonyms.ThemeColor = taxonColor;
                TagGroup_Synonyms.Tags = _CurrentTaxon.Synonyms.ToArray();

                //

                TagGroup_ViewMode_Tags.ThemeColor = taxonColor;
                TagGroup_ViewMode_Tags.Tags = _CurrentTaxon.Tags.ToArray();

                //

                Label_ViewMode_Desc_Value.Text = _CurrentTaxon.Description;
            }
        }

        // 更新父类群。
        private void _UpdateViewModeParents()
        {
            if (_CurrentTaxon.IsRoot)
            {
                TaxonNameButtonGroup_ViewMode_Parents.StartEditing();
                TaxonNameButtonGroup_ViewMode_Parents.Clear();
                TaxonNameButtonGroup_ViewMode_Parents.FinishEditing();
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

                _UpdateParents(parents, false);
            }
        }

        // 更新子类群。
        private void _UpdateViewModeChildren()
        {
            var children = _CurrentTaxon.GetNamedChildren(true);

            if (children.Count > 0)
            {
                _UpdateChildren(children, false);
            }
        }

        // 更新布局。
        private void _UpdateViewModeLayout()
        {
            Panel_ViewMode_Title.Height = (_CurrentTaxon.IsRoot ? 0 : Panel_ViewMode_TaxonName.Bottom);

            Label_ViewMode_Tags.Height = TagGroup_ViewMode_Tags.Height;
            Panel_ViewMode_Tags.Height = (_CurrentTaxon.Tags.Count <= 0 ? 0 : TagGroup_ViewMode_Tags.Bottom);
            Panel_ViewMode_Tags.Top = Panel_ViewMode_Title.Bottom;

            Panel_ViewMode_Parents.Height = (_CurrentTaxon.IsRoot ? 0 : TaxonNameButtonGroup_ViewMode_Parents.Bottom);
            Panel_ViewMode_Parents.Top = Panel_ViewMode_Tags.Bottom;

            Panel_ViewMode_Children.Height = (_CurrentTaxon.IsFinal ? 0 : TaxonNameButtonGroup_ViewMode_Children.Bottom);
            Panel_ViewMode_Children.Top = Panel_ViewMode_Parents.Bottom;

            Panel_ViewMode_Synonyms.Height = (_CurrentTaxon.Synonyms.Count <= 0 ? 0 : TagGroup_Synonyms.Bottom);
            Panel_ViewMode_Synonyms.Top = Panel_ViewMode_Children.Bottom;

            Label_ViewMode_Desc.Height = Label_ViewMode_Desc_Value.Height;
            Panel_ViewMode_Desc.Height = (string.IsNullOrWhiteSpace(_CurrentTaxon.Description) ? 0 : Label_ViewMode_Desc_Value.Bottom);
            Panel_ViewMode_Desc.Top = Panel_ViewMode_Synonyms.Bottom;

            Button_EnterEditMode.Top = Panel_ViewMode_Desc.Bottom + 25;

            Panel_TaxonInfo_ViewMode.Height = Button_EnterEditMode.Bottom + 25;
        }

        //

        private void Button_EnterEditMode_Click(object sender, EventArgs e)
        {
            _SetMode(true);
        }

        #endregion

        #region 编辑模式

        // 更新所有信息。
        private void _UpdateEditModeInfo()
        {
            ColorX taxonColor = _CurrentTaxon.GetThemeColor();

            Label_EditMode_CategoryName.Text = (_CurrentTaxon.IsAnonymous() ? string.Empty : _CurrentTaxon.Category.Name());
            Label_EditMode_TaxonName.Text = _CurrentTaxon.ShortName('\n');

            Panel_EditMode_TaxonName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

            Label_EditMode_CategoryName.ForeColor = (_IsDarkTheme ? Color.Black : Color.White);
            Label_EditMode_CategoryName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

            Label_EditMode_TaxonName.ForeColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToColor();
            Label_EditMode_TaxonName.BackColor = taxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToColor();

            //

            TextBox_EditMode_Name.Text = _CurrentTaxon.BotanicalName;
            TextBox_EditMode_ChsName.Text = _CurrentTaxon.ChineseName;
            CheckBox_EditMode_EX.Checked = _CurrentTaxon.IsExtinct;
            CheckBox_EditMode_Unsure.Checked = _CurrentTaxon.Unsure;
            CategorySelector_EditMode_Category.Category = _CurrentTaxon.Category;
            TextBox_EditMode_Synonyms.Lines = _CurrentTaxon.Synonyms.ToArray();
            TextBox_EditMode_Tags.Lines = _CurrentTaxon.Tags.ToArray();
            TextBox_EditMode_Desc.Text = _CurrentTaxon.Description;
            TextBox_EditMode_AddParent.Text = string.Empty;
            TextBox_EditMode_AddChildren.Text = string.Empty;
        }

        // 更新父类群。
        private void _UpdateEditModeParents()
        {
            if (_CurrentTaxon.IsRoot)
            {
                TaxonNameButtonGroup_EditMode_Parents.StartEditing();
                TaxonNameButtonGroup_EditMode_Parents.Clear();
                TaxonNameButtonGroup_EditMode_Parents.FinishEditing();
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

                _UpdateParents(parents, true);
            }
        }

        // 更新子类群。
        private void _UpdateEditModeChildren()
        {
            var children = _CurrentTaxon.Children;

            if (children.Count > 0)
            {
                _UpdateChildren(children, true);
            }
        }

        // 更新布局。
        private void _UpdateEditModeLayout()
        {
            Panel_EditMode_Title.Height = (_CurrentTaxon.IsRoot ? 0 : Panel_EditMode_TaxonName.Bottom);

            Panel_EditMode_Name.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_ChsName.Bottom);
            Panel_EditMode_Name.Top = Panel_EditMode_Title.Bottom;

            Panel_EditMode_State.Height = (_CurrentTaxon.IsRoot ? 0 : CheckBox_EditMode_Unsure.Bottom);
            Panel_EditMode_State.Top = Panel_EditMode_Name.Bottom;

            Label_EditMode_Category.Height = CategorySelector_EditMode_Category.Height;
            Panel_EditMode_Category.Height = (_CurrentTaxon.IsRoot ? 0 : CategorySelector_EditMode_Category.Bottom);
            Panel_EditMode_Category.Top = Panel_EditMode_State.Bottom;

            Panel_EditMode_Synonyms.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Synonyms.Bottom);
            Panel_EditMode_Synonyms.Top = Panel_EditMode_Category.Bottom;

            Panel_EditMode_Tags.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Tags.Bottom);
            Panel_EditMode_Tags.Top = Panel_EditMode_Synonyms.Bottom;

            Panel_EditMode_Desc.Height = (_CurrentTaxon.IsRoot ? 0 : TextBox_EditMode_Desc.Bottom);
            Panel_EditMode_Desc.Top = Panel_EditMode_Tags.Bottom;

            Panel_EditMode_Parents.Height = (_CurrentTaxon.IsRoot ? 0 : TaxonNameButtonGroup_EditMode_Parents.Bottom);
            Panel_EditMode_Parents.Top = Panel_EditMode_Desc.Bottom;

            Panel_EditMode_AddParent.Height = (_CurrentTaxon.IsRoot ? 0 : Button_EditMode_AddParentDownlevel.Bottom);
            Panel_EditMode_AddParent.Top = Panel_EditMode_Parents.Bottom;

            Panel_EditMode_Children.Height = (_CurrentTaxon.IsFinal ? 0 : TaxonNameButtonGroup_EditMode_Children.Bottom);
            Panel_EditMode_Children.Top = Panel_EditMode_AddParent.Bottom;

            Panel_EditMode_AddChildren.Top = Panel_EditMode_Children.Bottom;

            Button_EnterViewMode.Top = Panel_EditMode_AddChildren.Bottom + 25;

            Panel_TaxonInfo_EditMode.Height = Button_EnterViewMode.Bottom + 25;
        }

        // 应用所有信息。
        private void _ApplyEditModeInfo()
        {
            _CurrentTaxon.BotanicalName = TextBox_EditMode_Name.Text.Trim();
            _CurrentTaxon.ChineseName = TextBox_EditMode_ChsName.Text.Trim();
            _CurrentTaxon.IsExtinct = CheckBox_EditMode_EX.Checked;
            _CurrentTaxon.Unsure = CheckBox_EditMode_Unsure.Checked;
            _CurrentTaxon.Category = CategorySelector_EditMode_Category.Category;

            _CurrentTaxon.Synonyms.Clear();

            foreach (string synonym in TextBox_EditMode_Synonyms.Lines)
            {
                _CurrentTaxon.Synonyms.Add(synonym.Trim());
            }

            _CurrentTaxon.Tags.Clear();

            foreach (string tag in TextBox_EditMode_Tags.Lines)
            {
                _CurrentTaxon.Tags.Add(tag.Trim());
            }

            _CurrentTaxon.Description = TextBox_EditMode_Desc.Text.Trim();
        }

        // 编辑过程中更新标题信息。
        private void _UpdateEditModeTitle()
        {
            TaxonomicCategory category = CategorySelector_EditMode_Category.Category;

            ColorX taxonColor = (category.IsPrimaryCategory() || category.IsSecondaryCategory() ? category.GetThemeColor() : _CurrentTaxon.Parent.GetThemeColor());

            string name = TextBox_EditMode_Name.Text.Trim();
            string chsName = TextBox_EditMode_ChsName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(chsName))
            {
                Label_EditMode_CategoryName.Text = string.Empty;
                Label_EditMode_TaxonName.Text = "(未命名)";
            }
            else
            {
                Label_EditMode_CategoryName.Text = category.Name();

                StringBuilder taxonName = new StringBuilder();

                if (CheckBox_EditMode_Unsure.Checked || CheckBox_EditMode_EX.Checked)
                {
                    if (CheckBox_EditMode_Unsure.Checked)
                    {
                        taxonName.Append('?');
                    }

                    if (CheckBox_EditMode_EX.Checked)
                    {
                        taxonName.Append('†');
                    }

                    taxonName.Append(' ');
                }

                if (!string.IsNullOrWhiteSpace(chsName))
                {
                    taxonName.Append(chsName);

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        taxonName.Append('\n');
                        taxonName.Append(name);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(name))
                {
                    taxonName.Append(name);
                }

                Label_EditMode_TaxonName.Text = taxonName.ToString();
            }

            Panel_EditMode_TaxonName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

            Label_EditMode_CategoryName.ForeColor = (_IsDarkTheme ? Color.Black : Color.White);
            Label_EditMode_CategoryName.BackColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 30 : 70).ToColor();

            Label_EditMode_TaxonName.ForeColor = taxonColor.AtLightness_LAB(_IsDarkTheme ? 60 : 40).ToColor();
            Label_EditMode_TaxonName.BackColor = taxonColor.AtLightness_HSL(_IsDarkTheme ? 10 : 90).ToColor();
        }

        // 注册编辑过程中用于更新标题信息的事件。
        private void _EditModeAddEvent()
        {
            TextBox_EditMode_Name.TextChanged += TextBox_EditMode_Name_TextChanged;
            TextBox_EditMode_ChsName.TextChanged += TextBox_EditMode_ChsName_TextChanged;
            CheckBox_EditMode_EX.CheckedChanged += CheckBox_EditMode_EX_CheckedChanged;
            CheckBox_EditMode_Unsure.CheckedChanged += CheckBox_EditMode_Unsure_CheckedChanged;
            CategorySelector_EditMode_Category.Click += CategorySelector_EditMode_Category_Click;
        }

        // 注册编辑过程中用于更新标题信息的事件。
        private void _EditModeRemoveEvent()
        {
            TextBox_EditMode_Name.TextChanged -= TextBox_EditMode_Name_TextChanged;
            TextBox_EditMode_ChsName.TextChanged -= TextBox_EditMode_ChsName_TextChanged;
            CheckBox_EditMode_EX.CheckedChanged -= CheckBox_EditMode_EX_CheckedChanged;
            CheckBox_EditMode_Unsure.CheckedChanged -= CheckBox_EditMode_Unsure_CheckedChanged;
            CategorySelector_EditMode_Category.Click -= CategorySelector_EditMode_Category_Click;
        }

        private void CategorySelector_EditMode_Category_Click(object sender, EventArgs e)
        {
            _UpdateEditModeTitle();
        }

        private void CheckBox_EditMode_Unsure_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateEditModeTitle();
        }

        private void CheckBox_EditMode_EX_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateEditModeTitle();
        }

        private void TextBox_EditMode_ChsName_TextChanged(object sender, EventArgs e)
        {
            _UpdateEditModeTitle();
        }

        private void TextBox_EditMode_Name_TextChanged(object sender, EventArgs e)
        {
            _UpdateEditModeTitle();
        }

        //

        private void Button_EditMode_AddParentUplevel_Click(object sender, EventArgs e)
        {
            Taxon parent = _CurrentTaxon.AddParentUplevel();

            if (!string.IsNullOrEmpty(TextBox_EditMode_AddParent.Text))
            {
                parent.ParseCurrent(TextBox_EditMode_AddParent.Text);

                TextBox_EditMode_AddParent.Clear();
            }

            //

            _UpdateEditModeParents();
            _UpdateEditModeLayout();
        }

        private void Button_EditMode_AddParentDownlevel_Click(object sender, EventArgs e)
        {
            Taxon parent = _CurrentTaxon.AddParentDownlevel();

            if (!string.IsNullOrEmpty(TextBox_EditMode_AddParent.Text))
            {
                parent.ParseCurrent(TextBox_EditMode_AddParent.Text);

                TextBox_EditMode_AddParent.Clear();
            }

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void Button_EditMode_AddChildren_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox_EditMode_AddChildren.Text))
            {
                _CurrentTaxon.AddChild();
            }
            else
            {
                _CurrentTaxon.ParseChildren(TextBox_EditMode_AddChildren.Lines);

                TextBox_EditMode_AddChildren.Clear();
            }

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        //

        private Taxon _RightButtonTaxon = null;
        private Taxon _SelectedTaxon = null;

        private void _ShowMenuItem_Parents(Taxon rightButtonTaxon)
        {
            _RightButtonTaxon = rightButtonTaxon;

            if (_RightButtonTaxon != null)
            {
                ContextMenuStrip_Parents.Show(Cursor.Position);
            }
        }

        private void ToolStripMenuItem_Parents_Select_Click(object sender, EventArgs e)
        {
            _SelectedTaxon = _RightButtonTaxon;
        }

        private void _ShowMenuItem_Children(Taxon rightButtonTaxon)
        {
            _RightButtonTaxon = rightButtonTaxon;

            if (_RightButtonTaxon != null)
            {
                ToolStripMenuItem_Children_SetParent.Enabled = (_SelectedTaxon != null && _SelectedTaxon != _RightButtonTaxon.Parent && !_SelectedTaxon.InheritFrom(_RightButtonTaxon));
                ToolStripMenuItem_Children_MoveTop.Enabled = ToolStripMenuItem_Children_MoveUp.Enabled = (!_RightButtonTaxon.IsRoot && _RightButtonTaxon.Index > 0);
                ToolStripMenuItem_Children_MoveBottom.Enabled = ToolStripMenuItem_Children_MoveDown.Enabled = (!_RightButtonTaxon.IsRoot && _RightButtonTaxon.Index < _RightButtonTaxon.Parent.Children.Count - 1);

                if (ToolStripMenuItem_Children_SetParent.Enabled)
                {
                    if (_SelectedTaxon.IsAnonymous())
                    {
                        ToolStripMenuItem_Children_SetParent.Text = "继承选择的类群 (未命名)";
                    }
                    else
                    {
                        string taxonName = _SelectedTaxon.LongName();

                        if (taxonName.Length > 16)
                        {
                            ToolStripMenuItem_Children_SetParent.Text = string.Concat("继承选择的类群 (", taxonName.Substring(0, 16), "...)");
                        }
                        else
                        {
                            ToolStripMenuItem_Children_SetParent.Text = string.Concat("继承选择的类群 (", taxonName, ")");
                        }
                    }
                }
                else
                {
                    ToolStripMenuItem_Children_SetParent.Text = "继承选择的类群";
                }

                if (_RightButtonTaxon.IsFinal)
                {
                    ToolStripMenuItem_Children_DeleteWithoutChildren.Visible = false;

                    ToolStripMenuItem_Children_DeleteWithinChildren.Text = "删除";
                }
                else
                {
                    ToolStripMenuItem_Children_DeleteWithoutChildren.Visible = true;

                    ToolStripMenuItem_Children_DeleteWithinChildren.Text = "删除 (并且删除下级类群)";
                }

                //

                ContextMenuStrip_Children.Show(Cursor.Position);
            }
        }

        private void ToolStripMenuItem_Children_Select_Click(object sender, EventArgs e)
        {
            _SelectedTaxon = _RightButtonTaxon;
        }

        private void ToolStripMenuItem_Children_SetParent_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.SetParent(_SelectedTaxon);

            //

            _UpdateEditModeParents();
            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_MoveTop_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.Parent.MoveChild(_RightButtonTaxon.Index, 0);

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_MoveUp_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.Parent.SwapChild(_RightButtonTaxon.Index, _RightButtonTaxon.Index - 1);

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_MoveDown_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.Parent.SwapChild(_RightButtonTaxon.Index, _RightButtonTaxon.Index + 1);

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_MoveBottom_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.Parent.MoveChild(_RightButtonTaxon.Index, _RightButtonTaxon.Parent.Children.Count - 1);

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_DeleteWithoutChildren_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.RemoveCurrent(false);

            if (_SelectedTaxon == _RightButtonTaxon)
            {
                _SelectedTaxon = null;
            }

            _RightButtonTaxon = null;

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        private void ToolStripMenuItem_Children_DeleteWithinChildren_Click(object sender, EventArgs e)
        {
            _RightButtonTaxon.RemoveCurrent(true);

            if (_SelectedTaxon == _RightButtonTaxon)
            {
                _SelectedTaxon = null;
            }

            _RightButtonTaxon = null;

            //

            _UpdateEditModeChildren();
            _UpdateEditModeLayout();
        }

        //

        private void Button_EnterViewMode_Click(object sender, EventArgs e)
        {
            _ApplyEditModeInfo();
            _SetMode(false);
        }

        #endregion

        #endregion

        #region 系统发生树（临时）

        private void _RecursiveFillStringBuilder(StringBuilder sb, Taxon taxon)
        {
            if (sb == null || taxon == null)
            {
                throw new ArgumentNullException();
            }

            //

            foreach (var child in taxon.Children)
            {
                char[] ch = new char[child.Level * 2];

                for (int i = 0; i < ch.Length; i++)
                {
                    ch[i] = '　';
                }

                int chIndex = ch.Length - 2;

                if (child.Index < child.Parent.Children.Count - 1)
                {
                    ch[chIndex] = '├';
                }
                else
                {
                    ch[chIndex] = '└';
                }

                chIndex -= 2;

                Taxon t = child.Parent;

                while (t.Parent != null)
                {
                    if (t.Index < t.Parent.Children.Count - 1)
                    {
                        ch[chIndex] = '│';
                    }

                    chIndex -= 2;
                    t = t.Parent;
                }

                sb.Append(ch);

                if (child.IsNamed())
                {
                    sb.AppendLine(child.LongName());
                }
                else
                {
                    sb.AppendLine("─");
                }

                _RecursiveFillStringBuilder(sb, child);
            }
        }

        // 更新系统发生树。
        private void _UpdatePhylogeneticTree()
        {
            TextBox_PhylogeneticTree.Clear();

            StringBuilder sb = new StringBuilder();

            _RecursiveFillStringBuilder(sb, Phylogenesis.Root);

            TextBox_PhylogeneticTree.Text = sb.ToString();
        }

        #endregion
    }
}
