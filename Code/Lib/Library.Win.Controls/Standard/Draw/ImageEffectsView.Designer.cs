using System.Windows.Forms;

namespace Library.Controls
{
    partial class ImageEffectsView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.LBEffects = new System.Windows.Forms.ListBox();
            this.grid = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnSelectImage = new Library.Controls.Button();
            this.BtnBuilder = new Library.Controls.Button();
            this.BtnOK = new Library.Controls.Button();
            this.checkBox1 = new Library.Controls.CheckBox();
            this.btnSave = new Library.Controls.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(596, 615);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(596, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 615);
            this.panel1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(260, 544);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.propertyGrid1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(252, 518);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "圖像信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(246, 512);
            this.propertyGrid1.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.LBEffects);
            this.tabPage2.Controls.Add(this.grid);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(252, 518);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "濾鏡信息";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // LBEffects
            // 
            this.LBEffects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LBEffects.FormattingEnabled = true;
            this.LBEffects.ItemHeight = 12;
            this.LBEffects.Location = new System.Drawing.Point(3, 3);
            this.LBEffects.Name = "LBEffects";
            this.LBEffects.Size = new System.Drawing.Size(246, 329);
            this.LBEffects.TabIndex = 8;
            this.LBEffects.SelectedIndexChanged += new System.EventHandler(this.LBEffects_SelectedIndexChanged);
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grid.Location = new System.Drawing.Point(3, 332);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(246, 183);
            this.grid.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.BtnSelectImage);
            this.panel2.Controls.Add(this.BtnBuilder);
            this.panel2.Controls.Add(this.BtnOK);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 544);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(260, 71);
            this.panel2.TabIndex = 4;
            // 
            // BtnSelectImage
            // 
            this.BtnSelectImage.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BtnSelectImage.Location = new System.Drawing.Point(12, 6);
            this.BtnSelectImage.Name = "BtnSelectImage";
            this.BtnSelectImage.Size = new System.Drawing.Size(68, 23);
            this.BtnSelectImage.TabIndex = 1;
            this.BtnSelectImage.Text = "選擇圖像";
            this.BtnSelectImage.UseVisualStyleBackColor = true;
            this.BtnSelectImage.Click += new System.EventHandler(this.BtnSelectImage_Click);
            // 
            // BtnBuilder
            // 
            this.BtnBuilder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BtnBuilder.Location = new System.Drawing.Point(95, 36);
            this.BtnBuilder.Name = "BtnBuilder";
            this.BtnBuilder.Size = new System.Drawing.Size(68, 23);
            this.BtnBuilder.TabIndex = 1;
            this.BtnBuilder.Text = "生成";
            this.BtnBuilder.UseVisualStyleBackColor = true;
            this.BtnBuilder.Click += new System.EventHandler(this.BtnBuilder_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BtnOK.Location = new System.Drawing.Point(179, 36);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(68, 23);
            this.BtnOK.TabIndex = 3;
            this.BtnOK.Text = "確定";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.checkBox1.Location = new System.Drawing.Point(12, 36);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "指針算法";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.btnSave.Location = new System.Drawing.Point(179, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(68, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存圖像";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ImageEffectsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 615);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "ImageEffectsView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "圖像處理";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private CheckBox checkBox1;
        private Button BtnBuilder;
        private Button BtnOK;
        private Panel panel1;
        private Panel panel2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private PropertyGrid propertyGrid1;
        private TabPage tabPage2;
        private ListBox LBEffects;
        private PropertyGrid grid;
        private Button BtnSelectImage;
        private Button btnSave;
    }
}