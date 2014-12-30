using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace Library.Controls
{
    [ToolboxItem(false)]
    public class DataGridViewDateTimeColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewDateTimeColumn()
        {
            this.CellTemplate = new DataGridViewDateTimeCell();
            Format = DateTimePickerFormat.Short;
        }
        [DefaultValue(DateTimePickerFormat.Short)]
        public DateTimePickerFormat Format { get; set; }
        [DefaultValue("")]
        [Localizable(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        public string CustomFormat { get; set; }

        public override object Clone()
        {
            var tmp = (DataGridViewDateTimeColumn)base.Clone();
            if (tmp == null) return null;
            tmp.CustomFormat = this.CustomFormat;
            tmp.Format = this.Format;
            return tmp;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(64);
            stringBuilder.Append("DataGridViewDateTimeColumn { Name=");
            stringBuilder.Append(this.Name);
            stringBuilder.Append(", Index=");
            stringBuilder.Append(this.Index.ToString((IFormatProvider)CultureInfo.CurrentCulture));
            stringBuilder.Append(" }");
            return ((object)stringBuilder).ToString();
        }
    }
    [ToolboxItem(false)]
    public class DataGridViewDateTimeEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {

        }

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

        public bool RepositionEditingControlOnValueChange { get; private set; }
    }

    [ToolboxItem(false)]
    public class DataGridViewDateTimeCell : DataGridViewTextBoxCell
    {
    

        private static Type defaultEditType = typeof(DataGridViewDateTimeEditingControl);
        private static Type defaultValueType = typeof(DateTime);
        public override Type EditType
        {
            get
            {
                return defaultEditType; // the type is DataGridViewNumericUpDownEditingControl
            }
        }

        public override Type ValueType
        {
            get { return defaultValueType; }

        }
        public override string ToString()
        {
            return "DataGridViewDateTimeCell { ColumnIndex=" + this.ColumnIndex.ToString((IFormatProvider)CultureInfo.CurrentCulture) + ", RowIndex=" + this.RowIndex.ToString((IFormatProvider)CultureInfo.CurrentCulture) + " }";
        }
     
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            var cont = this.DataGridView.EditingControl as DateTimePicker;
            var column = this.OwningColumn as DataGridViewDateTimeColumn;
            if (cont == null || column == null) return;
            cont.Format = column.Format;
            cont.CustomFormat = column.CustomFormat;
            var value=GetValue( rowIndex);
            if (value is DateTime) cont.Value = (DateTime)value;

        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var column = this.OwningColumn as DataGridViewDateTimeColumn;
            object dis = value;
            if (column != null)
            {
                switch (column.Format)
                {
                    case DateTimePickerFormat.Custom:
                        if (string.IsNullOrEmpty(column.CustomFormat)) goto default;
                        dis = string.Format("{0:" + column.CustomFormat + "}", value);
                        break;
                    case DateTimePickerFormat.Short:
                        dis = string.Format("{0:yyyy-MM-dd}", value);
                        break;
                    case DateTimePickerFormat.Time:
                        dis = string.Format("{0:HH:mm:ss}", value);
                        break;
                    default:
                        dis = string.Format("{0:yyyy-MM-dd HH:mm:ss}", value);
                        break;
                }
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, dis, dis, errorText, cellStyle, advancedBorderStyle, paintParts);
        }
       
        public override void DetachEditingControl()
        {
            DataGridView dataGridView = this.DataGridView;
            if (dataGridView == null || dataGridView.EditingControl == null)
            {
                throw new InvalidOperationException("Cell is detached or its grid has no editing control.");
            }

            var dateTimePicker = dataGridView.EditingControl as DataGridViewDateTimeEditingControl;
            if (dateTimePicker != null )
            {
                this.SetValue(dateTimePicker.EditingControlRowIndex, dateTimePicker.Value);
            }

            base.DetachEditingControl();
        }
    }




}