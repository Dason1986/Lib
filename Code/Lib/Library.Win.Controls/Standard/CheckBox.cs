﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    /// 仿QQ效果的CheckBox
    /// </summary>
    public sealed class CheckBox : System.Windows.Forms.CheckBox
    {
        #region Field

        private ControlState _state = ControlState.Normal;
        private Image _checkImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.check.png");
        private Font _defaultFont = new Font("微软雅黑", 9);

        private const ContentAlignment RightAlignment = (ContentAlignment)1092;//ContentAlignment.TopRight | ContentAlignment.BottomRight | ContentAlignment.MiddleRight
        private const ContentAlignment LeftAlignment = (ContentAlignment)273;//ContentAlignment.TopLeft | ContentAlignment.BottomLeft | ContentAlignment.MiddleLeft

        #endregion

        #region Constructor

        public CheckBox()
        {
            SetStyles();
            this.BackColor = Color.Transparent;
            this.Font = _defaultFont;
        }

        #endregion

        #region Properites

        [Description("获取QQCheckBox左边正方形的宽度"),]
        private int CheckRectWidth
        {
            get { return 13; }
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
            base.OnPaintBackground(pevent);

            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle checkRect, textRect;
            CalculateRect(out checkRect, out textRect);

            if (Enabled == false)
            {
                _state = ControlState.Disabled;
            }

            switch (_state)
            {
                case ControlState.Highlight:
                case ControlState.Down:
                    DrawHighLightCheckRect(g, checkRect);
                    break;
                case ControlState.Disabled:
                    DrawDisabledCheckRect(g, checkRect);
                    break;
                default:
                    DrawNormalCheckRect(g, checkRect);
                    break;
            }

            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            TextRenderer.DrawText(g,Text,Font,textRect,textColor,GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_checkImg != null)
                {
                    _checkImg.Dispose();
                }
                if (_defaultFont != null)
                {
                    _defaultFont.Dispose();
                }
            }

            _checkImg = null;
            _defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion

        #region Private

        private void DrawNormalCheckRect(Graphics g, Rectangle checkRect)
        {
            g.FillRectangle(Brushes.White, checkRect);
            using (Pen borderPen = new Pen(ColorTable.QQBorderColor))
            {
                g.DrawRectangle(borderPen, checkRect);
            }
            if (Checked)
            {
                g.DrawImage(_checkImg, checkRect, 0, 0, _checkImg.Width, _checkImg.Height, GraphicsUnit.Pixel);
              
            }
        }

        private void DrawHighLightCheckRect(Graphics g, Rectangle checkRect)
        {
            DrawNormalCheckRect(g, checkRect);
            using (Pen p = new Pen(ColorTable.QQHighLightInnerColor))
            {
                g.DrawRectangle(p, checkRect);

                checkRect.Inflate(1, 1);
                p.Color = ColorTable.QQHighLightColor;

                g.DrawRectangle(p, checkRect);
            }


        }

        private void DrawDisabledCheckRect(Graphics g, Rectangle checkRect)
        {
            g.DrawRectangle(SystemPens.ControlDark, checkRect);
            if (Checked)
            {
                g.DrawImage(_checkImg, checkRect, 0, 0, _checkImg.Width, _checkImg.Height, GraphicsUnit.Pixel);
            }
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

        private void CalculateRect(out Rectangle checkRect, out Rectangle textRect)
        {
            checkRect = new Rectangle(
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
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = 1;

                textRect = new Rectangle(
                    checkRect.Right + 2,
                    0,
                    Width - checkRect.Right - 4,
                    Height);
            }
            else if (bCheckAlignRight|| bCheckAlignLeft)
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        break;
                }

                checkRect.X = Width - CheckRectWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - CheckRectWidth - 6, Height);
            }
            else
            {
                switch (CheckAlign)
                {
                    case ContentAlignment.TopCenter:
                        checkRect.Y = 2;
                        textRect.Y = checkRect.Bottom + 2;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        checkRect.Y = (Height - CheckRectWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        checkRect.Y = Height - CheckRectWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - CheckRectWidth - 6;
                        break;
                }

                checkRect.X = (Width - CheckRectWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        private static TextFormatFlags GetTextFormatFlags(ContentAlignment alignment, bool rightToleft)
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                                    TextFormatFlags.SingleLine;
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