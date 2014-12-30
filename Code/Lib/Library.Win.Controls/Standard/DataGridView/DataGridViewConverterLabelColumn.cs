using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;


namespace Library.Controls
{
    [ToolboxItem(false)]
    public class DataGridViewConverterLabelColumn : DataGridViewTextBoxColumn
    {
        private const bool _readOnly = true;

        public DataGridViewConverterLabelColumn()
        {
            this.CellTemplate = new DataGridViewConverterLabelCell();

        }
        [DefaultValue(false)]
        public override bool ReadOnly
        {
            get { return _readOnly; }

        }
        [DefaultValue(null)]
        public IDataCellConverter Converter { get; set; }
        [DefaultValue(null)]
        public object Parameter { get; set; }

        public override object Clone()
        {
            var obj = (DataGridViewConverterLabelColumn)base.Clone();
            if (obj == null) return null;
            obj.Converter = this.Converter;
            obj.Parameter = this.Parameter;
            return obj;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(64);
            stringBuilder.Append("DataGridViewConverterLabelColumn { Name=");
            stringBuilder.Append(this.Name);
            stringBuilder.Append(", Index=");
            stringBuilder.Append(this.Index.ToString((IFormatProvider)CultureInfo.CurrentCulture));
            stringBuilder.Append(" }");
            return ((object)stringBuilder).ToString();
        }
    }

    public interface IDataCellConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="parameter"></param>
        /// <param name="cellStyle"></param>
        /// <returns></returns>
        object Convert(object value, object parameter, DataGridViewCellStyle cellStyle);

    }

    public class DataConverterDelegate : IDataCellConverter
    {
        private readonly Func<object, object, object> _func;

        public DataConverterDelegate(Func<object, object, object> func)
        {
            _func = func;
            if (func == null) throw new ArgumentNullException("func");
        }

        public object Convert(object value, object parameter, DataGridViewCellStyle cellStyle)
        {
            return _func(value, parameter);
        }
    }

    [ToolboxItem(false)]
    public class DataGridViewConverterLabelCell : DataGridViewTextBoxCell
    {
        private static Type defaultEditType = typeof(DataGridViewTextBoxEditingControl);

        public override Type EditType
        {
            get
            {
                return defaultEditType; // the type is DataGridViewNumericUpDownEditingControl
            }
        }
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            var column = this.OwningColumn as DataGridViewConverterLabelColumn;
            object dis = GetDisplayValue(rowIndex, cellStyle, column, value);
            if (dis is Image)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, null, null, errorText, cellStyle, advancedBorderStyle, paintParts);
                var img = (Image)dis;
                SizeF sizef = GetSize(cellBounds, img);
                graphics.DrawImage(img, new RectangleF(cellBounds.X + 2, cellBounds.Y + 2, sizef.Width, sizef.Height));

            }
            else
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, dis, dis, errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        private object GetDisplayValue(int rowIndex, DataGridViewCellStyle cellStyle, DataGridViewConverterLabelColumn column, object dis)
        {
            if (column == null || column.Converter == null) return dis;
            if (column.DataPropertyName == string.Empty)
            {
                IList list = null;
                if (this.DataGridView.DataSource is IListSource)
                {
                    var dt = this.DataGridView.DataSource as IListSource;
                    list = dt.GetList();
                }
                else if (this.DataGridView.DataSource is IList)
                {
                    list = this.DataGridView.DataSource as IList;
                }
                if (list != null && list.Count > rowIndex)
                    dis = list[rowIndex];
            }
            dis = column.Converter.Convert(dis, column.Parameter, cellStyle);
            return dis;
        }

        private static SizeF GetSize(Rectangle cellBounds, Image img)
        {
            float width = 0;
            float height = 0;
            height = 0;
            if (img.Width < cellBounds.Width && img.Height < cellBounds.Height)
            {
                width = img.Width;
                height = img.Height;
            }
            else
            {
                if (img.Width > cellBounds.Width)
                {
                    width = img.Width * ((float)cellBounds.Height / (float)img.Height);
                    height = cellBounds.Height;
                }
                else if (img.Height > cellBounds.Height)
                {
                    height = img.Height * ((float)cellBounds.Width / (float)img.Width);
                    width = cellBounds.Width;
                }
            }
            width = width - 4;
            height = height - 4;
            return new SizeF(width, height);
        }

        public override string ToString()
        {
            return "DataGridViewConverterLabelCell { ColumnIndex=" + this.ColumnIndex.ToString((IFormatProvider)CultureInfo.CurrentCulture) + ", RowIndex=" + this.RowIndex.ToString((IFormatProvider)CultureInfo.CurrentCulture) + " }";
        }
    }
}