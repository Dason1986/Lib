using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;  
using Library.Data;
using Library.HelperUtility;

namespace Library.Controls
{


    /// <summary>
    /// 
    /// </summary>
    public class QueryLabel : ListControl, IQueryControl
    {

        public static IQueryDataProvider DefaultQueryDataProvider { get; set; }


        #region Private


        private static readonly Image NormalImg = RenderHelper.GetImageFormResourceStream("Library.Win.Controls.Standard.Image.qqcmb.png");


        private void DrawImageButton(Graphics graphics)
        {
            //Image image = new Bitmap(19, 22);
            //var g = Graphics.FromImage(image);


            var rec = new Rectangle(Width - 21, 0, 19, 20);
            graphics.DrawImage(NormalImg, rec, new Rectangle(0, -2, 19, 22), GraphicsUnit.Pixel);
            //var dropDownRect = new Rectangle(0, 0, image.Width, image.Height + 2);
            //g.DrawImage(_normalImg, dropDownRect);
            //   g.DrawImage(_normalImg, new Rectangle(1, 1, dropDownRect.Width - 1, dropDownRect.Height - 2), dropDownRect, GraphicsUnit.Pixel);
            Brush brush = (this.Enabled) ? SystemBrushes.ControlText : SystemBrushes.ControlDark;

            Point middle = new Point(Width - 18, NormalImg.Height / 2);




            graphics.DrawString("…", _morefont, brush, middle);
            graphics.Save();
            graphics.Dispose();


            if (_gpRealTime.PathData.Points.Length > 0) return;
            _gpRealTime.AddRectangle(rec);
        }





        protected override void Dispose(bool disposing)
        {

            if (disposing)
            {
                if (_morefont != null)
                {
                    _morefont.Dispose();
                }
                if (_components != null)
                {
                    _components.Dispose();
                }
                if (this._valueText != null)
                {
                    this._valueText.KeyDown -= _valueText_KeyDown;

                    this._valueText.LostFocus -= InputCode_LostFocus;
                }
                if (this._txtContext != null)
                {
                    this._txtContext.Click -= _txtContext_Click;
                    this._txtContext.MouseEnter -= _txtContext_MouseEnter;
                }
            }
            _morefont = null;
            base.Dispose(disposing);
        }


        #endregion
        #region Properites
        private Label _txtContext;


        private Font _morefont = new Font("宋体", 9);


        public event EventHandler ChildFormClosed;





        protected virtual void OnChildFormClosed()
        {

            EventHandler handler = ChildFormClosed;
            if (handler != null) handler(this, EventArgs.Empty);

        }

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


        [DefaultValue("")]
        public override string Text
        {
            get
            {

                return base.Text;
            }
            set
            {
                base.Text = value;

                if (value == null)
                {
                    this.SelectedIndex = -1;
                }
                else
                {


                    int stringIgnoreCase = this.GetTextIndex(value);
                    if (stringIgnoreCase == -1)
                        return;
                    this.SelectedIndex = stringIgnoreCase;

                }
            }
        }
        public new object SelectedValue
        {
            get
            {

                return base.SelectedValue;
            }
            set
            {
                if (this.DataManager == null)
                    return;
                var prop = this.DataManager.GetItemProperties();

                var index = this.Find(prop.Find(this.ValueMember, true), value, true);
                SelectedIndex = index;
            }
        }
        internal int Find(PropertyDescriptor property, object key, bool keepIndex)
        {
            if (key == null) return -1;
            var list = this.DataManager.List;
            if (property != null && list is IBindingList && ((IBindingList)list).SupportsSearching)
                return ((IBindingList)list).Find(property, key);
            if (property != null)
            {
                for (int index = 0; index < list.Count; ++index)
                {
                    object obj = property.GetValue(list[index]);
                    if (key.Equals(obj))
                        return index;
                }
            }
            return -1;
        }
        [DefaultValue(-1)]
        public override int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (DataManager == null)
                {
                    //GetDataSource();
                    //if (DataManager == null) 
                    return;
                }
                if (_selectedIndex == value) return;

                int num = this.DataManager.Count;
                if (value < -1 || value >= num) throw new ArgumentOutOfRangeException("SelectedIndex");
                _selectedIndex = value;
                if (value != -1)
                {
                    this.DataManager.Position = value;
                    SetBindingValue();
                }
                else
                {
                    base.Text = null;
                    SelectedValue = null;
                }
                this.OnSelectedIndexChanged(EventArgs.Empty);
            }
        }
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign { get; set; }


        [
        Description("当Text属性为空时编辑框内出现的提示文本"),
        DefaultValue("")
        ]
        public String EmptyTextTip
        {
            get { return _emptyTextTip; }
            set
            {
                if (string.Equals(_emptyTextTip, value)) return;
                _emptyTextTip = value;
                //  Invalidate();
            }
        }
        // [DefaultValue(typeof(Color))]
        [DefaultValue(typeof(Color), "255,169,169,169")]
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

        [DefaultValue(true)]
        public bool CanChangeValue
        {
            get { return _canChangeValue; }
            set { _canChangeValue = value; }
        }

        [DefaultValue(null)]
        public string SelectedColumnNames { get; set; }

        [DefaultValue(null)]
        public FieldCollection Fields
        {
            get { return _fields; }

        }

        [DefaultValue(null)]
        public FilterCollection Filters
        {
            get { return _filters; }

        }

        [DefaultValue(null)]
        public OrderCollection Orders
        {
            get { return _orders; }

        }

        private Color _emptyTextTipColor = Color.DarkGray;
        private string _emptyTextTip;
        private ControlState _state;
        /// <summary>
        /// 按钮的位置范围
        /// </summary>
        readonly GraphicsPath _gpRealTime = new GraphicsPath();

        private System.Windows.Forms.TextBox _valueText;
        readonly ErrorProvider _errorProvider = new ErrorProvider();
        private int _selectedIndex = -1;
        private Container _components;
        private string _queryDataID;
        private bool _canChangeValue = true;
        private readonly FieldCollection _fields = new FieldCollection();
        private readonly FilterCollection _filters = new FilterCollection();
        private readonly OrderCollection _orders = new OrderCollection();
        private IQueryDataProvider _currentQueryDataProvider;

        #endregion
        public QueryLabel()
        {
            // CanChangeValue = true;
            InitializeComponent();
        }



        private void InitializeComponent()
        {
            this._components = new System.ComponentModel.Container();
            this._txtContext = new Label();
            this._valueText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();


            this._txtContext.DataBindings.Add("Text", this, "Text", true, DataSourceUpdateMode.OnPropertyChanged);
            this._txtContext.Text = Text;
            this._txtContext.Name = "_txtContext";
            this._txtContext.AutoSize = true;
            this._txtContext.BackColor = Color.Transparent;
            this._txtContext.Location = new System.Drawing.Point(2, 4);
            this._txtContext.TextAlign = ContentAlignment.MiddleLeft;
            this._txtContext.Click += _txtContext_Click;
            this._txtContext.MouseEnter += _txtContext_MouseEnter;
            //    this._valueText.DataBindings.Add("Text", this, "SelectedValue", true, DataSourceUpdateMode.OnPropertyChanged);
            this._valueText.Name = "_valueText";
            this._valueText.AutoSize = true;
            this._valueText.Location = new System.Drawing.Point(2, 4);
            this._valueText.Visible = false;
            this._valueText.BorderStyle = BorderStyle.None;
            this._valueText.LostFocus += InputCode_LostFocus;
            this._valueText.KeyDown += _valueText_KeyDown;
            // 
            // QueryTextBox
            // 
            //  this.Controls.Add(this._pictureBox1);
            this.Controls.Add(this._txtContext);
            this.Controls.Add(this._valueText);

            this.ResumeLayout(false);

        }

        void _valueText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                InputCode_LostFocus(sender, EventArgs.Empty);
            }

        }



        void _txtContext_MouseEnter(object sender, EventArgs e)
        {
            _state = ControlState.Highlight;
            DrawLabel();
        }

        void _txtContext_Click(object sender, EventArgs e)
        {
            InputCode();
        }


        //protected override void OnSelectedValueChanged(EventArgs e)
        //{
        //    GetDataSource();
        //}
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

        #region darw


        protected override void WndProc(ref Message m)
        {//TextBox是由系统进程绘制，重载OnPaint方法将不起作用

            base.WndProc(ref m);
            if (m.Msg == Win32.WM_PAINT || m.Msg == Win32.WM_CTLCOLOREDIT || m.Msg == 49661 || m.Msg == 675 || m.Msg == 49587)
            {
                DrawLabel();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawImageButton(e.Graphics);

        }

        private void DrawLabel()
        {
            Graphics g = Graphics.FromHwnd(base.Handle);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            switch (_state)
            {
                case ControlState.Normal:
                    DrawNormalTex(g);
                    break;
                case ControlState.Highlight:
                    DrawHighLightText(g);
                    break;
                case ControlState.Focus:
                    DrawFocusText(g);
                    break;

            }



            g.DrawLine(new Pen(Color.DimGray, 1), new PointF(0, this.Height - 2), new PointF(this.Width - 27, this.Height - 2));

            if (SelectedValue == null && Text.Length == 0 && !string.IsNullOrEmpty(EmptyTextTip) && !Focused && Enabled)
            {
                var tmp = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 20, ClientRectangle.Height);
                TextRenderer.DrawText(g, EmptyTextTip, Font, tmp, EmptyTextTipColor, GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
        }


        private void DrawFocusText(Graphics g)
        {
            using (Pen focusedBorderPen = new Pen(ColorTable.QQHighLightInnerColor))
            {
                g.DrawRectangle(focusedBorderPen, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1));
            }
        }
        private void DrawNormalTex(Graphics g)
        {

            using (Pen highLightPen = new Pen(this.BackColor))
            {
                Rectangle drawRect = new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height - 1);
                g.DrawRectangle(highLightPen, drawRect);
                drawRect.Inflate(-1, -1);
                highLightPen.Color = this.BackColor;
                g.DrawRectangle(highLightPen, drawRect);
            }
        }

        private void DrawHighLightText(Graphics g)
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
        protected override void OnMouseEnter(EventArgs e)
        {
            _state = ControlState.Highlight;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //  _state = ControlState.Highlight;
            if (CanChangeValue)
                this.Cursor = _gpRealTime.IsVisible(e.Location) ? Cursors.Arrow : Cursors.IBeam;
        }

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
                this._valueText.Visible = false;
            }

            base.OnMouseLeave(e);
        }
        #endregion

        #region InputCode

        protected override void OnLostFocus(EventArgs e)
        {
            _state = ControlState.Normal;

            base.OnLostFocus(e);
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_gpRealTime.IsVisible(e.Location))
            {
                ShowForm();
            }
            else
            {


                InputCode();

            }
        }

        private void InputCode()
        {
            if (!CanChangeValue || DataSource == null) return;
            _state = ControlState.Focus;
            // this.Focus();
            var txtContext = this._txtContext;
            if (txtContext != null) txtContext.Visible = false;
            var selectedValue = this.SelectedValue;
            if (selectedValue != null) this._valueText.Text = selectedValue.ToString();
            var valueText = this._valueText;
            if (valueText != null)
            {
                valueText.Visible = true;
                valueText.SelectAll();
                valueText.Focus();
            }
        }

        void InputCode_LostFocus(object sender, EventArgs e)
        {
            _state = ControlState.Normal;
            if (!CanChangeValue || DataSource == null) return;
            var valueText = this._valueText;
            var txtContext = this._txtContext;
            if (valueText != null && txtContext != null)
            {
                valueText.Visible = false;

                txtContext.Visible = true;
                var codevalue = GetItemByCode(valueText.Text);
                if (codevalue == null)
                {
                    _errorProvider.SetError(this, string.Format("对有对应的值[{0}]", valueText.Text));

                    return;
                }
                _errorProvider.SetError(this, null);
                txtContext.Text = codevalue.Item2;
                SelectedValue = codevalue.Item1;

            }
            SetBindingValue();
        }

        #endregion


        void ShowForm()
        {
            if (this.DataSource == null) return;
            var form = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false,
                Text = "选择数据",
                StartPosition = FormStartPosition.CenterParent
            };
            DataGridView dataGrid = new DataGridView
            {
                Parent = form,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToOrderColumns = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Dock = DockStyle.Fill,
                ReadOnly = true,

            };
            if (Fields.HasRecord())
            {
                dataGrid.AutoGenerateColumns = false;

                dataGrid.Columns.Clear();
                foreach (var field in Fields.Where(n => n.IsSelected).Distinct(QueryField.Comparer))
                {
                    //  if (names.Contains(field.Name) || (hasSelect && field.IsSelected) || !hasSelect)
                    dataGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = field.Name, HeaderText = field.DisplayName ?? field.Name });

                }
            }

            dataGrid.DataSource = this.DataSource;

            if (SelectedIndex >= 0)
            {
                dataGrid.Rows[SelectedIndex].Selected = true;
                dataGrid.CurrentCell = dataGrid.Rows[SelectedIndex].Cells[0];
            }
            Form form1 = form;
            dataGrid.CellDoubleClick += (xx, yy) =>
            {
                if (yy.RowIndex < 0) return;

                SelectedIndex = yy.RowIndex;
                form1.Close();
            };

            //  form.Location=this.Parent
            form.ShowDialog();
            form.Dispose();
            OnChildFormClosed();
            //OnBtnClick();
            //  
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

        private int GetTextIndex(string text)
        {
            var currencyManager = this.DataManager;
            if (currencyManager == null) return -1;
            var properies = currencyManager.GetItemProperties();
            var valuepropery = properies[this.DisplayMember];
            for (int i = 0; i < currencyManager.List.Count; i++)
            {
                var obj = valuepropery.GetValue(currencyManager.List[i]);
                if (obj == text) return i;
            }
            return -1;
        }

        private void SetBindingValue()
        {
            if (_selectedIndex == -1) return;
            var currencyManager = this.DataManager;
            if (currencyManager == null) return;
            var obj = currencyManager.Current;
            if (obj == null) return;
            var text = GetDisplayText();
            if (Text != text)
            {
                base.Text = text;
                this.OnTextChanged(EventArgs.Empty);
            }
            var properies = currencyManager.GetItemProperties();
            var valuepropery = properies[this.ValueMember];
            if (valuepropery != null)
            {
                var values = valuepropery.GetValue(obj);

                if (!object.Equals(values, SelectedValue)) SelectedValue = values;
                this.OnSelectedValueChanged(EventArgs.Empty);
            }
        }

        private string GetDisplayText()
        {
            if (_selectedIndex == -1) return string.Empty;
            var currencyManager = this.DataManager;
            if (currencyManager == null) return null;
            var obj = currencyManager.Current;
            if (obj == null) return null;
            var properies = currencyManager.GetItemProperties();
            var displaypropery = properies[this.DisplayMember];

            if (displaypropery == null) return null;
            var values = displaypropery.GetValue(obj);
            return values != null ? values.ToString() : null;
        }
        private Tuple<object, string> GetItemByCode(string text)
        {
            if (_selectedIndex == -1) return null;
            var currencyManager = this.DataManager;
            if (currencyManager == null) return null;
            var properies = currencyManager.GetItemProperties();
            var displaypropery = properies[this.DisplayMember];
            var valuepropery = properies[this.ValueMember];

            for (int i = 0; i < currencyManager.List.Count; i++)
            {
                var curr = currencyManager.List[i];
                var obj = valuepropery.GetValue(curr);
                if (obj != null && obj.ToString() == text)
                {
                    var value = displaypropery.GetValue(curr);
                    if (value != null) return new Tuple<object, string>(obj, value.ToString());
                }
            }
            return null;
        }
        protected override void RefreshItem(int index)
        {

            this.DataManager.Position = index;
            SetBindingValue();
        }


        /// <summary>
        /// 在派生类中重写时，在派生类中设置具有指定索引的对象。
        /// </summary>
        /// <param name="index">对象的数组索引。</param><param name="value">设置的对象。</param>
        protected override void SetItemCore(int index, object value)
        {

        }
        protected override void SetItemsCore(IList items)
        {

            if (this.DataManager != null)
            {
                _selectedIndex = -1;
                base.Text = string.Empty;
            }

        }
    }
}