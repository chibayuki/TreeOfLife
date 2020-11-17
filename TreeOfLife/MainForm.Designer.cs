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
            this.taxonNameButtonGroup_Children = new TreeOfLife.TaxonNameButtonGroup();
            this.taxonNameButtonGroup_Parents = new TreeOfLife.TaxonNameButtonGroup();
            this.SuspendLayout();
            // 
            // taxonNameButtonGroup_Children
            // 
            this.taxonNameButtonGroup_Children.BackColor = System.Drawing.Color.Transparent;
            this.taxonNameButtonGroup_Children.ButtonHeight = 20;
            this.taxonNameButtonGroup_Children.ButtonPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.taxonNameButtonGroup_Children.CategoryNameWidth = 50;
            this.taxonNameButtonGroup_Children.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.taxonNameButtonGroup_Children.GroupNameWidth = 0;
            this.taxonNameButtonGroup_Children.GroupPadding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.taxonNameButtonGroup_Children.IsDarkTheme = false;
            this.taxonNameButtonGroup_Children.Location = new System.Drawing.Point(13, 238);
            this.taxonNameButtonGroup_Children.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.taxonNameButtonGroup_Children.Name = "taxonNameButtonGroup_Children";
            this.taxonNameButtonGroup_Children.Size = new System.Drawing.Size(405, 198);
            this.taxonNameButtonGroup_Children.TabIndex = 17;
            // 
            // taxonNameButtonGroup_Parents
            // 
            this.taxonNameButtonGroup_Parents.BackColor = System.Drawing.Color.Transparent;
            this.taxonNameButtonGroup_Parents.ButtonHeight = 20;
            this.taxonNameButtonGroup_Parents.ButtonPadding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.taxonNameButtonGroup_Parents.CategoryNameWidth = 50;
            this.taxonNameButtonGroup_Parents.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.taxonNameButtonGroup_Parents.GroupNameWidth = 25;
            this.taxonNameButtonGroup_Parents.GroupPadding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.taxonNameButtonGroup_Parents.IsDarkTheme = false;
            this.taxonNameButtonGroup_Parents.Location = new System.Drawing.Point(426, 13);
            this.taxonNameButtonGroup_Parents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.taxonNameButtonGroup_Parents.Name = "taxonNameButtonGroup_Parents";
            this.taxonNameButtonGroup_Parents.Size = new System.Drawing.Size(361, 423);
            this.taxonNameButtonGroup_Parents.TabIndex = 16;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.taxonNameButtonGroup_Children);
            this.Controls.Add(this.taxonNameButtonGroup_Parents);
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private TaxonNameButtonGroup taxonNameButtonGroup_Parents;
        private TaxonNameButtonGroup taxonNameButtonGroup_Children;
    }
}

