using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using Library.Controls;

namespace Library.Controls
{
    /// <summary>
    /// 自定义控件:半透明控件
    /// </summary>
    /* 
     * [ToolboxBitmap(typeof(MyOpaqueLayer))]
     * 用于指定当把你做好的自定义控件添加到工具栏时，工具栏显示的图标。
     * 正确写法应该是
     * [ToolboxBitmap(typeof(XXXXControl),"xxx.bmp")]
     * 其中XXXXControl是你的自定义控件，"xxx.bmp"是你要用的图标名称。
    */
    [ToolboxBitmap(typeof(OpaqueLayer))]
    public class OpaqueLayer : System.Windows.Forms.Control
    {
        internal readonly static IDictionary<object, OpaqueCommand> ControList = new Dictionary<object, OpaqueCommand>();
        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="alpha">透明度</param>
        /// <param name="isShowLoadingImage">是否显示图标</param>
        public static OpaqueCommand Show(Control control, int alpha, bool isShowLoadingImage)
        {
            OpaqueCommand command;
            if (ControList.ContainsKey(control))
            {
                command = ControList[control];
            }
            else
            {
                command = new OpaqueCommand();
                ControList.Add(control, command);
            }
            command.ShowOpaqueLayer(control, alpha, isShowLoadingImage);
            return command;
        }


        private bool _transparentBG = true;//是否使用透明
        private int _alpha = 125;//设置透明度

        private System.ComponentModel.Container components = new System.ComponentModel.Container();

        internal OpaqueLayer()
            : this(125, true)
        {
        }
        static readonly Image Imageloading = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.loading.gif");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="isShowLoadingImage"></param>
        internal OpaqueLayer(int alpha, bool isShowLoadingImage)
        {
            SetStyle(System.Windows.Forms.ControlStyles.Opaque, true);
            base.CreateControl();

            this._alpha = alpha;
            if (!isShowLoadingImage) return;
            PictureBox pictureBox_Loading = new PictureBox();
            pictureBox_Loading.BackColor = System.Drawing.Color.White;
            pictureBox_Loading.Image = Imageloading;
            pictureBox_Loading.Name = "pictureBox_Loading";
            pictureBox_Loading.Size = new System.Drawing.Size(48, 48);
            pictureBox_Loading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            Point Location = new Point(this.Location.X + (this.Width - pictureBox_Loading.Width) / 2, this.Location.Y + (this.Height - pictureBox_Loading.Height) / 2);//居中
            pictureBox_Loading.Location = Location;
            pictureBox_Loading.Anchor = AnchorStyles.None;
            this.Controls.Add(pictureBox_Loading);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!((components == null)))
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 自定义绘制窗体
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Pen labelBorderPen;
            SolidBrush labelBackColorBrush;

            if (_transparentBG)
            {
                Color drawColor = Color.FromArgb(this._alpha, this.BackColor);
                labelBorderPen = new Pen(drawColor, 0);
                labelBackColorBrush = new SolidBrush(drawColor);
            }
            else
            {
                labelBorderPen = new Pen(this.BackColor, 0);
                labelBackColorBrush = new SolidBrush(this.BackColor);
            }
            base.OnPaint(e);
            float vlblControlWidth = this.Size.Width;
            float vlblControlHeight = this.Size.Height;
            e.Graphics.DrawRectangle(labelBorderPen, 0, 0, vlblControlWidth, vlblControlHeight);
            e.Graphics.FillRectangle(labelBackColorBrush, 0, 0, vlblControlWidth, vlblControlHeight);
        }


        protected override CreateParams CreateParams//v1.10 
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; //0x20;  // 开启 WS_EX_TRANSPARENT,使控件支持透明
                return cp;
            }
        }

        /*
         * [Category("myOpaqueLayer"), Description("是否使用透明,默认为True")]
         * 一般用于说明你自定义控件的属性（Property）。
         * Category用于说明该属性属于哪个分类，Description自然就是该属性的含义解释。
         */
        [Category("OpaqueLayer"), Description("是否使用透明,默认为True")]
        public bool TransparentBG
        {
            get
            {
                return _transparentBG;
            }
            set
            {
                _transparentBG = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Category("OpaqueLayer"), Description("设置透明度")]
        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                _alpha = value;
                this.Invalidate();
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class OpaqueCommand
    {
        private OpaqueLayer m_OpaqueLayer = null;//半透明蒙板层
        private Control _control;
        /// <summary>
        /// 显示遮罩层
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="alpha">透明度</param>
        /// <param name="isShowLoadingImage">是否显示图标</param>
        public void ShowOpaqueLayer(Control control, int alpha, bool isShowLoadingImage)
        {
            try
            {
                _control = control;
                if (this.m_OpaqueLayer == null)
                {
                    this.m_OpaqueLayer = new OpaqueLayer(alpha, isShowLoadingImage);
                    control.Controls.Add(this.m_OpaqueLayer);
                    this.m_OpaqueLayer.Dock = DockStyle.Fill;
                    this.m_OpaqueLayer.BringToFront();
                }
                this.m_OpaqueLayer.Enabled = true;
                this.m_OpaqueLayer.Visible = true;
            }
            catch { }
        }

        /// <summary>
        /// 隐藏遮罩层
        /// </summary>
        public void HideOpaqueLayer()
        {
            try
            {
              
                if (this.m_OpaqueLayer != null)
                {
                    this.m_OpaqueLayer.Visible = false;
                    this.m_OpaqueLayer.Enabled = false;
                }
                OpaqueLayer.ControList.Remove(_control);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

    }
}
