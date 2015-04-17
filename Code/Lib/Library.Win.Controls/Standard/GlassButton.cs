using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// 仿QQ效果的GlassButton
    /// </summary>
    public sealed class GlassButton : PictureBox, IButtonControl
    {
        #region  Fileds

        private DialogResult _dialogResult;
        private bool _isDefault;
        private bool _holdingSpace;

        private ControlState _state = ControlState.Normal;
        private Font _defaultFont = new Font("微软雅黑", 9);

        private Image _glassHotImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.glassbtn_hot.png");
        private Image _glassDownImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.glassbtn_down.png");
        private Image _totalImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.total.png");

        private ToolTip _toolTip = new ToolTip();
        private ContentAlignment _textAlign= ContentAlignment.MiddleRight;

        #endregion



        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public GlassButton()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
            this.BorderStyle = BorderStyle.None;
            this.Font = _defaultFont;
        }

        #endregion

        #region IButtonControl Members
        /// <summary>
        /// 
        /// </summary>
        public DialogResult DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                if (Enum.IsDefined(typeof(DialogResult), value))
                {
                    _dialogResult = value;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void NotifyDefault(bool value)
        {
            if (_isDefault != value)
            {
                _isDefault = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void PerformClick()
        {
            base.OnClick(EventArgs.Empty);
        }

        #endregion

        #region  Properties
        /// <summary>
        /// 数量
        /// </summary>
        [Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("Appearance"),
        Description("The text associated with the control."),
        DefaultValue(null)]
        public short? Total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("Appearance"),
        Description("The text associated with the control.")]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
        Category("Appearance"),
        Description("The font used to display text in the control.")]
        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("当鼠标放在控件可见处的提示文本")]
        public string ToolTipText { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("当鼠标放在控件可见处的提示文本"), DefaultValue(ContentAlignment.MiddleRight)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; }
        }

        #endregion

        #region Description Changes
        /// <summary>
        /// 
        /// </summary>
        [Description("Controls how the ImageButton will handle image placement and control sizing.")]
        public new PictureBoxSizeMode SizeMode { get { return base.SizeMode; } set { base.SizeMode = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Description("Controls what type of border the ImageButton should have.")]
        public new BorderStyle BorderStyle { get { return base.BorderStyle; } set { base.BorderStyle = value; } }
        #endregion

        #region Hiding
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ImageLayout BackgroundImageLayout { get { return base.BackgroundImageLayout; } set { base.BackgroundImageLayout = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage { get { return base.BackgroundImage; } set { base.BackgroundImage = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new String ImageLocation { get { return base.ImageLocation; } set { base.ImageLocation = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image ErrorImage { get { return base.ErrorImage; } set { base.ErrorImage = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image InitialImage { get { return base.InitialImage; } set { base.InitialImage = value; } }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool WaitOnLoad { get { return base.WaitOnLoad; } set { base.WaitOnLoad = value; } }
        #endregion

        #region override
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            //show tool tip 
            if (ToolTipText != string.Empty)
            {
                HideToolTip();
                ShowTooTip(ToolTipText);
            }

            _state = ControlState.Highlight;
            Invalidate();
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _state = ControlState.Normal;
            Invalidate();
            base.OnMouseLeave(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _state = ControlState.Down;
            Invalidate();
            base.OnMouseDown(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _state = ClientRectangle.Contains(e.Location) ? ControlState.Highlight : ControlState.Normal;
            }
            Invalidate();
            base.OnMouseUp(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {

            _state = ControlState.Normal;
            Invalidate();
            _holdingSpace = false;
            base.OnLostFocus(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            Rectangle imageRect, textRect;
            var g = pe.Graphics;
            CalculateRect(out imageRect, out textRect);
            switch (_state)
            {
                case ControlState.Highlight:
                    RenderHelper.DrawImageWithNineRect(g, _glassHotImg, ClientRectangle, new Rectangle(0, 0, _glassDownImg.Width, _glassDownImg.Height));
                    break;
                case ControlState.Down:
                    RenderHelper.DrawImageWithNineRect(g, _glassDownImg, ClientRectangle, new Rectangle(0, 0, _glassDownImg.Width, _glassDownImg.Height));
                    break;
            }
            if (Total != null && _totalImg != null)
            {
                var f = new Font("宋体", 9, FontStyle.Bold);
                var txt = Total.ToString();
                g.DrawImage(_totalImg, new Rectangle(this.ClientRectangle.Width - 30, 0, 26, 26));

                var leng = txt.Length * 4.5;
                var width = this.ClientRectangle.Width - 16 - (int)leng;
                TextRenderer.DrawText(g, txt, f, new Point(width, 8), SystemColors.ControlText);
            }
            if (Image != null)
            {
                pe.Graphics.DrawImage(Image, imageRect, new Rectangle(0, 0, Image.Width, Image.Height), GraphicsUnit.Pixel);
            }

            if (Text.Trim().Length != 0)
            {
                TextRenderer.DrawText(pe.Graphics, Text, Font, textRect, SystemColors.ControlText);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override bool PreProcessMessage(ref Message msg)
        {
            switch (msg.Msg)
            {
                case Win32.WM_KEYUP:
                    if (_holdingSpace)
                    {
                        switch ((Keys)msg.WParam)
                        {
                            case Keys.Space:
                                OnMouseUp(null);
                                PerformClick();
                                break;
                            case Keys.Tab:
                            case Keys.Escape:
                                _holdingSpace = false;
                                OnMouseUp(null);
                                break;
                        }
                    }
                    return true;
                case Win32.WM_KEYDOWN:
                    switch ((Keys)msg.WParam)
                    {
                        case Keys.Space:
                            _holdingSpace = true;
                            OnMouseDown(null);
                            break;
                        case Keys.Enter:
                            PerformClick();
                            break;
                    }
                    return true;
                default:
                    return base.PreProcessMessage(ref msg);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_defaultFont != null)
                    _defaultFont.Dispose();
                if (_glassDownImg != null)
                    _glassDownImg.Dispose();
                if (_glassHotImg != null)
                    _glassHotImg.Dispose();
                if (_toolTip != null)
                    _toolTip.Dispose();
            }
            _defaultFont = null;
            _glassDownImg = null;
            _glassHotImg = null;
            _toolTip = null;
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnTextChanged(EventArgs e)
        {
            Refresh();
            base.OnTextChanged(e);
        }

        #endregion

        #region Private

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;

            if (Image == null && !string.IsNullOrEmpty(Text))
            {
                textRect = new Rectangle(3, 0, Width - 6, Height);
            }
            else if (Image != null && string.IsNullOrEmpty(Text))
            {
                imageRect = new Rectangle((Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
            }
            else if (Image != null && !string.IsNullOrEmpty(Text))
            {
                switch (TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        {
                            imageRect = new Rectangle((Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
                            textRect = new Rectangle(3, Height - Font.Height - 4, Width - 6, Font.Height);
                        }
                        break;
                    case ContentAlignment.MiddleCenter:
                        {
                            imageRect = new Rectangle((Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
                            textRect = new Rectangle(3, Height / 2 - Font.Height, Width - 6, Height / 2);
                        }
                        break;
                    case ContentAlignment.TopCenter:
                        {
                            imageRect = new Rectangle((Width - Image.Width) / 2,   Font.Height+3, Image.Width, Image.Height);
                            textRect = new Rectangle(3, 0, Width - 6, Font.Height);
                        }
                        break;
                    case ContentAlignment.MiddleLeft:
                        {
                            textRect = new Rectangle(3, Height - Font.Height - 4, Width - 6, Font.Height);
                            imageRect = new Rectangle(textRect.Right + (Width - Image.Width) / 2, (Height - Image.Height) / 2, Image.Width, Image.Height);
                         
                        }
                        break;
                    case ContentAlignment.BottomLeft:
                        {
                            textRect = new Rectangle(3, 0, Width - 6, Height);
                            imageRect = new Rectangle(textRect.Right + (Width - Image.Width) / 2, (Height - Image.Height), Image.Width, Image.Height);

                        }
                        break;
                    case ContentAlignment.TopLeft:
                        {
                            textRect = new Rectangle(3, 0, Width - 6, Height);
                            imageRect = new Rectangle(textRect.Right + (Width - Image.Width) / 2, (Height - Image.Height), Image.Width, Image.Height);

                        }
                        break;
                    default:
                        {
                            imageRect = new Rectangle(4, (Height - Image.Height) / 2, Image.Width, Image.Height);
                            textRect = new Rectangle(imageRect.Right + 1, 0, Width - 4 * 2 - imageRect.Width - 1, Height);
                        }
                        break;
                }
            }
        }

        private void ShowTooTip(string toolTipText)
        {
            _toolTip.Active = true;
            _toolTip.SetToolTip(this, toolTipText);
        }

        private void HideToolTip()
        {
            _toolTip.Active = false;
        }

        #endregion
    }
}