using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// 仿QQ效果的TextBox
    /// </summary>
    public class TextBox : System.Windows.Forms.TextBox
    {
        #region Field

        protected ControlState State = ControlState.Normal;
        private Font _defaultFont = new Font("微软雅黑", 9);

        //当Text属性为空时编辑框内出现的提示文本
        private string _emptyTextTip;
        private Color _emptyTextTipColor = Color.DarkGray;

        #endregion

        #region Constructor

        public TextBox()
        {
            SetStyles();
            this.Font = _defaultFont;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Properites

        [Description("当Text属性为空时编辑框内出现的提示文本")]
        public String EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                if (_emptyTextTip == value) return;
                _emptyTextTip = value;
                Invalidate();
            }
        }

        [Description("获取或设置EmptyTextTip的颜色")]
        public Color EmptyTextTipColor
        {
            get { return _emptyTextTipColor; }
            set
            {
                if (_emptyTextTipColor == value) return;
                _emptyTextTipColor = value;
                Invalidate();
            }
        }

        #endregion

        #region Override

        protected override void OnMouseEnter(EventArgs e)
        {
            State = ControlState.Highlight;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (State == ControlState.Highlight && Focused)
            {
                State = ControlState.Focus;
            }
            else if (State == ControlState.Focus)
            {
                State = ControlState.Focus;
            }
            else
            {
                State = ControlState.Normal;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                State = ControlState.Highlight;
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                State = ClientRectangle.Contains(mevent.Location) ? ControlState.Highlight : ControlState.Focus;
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            State = ControlState.Normal;
            base.OnLostFocus(e);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            State = Enabled ? ControlState.Normal : ControlState.Disabled;
            base.OnEnabledChanged(e);
        }

        protected override void WndProc(ref Message m)
        {//TextBox是由系统进程绘制，重载OnPaint方法将不起作用

            base.WndProc(ref m);
            if (m.Msg == Win32.WM_PAINT || m.Msg == Win32.WM_CTLCOLOREDIT)
            {
                WmPaint(ref m);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_defaultFont != null)
                {
                    _defaultFont.Dispose();
                }
            }

            _defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void SetStyles()
        {
            // TextBox由系统绘制，不能设置 ControlStyles.UserPaint样式
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        protected virtual void WmPaint(ref Message m)
        {
            Draw();
        }

        protected void Draw()
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (!Enabled)
            {
                State = ControlState.Disabled;
            }

            switch (State)
            {
                case ControlState.Normal:
                    DrawNormalTextBox(g);
                    break;
                case ControlState.Highlight:
                    DrawHighLightTextBox(g);
                    break;
                case ControlState.Focus:
                    DrawFocusTextBox(g);
                    break;
                case ControlState.Disabled:
                    DrawDisabledTextBox(g);
                    break;

            }

            if (Text.Length == 0 && !string.IsNullOrEmpty(EmptyTextTip) && !Focused && !this.ReadOnly && Enabled)
            {
                TextRenderer.DrawText(g, EmptyTextTip, Font, ClientRectangle, EmptyTextTipColor, GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
        }

        private void DrawNormalTextBox(Graphics g)
        {
            using (Pen borderPen = new Pen(Color.LightGray))
            {
                g.DrawRectangle(borderPen, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
            }
        }

        private void DrawHighLightTextBox(Graphics g)
        {
            using (Pen highLightPen = new Pen(ColorTable.QQHighLightColor))
            {
                Rectangle drawRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);

                g.DrawRectangle(highLightPen, drawRect);

                //InnerRect
                drawRect.Inflate(-1, -1);
                highLightPen.Color = ColorTable.QQHighLightInnerColor;
                g.DrawRectangle(highLightPen, drawRect);
            }
        }

        private void DrawFocusTextBox(Graphics g)
        {
            using (Pen focusedBorderPen = new Pen(ColorTable.QQHighLightInnerColor))
            {
                g.DrawRectangle(focusedBorderPen, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
            }
        }

        private void DrawDisabledTextBox(Graphics g)
        {
            using (Pen disabledPen = new Pen(SystemColors.ControlDark))
            {
                g.DrawRectangle(disabledPen, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
            }
        }

        private static TextFormatFlags GetTextFormatFlags(HorizontalAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Left:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        #endregion
    }
}
