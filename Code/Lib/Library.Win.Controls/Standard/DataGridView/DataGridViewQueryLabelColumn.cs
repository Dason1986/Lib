using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using Library.Data;


namespace Library.Controls
{
    /// <summary>
    /// 
    /// </summary>
    [ToolboxItem(false)]
    public class DataGridViewQueryLabelColumn : DataGridViewTextBoxColumn, IQueryControl
    {
        private readonly FilterCollection _filters = new FilterCollection();
        private readonly FieldCollection _fields = new FieldCollection();
        private readonly OrderCollection _orders = new OrderCollection();
        private object _dataSource;

        readonly BindingContext _context = new BindingContext();
        private CurrencyManager _manager;


        /// <summary>
        /// 
        /// </summary>
        public DataGridViewQueryLabelColumn()
        {
            this.CellTemplate = new DataGridViewQueryLabelCell();
        }

        public override sealed DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set { base.CellTemplate = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(null)]
        public IQueryDataProvider CurrentQueryDataProvider { get; set; }
        [DefaultValue(null)]
        public string QueryDataID { get; set; }

        [DefaultValue(null)]
        public FieldCollection Fields
        {
            get { return _fields; }

        }
        [DefaultValue(null)]
        public OrderCollection Orders
        {
            get { return _orders; }
        }
        [DefaultValue(null)]
        public FilterCollection Filters
        {
            get { return _filters; }

        }
        [DefaultValue(null)]
        public object DataSource
        {
            get { return _dataSource; }
            set
            {
                _dataSource = value;
                if (_dataSource != null) _manager = (CurrencyManager)_context[_dataSource];
                else _manager = null;
            }
        }

        [DefaultValue(null)]
        public string DisplayMember { get; set; }

        [DefaultValue(null)]
        public string ValueMember { get; set; }

        object IQueryControl.SelectedValue { get { throw new NotSupportedException(); } set { throw new NotSupportedException(); } }
        internal object GetDislpayName(object key)
        {
            if (_manager == null)
            {
                var query = this.CurrentQueryDataProvider ?? QueryLabel.DefaultQueryDataProvider;
                if (query == null) return null;
                _dataSource = query.GetDataSource(this);
                if (_dataSource != null) _manager = (CurrencyManager)_context[_dataSource];
            }
            if (_manager == null || key == null) return null;


            var properies = _manager.GetItemProperties();
            var displaypropery = properies[this.DisplayMember];
            var valuepropery = properies[this.ValueMember];

            if (displaypropery == null) return null;
            for (int i = 0; i < _manager.Count; i++)
            {
                var curr = _manager.List[i];
                var obj = valuepropery.GetValue(curr);
                if (obj == null || obj.ToString() != key.ToString()) continue;
                var value = displaypropery.GetValue(curr);
                return value;
            }
            return null;
        }



        public override object Clone()
        {
            var obj = (DataGridViewQueryLabelColumn)base.Clone();
            if (obj == null) return null;
            obj.DataSource = this.DataSource;
            obj.DisplayMember = this.DisplayMember;
            obj.ValueMember = this.ValueMember;
            obj.QueryDataID = this.QueryDataID;
            obj.Fields.ReSet(Fields);
            obj.Filters.ReSet(Filters);
            obj.Orders.ReSet(Orders);
            return obj;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(64);
            stringBuilder.Append("DataGridViewQueryLabelColumn { Name=");
            stringBuilder.Append(this.Name);
            stringBuilder.Append(", Index=");
            stringBuilder.Append(this.Index.ToString(CultureInfo.CurrentCulture));
            stringBuilder.Append(" }");
            return stringBuilder.ToString();
        }



    }


    /// <summary>
    /// 
    /// </summary>
    [ToolboxItem(false)]
    public class DataGridViewQueryLabelEditingControl : QueryLabel, IDataGridViewEditingControl
    {
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        public DataGridViewQueryLabelCell OwnerCell { get; set; }
        public DataGridView EditingControlDataGridView
        {
            get;
            set;
        }

        public object EditingControlFormattedValue
        {
            get;
            set;
        }

        public int EditingControlRowIndex
        {
            get;
            set;
        }

        public bool EditingControlValueChanged
        {
            get;
            set;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return false;
        }

        public Cursor EditingPanelCursor
        {
            get { return Cursors.Arrow; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return null;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {

        }



        public bool RepositionEditingControlOnValueChange { get; protected set; }

    }

    /// <summary>
    /// 
    /// </summary>
    [ToolboxItem(false)]
    public class DataGridViewQueryLabelCell : DataGridViewTextBoxCell
    {

        private static readonly Type DefaultEditType = typeof(DataGridViewQueryLabelEditingControl);

        public override Type EditType
        {
            get
            {
                return DefaultEditType; // the type is DataGridViewNumericUpDownEditingControl
            }
        }

        public override string ToString()
        {
            return "DataGridViewQueryLabelCell { ColumnIndex=" + this.ColumnIndex.ToString(CultureInfo.CurrentCulture) + ", RowIndex=" + this.RowIndex.ToString(CultureInfo.CurrentCulture) + " }";
        }
        /// <summary>
        /// 附加并初始化寄宿的编辑控件。
        /// </summary>
        /// <param name="rowIndex">所编辑的行的索引。</param><param name="initialFormattedValue">要在控件中显示的初始值。</param><param name="dataGridViewCellStyle">用于确定寄宿控件外观的单元格样式。</param><filterpriority>1</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewQueryLabelEditingControl queryLabel = this.DataGridView.EditingControl as DataGridViewQueryLabelEditingControl;
            var column = this.OwningColumn as DataGridViewQueryLabelColumn;

            if (queryLabel == null || column == null)
                return;
            queryLabel.CanChangeValue = false;
            queryLabel.Fields.ReSet(column.Fields);
            queryLabel.Orders.ReSet(column.Orders);
            queryLabel.Filters.ReSet(column.Filters);
           
            object value = column.GetDislpayName(GetValue(rowIndex));
            queryLabel.DataSource = column.DataSource;
            queryLabel.QueryDataID = column.QueryDataID;
            queryLabel.ValueMember = column.ValueMember;
            queryLabel.DisplayMember = column.DisplayMember;

            queryLabel.Text = value == null ? string.Empty : value.ToString();
        }


        /// <param name="graphics">The <see cref="T:System.Drawing.Graphics"/> used to paint the <see cref="T:System.Windows.Forms.DataGridViewCell"/>.</param><param name="clipBounds">A <see cref="T:System.Drawing.Rectangle"/> that represents the area of the <see cref="T:System.Windows.Forms.DataGridView"/> that needs to be repainted.</param><param name="cellBounds">A <see cref="T:System.Drawing.Rectangle"/> that contains the bounds of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param><param name="rowIndex">The row index of the cell that is being painted.</param><param name="cellState">A bitwise combination of <see cref="T:System.Windows.Forms.DataGridViewElementStates"/> values that specifies the state of the cell.</param><param name="value">The data of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param><param name="formattedValue">The formatted data of the <see cref="T:System.Windows.Forms.DataGridViewCell"/> that is being painted.</param><param name="errorText">An error message that is associated with the cell.</param><param name="cellStyle">A <see cref="T:System.Windows.Forms.DataGridViewCellStyle"/> that contains formatting and style information about the cell.</param><param name="advancedBorderStyle">A <see cref="T:System.Windows.Forms.DataGridViewAdvancedBorderStyle"/> that contains border styles for the cell that is being painted.</param><param name="paintParts">A bitwise combination of the <see cref="T:System.Windows.Forms.DataGridViewPaintParts"/> values that specifies which parts of the cell need to be painted.</param>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var column = this.OwningColumn as DataGridViewQueryLabelColumn;
            object dis = value;
            if (column != null) dis = column.GetDislpayName(value);
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, dis, dis, errorText, cellStyle, advancedBorderStyle, paintParts);

            if (cellStyle == null)
                throw new ArgumentNullException("cellStyle");
            this.PaintPrivate(graphics, clipBounds, cellBounds, rowIndex, cellState, dis, errorText, cellStyle, advancedBorderStyle, paintParts, false, false, true);
        }

        private Rectangle PaintPrivate(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts, bool computeContentBounds, bool computeErrorIconBounds, bool paint)
        {
            Rectangle rectangle1 = Rectangle.Empty;


            Rectangle rectangle2 = this.BorderWidths(advancedBorderStyle);
            Rectangle rectangle3 = cellBounds;
            rectangle3.Offset(rectangle2.X, rectangle2.Y);
            rectangle3.Width -= rectangle2.Right;
            rectangle3.Height -= rectangle2.Bottom;
            Point currentCellAddress = this.DataGridView.CurrentCellAddress;
            bool flag1 = currentCellAddress.X == this.ColumnIndex && currentCellAddress.Y == rowIndex;
            bool flag2 = flag1 && this.DataGridView.EditingControl != null;
            bool flag3 = (cellState & DataGridViewElementStates.Selected) != DataGridViewElementStates.None;
            SolidBrush solidBrush = !flag3 || flag2 ? new SolidBrush(cellStyle.BackColor) : new SolidBrush(cellStyle.SelectionBackColor);
            graphics.FillRectangle((Brush)solidBrush, rectangle3);
            if (cellStyle.Padding != Padding.Empty)
            {

                rectangle3.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                rectangle3.Width -= cellStyle.Padding.Horizontal;
                rectangle3.Height -= cellStyle.Padding.Vertical;
            }

            Rectangle cellValueBounds = rectangle3;
            string text = formattedValue as string;
            if (text != null)
            {
                int y = cellStyle.WrapMode == DataGridViewTriState.True ? 1 : 2;
                rectangle3.Offset(0, y);

                if (rectangle3.Width > 0 && rectangle3.Height > 0)
                {
                    TextFormatFlags cellStyleAlignment = TextFormatFlags.Default;
                    if (paint)
                    {
                        // if (DataGridViewCell.PaintContentForeground(paintParts))
                        {
                            if ((cellStyleAlignment & TextFormatFlags.SingleLine) != TextFormatFlags.Default)
                                cellStyleAlignment |= TextFormatFlags.EndEllipsis;
                            TextRenderer.DrawText((IDeviceContext)graphics, text, cellStyle.Font, rectangle3, flag3 ? cellStyle.SelectionForeColor : cellStyle.ForeColor, cellStyleAlignment);
                        }
                    }
                    else
                        rectangle1 = GetTextBounds(rectangle3, text, cellStyleAlignment, cellStyle, cellStyle.Font);
                }
            }

            return rectangle1;

        }
        internal static Point GetTextLocation(Rectangle cellBounds, Size sizeText, TextFormatFlags flags, DataGridViewCellStyle cellStyle)
        {
            Point point = new Point(0, 0);
            DataGridViewContentAlignment contentAlignment = cellStyle.Alignment;
            if ((flags & TextFormatFlags.RightToLeft) != TextFormatFlags.Default)
            {
                switch (contentAlignment)
                {
                    case DataGridViewContentAlignment.MiddleRight:
                        contentAlignment = DataGridViewContentAlignment.MiddleLeft;
                        break;
                    case DataGridViewContentAlignment.BottomLeft:
                        contentAlignment = DataGridViewContentAlignment.BottomRight;
                        break;
                    case DataGridViewContentAlignment.BottomRight:
                        contentAlignment = DataGridViewContentAlignment.BottomLeft;
                        break;
                    case DataGridViewContentAlignment.TopLeft:
                        contentAlignment = DataGridViewContentAlignment.TopRight;
                        break;
                    case DataGridViewContentAlignment.TopRight:
                        contentAlignment = DataGridViewContentAlignment.TopLeft;
                        break;
                    case DataGridViewContentAlignment.MiddleLeft:
                        contentAlignment = DataGridViewContentAlignment.MiddleRight;
                        break;
                }
            }
            switch (contentAlignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                    point.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
                    point.Y = cellBounds.Bottom - sizeText.Height;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                    point.X = cellBounds.Right - sizeText.Width;
                    point.Y = cellBounds.Bottom - sizeText.Height;
                    break;
                case DataGridViewContentAlignment.MiddleRight:
                    point.X = cellBounds.Right - sizeText.Width;
                    point.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
                    break;
                case DataGridViewContentAlignment.BottomLeft:
                    point.X = cellBounds.X;
                    point.Y = cellBounds.Bottom - sizeText.Height;
                    break;
                case DataGridViewContentAlignment.TopLeft:
                    point.X = cellBounds.X;
                    point.Y = cellBounds.Y;
                    break;
                case DataGridViewContentAlignment.TopCenter:
                    point.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
                    point.Y = cellBounds.Y;
                    break;
                case DataGridViewContentAlignment.TopRight:
                    point.X = cellBounds.Right - sizeText.Width;
                    point.Y = cellBounds.Y;
                    break;
                case DataGridViewContentAlignment.MiddleLeft:
                    point.X = cellBounds.X;
                    point.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
                    break;
                case DataGridViewContentAlignment.MiddleCenter:
                    point.X = cellBounds.X + (cellBounds.Width - sizeText.Width) / 2;
                    point.Y = cellBounds.Y + (cellBounds.Height - sizeText.Height) / 2;
                    break;
            }
            return point;
        }
        internal static Rectangle GetTextBounds(Rectangle cellBounds, string text, TextFormatFlags flags, DataGridViewCellStyle cellStyle, Font font)
        {
            if ((flags & TextFormatFlags.SingleLine) != TextFormatFlags.Default && TextRenderer.MeasureText(text, font, new Size(int.MaxValue, int.MaxValue), flags).Width > cellBounds.Width)
                flags |= TextFormatFlags.EndEllipsis;
            Size proposedSize = new Size(cellBounds.Width, cellBounds.Height);
            Size size = TextRenderer.MeasureText(text, font, proposedSize, flags);
            if (size.Width > proposedSize.Width)
                size.Width = proposedSize.Width;
            if (size.Height > proposedSize.Height)
                size.Height = proposedSize.Height;
            return size == proposedSize ? cellBounds : new Rectangle(GetTextLocation(cellBounds, size, flags, cellStyle), size);
        }



        internal void ComputeBorderStyleCellStateAndCellBounds(int rowIndex, out DataGridViewAdvancedBorderStyle dgvabsEffective, out DataGridViewElementStates cellState, out Rectangle cellBounds)
        {
            bool singleVerticalBorderAdded = !this.DataGridView.RowHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            bool singleHorizontalBorderAdded = !this.DataGridView.ColumnHeadersVisible && this.DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single;
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceholder = new DataGridViewAdvancedBorderStyle();
            if (rowIndex > -1 && this.OwningColumn != null)
            {
                //  dgvabsEffective = this.AdjustCellBorderStyle(this.DataGridView.AdvancedCellBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, this.ColumnIndex == this.DataGridView.FirstDisplayedColumnIndex, rowIndex == this.DataGridView.FirstDisplayedRowIndex);
                DataGridViewElementStates rowState = this.DataGridView.Rows.GetRowState(rowIndex);
                // cellState = this.CellStateFromColumnRowStates(rowState);
                // cellState |= this.State;
            }
            else if (this.OwningColumn != null)
            {
                DataGridViewColumn lastColumn = this.DataGridView.Columns.GetLastColumn(DataGridViewElementStates.Visible, DataGridViewElementStates.None);
                bool isLastVisibleColumn = lastColumn != null && lastColumn.Index == this.ColumnIndex;
                //     dgvabsEffective = this.DataGridView.AdjustColumnHeaderBorderStyle(this.DataGridView.AdvancedColumnHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, this.ColumnIndex == this.DataGridView.FirstDisplayedColumnIndex, isLastVisibleColumn);
                cellState = this.OwningColumn.State | this.State;
            }
            else if (this.OwningRow != null)
            {
                //   dgvabsEffective = this.OwningRow.AdjustRowHeaderBorderStyle(this.DataGridView.AdvancedRowHeadersBorderStyle, dataGridViewAdvancedBorderStylePlaceholder, singleVerticalBorderAdded, singleHorizontalBorderAdded, rowIndex == this.DataGridView.FirstDisplayedRowIndex, rowIndex == this.DataGridView.Rows.GetLastRow(DataGridViewElementStates.Visible));
                cellState = this.OwningRow.GetState(rowIndex) | this.State;
            }
            else
            {
                dgvabsEffective = this.DataGridView.AdjustedTopLeftHeaderBorderStyle;
                cellState = this.State;
            }
            cellBounds = new Rectangle(new Point(0, 0), this.GetSize(rowIndex));
            dgvabsEffective = null;
            cellState = DataGridViewElementStates.None;
        }
        public override void DetachEditingControl()
        {
            DataGridView dataGridView = this.DataGridView;
            if (dataGridView == null || dataGridView.EditingControl == null)
            {
                throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
            }

            var queryLabel = dataGridView.EditingControl as DataGridViewQueryLabelEditingControl;
            if (queryLabel != null )
            {
              
                this.SetValue(queryLabel.EditingControlRowIndex, queryLabel.SelectedValue); 
                queryLabel.Fields.Clear();
                queryLabel.Orders.Clear();
                queryLabel.Filters.Clear();
                queryLabel.DataSource = null;
            }
            
            base.DetachEditingControl();
        }
    }

}