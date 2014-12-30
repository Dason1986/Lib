﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// 仿QQ效果的RadioButton
    /// </summary>
    public sealed class RadioButton : System.Windows.Forms.RadioButton
    {
        #region Field

        private ControlState _state = ControlState.Normal;
        private Image _dotImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.dot.png");
        private Font _defaultFont = new Font("微软雅黑", 9);

        private const ContentAlignment RightAlignment = (ContentAlignment)1092;//ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight
        private const ContentAlignment LeftAlignment = (ContentAlignment)273;//ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft

        #endregion

        #region Constructor

        public RadioButton()
        {
            SetStyles();
            this.BackColor = Color.Transparent;
            this.Font = _defaultFont;
        }

        #endregion

        #region Properites

        [Description("获取QQRadioButton左边正方形的宽度")]
        private int CheckRectWidth
        {
            get { return 12; }
        }

        #endregion

        #region Override

        protected override void OnMouseEnter(EventArgs e)
        {
            _state = ControlState.Highlight;
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _state = ControlState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = ControlState.Down;
            }
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = ClientRectangle.Contains(mevent.Location) ? ControlState.Highlight : ControlState.Normal;
            }
            base.OnMouseUp(mevent);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            _state = Enabled ? ControlState.Normal : ControlState.Disabled;
            base.OnEnabledChanged(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            OnPaintBackground(pevent);

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle circleRect, textRect;
            CalculateRect(out circleRect, out textRect);

            if (Enabled == false)
            {
                _state = ControlState.Disabled;
            }

            switch (_state)
            {
                case ControlState.Highlight:
                case ControlState.Down:
                    DrawHighLightCircle(g, circleRect);
                    break;
                case ControlState.Disabled:
                    DrawDisabledCircle(g, circleRect);
                    break;
                default:
                    DrawNormalCircle(g, circleRect);
                    break;
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRect,
                textColor,
                GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dotImg != null)
                {
                    _dotImg.Dispose();
                }
                if (_defaultFont != null)
                {
                    _defaultFont.Dispose();
                }
            }

            _dotImg = null;
            _defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void DrawNormalCircle(Graphics g, Rectangle circleRect)
        {
            g.FillEllipse(Brushes.White, circleRect);
            using (Pen borderPen = new Pen(ColorTable.QQBorderColor))
            {
                g.DrawEllipse(borderPen, circleRect);
            }
            if (!Checked) return;
            circleRect.Inflate(-2, -2);
            var rect = new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1);
            g.DrawImage(_dotImg, rect, 0, 0, _dotImg.Width, _dotImg.Height, GraphicsUnit.Pixel);
        }

        private void DrawHighLightCircle(Graphics g, Rectangle circleRect)
        {
            DrawNormalCircle(g, circleRect);
            using (Pen p = new Pen(ColorTable.QQHighLightInnerColor))
            {
                g.DrawEllipse(p, circleRect);

                circleRect.Inflate(1, 1);
                p.Color = ColorTable.QQHighLightColor;

                g.DrawEllipse(p, circleRect);
            }


        }

        private void DrawDisabledCircle(Graphics g, Rectangle circleRect)
        {
            g.DrawEllipse(SystemPens.ControlDark, circleRect);
            if (!Checked) return;
            circleRect.Inflate(-2, -2);
            var rect = new Rectangle(circleRect.X + 1, circleRect.Y + 1, circleRect.Width - 1, circleRect.Height - 1);
            g.DrawImage(_dotImg, rect, 0, 0, _dotImg.Width, _dotImg.Height, GraphicsUnit.Pixel);
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }

        private void CalculateRect(out Rectangle circleRect, out Rectangle textRect)
        {
            circleRect = new Rectangle(
                0, 0, CheckRectWidth, CheckRectWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAlignment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = (RightToLeft == RightToLeft.Yes);

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft))
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = 1;

                textRect = new Rectangle(circleRect.Right + 2,0,Width - circleRect.Right - 4,Height);
            }
            else if (bCheckAlignRight || bCheckAlignLeft)
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        circleRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                circleRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        circleRect.Y = 2;
                        textRect.Y = circleRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        circleRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        circleRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                circleRect.X = (Width - CheckRectWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        private static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                             TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }


        #endregion
    }
}