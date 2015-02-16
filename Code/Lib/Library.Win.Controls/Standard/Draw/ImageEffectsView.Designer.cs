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
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new Library.Controls.GroupBox();
            this.groupBox1 = new Library.Controls.GroupBox();
            this.grid = new System.Windows.Forms.PropertyGrid();
            this.BtnBuilder = new Library.Controls.Button();
            this.BtnOK = new Library.Controls.Button();
            this.checkBox1 = new Library.Controls.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(589, 615);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(589, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(267, 615);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnBuilder);
            this.panel2.Controls.Add(this.BtnOK);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 563);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(267, 52);
            this.panel2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 297);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "濾鏡選擇";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grid);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 266);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "濾鏡屬性";
            // 
            // grid
            // 
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(3, 18);
            this.grid.Name = "grid";
            this.grid.Size = new System.Drawing.Size(261, 245);
            this.grid.TabIndex = 2;
            // 
            // BtnBuilder
            // 
            this.BtnBuilder.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.BtnBuilder.Location = new System.Drawing.Point(100, 15);
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
            this.BtnOK.Location = new System.Drawing.Point(184, 15);
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
            this.checkBox1.Location = new System.Drawing.Point(17, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(75, 21);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "指針算法";
            this.checkBox1.UseVisualStyleBackColor = false;
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
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PropertyGrid grid;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GroupBox groupBox1;
        private CheckBox checkBox1;
        private Button BtnBuilder;
        private Button BtnOK;
        private Panel panel1;
        private Panel panel2;
        private GroupBox groupBox2;
    }
}