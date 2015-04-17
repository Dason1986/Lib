using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// 图形按钮类
    /// </summary>
    public sealed class ImageButton : PictureBox, IButtonControl
    {
        #region  Fileds

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;

        private DialogResult _dialogResult;
        private Image _hoverImage;
        private Image _downImage;
        private Image _normalImage;
        private bool _hover;
        private bool _down;
        private bool _isDefault;
        private bool _holdingSpace;

        private ToolTip _toolTip = new ToolTip();

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ImageButton()
        {
            this.BackColor = Color.Transparent;
            this.Size = new Size(75, 23);
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
        /// 
        /// </summary>
        [Category("Appearance")]
        [Description("Image to show when the button is hovered over.")]
        public Image HoverImage
        {
            get { return _hoverImage; }
            set { _hoverImage = value; if (_hover) Image = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("Appearance")]
        [Description("Image to show when the button is depressed.")]
        public Image DownImage
        {
            get { return _downImage; }
            set { _downImage = value; if (_down) Image = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Category("Appearance")]
        [Description("Image to show when the button is not in any other state.")]
        public Image NormalImage
        {
            get { return _normalImage; }
            set { _normalImage = value; if (!(_hover || _down)) Image = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("The text associated with the control.")]
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
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Category("Appearance")]
        [Description("The font used to display text in the control.")]
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

        #endregion
        /// <summary>
        /// 
        /// </summary>
        #region Description Changes
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
        public new Image Image { get { return base.Image; } set { base.Image = value; } }
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
            base.OnMouseEnter(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            _hover = true;
            if (_down)
            {
                if ((_downImage != null) && (Image != _downImage))
                    Image = _downImage;
            }
            else
                if (_hoverImage != null)
                    Image = _hoverImage;
                else
                    Image = _normalImage;
            base.OnMouseMove(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _hover = false;
            Image = _normalImage;
            base.OnMouseLeave(e);
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.Focus();
            OnMouseUp(null);
            _down = true;
            if (_downImage != null)
                Image = _downImage;
            base.OnMouseDown(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _down = false;
            if (_hover)
            {
                if (_hoverImage != null)
                    Image = _hoverImage;
            }
            else
                Image = _normalImage;
            base.OnMouseUp(e);
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
                case WM_KEYUP:
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
                case WM_KEYDOWN:
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
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            _holdingSpace = false;
            OnMouseUp(null);
            base.OnLostFocus(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if ((!string.IsNullOrEmpty(Text)) && (pe != null) && (base.Font != null))
            {
                SizeF drawStringSize = pe.Graphics.MeasureString(base.Text, base.Font);
                PointF drawPoint;
                if (base.Image != null)
                {
                    drawPoint = new PointF(base.Image.Width / 2 - drawStringSize.Width / 2,base.Image.Height / 2 - drawStringSize.Height / 2);
                }
                else
                {
                    drawPoint = new PointF(base.Width / 2 - drawStringSize.Width / 2,base.Height / 2 - drawStringSize.Height / 2);
                }

                using (SolidBrush drawBrush = new SolidBrush(base.ForeColor))
                {
                    pe.Graphics.DrawString(base.Text, base.Font, drawBrush, drawPoint);
                }

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
                if (_toolTip != null)
                    _toolTip.Dispose();
            }
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
