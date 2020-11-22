namespace TreeOfLife
{
    partial class CategoryNameButton
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Panel_Main = new System.Windows.Forms.Panel();
            this.Label_CategoryName = new System.Windows.Forms.Label();
            this.Panel_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Main
            // 
            this.Panel_Main.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Main.Controls.Add(this.Label_CategoryName);
            this.Panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_Main.Location = new System.Drawing.Point(0, 0);
            this.Panel_Main.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Panel_Main.Name = "Panel_Main";
            this.Panel_Main.Size = new System.Drawing.Size(150, 150);
            this.Panel_Main.TabIndex = 0;
            // 
            // Label_CategoryName
            // 
            this.Label_CategoryName.AutoEllipsis = true;
            this.Label_CategoryName.BackColor = System.Drawing.Color.Transparent;
            this.Label_CategoryName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Label_CategoryName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label_CategoryName.ForeColor = System.Drawing.Color.White;
            this.Label_CategoryName.Location = new System.Drawing.Point(0, 0);
            this.Label_CategoryName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label_CategoryName.Name = "Label_CategoryName";
            this.Label_CategoryName.Size = new System.Drawing.Size(150, 150);
            this.Label_CategoryName.TabIndex = 0;
            this.Label_CategoryName.Text = "CN";
            this.Label_CategoryName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label_CategoryName.Click += new System.EventHandler(this.Label_CategoryName_Click);
            // 
            // CategoryNameButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.Panel_Main);
            this.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "CategoryNameButton";
            this.Panel_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_Main;
        private System.Windows.Forms.Label Label_CategoryName;
    }
}
