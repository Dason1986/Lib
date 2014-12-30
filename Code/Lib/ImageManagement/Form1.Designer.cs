namespace ImageManagement
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.PicSource = new System.Windows.Forms.PictureBox();
            this.PicWaterSource = new System.Windows.Forms.PictureBox();
            this.PicTarget = new System.Windows.Forms.PictureBox();
            this.BtnBuilder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtWaterPic = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnWaterPic = new System.Windows.Forms.Button();
            this.ListBSource = new System.Windows.Forms.ListBox();
            this.BtnSelectSourcePic = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ChkPreview = new System.Windows.Forms.CheckBox();
            this.BtnSelectFolder = new System.Windows.Forms.Button();
            this.NumOpacity = new System.Windows.Forms.NumericUpDown();
            this.NumOffsetX = new System.Windows.Forms.NumericUpDown();
            this.NumOffsetY = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.NumspaceY = new System.Windows.Forms.NumericUpDown();
            this.NumspaceX = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.PicSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWaterSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicTarget)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumOpacity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOffsetX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumspaceY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumspaceX)).BeginInit();
            this.SuspendLayout();
            // 
            // PicSource
            // 
            this.PicSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicSource.Location = new System.Drawing.Point(17, 19);
            this.PicSource.Name = "PicSource";
            this.PicSource.Size = new System.Drawing.Size(158, 159);
            this.PicSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicSource.TabIndex = 0;
            this.PicSource.TabStop = false;
            // 
            // PicWaterSource
            // 
            this.PicWaterSource.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicWaterSource.Location = new System.Drawing.Point(211, 19);
            this.PicWaterSource.Name = "PicWaterSource";
            this.PicWaterSource.Size = new System.Drawing.Size(158, 159);
            this.PicWaterSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicWaterSource.TabIndex = 0;
            this.PicWaterSource.TabStop = false;
            // 
            // PicTarget
            // 
            this.PicTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicTarget.Location = new System.Drawing.Point(410, 19);
            this.PicTarget.Name = "PicTarget";
            this.PicTarget.Size = new System.Drawing.Size(158, 159);
            this.PicTarget.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicTarget.TabIndex = 0;
            this.PicTarget.TabStop = false;
            // 
            // BtnBuilder
            // 
            this.BtnBuilder.Location = new System.Drawing.Point(47, 452);
            this.BtnBuilder.Name = "BtnBuilder";
            this.BtnBuilder.Size = new System.Drawing.Size(75, 23);
            this.BtnBuilder.TabIndex = 1;
            this.BtnBuilder.Text = "生成";
            this.BtnBuilder.UseVisualStyleBackColor = true;
            this.BtnBuilder.Click += new System.EventHandler(this.BtnBuilder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.PicTarget);
            this.groupBox1.Controls.Add(this.PicSource);
            this.groupBox1.Controls.Add(this.PicWaterSource);
            this.groupBox1.Location = new System.Drawing.Point(30, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(671, 202);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "水印生成";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(465, 181);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "效果";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(269, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "水印";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(385, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "=";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "+";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(73, 181);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "原圖";
            // 
            // TxtWaterPic
            // 
            this.TxtWaterPic.Location = new System.Drawing.Point(75, 20);
            this.TxtWaterPic.Name = "TxtWaterPic";
            this.TxtWaterPic.Size = new System.Drawing.Size(281, 22);
            this.TxtWaterPic.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "水印：";
            // 
            // BtnWaterPic
            // 
            this.BtnWaterPic.Location = new System.Drawing.Point(373, 20);
            this.BtnWaterPic.Name = "BtnWaterPic";
            this.BtnWaterPic.Size = new System.Drawing.Size(75, 23);
            this.BtnWaterPic.TabIndex = 5;
            this.BtnWaterPic.Text = "選擇水印圖";
            this.BtnWaterPic.UseVisualStyleBackColor = true;
            this.BtnWaterPic.Click += new System.EventHandler(this.BtnWaterPic_Click);
            // 
            // ListBSource
            // 
            this.ListBSource.FormattingEnabled = true;
            this.ListBSource.ItemHeight = 12;
            this.ListBSource.Location = new System.Drawing.Point(75, 71);
            this.ListBSource.Name = "ListBSource";
            this.ListBSource.Size = new System.Drawing.Size(281, 136);
            this.ListBSource.TabIndex = 6;
            this.ListBSource.SelectedValueChanged += new System.EventHandler(this.ListBSource_SelectedValueChanged);
            // 
            // BtnSelectSourcePic
            // 
            this.BtnSelectSourcePic.Location = new System.Drawing.Point(373, 184);
            this.BtnSelectSourcePic.Name = "BtnSelectSourcePic";
            this.BtnSelectSourcePic.Size = new System.Drawing.Size(75, 23);
            this.BtnSelectSourcePic.TabIndex = 7;
            this.BtnSelectSourcePic.Text = "選擇圖片";
            this.BtnSelectSourcePic.UseVisualStyleBackColor = true;
            this.BtnSelectSourcePic.Click += new System.EventHandler(this.BtnSelectSourcePic_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 71);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 4;
            this.label7.Text = "原圖：";
            // 
            // ChkPreview
            // 
            this.ChkPreview.AutoSize = true;
            this.ChkPreview.Checked = true;
            this.ChkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkPreview.Location = new System.Drawing.Point(376, 159);
            this.ChkPreview.Name = "ChkPreview";
            this.ChkPreview.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ChkPreview.Size = new System.Drawing.Size(72, 16);
            this.ChkPreview.TabIndex = 8;
            this.ChkPreview.Text = "直接預覽";
            this.ChkPreview.UseVisualStyleBackColor = true;
            // 
            // BtnSelectFolder
            // 
            this.BtnSelectFolder.Location = new System.Drawing.Point(467, 184);
            this.BtnSelectFolder.Name = "BtnSelectFolder";
            this.BtnSelectFolder.Size = new System.Drawing.Size(75, 23);
            this.BtnSelectFolder.TabIndex = 9;
            this.BtnSelectFolder.Text = "選擇文件夾";
            this.BtnSelectFolder.UseVisualStyleBackColor = true;
            this.BtnSelectFolder.Click += new System.EventHandler(this.BtnSelectFolder_Click);
            // 
            // NumOpacity
            // 
            this.NumOpacity.Location = new System.Drawing.Point(433, 126);
            this.NumOpacity.Name = "NumOpacity";
            this.NumOpacity.Size = new System.Drawing.Size(75, 22);
            this.NumOpacity.TabIndex = 10;
            this.NumOpacity.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // NumOffsetX
            // 
            this.NumOffsetX.Location = new System.Drawing.Point(433, 67);
            this.NumOffsetX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumOffsetX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.NumOffsetX.Name = "NumOffsetX";
            this.NumOffsetX.Size = new System.Drawing.Size(75, 22);
            this.NumOffsetX.TabIndex = 11;
            this.NumOffsetX.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // NumOffsetY
            // 
            this.NumOffsetY.Location = new System.Drawing.Point(433, 95);
            this.NumOffsetY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumOffsetY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.NumOffsetY.Name = "NumOffsetY";
            this.NumOffsetY.Size = new System.Drawing.Size(75, 22);
            this.NumOffsetY.TabIndex = 11;
            this.NumOffsetY.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(374, 131);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "不透明度";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(374, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(37, 12);
            this.label9.TabIndex = 13;
            this.label9.Text = "偏移X";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(374, 100);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "偏移Y";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(531, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(37, 12);
            this.label11.TabIndex = 18;
            this.label11.Text = "間隔Y";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(531, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 12);
            this.label12.TabIndex = 17;
            this.label12.Text = "間隔X";
            // 
            // NumspaceY
            // 
            this.NumspaceY.Location = new System.Drawing.Point(590, 95);
            this.NumspaceY.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumspaceY.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.NumspaceY.Name = "NumspaceY";
            this.NumspaceY.Size = new System.Drawing.Size(75, 22);
            this.NumspaceY.TabIndex = 15;
            this.NumspaceY.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // NumspaceX
            // 
            this.NumspaceX.Location = new System.Drawing.Point(590, 67);
            this.NumspaceX.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.NumspaceX.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.NumspaceX.Name = "NumspaceX";
            this.NumspaceX.Size = new System.Drawing.Size(75, 22);
            this.NumspaceX.TabIndex = 16;
            this.NumspaceX.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 499);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.NumspaceY);
            this.Controls.Add(this.NumspaceX);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.NumOffsetY);
            this.Controls.Add(this.NumOffsetX);
            this.Controls.Add(this.NumOpacity);
            this.Controls.Add(this.BtnSelectFolder);
            this.Controls.Add(this.ChkPreview);
            this.Controls.Add(this.BtnSelectSourcePic);
            this.Controls.Add(this.BtnBuilder);
            this.Controls.Add(this.ListBSource);
            this.Controls.Add(this.BtnWaterPic);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TxtWaterPic);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.PicSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicWaterSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicTarget)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumOpacity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOffsetX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumspaceY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumspaceX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PicSource;
        private System.Windows.Forms.PictureBox PicWaterSource;
        private System.Windows.Forms.PictureBox PicTarget;
        private System.Windows.Forms.Button BtnBuilder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtWaterPic;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnWaterPic;
        private System.Windows.Forms.ListBox ListBSource;
        private System.Windows.Forms.Button BtnSelectSourcePic;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox ChkPreview;
        private System.Windows.Forms.Button BtnSelectFolder;
        private System.Windows.Forms.NumericUpDown NumOpacity;
        private System.Windows.Forms.NumericUpDown NumOffsetX;
        private System.Windows.Forms.NumericUpDown NumOffsetY;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown NumspaceY;
        private System.Windows.Forms.NumericUpDown NumspaceX;
    }
}

