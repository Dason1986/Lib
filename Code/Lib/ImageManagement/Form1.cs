﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Library.Draw;
using Library.Draw.Effects;
using Library.Draw.Water;

namespace ImageManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.PicTarget.ContextMenu = new ContextMenu();
            this.PicTarget.ContextMenu.MenuItems.Add("asdf");
        }


        private void BtnWaterPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "透明水印圖片類型(*.png,*.gif)|*.png;*.gif";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() != DialogResult.OK) return;
            TxtWaterPic.Text = dialog.FileName;
            PicWaterSource.Image = Image.FromFile(dialog.FileName);
        }

        private void BtnSelectSourcePic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "支持圖片類型(*.png,*.gif,*.jpg,*.bmp)|*.png;*.gif;*.jpg;*.bmp";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() != DialogResult.OK) return;
            if (dialog.FileNames.Length == 0) return;
            foreach (var fileName in dialog.FileNames)
            {
                ListBSource.Items.Add(fileName);
            }
        }

        private void ListBSource_SelectedValueChanged(object sender, EventArgs e)
        {
            if (ListBSource.SelectedItem == null) return;
            var path = ListBSource.SelectedItem as string;
            if (!File.Exists(path)) return;
            PicSource.Image = Image.FromFile(path);
            if (ChkPreview.Checked) Builder(path, TxtWaterPic.Text);
        }

        readonly WaterImageBuilder _builder = WaterImageFactory.CreateBuilder(WaterImageType.Tile);
        public void Builder(string sourceImg, string waterImg)
        {
            _builder.SetSourceImage(sourceImg);

            _builder.SetWaterImage ( waterImg);
     
           var opetion = new WaterImageTileOption
            {
                Offset = new Point((int) NumOffsetX.Value, (int) NumOffsetY.Value),
                Space = new Size((int) NumspaceX.Value, (int) NumspaceY.Value),
                Opacity = (float) NumOpacity.Value/100
            };
           PicTarget.Image = _builder.ProcessBitmap(opetion);

        }

        private void BtnBuilder_Click(object sender, EventArgs e)
        {

        }

        private void BtnSelectFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;
            var files = System.IO.Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                ListBSource.Items.Add(file);
            }
        }
    }
   
   
}
