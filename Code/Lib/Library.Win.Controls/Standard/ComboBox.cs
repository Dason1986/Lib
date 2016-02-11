using Library.Data;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Library.Controls
{
    /// <summary>
    ///
    /// </summary>
    public class ComboBox : System.Windows.Forms.ComboBox, IQueryControl
    {
        private ControlState _state = ControlState.Normal;
        private Font _defaultFont = new Font("微软雅黑", 9);

        #region Properites

        //[Description("当Text属性为空时编辑框内出现的提示文本")]
        //private string _emptyTextTip;
        //private Color _emptyTextTipColor = Color.DarkGray;
        //public String EmptyTextTip
        //{
        //    get { return _emptyTextTip; }
        //    set
        //    {
        //        if (_emptyTextTip == value) return;
        //        _emptyTextTip = value;
        //        Invalidate();
        //    }
        //}

        //[Description("获取或设置EmptyTextTip的颜色")]
        //public Color EmptyTextTipColor
        //{
        //    get { return _emptyTextTipColor; }
        //    set
        //    {
        //        if (_emptyTextTipColor == value) return;
        //        _emptyTextTipColor = value;
        //        Invalidate();
        //    }
        //}

        #endregion Properites

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public ComboBox()
        {
            SetStyles();
            this.Font = _defaultFont;
        }

        #endregion Constructor

        #region Override

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _state = ControlState.Highlight;
            base.OnMouseEnter(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (_state == ControlState.Highlight && Focused)
            {
                _state = ControlState.Focus;
            }
            else if (_state == ControlState.Focus)
            {
                _state = ControlState.Focus;
            }
            else
            {
                _state = ControlState.Normal;
            }
            base.OnMouseLeave(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = ControlState.Highlight;
            }
            base.OnMouseDown(mevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mevent"></param>
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            if (mevent.Button == MouseButtons.Left)
            {
                _state = ClientRectangle.Contains(mevent.Location) ? ControlState.Highlight : ControlState.Focus;
            }
            base.OnMouseUp(mevent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLostFocus(EventArgs e)
        {
            _state = ControlState.Normal;
            base.OnLostFocus(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            _state = Enabled ? ControlState.Normal : ControlState.Disabled;
            base.OnEnabledChanged(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {//TextBox是由系统进程绘制，重载OnPaint方法将不起作用
            switch (m.Msg)
            {
                case Win32.WM_CTLCOLOREDIT:
                case Win32.WM_PAINT:

                    base.WndProc(ref m);
                    WmPaint(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
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
                {
                    _defaultFont.Dispose();
                }
            }

            _defaultFont = null;
            base.Dispose(disposing);
        }

        #endregion Override

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

        private readonly Image _normalImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.qqcmb.png");

        /// <summary>
        ///
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="g"></param>
        protected virtual void DrawFlatComboDropDown(ComboBox comboBox, Graphics g)
        {
            var clientRect = comboBox.ClientRectangle;
            int dropDownButtonWidth = System.Windows.Forms.SystemInformation.HorizontalScrollBarArrowWidth;
            var outerBorder = new Rectangle(clientRect.Location, new Size(clientRect.Width - 1, clientRect.Height - 1));
            var innerBorder = new Rectangle(outerBorder.X, outerBorder.Y + 2, outerBorder.Width - dropDownButtonWidth - 2, outerBorder.Height - 2);

            var dropDownRect = new Rectangle(innerBorder.Right, innerBorder.Y - 1, dropDownButtonWidth + 2, innerBorder.Height + 2);
            g.DrawImage(_normalImg, dropDownRect);
            Brush brush = (comboBox.Enabled) ? SystemBrushes.ControlText : SystemBrushes.ControlDark;

            Point middle = new Point(dropDownRect.Left + dropDownRect.Width / 2, dropDownRect.Top + dropDownRect.Height / 2);

            middle.X += (dropDownRect.Width % 2);

            g.FillPolygon(brush, new[] {
                     new Point(middle.X - 3, middle.Y - 1),
                     new Point(middle.X + 4, middle.Y - 1),
                     new Point(middle.X, middle.Y + 3)
                 });
        }

        private void WmPaint(ref Message m)
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (GetStyle(ControlStyles.UserPaint) == false)
            {
                this.DrawFlatComboDropDown(this, g);
            }

            if (!Enabled)
            {
                _state = ControlState.Disabled;
            }

            switch (_state)
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
            //if (Text.Length == 0 && !string.IsNullOrEmpty(EmptyTextTip) && !Focused)
            //{
            //    var tmp = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            //    TextRenderer.DrawText(g, EmptyTextTip, Font, tmp, EmptyTextTipColor);
            //}
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

        private static TextFormatFlags GetTextFormatFlags(
            //  HorizontalAlignment alignment,
            bool rightToleft
            )
        {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            //switch (alignment)
            //{
            //    case HorizontalAlignment.Center:
            //        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
            //        break;
            //    case HorizontalAlignment.Left:
            //        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
            //        break;
            //    case HorizontalAlignment.Right:
            //        flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
            //        break;
            //}
            return flags;
        }

        #endregion Private

        /// <summary>
        ///
        /// </summary>
        public static IQueryDataProvider DefaultQueryDataProvider { get; set; }

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(null)]
        public IQueryDataProvider CurrentQueryDataProvider
        {
            get { return _currentQueryDataProvider; }
            set
            {
                if (Equals(_currentQueryDataProvider, value)) return;
                _currentQueryDataProvider = value;
                DataSource = null;
                GetDataSource();
            }
        }

        private string _queryDataID;
        private IQueryDataProvider _currentQueryDataProvider;

        /// <summary>
        ///
        /// </summary>
        [DefaultValue(null)]
        public string QueryDataID
        {
            get { return _queryDataID; }
            set
            {
                if (string.Equals(_queryDataID, value)) return;
                _queryDataID = value;
                DataSource = null;
                GetDataSource();
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool LoadDataing { get; protected set; }

        private void GetDataSource()
        {
            if (string.IsNullOrEmpty(QueryDataID))
            {
                DataSource = null;
                return;
            }
            if (DataSource != null) return;
            var query = this.CurrentQueryDataProvider ?? DefaultQueryDataProvider;
            if (query == null) return;
            if (this.LoadDataing) return;
            LoadDataing = true;
            var dt = query.GetDataSource(this);
            LoadDataing = false;
            this.DataSource = dt;
        }

        FieldCollection IQueryControl.Fields
        {
            get { return null; }
        }

        FilterCollection IQueryControl.Filters
        {
            get { return null; }
        }

        OrderCollection IQueryControl.Orders
        {
            get { return null; }
        }
    }
}