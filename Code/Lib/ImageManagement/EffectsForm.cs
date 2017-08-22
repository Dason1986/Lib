using Library;
using Library.Draw;
using Library.Draw.Effects;
using Library.HelperUtility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace TestWinform
{
    public partial class EffectsForm : Form
    {
        public EffectsForm()
        {
            InitializeComponent();
            dt = EffectsHelper.GetEffects();
            comboBox1.DisplayMember = "DisplayName";
            comboBox1.ValueMember = "ImageBuilder";
            comboBox1.DataSource = dt;

            if (Program.Original == null) return;
            sourceiamge = new Bitmap(new MemoryStream(Program.Original));

           
            this.pictureBox1.Image = sourceiamge;
            //    fileBytes = Program.Original;
            CreateBuilder();
        }

        private Image sourceiamge;

        private DataTable dt;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "所支持图片|*.png;*.jpg;*.gif";
            var result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                //    fileBytes = File.ReadAllBytes(fileDialog.FileName);
                var stream = fileDialog.OpenFile();
                sourceiamge = new Bitmap(stream);
                this.pictureBox1.Image = sourceiamge;
                CreateBuilder();
            }
        }

        private IImageBuilder builderobj;
        private ImageOption option;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Enabled = false;
                builderobj.SetOpetion(option);
                builderobj.SetSourceImage(this.sourceiamge);
                this.pictureBox2.Image = checkBox1.Checked
                    ? builderobj.UnsafeProcessBitmap()
                    : builderobj.ProcessBitmap();
            }
            catch (Exception ex)
            {
                this.pictureBox2.Image = null;
                MessageBox.Show(ex.Message);
            }
            finally
            {
                panel1.Enabled = true;
            }
        }

        private readonly IDictionary<Type, IImageBuilder> _dicinObjects = new ConcurrentDictionary<Type, IImageBuilder>();

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateBuilder();
        }

        private void CreateBuilder()
        {
            builderobj = comboBox1.SelectedValue as IImageBuilder;

            option = builderobj.CreateOption();
            grid.SelectedObject = option;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (var imageBuilder in _dicinObjects.Values)
            {
                imageBuilder.ProcessCompleted -= builderobj_ProcessCompleted;
            }
            _dicinObjects.Clear();
            base.OnClosed(e);
        }

        private void builderobj_ProcessCompleted(object sender, ImageEventArgs e)
        {
            this.pictureBox2.Image = e.Image;
            builderobj.ProcessCompleted -= builderobj_ProcessCompleted;
            if (e.Error != null) MessageBox.Show(e.Error.Message);
            panel1.Enabled = true;
     
        }

      

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Enabled = false;
        
            builderobj.SetOpetion(option);
            builderobj.SetSourceImage(this.sourceiamge);
            builderobj.ProcessCompleted += builderobj_ProcessCompleted;
            if (checkBox1.Checked)
            {
                builderobj.UnsafeProcessBitmapAsync();
            }
            else
            {
                builderobj.ProcessBitmapAsync();
            }
        }
    }
}