namespace TreeOfLife
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Panel_PhylogeneticTree = new System.Windows.Forms.Panel();
            this.TextBox_PhylogeneticTree = new System.Windows.Forms.TextBox();
            this.ContextMenuStrip_File = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_File_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_File_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_File_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_File_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.Panel_TaxonInfo = new System.Windows.Forms.Panel();
            this.Panel_TaxonInfo_ViewMode = new System.Windows.Forms.Panel();
            this.Panel_ViewMode_Desc = new System.Windows.Forms.Panel();
            this.Label_ViewMode_Desc_Value = new System.Windows.Forms.Label();
            this.Label_ViewMode_Desc = new System.Windows.Forms.Label();
            this.Panel_ViewMode_Tags = new System.Windows.Forms.Panel();
            this.Label_ViewMode_Tags = new System.Windows.Forms.Label();
            this.Panel_ViewMode_Synonyms = new System.Windows.Forms.Panel();
            this.Label_ViewMode_Synonyms = new System.Windows.Forms.Label();
            this.Button_EnterEditMode = new System.Windows.Forms.Button();
            this.Panel_ViewMode_Children = new System.Windows.Forms.Panel();
            this.Label_ViewMode_Children = new System.Windows.Forms.Label();
            this.Panel_ViewMode_Parents = new System.Windows.Forms.Panel();
            this.Label_ViewMode_Parents = new System.Windows.Forms.Label();
            this.Panel_ViewMode_Title = new System.Windows.Forms.Panel();
            this.Panel_ViewMode_TaxonName = new System.Windows.Forms.Panel();
            this.Label_ViewMode_CategoryName = new System.Windows.Forms.Label();
            this.Label_ViewMode_TaxonName = new System.Windows.Forms.Label();
            this.Panel_TaxonInfo_EditMode = new System.Windows.Forms.Panel();
            this.Panel_EditMode_Children = new System.Windows.Forms.Panel();
            this.Label_EditMode_Children = new System.Windows.Forms.Label();
            this.Panel_EditMode_Parents = new System.Windows.Forms.Panel();
            this.Label_EditMode_Parents = new System.Windows.Forms.Label();
            this.Panel_EditMode_AddChildren = new System.Windows.Forms.Panel();
            this.Button_EditMode_AddChildren = new System.Windows.Forms.Button();
            this.TextBox_EditMode_AddChildren = new System.Windows.Forms.TextBox();
            this.Label_EditMode_AddChildren = new System.Windows.Forms.Label();
            this.Panel_EditMode_AddParent = new System.Windows.Forms.Panel();
            this.Button_EditMode_AddParentDownlevel = new System.Windows.Forms.Button();
            this.Button_EditMode_AddParentUplevel = new System.Windows.Forms.Button();
            this.TextBox_EditMode_AddParent = new System.Windows.Forms.TextBox();
            this.Label_EditMode_AddParent = new System.Windows.Forms.Label();
            this.Panel_EditMode_Desc = new System.Windows.Forms.Panel();
            this.TextBox_EditMode_Desc = new System.Windows.Forms.TextBox();
            this.Label_EditMode_Desc = new System.Windows.Forms.Label();
            this.Panel_EditMode_Tags = new System.Windows.Forms.Panel();
            this.TextBox_EditMode_Tags = new System.Windows.Forms.TextBox();
            this.Label_EditMode_Tags = new System.Windows.Forms.Label();
            this.Panel_EditMode_Synonyms = new System.Windows.Forms.Panel();
            this.TextBox_EditMode_Synonyms = new System.Windows.Forms.TextBox();
            this.Label_EditMode_Synonyms = new System.Windows.Forms.Label();
            this.Panel_EditMode_Category = new System.Windows.Forms.Panel();
            this.Label_EditMode_Category = new System.Windows.Forms.Label();
            this.Panel_EditMode_State = new System.Windows.Forms.Panel();
            this.Label_EditMode_State = new System.Windows.Forms.Label();
            this.CheckBox_EditMode_Unsure = new System.Windows.Forms.CheckBox();
            this.CheckBox_EditMode_EX = new System.Windows.Forms.CheckBox();
            this.Panel_EditMode_TaxonName = new System.Windows.Forms.Panel();
            this.TextBox_EditMode_ChsName = new System.Windows.Forms.TextBox();
            this.Label_EditMode_ChsName = new System.Windows.Forms.Label();
            this.TextBox_EditMode_Name = new System.Windows.Forms.TextBox();
            this.Label_EditMode_Name = new System.Windows.Forms.Label();
            this.Button_EnterViewMode = new System.Windows.Forms.Button();
            this.OpenFileDialog_Open = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog_SaveAs = new System.Windows.Forms.SaveFileDialog();
            this.ContextMenuStrip_Children = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Children_Select = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Children_SetParent = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Children_MoveTop = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Children_MoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Children_MoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Children_MoveBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItem_Children_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem_Children_DeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextMenuStrip_Parents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ToolStripMenuItem_Parents_Select = new System.Windows.Forms.ToolStripMenuItem();
            this.TagGroup_ViewMode_Tags = new TreeOfLife.TagGroup();
            this.TagGroup_Synonyms = new TreeOfLife.TagGroup();
            this.TaxonNameButtonGroup_ViewMode_Children = new TreeOfLife.TaxonNameButtonGroup();
            this.TaxonNameButtonGroup_ViewMode_Parents = new TreeOfLife.TaxonNameButtonGroup();
            this.TaxonNameButtonGroup_EditMode_Children = new TreeOfLife.TaxonNameButtonGroup();
            this.TaxonNameButtonGroup_EditMode_Parents = new TreeOfLife.TaxonNameButtonGroup();
            this.CategorySelector_EditMode_Category = new TreeOfLife.CategorySelector();
            this.Panel_Main.SuspendLayout();
            this.Panel_PhylogeneticTree.SuspendLayout();
            this.ContextMenuStrip_File.SuspendLayout();
            this.Panel_TaxonInfo.SuspendLayout();
            this.Panel_TaxonInfo_ViewMode.SuspendLayout();
            this.Panel_ViewMode_Desc.SuspendLayout();
            this.Panel_ViewMode_Tags.SuspendLayout();
            this.Panel_ViewMode_Synonyms.SuspendLayout();
            this.Panel_ViewMode_Children.SuspendLayout();
            this.Panel_ViewMode_Parents.SuspendLayout();
            this.Panel_ViewMode_Title.SuspendLayout();
            this.Panel_ViewMode_TaxonName.SuspendLayout();
            this.Panel_TaxonInfo_EditMode.SuspendLayout();
            this.Panel_EditMode_Children.SuspendLayout();
            this.Panel_EditMode_Parents.SuspendLayout();
            this.Panel_EditMode_AddChildren.SuspendLayout();
            this.Panel_EditMode_AddParent.SuspendLayout();
            this.Panel_EditMode_Desc.SuspendLayout();
            this.Panel_EditMode_Tags.SuspendLayout();
            this.Panel_EditMode_Synonyms.SuspendLayout();
            this.Panel_EditMode_Category.SuspendLayout();
            this.Panel_EditMode_State.SuspendLayout();
            this.Panel_EditMode_TaxonName.SuspendLayout();
            this.ContextMenuStrip_Children.SuspendLayout();
            this.ContextMenuStrip_Parents.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Main
            // 
            this.Panel_Main.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Main.Controls.Add(this.Panel_PhylogeneticTree);
            this.Panel_Main.Controls.Add(this.Panel_TaxonInfo);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 0);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(800, 450);
            this.Panel_Main.TabIndex = 0;
            this.Panel_Main.Visible = false;
            // 
            // Panel_PhylogeneticTree
            // 
            this.Panel_PhylogeneticTree.BackColor = System.Drawing.Color.Transparent;
            this.Panel_PhylogeneticTree.Controls.Add(this.TextBox_PhylogeneticTree);
            this.Panel_PhylogeneticTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_PhylogeneticTree.Location = new System.Drawing.Point(0, 0);
            this.Panel_PhylogeneticTree.Name = "Panel_PhylogeneticTree";
            this.Panel_PhylogeneticTree.Size = new System.Drawing.Size(400, 450);
            this.Panel_PhylogeneticTree.TabIndex = 0;
            // 
            // TextBox_PhylogeneticTree
            // 
            this.TextBox_PhylogeneticTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TextBox_PhylogeneticTree.ContextMenuStrip = this.ContextMenuStrip_File;
            this.TextBox_PhylogeneticTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox_PhylogeneticTree.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_PhylogeneticTree.Location = new System.Drawing.Point(0, 0);
            this.TextBox_PhylogeneticTree.Multiline = true;
            this.TextBox_PhylogeneticTree.Name = "TextBox_PhylogeneticTree";
            this.TextBox_PhylogeneticTree.ReadOnly = true;
            this.TextBox_PhylogeneticTree.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_PhylogeneticTree.Size = new System.Drawing.Size(400, 450);
            this.TextBox_PhylogeneticTree.TabIndex = 0;
            this.TextBox_PhylogeneticTree.TabStop = false;
            this.TextBox_PhylogeneticTree.WordWrap = false;
            // 
            // ContextMenuStrip_File
            // 
            this.ContextMenuStrip_File.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_File_Open,
            this.ToolStripMenuItem_File_Save,
            this.ToolStripMenuItem_File_SaveAs,
            this.ToolStripMenuItem_File_Close});
            this.ContextMenuStrip_File.Name = "ContextMenuStrip_File";
            this.ContextMenuStrip_File.Size = new System.Drawing.Size(122, 108);
            // 
            // ToolStripMenuItem_File_Open
            // 
            this.ToolStripMenuItem_File_Open.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_File_Open.Name = "ToolStripMenuItem_File_Open";
            this.ToolStripMenuItem_File_Open.Size = new System.Drawing.Size(121, 22);
            this.ToolStripMenuItem_File_Open.Text = "打开...";
            // 
            // ToolStripMenuItem_File_Save
            // 
            this.ToolStripMenuItem_File_Save.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_File_Save.Name = "ToolStripMenuItem_File_Save";
            this.ToolStripMenuItem_File_Save.Size = new System.Drawing.Size(121, 22);
            this.ToolStripMenuItem_File_Save.Text = "保存";
            // 
            // ToolStripMenuItem_File_SaveAs
            // 
            this.ToolStripMenuItem_File_SaveAs.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_File_SaveAs.Name = "ToolStripMenuItem_File_SaveAs";
            this.ToolStripMenuItem_File_SaveAs.Size = new System.Drawing.Size(121, 22);
            this.ToolStripMenuItem_File_SaveAs.Text = "另存为...";
            // 
            // ToolStripMenuItem_File_Close
            // 
            this.ToolStripMenuItem_File_Close.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_File_Close.Name = "ToolStripMenuItem_File_Close";
            this.ToolStripMenuItem_File_Close.Size = new System.Drawing.Size(121, 22);
            this.ToolStripMenuItem_File_Close.Text = "关闭";
            // 
            // Panel_TaxonInfo
            // 
            this.Panel_TaxonInfo.BackColor = System.Drawing.Color.Transparent;
            this.Panel_TaxonInfo.Controls.Add(this.Panel_TaxonInfo_ViewMode);
            this.Panel_TaxonInfo.Controls.Add(this.Panel_TaxonInfo_EditMode);
            this.Panel_TaxonInfo.Dock = System.Windows.Forms.DockStyle.Right;
            this.Panel_TaxonInfo.Location = new System.Drawing.Point(400, 0);
            this.Panel_TaxonInfo.Name = "Panel_TaxonInfo";
            this.Panel_TaxonInfo.Size = new System.Drawing.Size(400, 450);
            this.Panel_TaxonInfo.TabIndex = 0;
            // 
            // Panel_TaxonInfo_ViewMode
            // 
            this.Panel_TaxonInfo_ViewMode.AutoScroll = true;
            this.Panel_TaxonInfo_ViewMode.AutoScrollMargin = new System.Drawing.Size(0, 25);
            this.Panel_TaxonInfo_ViewMode.BackColor = System.Drawing.Color.Transparent;
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Desc);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Tags);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Synonyms);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Button_EnterEditMode);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Children);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Parents);
            this.Panel_TaxonInfo_ViewMode.Controls.Add(this.Panel_ViewMode_Title);
            this.Panel_TaxonInfo_ViewMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_TaxonInfo_ViewMode.Location = new System.Drawing.Point(0, 0);
            this.Panel_TaxonInfo_ViewMode.Name = "Panel_TaxonInfo_ViewMode";
            this.Panel_TaxonInfo_ViewMode.Size = new System.Drawing.Size(400, 450);
            this.Panel_TaxonInfo_ViewMode.TabIndex = 0;
            // 
            // Panel_ViewMode_Desc
            // 
            this.Panel_ViewMode_Desc.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ViewMode_Desc.Controls.Add(this.Label_ViewMode_Desc_Value);
            this.Panel_ViewMode_Desc.Controls.Add(this.Label_ViewMode_Desc);
            this.Panel_ViewMode_Desc.Location = new System.Drawing.Point(25, 425);
            this.Panel_ViewMode_Desc.Name = "Panel_ViewMode_Desc";
            this.Panel_ViewMode_Desc.Size = new System.Drawing.Size(350, 50);
            this.Panel_ViewMode_Desc.TabIndex = 0;
            // 
            // Label_ViewMode_Desc_Value
            // 
            this.Label_ViewMode_Desc_Value.AutoSize = true;
            this.Label_ViewMode_Desc_Value.BackColor = System.Drawing.Color.Transparent;
            this.Label_ViewMode_Desc_Value.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Desc_Value.Location = new System.Drawing.Point(25, 25);
            this.Label_ViewMode_Desc_Value.MaximumSize = new System.Drawing.Size(300, 0);
            this.Label_ViewMode_Desc_Value.MinimumSize = new System.Drawing.Size(300, 25);
            this.Label_ViewMode_Desc_Value.Name = "Label_ViewMode_Desc_Value";
            this.Label_ViewMode_Desc_Value.Size = new System.Drawing.Size(300, 25);
            this.Label_ViewMode_Desc_Value.TabIndex = 0;
            // 
            // Label_ViewMode_Desc
            // 
            this.Label_ViewMode_Desc.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_Desc.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Desc.Location = new System.Drawing.Point(0, 25);
            this.Label_ViewMode_Desc.Name = "Label_ViewMode_Desc";
            this.Label_ViewMode_Desc.Size = new System.Drawing.Size(7, 25);
            this.Label_ViewMode_Desc.TabIndex = 0;
            this.Label_ViewMode_Desc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_ViewMode_Tags
            // 
            this.Panel_ViewMode_Tags.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ViewMode_Tags.Controls.Add(this.TagGroup_ViewMode_Tags);
            this.Panel_ViewMode_Tags.Controls.Add(this.Label_ViewMode_Tags);
            this.Panel_ViewMode_Tags.Location = new System.Drawing.Point(25, 75);
            this.Panel_ViewMode_Tags.Name = "Panel_ViewMode_Tags";
            this.Panel_ViewMode_Tags.Size = new System.Drawing.Size(350, 50);
            this.Panel_ViewMode_Tags.TabIndex = 0;
            // 
            // Label_ViewMode_Tags
            // 
            this.Label_ViewMode_Tags.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_Tags.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Tags.Location = new System.Drawing.Point(0, 25);
            this.Label_ViewMode_Tags.Name = "Label_ViewMode_Tags";
            this.Label_ViewMode_Tags.Size = new System.Drawing.Size(7, 25);
            this.Label_ViewMode_Tags.TabIndex = 0;
            this.Label_ViewMode_Tags.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_ViewMode_Synonyms
            // 
            this.Panel_ViewMode_Synonyms.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ViewMode_Synonyms.Controls.Add(this.TagGroup_Synonyms);
            this.Panel_ViewMode_Synonyms.Controls.Add(this.Label_ViewMode_Synonyms);
            this.Panel_ViewMode_Synonyms.Location = new System.Drawing.Point(25, 325);
            this.Panel_ViewMode_Synonyms.Name = "Panel_ViewMode_Synonyms";
            this.Panel_ViewMode_Synonyms.Size = new System.Drawing.Size(350, 100);
            this.Panel_ViewMode_Synonyms.TabIndex = 0;
            // 
            // Label_ViewMode_Synonyms
            // 
            this.Label_ViewMode_Synonyms.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_Synonyms.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Synonyms.Location = new System.Drawing.Point(0, 25);
            this.Label_ViewMode_Synonyms.Name = "Label_ViewMode_Synonyms";
            this.Label_ViewMode_Synonyms.Size = new System.Drawing.Size(350, 25);
            this.Label_ViewMode_Synonyms.TabIndex = 0;
            this.Label_ViewMode_Synonyms.Text = "异名";
            this.Label_ViewMode_Synonyms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button_EnterEditMode
            // 
            this.Button_EnterEditMode.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_EnterEditMode.Location = new System.Drawing.Point(285, 500);
            this.Button_EnterEditMode.Name = "Button_EnterEditMode";
            this.Button_EnterEditMode.Size = new System.Drawing.Size(90, 30);
            this.Button_EnterEditMode.TabIndex = 0;
            this.Button_EnterEditMode.TabStop = false;
            this.Button_EnterEditMode.Text = "编辑";
            this.Button_EnterEditMode.UseVisualStyleBackColor = true;
            this.Button_EnterEditMode.Click += new System.EventHandler(this.Button_EnterEditMode_Click);
            // 
            // Panel_ViewMode_Children
            // 
            this.Panel_ViewMode_Children.Controls.Add(this.Label_ViewMode_Children);
            this.Panel_ViewMode_Children.Controls.Add(this.TaxonNameButtonGroup_ViewMode_Children);
            this.Panel_ViewMode_Children.Location = new System.Drawing.Point(25, 225);
            this.Panel_ViewMode_Children.Name = "Panel_ViewMode_Children";
            this.Panel_ViewMode_Children.Size = new System.Drawing.Size(350, 100);
            this.Panel_ViewMode_Children.TabIndex = 0;
            // 
            // Label_ViewMode_Children
            // 
            this.Label_ViewMode_Children.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_Children.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Children.Location = new System.Drawing.Point(0, 25);
            this.Label_ViewMode_Children.Name = "Label_ViewMode_Children";
            this.Label_ViewMode_Children.Size = new System.Drawing.Size(350, 25);
            this.Label_ViewMode_Children.TabIndex = 0;
            this.Label_ViewMode_Children.Text = "下属类群";
            this.Label_ViewMode_Children.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_ViewMode_Parents
            // 
            this.Panel_ViewMode_Parents.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ViewMode_Parents.Controls.Add(this.Label_ViewMode_Parents);
            this.Panel_ViewMode_Parents.Controls.Add(this.TaxonNameButtonGroup_ViewMode_Parents);
            this.Panel_ViewMode_Parents.Location = new System.Drawing.Point(25, 125);
            this.Panel_ViewMode_Parents.Name = "Panel_ViewMode_Parents";
            this.Panel_ViewMode_Parents.Size = new System.Drawing.Size(350, 100);
            this.Panel_ViewMode_Parents.TabIndex = 0;
            // 
            // Label_ViewMode_Parents
            // 
            this.Label_ViewMode_Parents.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_Parents.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_Parents.Location = new System.Drawing.Point(0, 25);
            this.Label_ViewMode_Parents.Name = "Label_ViewMode_Parents";
            this.Label_ViewMode_Parents.Size = new System.Drawing.Size(350, 25);
            this.Label_ViewMode_Parents.TabIndex = 0;
            this.Label_ViewMode_Parents.Text = "科学分类";
            this.Label_ViewMode_Parents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_ViewMode_Title
            // 
            this.Panel_ViewMode_Title.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ViewMode_Title.Controls.Add(this.Panel_ViewMode_TaxonName);
            this.Panel_ViewMode_Title.Location = new System.Drawing.Point(25, 0);
            this.Panel_ViewMode_Title.Name = "Panel_ViewMode_Title";
            this.Panel_ViewMode_Title.Size = new System.Drawing.Size(350, 75);
            this.Panel_ViewMode_Title.TabIndex = 0;
            // 
            // Panel_ViewMode_TaxonName
            // 
            this.Panel_ViewMode_TaxonName.Controls.Add(this.Label_ViewMode_CategoryName);
            this.Panel_ViewMode_TaxonName.Controls.Add(this.Label_ViewMode_TaxonName);
            this.Panel_ViewMode_TaxonName.Location = new System.Drawing.Point(0, 25);
            this.Panel_ViewMode_TaxonName.Name = "Panel_ViewMode_TaxonName";
            this.Panel_ViewMode_TaxonName.Size = new System.Drawing.Size(350, 50);
            this.Panel_ViewMode_TaxonName.TabIndex = 0;
            // 
            // Label_ViewMode_CategoryName
            // 
            this.Label_ViewMode_CategoryName.BackColor = System.Drawing.Color.DimGray;
            this.Label_ViewMode_CategoryName.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_CategoryName.ForeColor = System.Drawing.Color.White;
            this.Label_ViewMode_CategoryName.Location = new System.Drawing.Point(1, 1);
            this.Label_ViewMode_CategoryName.Name = "Label_ViewMode_CategoryName";
            this.Label_ViewMode_CategoryName.Size = new System.Drawing.Size(79, 48);
            this.Label_ViewMode_CategoryName.TabIndex = 0;
            this.Label_ViewMode_CategoryName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label_ViewMode_TaxonName
            // 
            this.Label_ViewMode_TaxonName.AutoEllipsis = true;
            this.Label_ViewMode_TaxonName.BackColor = System.Drawing.Color.Silver;
            this.Label_ViewMode_TaxonName.Font = new System.Drawing.Font("微软雅黑", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_ViewMode_TaxonName.Location = new System.Drawing.Point(80, 1);
            this.Label_ViewMode_TaxonName.Name = "Label_ViewMode_TaxonName";
            this.Label_ViewMode_TaxonName.Size = new System.Drawing.Size(269, 48);
            this.Label_ViewMode_TaxonName.TabIndex = 0;
            this.Label_ViewMode_TaxonName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_TaxonInfo_EditMode
            // 
            this.Panel_TaxonInfo_EditMode.AutoScroll = true;
            this.Panel_TaxonInfo_EditMode.AutoScrollMargin = new System.Drawing.Size(0, 25);
            this.Panel_TaxonInfo_EditMode.BackColor = System.Drawing.Color.Transparent;
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Children);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Parents);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_AddChildren);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_AddParent);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Desc);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Tags);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Synonyms);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_Category);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_State);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Panel_EditMode_TaxonName);
            this.Panel_TaxonInfo_EditMode.Controls.Add(this.Button_EnterViewMode);
            this.Panel_TaxonInfo_EditMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_TaxonInfo_EditMode.Location = new System.Drawing.Point(0, 0);
            this.Panel_TaxonInfo_EditMode.Name = "Panel_TaxonInfo_EditMode";
            this.Panel_TaxonInfo_EditMode.Size = new System.Drawing.Size(400, 450);
            this.Panel_TaxonInfo_EditMode.TabIndex = 0;
            this.Panel_TaxonInfo_EditMode.Visible = false;
            // 
            // Panel_EditMode_Children
            // 
            this.Panel_EditMode_Children.Controls.Add(this.Label_EditMode_Children);
            this.Panel_EditMode_Children.Controls.Add(this.TaxonNameButtonGroup_EditMode_Children);
            this.Panel_EditMode_Children.Location = new System.Drawing.Point(25, 750);
            this.Panel_EditMode_Children.Name = "Panel_EditMode_Children";
            this.Panel_EditMode_Children.Size = new System.Drawing.Size(350, 100);
            this.Panel_EditMode_Children.TabIndex = 0;
            // 
            // Label_EditMode_Children
            // 
            this.Label_EditMode_Children.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Children.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Children.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Children.Name = "Label_EditMode_Children";
            this.Label_EditMode_Children.Size = new System.Drawing.Size(350, 25);
            this.Label_EditMode_Children.TabIndex = 0;
            this.Label_EditMode_Children.Text = "下级类群";
            this.Label_EditMode_Children.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_Parents
            // 
            this.Panel_EditMode_Parents.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_Parents.Controls.Add(this.Label_EditMode_Parents);
            this.Panel_EditMode_Parents.Controls.Add(this.TaxonNameButtonGroup_EditMode_Parents);
            this.Panel_EditMode_Parents.Location = new System.Drawing.Point(25, 560);
            this.Panel_EditMode_Parents.Name = "Panel_EditMode_Parents";
            this.Panel_EditMode_Parents.Size = new System.Drawing.Size(350, 100);
            this.Panel_EditMode_Parents.TabIndex = 0;
            // 
            // Label_EditMode_Parents
            // 
            this.Label_EditMode_Parents.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Parents.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Parents.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Parents.Name = "Label_EditMode_Parents";
            this.Label_EditMode_Parents.Size = new System.Drawing.Size(350, 25);
            this.Label_EditMode_Parents.TabIndex = 0;
            this.Label_EditMode_Parents.Text = "上级类群";
            this.Label_EditMode_Parents.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_AddChildren
            // 
            this.Panel_EditMode_AddChildren.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_AddChildren.Controls.Add(this.Button_EditMode_AddChildren);
            this.Panel_EditMode_AddChildren.Controls.Add(this.TextBox_EditMode_AddChildren);
            this.Panel_EditMode_AddChildren.Controls.Add(this.Label_EditMode_AddChildren);
            this.Panel_EditMode_AddChildren.Location = new System.Drawing.Point(25, 850);
            this.Panel_EditMode_AddChildren.Name = "Panel_EditMode_AddChildren";
            this.Panel_EditMode_AddChildren.Size = new System.Drawing.Size(350, 165);
            this.Panel_EditMode_AddChildren.TabIndex = 0;
            // 
            // Button_EditMode_AddChildren
            // 
            this.Button_EditMode_AddChildren.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_EditMode_AddChildren.Location = new System.Drawing.Point(260, 135);
            this.Button_EditMode_AddChildren.Name = "Button_EditMode_AddChildren";
            this.Button_EditMode_AddChildren.Size = new System.Drawing.Size(90, 30);
            this.Button_EditMode_AddChildren.TabIndex = 0;
            this.Button_EditMode_AddChildren.TabStop = false;
            this.Button_EditMode_AddChildren.Text = "添加";
            this.Button_EditMode_AddChildren.UseVisualStyleBackColor = true;
            // 
            // TextBox_EditMode_AddChildren
            // 
            this.TextBox_EditMode_AddChildren.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TextBox_EditMode_AddChildren.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_AddChildren.Multiline = true;
            this.TextBox_EditMode_AddChildren.Name = "TextBox_EditMode_AddChildren";
            this.TextBox_EditMode_AddChildren.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_EditMode_AddChildren.Size = new System.Drawing.Size(270, 100);
            this.TextBox_EditMode_AddChildren.TabIndex = 0;
            this.TextBox_EditMode_AddChildren.TabStop = false;
            this.TextBox_EditMode_AddChildren.WordWrap = false;
            // 
            // Label_EditMode_AddChildren
            // 
            this.Label_EditMode_AddChildren.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_AddChildren.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_AddChildren.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_AddChildren.Name = "Label_EditMode_AddChildren";
            this.Label_EditMode_AddChildren.Size = new System.Drawing.Size(70, 140);
            this.Label_EditMode_AddChildren.TabIndex = 0;
            this.Label_EditMode_AddChildren.Text = "添加\r\n下级\r\n类群";
            this.Label_EditMode_AddChildren.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_AddParent
            // 
            this.Panel_EditMode_AddParent.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_AddParent.Controls.Add(this.Button_EditMode_AddParentDownlevel);
            this.Panel_EditMode_AddParent.Controls.Add(this.Button_EditMode_AddParentUplevel);
            this.Panel_EditMode_AddParent.Controls.Add(this.TextBox_EditMode_AddParent);
            this.Panel_EditMode_AddParent.Controls.Add(this.Label_EditMode_AddParent);
            this.Panel_EditMode_AddParent.Location = new System.Drawing.Point(25, 660);
            this.Panel_EditMode_AddParent.Name = "Panel_EditMode_AddParent";
            this.Panel_EditMode_AddParent.Size = new System.Drawing.Size(350, 90);
            this.Panel_EditMode_AddParent.TabIndex = 0;
            // 
            // Button_EditMode_AddParentDownlevel
            // 
            this.Button_EditMode_AddParentDownlevel.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_EditMode_AddParentDownlevel.Location = new System.Drawing.Point(230, 60);
            this.Button_EditMode_AddParentDownlevel.Name = "Button_EditMode_AddParentDownlevel";
            this.Button_EditMode_AddParentDownlevel.Size = new System.Drawing.Size(120, 30);
            this.Button_EditMode_AddParentDownlevel.TabIndex = 0;
            this.Button_EditMode_AddParentDownlevel.TabStop = false;
            this.Button_EditMode_AddParentDownlevel.Text = "添加为下级";
            this.Button_EditMode_AddParentDownlevel.UseVisualStyleBackColor = true;
            // 
            // Button_EditMode_AddParentUplevel
            // 
            this.Button_EditMode_AddParentUplevel.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_EditMode_AddParentUplevel.Location = new System.Drawing.Point(100, 60);
            this.Button_EditMode_AddParentUplevel.Name = "Button_EditMode_AddParentUplevel";
            this.Button_EditMode_AddParentUplevel.Size = new System.Drawing.Size(120, 30);
            this.Button_EditMode_AddParentUplevel.TabIndex = 0;
            this.Button_EditMode_AddParentUplevel.TabStop = false;
            this.Button_EditMode_AddParentUplevel.Text = "添加为上级";
            this.Button_EditMode_AddParentUplevel.UseVisualStyleBackColor = true;
            // 
            // TextBox_EditMode_AddParent
            // 
            this.TextBox_EditMode_AddParent.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TextBox_EditMode_AddParent.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_AddParent.Name = "TextBox_EditMode_AddParent";
            this.TextBox_EditMode_AddParent.Size = new System.Drawing.Size(270, 25);
            this.TextBox_EditMode_AddParent.TabIndex = 0;
            this.TextBox_EditMode_AddParent.TabStop = false;
            // 
            // Label_EditMode_AddParent
            // 
            this.Label_EditMode_AddParent.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_AddParent.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_AddParent.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_AddParent.Name = "Label_EditMode_AddParent";
            this.Label_EditMode_AddParent.Size = new System.Drawing.Size(70, 65);
            this.Label_EditMode_AddParent.TabIndex = 0;
            this.Label_EditMode_AddParent.Text = "插入\r\n类群";
            this.Label_EditMode_AddParent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_Desc
            // 
            this.Panel_EditMode_Desc.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_Desc.Controls.Add(this.TextBox_EditMode_Desc);
            this.Panel_EditMode_Desc.Controls.Add(this.Label_EditMode_Desc);
            this.Panel_EditMode_Desc.Location = new System.Drawing.Point(25, 435);
            this.Panel_EditMode_Desc.Name = "Panel_EditMode_Desc";
            this.Panel_EditMode_Desc.Size = new System.Drawing.Size(350, 125);
            this.Panel_EditMode_Desc.TabIndex = 0;
            // 
            // TextBox_EditMode_Desc
            // 
            this.TextBox_EditMode_Desc.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TextBox_EditMode_Desc.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_Desc.Multiline = true;
            this.TextBox_EditMode_Desc.Name = "TextBox_EditMode_Desc";
            this.TextBox_EditMode_Desc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TextBox_EditMode_Desc.Size = new System.Drawing.Size(270, 100);
            this.TextBox_EditMode_Desc.TabIndex = 0;
            this.TextBox_EditMode_Desc.TabStop = false;
            // 
            // Label_EditMode_Desc
            // 
            this.Label_EditMode_Desc.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Desc.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Desc.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Desc.Name = "Label_EditMode_Desc";
            this.Label_EditMode_Desc.Size = new System.Drawing.Size(70, 100);
            this.Label_EditMode_Desc.TabIndex = 0;
            this.Label_EditMode_Desc.Text = "描述";
            this.Label_EditMode_Desc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_Tags
            // 
            this.Panel_EditMode_Tags.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_Tags.Controls.Add(this.TextBox_EditMode_Tags);
            this.Panel_EditMode_Tags.Controls.Add(this.Label_EditMode_Tags);
            this.Panel_EditMode_Tags.Location = new System.Drawing.Point(25, 310);
            this.Panel_EditMode_Tags.Name = "Panel_EditMode_Tags";
            this.Panel_EditMode_Tags.Size = new System.Drawing.Size(350, 125);
            this.Panel_EditMode_Tags.TabIndex = 0;
            // 
            // TextBox_EditMode_Tags
            // 
            this.TextBox_EditMode_Tags.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_EditMode_Tags.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_Tags.Multiline = true;
            this.TextBox_EditMode_Tags.Name = "TextBox_EditMode_Tags";
            this.TextBox_EditMode_Tags.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_EditMode_Tags.Size = new System.Drawing.Size(270, 100);
            this.TextBox_EditMode_Tags.TabIndex = 0;
            this.TextBox_EditMode_Tags.TabStop = false;
            this.TextBox_EditMode_Tags.WordWrap = false;
            // 
            // Label_EditMode_Tags
            // 
            this.Label_EditMode_Tags.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Tags.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Tags.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Tags.Name = "Label_EditMode_Tags";
            this.Label_EditMode_Tags.Size = new System.Drawing.Size(70, 100);
            this.Label_EditMode_Tags.TabIndex = 0;
            this.Label_EditMode_Tags.Text = "标签";
            this.Label_EditMode_Tags.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_Synonyms
            // 
            this.Panel_EditMode_Synonyms.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_Synonyms.Controls.Add(this.TextBox_EditMode_Synonyms);
            this.Panel_EditMode_Synonyms.Controls.Add(this.Label_EditMode_Synonyms);
            this.Panel_EditMode_Synonyms.Location = new System.Drawing.Point(25, 185);
            this.Panel_EditMode_Synonyms.Name = "Panel_EditMode_Synonyms";
            this.Panel_EditMode_Synonyms.Size = new System.Drawing.Size(350, 125);
            this.Panel_EditMode_Synonyms.TabIndex = 0;
            // 
            // TextBox_EditMode_Synonyms
            // 
            this.TextBox_EditMode_Synonyms.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox_EditMode_Synonyms.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_Synonyms.Multiline = true;
            this.TextBox_EditMode_Synonyms.Name = "TextBox_EditMode_Synonyms";
            this.TextBox_EditMode_Synonyms.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBox_EditMode_Synonyms.Size = new System.Drawing.Size(270, 100);
            this.TextBox_EditMode_Synonyms.TabIndex = 0;
            this.TextBox_EditMode_Synonyms.TabStop = false;
            this.TextBox_EditMode_Synonyms.WordWrap = false;
            // 
            // Label_EditMode_Synonyms
            // 
            this.Label_EditMode_Synonyms.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Synonyms.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Synonyms.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Synonyms.Name = "Label_EditMode_Synonyms";
            this.Label_EditMode_Synonyms.Size = new System.Drawing.Size(70, 100);
            this.Label_EditMode_Synonyms.TabIndex = 0;
            this.Label_EditMode_Synonyms.Text = "异名";
            this.Label_EditMode_Synonyms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_Category
            // 
            this.Panel_EditMode_Category.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_Category.Controls.Add(this.CategorySelector_EditMode_Category);
            this.Panel_EditMode_Category.Controls.Add(this.Label_EditMode_Category);
            this.Panel_EditMode_Category.Location = new System.Drawing.Point(25, 135);
            this.Panel_EditMode_Category.Name = "Panel_EditMode_Category";
            this.Panel_EditMode_Category.Size = new System.Drawing.Size(350, 50);
            this.Panel_EditMode_Category.TabIndex = 0;
            // 
            // Label_EditMode_Category
            // 
            this.Label_EditMode_Category.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Category.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Category.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Category.Name = "Label_EditMode_Category";
            this.Label_EditMode_Category.Size = new System.Drawing.Size(70, 25);
            this.Label_EditMode_Category.TabIndex = 0;
            this.Label_EditMode_Category.Text = "分级";
            this.Label_EditMode_Category.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Panel_EditMode_State
            // 
            this.Panel_EditMode_State.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_State.Controls.Add(this.Label_EditMode_State);
            this.Panel_EditMode_State.Controls.Add(this.CheckBox_EditMode_Unsure);
            this.Panel_EditMode_State.Controls.Add(this.CheckBox_EditMode_EX);
            this.Panel_EditMode_State.Location = new System.Drawing.Point(25, 85);
            this.Panel_EditMode_State.Name = "Panel_EditMode_State";
            this.Panel_EditMode_State.Size = new System.Drawing.Size(350, 50);
            this.Panel_EditMode_State.TabIndex = 0;
            // 
            // Label_EditMode_State
            // 
            this.Label_EditMode_State.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_State.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_State.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_State.Name = "Label_EditMode_State";
            this.Label_EditMode_State.Size = new System.Drawing.Size(70, 25);
            this.Label_EditMode_State.TabIndex = 0;
            this.Label_EditMode_State.Text = "状态";
            this.Label_EditMode_State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CheckBox_EditMode_Unsure
            // 
            this.CheckBox_EditMode_Unsure.AutoSize = true;
            this.CheckBox_EditMode_Unsure.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_EditMode_Unsure.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.CheckBox_EditMode_Unsure.Location = new System.Drawing.Point(215, 25);
            this.CheckBox_EditMode_Unsure.MinimumSize = new System.Drawing.Size(0, 25);
            this.CheckBox_EditMode_Unsure.Name = "CheckBox_EditMode_Unsure";
            this.CheckBox_EditMode_Unsure.Size = new System.Drawing.Size(54, 25);
            this.CheckBox_EditMode_Unsure.TabIndex = 0;
            this.CheckBox_EditMode_Unsure.TabStop = false;
            this.CheckBox_EditMode_Unsure.Text = "存疑";
            this.CheckBox_EditMode_Unsure.UseVisualStyleBackColor = false;
            // 
            // CheckBox_EditMode_EX
            // 
            this.CheckBox_EditMode_EX.AutoSize = true;
            this.CheckBox_EditMode_EX.BackColor = System.Drawing.Color.Transparent;
            this.CheckBox_EditMode_EX.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.CheckBox_EditMode_EX.Location = new System.Drawing.Point(80, 25);
            this.CheckBox_EditMode_EX.MinimumSize = new System.Drawing.Size(0, 25);
            this.CheckBox_EditMode_EX.Name = "CheckBox_EditMode_EX";
            this.CheckBox_EditMode_EX.Size = new System.Drawing.Size(54, 25);
            this.CheckBox_EditMode_EX.TabIndex = 0;
            this.CheckBox_EditMode_EX.TabStop = false;
            this.CheckBox_EditMode_EX.Text = "灭绝";
            this.CheckBox_EditMode_EX.UseVisualStyleBackColor = false;
            // 
            // Panel_EditMode_TaxonName
            // 
            this.Panel_EditMode_TaxonName.BackColor = System.Drawing.Color.Transparent;
            this.Panel_EditMode_TaxonName.Controls.Add(this.TextBox_EditMode_ChsName);
            this.Panel_EditMode_TaxonName.Controls.Add(this.Label_EditMode_ChsName);
            this.Panel_EditMode_TaxonName.Controls.Add(this.TextBox_EditMode_Name);
            this.Panel_EditMode_TaxonName.Controls.Add(this.Label_EditMode_Name);
            this.Panel_EditMode_TaxonName.Location = new System.Drawing.Point(25, 0);
            this.Panel_EditMode_TaxonName.Name = "Panel_EditMode_TaxonName";
            this.Panel_EditMode_TaxonName.Size = new System.Drawing.Size(350, 85);
            this.Panel_EditMode_TaxonName.TabIndex = 0;
            // 
            // TextBox_EditMode_ChsName
            // 
            this.TextBox_EditMode_ChsName.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TextBox_EditMode_ChsName.Location = new System.Drawing.Point(80, 60);
            this.TextBox_EditMode_ChsName.Name = "TextBox_EditMode_ChsName";
            this.TextBox_EditMode_ChsName.Size = new System.Drawing.Size(270, 25);
            this.TextBox_EditMode_ChsName.TabIndex = 0;
            this.TextBox_EditMode_ChsName.TabStop = false;
            // 
            // Label_EditMode_ChsName
            // 
            this.Label_EditMode_ChsName.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_ChsName.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_ChsName.Location = new System.Drawing.Point(0, 60);
            this.Label_EditMode_ChsName.Name = "Label_EditMode_ChsName";
            this.Label_EditMode_ChsName.Size = new System.Drawing.Size(70, 25);
            this.Label_EditMode_ChsName.TabIndex = 0;
            this.Label_EditMode_ChsName.Text = "中文名";
            this.Label_EditMode_ChsName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBox_EditMode_Name
            // 
            this.TextBox_EditMode_Name.Font = new System.Drawing.Font("微软雅黑", 9.75F);
            this.TextBox_EditMode_Name.Location = new System.Drawing.Point(80, 25);
            this.TextBox_EditMode_Name.Name = "TextBox_EditMode_Name";
            this.TextBox_EditMode_Name.Size = new System.Drawing.Size(270, 25);
            this.TextBox_EditMode_Name.TabIndex = 0;
            this.TextBox_EditMode_Name.TabStop = false;
            // 
            // Label_EditMode_Name
            // 
            this.Label_EditMode_Name.BackColor = System.Drawing.Color.Silver;
            this.Label_EditMode_Name.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_EditMode_Name.Location = new System.Drawing.Point(0, 25);
            this.Label_EditMode_Name.Name = "Label_EditMode_Name";
            this.Label_EditMode_Name.Size = new System.Drawing.Size(70, 25);
            this.Label_EditMode_Name.TabIndex = 0;
            this.Label_EditMode_Name.Text = "学名";
            this.Label_EditMode_Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Button_EnterViewMode
            // 
            this.Button_EnterViewMode.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button_EnterViewMode.Location = new System.Drawing.Point(285, 1040);
            this.Button_EnterViewMode.Name = "Button_EnterViewMode";
            this.Button_EnterViewMode.Size = new System.Drawing.Size(90, 30);
            this.Button_EnterViewMode.TabIndex = 0;
            this.Button_EnterViewMode.TabStop = false;
            this.Button_EnterViewMode.Text = "完成";
            this.Button_EnterViewMode.UseVisualStyleBackColor = true;
            this.Button_EnterViewMode.Click += new System.EventHandler(this.Button_EnterViewMode_Click);
            // 
            // OpenFileDialog_Open
            // 
            this.OpenFileDialog_Open.Filter = "OTD 文件|*.otd";
            // 
            // SaveFileDialog_SaveAs
            // 
            this.SaveFileDialog_SaveAs.FileName = "未命名";
            this.SaveFileDialog_SaveAs.Filter = "OTD 文件|*.otd";
            // 
            // ContextMenuStrip_Children
            // 
            this.ContextMenuStrip_Children.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Children_Select,
            this.ToolStripMenuItem_Children_SetParent,
            this.toolStripSeparator1,
            this.ToolStripMenuItem_Children_MoveTop,
            this.ToolStripMenuItem_Children_MoveUp,
            this.ToolStripMenuItem_Children_MoveDown,
            this.ToolStripMenuItem_Children_MoveBottom,
            this.toolStripSeparator2,
            this.ToolStripMenuItem_Children_Delete,
            this.ToolStripMenuItem_Children_DeleteAll});
            this.ContextMenuStrip_Children.Name = "ContextMenuStrip_Children";
            this.ContextMenuStrip_Children.Size = new System.Drawing.Size(233, 232);
            // 
            // ToolStripMenuItem_Children_Select
            // 
            this.ToolStripMenuItem_Children_Select.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_Select.Name = "ToolStripMenuItem_Children_Select";
            this.ToolStripMenuItem_Children_Select.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_Select.Text = "选择...";
            // 
            // ToolStripMenuItem_Children_SetParent
            // 
            this.ToolStripMenuItem_Children_SetParent.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_SetParent.Name = "ToolStripMenuItem_Children_SetParent";
            this.ToolStripMenuItem_Children_SetParent.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_SetParent.Text = "将上级类群变更为选择的类群";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(229, 6);
            // 
            // ToolStripMenuItem_Children_MoveTop
            // 
            this.ToolStripMenuItem_Children_MoveTop.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_MoveTop.Name = "ToolStripMenuItem_Children_MoveTop";
            this.ToolStripMenuItem_Children_MoveTop.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_MoveTop.Text = "移至最上";
            // 
            // ToolStripMenuItem_Children_MoveUp
            // 
            this.ToolStripMenuItem_Children_MoveUp.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_MoveUp.Name = "ToolStripMenuItem_Children_MoveUp";
            this.ToolStripMenuItem_Children_MoveUp.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_MoveUp.Text = "上移";
            // 
            // ToolStripMenuItem_Children_MoveDown
            // 
            this.ToolStripMenuItem_Children_MoveDown.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_MoveDown.Name = "ToolStripMenuItem_Children_MoveDown";
            this.ToolStripMenuItem_Children_MoveDown.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_MoveDown.Text = "下移";
            // 
            // ToolStripMenuItem_Children_MoveBottom
            // 
            this.ToolStripMenuItem_Children_MoveBottom.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_MoveBottom.Name = "ToolStripMenuItem_Children_MoveBottom";
            this.ToolStripMenuItem_Children_MoveBottom.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_MoveBottom.Text = "移至最下";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(2);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // ToolStripMenuItem_Children_Delete
            // 
            this.ToolStripMenuItem_Children_Delete.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_Delete.Name = "ToolStripMenuItem_Children_Delete";
            this.ToolStripMenuItem_Children_Delete.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_Delete.Text = "删除并保留下级类群";
            // 
            // ToolStripMenuItem_Children_DeleteAll
            // 
            this.ToolStripMenuItem_Children_DeleteAll.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Children_DeleteAll.Name = "ToolStripMenuItem_Children_DeleteAll";
            this.ToolStripMenuItem_Children_DeleteAll.Size = new System.Drawing.Size(232, 22);
            this.ToolStripMenuItem_Children_DeleteAll.Text = "递归删除下级类群";
            // 
            // ContextMenuStrip_Parents
            // 
            this.ContextMenuStrip_Parents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Parents_Select});
            this.ContextMenuStrip_Parents.Name = "ContextMenuStrip_Parents";
            this.ContextMenuStrip_Parents.Size = new System.Drawing.Size(110, 30);
            // 
            // ToolStripMenuItem_Parents_Select
            // 
            this.ToolStripMenuItem_Parents_Select.Margin = new System.Windows.Forms.Padding(2);
            this.ToolStripMenuItem_Parents_Select.Name = "ToolStripMenuItem_Parents_Select";
            this.ToolStripMenuItem_Parents_Select.Size = new System.Drawing.Size(109, 22);
            this.ToolStripMenuItem_Parents_Select.Text = "选择...";
            // 
            // TagGroup_ViewMode_Tags
            // 
            this.TagGroup_ViewMode_Tags.BackColor = System.Drawing.Color.Transparent;
            this.TagGroup_ViewMode_Tags.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TagGroup_ViewMode_Tags.IsDarkTheme = false;
            this.TagGroup_ViewMode_Tags.Location = new System.Drawing.Point(25, 25);
            this.TagGroup_ViewMode_Tags.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TagGroup_ViewMode_Tags.MinTagSize = new System.Drawing.Size(32, 22);
            this.TagGroup_ViewMode_Tags.Name = "TagGroup_ViewMode_Tags";
            this.TagGroup_ViewMode_Tags.Size = new System.Drawing.Size(300, 25);
            this.TagGroup_ViewMode_Tags.TabIndex = 0;
            this.TagGroup_ViewMode_Tags.TabStop = false;
            this.TagGroup_ViewMode_Tags.TagPadding = new System.Windows.Forms.Padding(2);
            // 
            // TagGroup_Synonyms
            // 
            this.TagGroup_Synonyms.BackColor = System.Drawing.Color.Transparent;
            this.TagGroup_Synonyms.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TagGroup_Synonyms.IsDarkTheme = false;
            this.TagGroup_Synonyms.Location = new System.Drawing.Point(0, 60);
            this.TagGroup_Synonyms.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TagGroup_Synonyms.MinTagSize = new System.Drawing.Size(32, 22);
            this.TagGroup_Synonyms.Name = "TagGroup_Synonyms";
            this.TagGroup_Synonyms.Size = new System.Drawing.Size(350, 40);
            this.TagGroup_Synonyms.TabIndex = 0;
            this.TagGroup_Synonyms.TabStop = false;
            this.TagGroup_Synonyms.TagPadding = new System.Windows.Forms.Padding(2);
            // 
            // TaxonNameButtonGroup_ViewMode_Children
            // 
            this.TaxonNameButtonGroup_ViewMode_Children.BackColor = System.Drawing.Color.Transparent;
            this.TaxonNameButtonGroup_ViewMode_Children.ButtonHeight = 22;
            this.TaxonNameButtonGroup_ViewMode_Children.ButtonPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.TaxonNameButtonGroup_ViewMode_Children.CategoryNameWidth = 50;
            this.TaxonNameButtonGroup_ViewMode_Children.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TaxonNameButtonGroup_ViewMode_Children.GroupNameWidth = 0;
            this.TaxonNameButtonGroup_ViewMode_Children.GroupPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.TaxonNameButtonGroup_ViewMode_Children.IsDarkTheme = false;
            this.TaxonNameButtonGroup_ViewMode_Children.Location = new System.Drawing.Point(0, 60);
            this.TaxonNameButtonGroup_ViewMode_Children.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TaxonNameButtonGroup_ViewMode_Children.Name = "TaxonNameButtonGroup_ViewMode_Children";
            this.TaxonNameButtonGroup_ViewMode_Children.Size = new System.Drawing.Size(350, 40);
            this.TaxonNameButtonGroup_ViewMode_Children.TabIndex = 0;
            this.TaxonNameButtonGroup_ViewMode_Children.TabStop = false;
            // 
            // TaxonNameButtonGroup_ViewMode_Parents
            // 
            this.TaxonNameButtonGroup_ViewMode_Parents.BackColor = System.Drawing.Color.Transparent;
            this.TaxonNameButtonGroup_ViewMode_Parents.ButtonHeight = 22;
            this.TaxonNameButtonGroup_ViewMode_Parents.ButtonPadding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.TaxonNameButtonGroup_ViewMode_Parents.CategoryNameWidth = 50;
            this.TaxonNameButtonGroup_ViewMode_Parents.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TaxonNameButtonGroup_ViewMode_Parents.GroupNameWidth = 30;
            this.TaxonNameButtonGroup_ViewMode_Parents.GroupPadding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.TaxonNameButtonGroup_ViewMode_Parents.IsDarkTheme = false;
            this.TaxonNameButtonGroup_ViewMode_Parents.Location = new System.Drawing.Point(0, 60);
            this.TaxonNameButtonGroup_ViewMode_Parents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TaxonNameButtonGroup_ViewMode_Parents.Name = "TaxonNameButtonGroup_ViewMode_Parents";
            this.TaxonNameButtonGroup_ViewMode_Parents.Size = new System.Drawing.Size(350, 40);
            this.TaxonNameButtonGroup_ViewMode_Parents.TabIndex = 0;
            this.TaxonNameButtonGroup_ViewMode_Parents.TabStop = false;
            // 
            // TaxonNameButtonGroup_EditMode_Children
            // 
            this.TaxonNameButtonGroup_EditMode_Children.BackColor = System.Drawing.Color.Transparent;
            this.TaxonNameButtonGroup_EditMode_Children.ButtonHeight = 22;
            this.TaxonNameButtonGroup_EditMode_Children.ButtonPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.TaxonNameButtonGroup_EditMode_Children.CategoryNameWidth = 50;
            this.TaxonNameButtonGroup_EditMode_Children.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TaxonNameButtonGroup_EditMode_Children.GroupNameWidth = 0;
            this.TaxonNameButtonGroup_EditMode_Children.GroupPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.TaxonNameButtonGroup_EditMode_Children.IsDarkTheme = false;
            this.TaxonNameButtonGroup_EditMode_Children.Location = new System.Drawing.Point(0, 60);
            this.TaxonNameButtonGroup_EditMode_Children.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TaxonNameButtonGroup_EditMode_Children.Name = "TaxonNameButtonGroup_EditMode_Children";
            this.TaxonNameButtonGroup_EditMode_Children.Size = new System.Drawing.Size(350, 40);
            this.TaxonNameButtonGroup_EditMode_Children.TabIndex = 0;
            this.TaxonNameButtonGroup_EditMode_Children.TabStop = false;
            // 
            // TaxonNameButtonGroup_EditMode_Parents
            // 
            this.TaxonNameButtonGroup_EditMode_Parents.BackColor = System.Drawing.Color.Transparent;
            this.TaxonNameButtonGroup_EditMode_Parents.ButtonHeight = 22;
            this.TaxonNameButtonGroup_EditMode_Parents.ButtonPadding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.TaxonNameButtonGroup_EditMode_Parents.CategoryNameWidth = 50;
            this.TaxonNameButtonGroup_EditMode_Parents.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TaxonNameButtonGroup_EditMode_Parents.GroupNameWidth = 30;
            this.TaxonNameButtonGroup_EditMode_Parents.GroupPadding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.TaxonNameButtonGroup_EditMode_Parents.IsDarkTheme = false;
            this.TaxonNameButtonGroup_EditMode_Parents.Location = new System.Drawing.Point(0, 60);
            this.TaxonNameButtonGroup_EditMode_Parents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TaxonNameButtonGroup_EditMode_Parents.Name = "TaxonNameButtonGroup_EditMode_Parents";
            this.TaxonNameButtonGroup_EditMode_Parents.Size = new System.Drawing.Size(350, 40);
            this.TaxonNameButtonGroup_EditMode_Parents.TabIndex = 0;
            this.TaxonNameButtonGroup_EditMode_Parents.TabStop = false;
            // 
            // CategorySelector_EditMode_Category
            // 
            this.CategorySelector_EditMode_Category.AutoSize = true;
            this.CategorySelector_EditMode_Category.BackColor = System.Drawing.Color.Transparent;
            this.CategorySelector_EditMode_Category.ButtonPadding = new System.Windows.Forms.Padding(2);
            this.CategorySelector_EditMode_Category.Category = TreeOfLife.TaxonomicCategory.Unranked;
            this.CategorySelector_EditMode_Category.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CategorySelector_EditMode_Category.GroupPadding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.CategorySelector_EditMode_Category.IsDarkTheme = false;
            this.CategorySelector_EditMode_Category.Location = new System.Drawing.Point(80, 25);
            this.CategorySelector_EditMode_Category.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CategorySelector_EditMode_Category.MinButtonSize = new System.Drawing.Size(30, 22);
            this.CategorySelector_EditMode_Category.Name = "CategorySelector_EditMode_Category";
            this.CategorySelector_EditMode_Category.Size = new System.Drawing.Size(270, 22);
            this.CategorySelector_EditMode_Category.TabIndex = 0;
            this.CategorySelector_EditMode_Category.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Panel_Main);
            this.Name = "MainForm";
            this.Panel_Main.ResumeLayout(false);
            this.Panel_PhylogeneticTree.ResumeLayout(false);
            this.Panel_PhylogeneticTree.PerformLayout();
            this.ContextMenuStrip_File.ResumeLayout(false);
            this.Panel_TaxonInfo.ResumeLayout(false);
            this.Panel_TaxonInfo_ViewMode.ResumeLayout(false);
            this.Panel_ViewMode_Desc.ResumeLayout(false);
            this.Panel_ViewMode_Desc.PerformLayout();
            this.Panel_ViewMode_Tags.ResumeLayout(false);
            this.Panel_ViewMode_Synonyms.ResumeLayout(false);
            this.Panel_ViewMode_Children.ResumeLayout(false);
            this.Panel_ViewMode_Parents.ResumeLayout(false);
            this.Panel_ViewMode_Title.ResumeLayout(false);
            this.Panel_ViewMode_TaxonName.ResumeLayout(false);
            this.Panel_TaxonInfo_EditMode.ResumeLayout(false);
            this.Panel_EditMode_Children.ResumeLayout(false);
            this.Panel_EditMode_Parents.ResumeLayout(false);
            this.Panel_EditMode_AddChildren.ResumeLayout(false);
            this.Panel_EditMode_AddChildren.PerformLayout();
            this.Panel_EditMode_AddParent.ResumeLayout(false);
            this.Panel_EditMode_AddParent.PerformLayout();
            this.Panel_EditMode_Desc.ResumeLayout(false);
            this.Panel_EditMode_Desc.PerformLayout();
            this.Panel_EditMode_Tags.ResumeLayout(false);
            this.Panel_EditMode_Tags.PerformLayout();
            this.Panel_EditMode_Synonyms.ResumeLayout(false);
            this.Panel_EditMode_Synonyms.PerformLayout();
            this.Panel_EditMode_Category.ResumeLayout(false);
            this.Panel_EditMode_Category.PerformLayout();
            this.Panel_EditMode_State.ResumeLayout(false);
            this.Panel_EditMode_State.PerformLayout();
            this.Panel_EditMode_TaxonName.ResumeLayout(false);
            this.Panel_EditMode_TaxonName.PerformLayout();
            this.ContextMenuStrip_Children.ResumeLayout(false);
            this.ContextMenuStrip_Parents.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Panel Panel_TaxonInfo;
        private System.Windows.Forms.Panel Panel_TaxonInfo_EditMode;
        private System.Windows.Forms.Panel Panel_TaxonInfo_ViewMode;
        private TaxonNameButtonGroup TaxonNameButtonGroup_ViewMode_Parents;
        private TaxonNameButtonGroup TaxonNameButtonGroup_ViewMode_Children;
        private System.Windows.Forms.Label Label_ViewMode_TaxonName;
        private System.Windows.Forms.Label Label_ViewMode_CategoryName;
        private System.Windows.Forms.Panel Panel_ViewMode_Title;
        private System.Windows.Forms.Panel Panel_ViewMode_Parents;
        private System.Windows.Forms.Panel Panel_ViewMode_Children;
        private System.Windows.Forms.Label Label_ViewMode_Children;
        private System.Windows.Forms.Label Label_ViewMode_Parents;
        private System.Windows.Forms.Button Button_EnterViewMode;
        private System.Windows.Forms.Button Button_EnterEditMode;
        private System.Windows.Forms.Panel Panel_EditMode_TaxonName;
        private System.Windows.Forms.Label Label_EditMode_Name;
        private System.Windows.Forms.TextBox TextBox_EditMode_Name;
        private System.Windows.Forms.TextBox TextBox_EditMode_ChsName;
        private System.Windows.Forms.Label Label_EditMode_ChsName;
        private System.Windows.Forms.Panel Panel_EditMode_State;
        private System.Windows.Forms.CheckBox CheckBox_EditMode_EX;
        private System.Windows.Forms.CheckBox CheckBox_EditMode_Unsure;
        private System.Windows.Forms.Panel Panel_EditMode_Category;
        private System.Windows.Forms.Label Label_EditMode_Category;
        private System.Windows.Forms.Panel Panel_EditMode_Synonyms;
        private System.Windows.Forms.Label Label_EditMode_Synonyms;
        private System.Windows.Forms.TextBox TextBox_EditMode_Synonyms;
        private System.Windows.Forms.Panel Panel_EditMode_Tags;
        private System.Windows.Forms.TextBox TextBox_EditMode_Tags;
        private System.Windows.Forms.Label Label_EditMode_Tags;
        private System.Windows.Forms.Panel Panel_EditMode_Desc;
        private System.Windows.Forms.TextBox TextBox_EditMode_Desc;
        private System.Windows.Forms.Label Label_EditMode_Desc;
        private System.Windows.Forms.Panel Panel_EditMode_AddParent;
        private System.Windows.Forms.TextBox TextBox_EditMode_AddParent;
        private System.Windows.Forms.Label Label_EditMode_AddParent;
        private System.Windows.Forms.Button Button_EditMode_AddParentUplevel;
        private System.Windows.Forms.Panel Panel_EditMode_AddChildren;
        private System.Windows.Forms.TextBox TextBox_EditMode_AddChildren;
        private System.Windows.Forms.Label Label_EditMode_AddChildren;
        private System.Windows.Forms.Button Button_EditMode_AddChildren;
        private System.Windows.Forms.Panel Panel_EditMode_Children;
        private System.Windows.Forms.Label Label_EditMode_Children;
        private TaxonNameButtonGroup TaxonNameButtonGroup_EditMode_Children;
        private System.Windows.Forms.Panel Panel_EditMode_Parents;
        private System.Windows.Forms.Label Label_EditMode_Parents;
        private TaxonNameButtonGroup TaxonNameButtonGroup_EditMode_Parents;
        private System.Windows.Forms.Label Label_EditMode_State;
        private CategorySelector CategorySelector_EditMode_Category;
        private System.Windows.Forms.Panel Panel_ViewMode_Synonyms;
        private System.Windows.Forms.Label Label_ViewMode_Synonyms;
        private System.Windows.Forms.Panel Panel_ViewMode_Tags;
        private System.Windows.Forms.Label Label_ViewMode_Tags;
        private System.Windows.Forms.Panel Panel_ViewMode_Desc;
        private System.Windows.Forms.Label Label_ViewMode_Desc;
        private System.Windows.Forms.Label Label_ViewMode_Desc_Value;
        private TagGroup TagGroup_ViewMode_Tags;
        private TagGroup TagGroup_Synonyms;
        private System.Windows.Forms.Panel Panel_PhylogeneticTree;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_File;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_File_Open;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_File_Save;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_File_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_File_Close;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog_Open;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog_SaveAs;
        private System.Windows.Forms.TextBox TextBox_PhylogeneticTree;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_Children;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_MoveTop;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_MoveUp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_MoveDown;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_MoveBottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_Delete;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_DeleteAll;
        private System.Windows.Forms.Button Button_EditMode_AddParentDownlevel;
        private System.Windows.Forms.ContextMenuStrip ContextMenuStrip_Parents;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Parents_Select;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_Select;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Children_SetParent;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel Panel_ViewMode_TaxonName;
    }
}

