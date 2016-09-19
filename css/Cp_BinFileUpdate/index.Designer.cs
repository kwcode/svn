namespace DataBack
{
    partial class index
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.Log = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SourceBinTs = new System.Windows.Forms.Label();
            this.TargetBinTs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Log
            // 
            this.Log.AutoSize = true;
            this.Log.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.Log.Location = new System.Drawing.Point(43, 191);
            this.Log.Name = "Log";
            this.Log.Size = new System.Drawing.Size(53, 12);
            this.Log.TabIndex = 1;
            this.Log.Text = "日志提示";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(240, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "开始更新";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SourceBinTs
            // 
            this.SourceBinTs.AutoSize = true;
            this.SourceBinTs.ForeColor = System.Drawing.Color.Purple;
            this.SourceBinTs.Location = new System.Drawing.Point(34, 22);
            this.SourceBinTs.Name = "SourceBinTs";
            this.SourceBinTs.Size = new System.Drawing.Size(65, 12);
            this.SourceBinTs.TabIndex = 0;
            this.SourceBinTs.Text = "源文件目录";
            // 
            // TargetBinTs
            // 
            this.TargetBinTs.AutoSize = true;
            this.TargetBinTs.ForeColor = System.Drawing.Color.Blue;
            this.TargetBinTs.Location = new System.Drawing.Point(34, 51);
            this.TargetBinTs.Name = "TargetBinTs";
            this.TargetBinTs.Size = new System.Drawing.Size(89, 12);
            this.TargetBinTs.TabIndex = 3;
            this.TargetBinTs.Text = "要更新文件目录";
            // 
            // index
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 340);
            this.Controls.Add(this.TargetBinTs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Log);
            this.Controls.Add(this.SourceBinTs);
            this.Name = "index";
            this.Text = "按日期，删除备份超过N天的数据";
            this.Load += new System.EventHandler(this.index_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Log;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label SourceBinTs;
        private System.Windows.Forms.Label TargetBinTs;
    }
}

