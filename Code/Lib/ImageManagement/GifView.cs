using Library.Draw.Gif;
using Library.HelperUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWinform
{
    public partial class GifView : Form
    {
        private readonly Image Gbitmap;
        private byte[] giphy;

        public GifView()
        {
            InitializeComponent();
            var file = typeof(SheetForm).Assembly.GetManifestResourceStream("TestWinform.giphy.gif");
            if (file != null)
            {
                giphy = file.ToArray();
            }
            Gbitmap = new Bitmap(new MemoryStream(giphy));
            pictureBox1.Image = Gbitmap;
            GifReader readre = new GifReader(new MemoryStream(giphy));
      //      imageList21.ImageSize = readre.Size;
            for (var i = 0; i < readre.FrameCount; i++)
            {
                var image = readre.GetFrame(i);
                imageList1.Images.Add(image);
                images.Add(image);
                listView1.Items.Add(i.ToString(), i);
            
            }
        }

    //    private System.Windows.Forms.ImageList imageList21 = new ImageList( );
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices.Count > 0) {
                pictureBox2.Image = images[listView1.SelectedIndices[0]];
            }
        }
       IList<Image> images = new List<Image>();
        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
