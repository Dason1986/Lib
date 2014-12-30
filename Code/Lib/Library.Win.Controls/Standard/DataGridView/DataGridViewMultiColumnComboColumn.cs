﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Library.HelperUtility;


namespace Library.Controls
{
    [ToolboxItem(false)]
    public class DataGridViewMultiColumnComboColumn : DataGridViewComboBoxColumn
    {
        public DataGridViewMultiColumnComboColumn()
        {
            //Set the type used in the DataGridView
            this.CellTemplate = new DataGridViewMultiColumnComboCell();

        }

        public string[] DisplayNames { get; set; }
    }



    [ToolboxItem(false)]
    public class DataGridViewMultiColumnComboCell : DataGridViewComboBoxCell
    {
        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewMultiColumnComboEditingControl);
            }
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewMultiColumnComboEditingControl ctrl = DataGridView.EditingControl as DataGridViewMultiColumnComboEditingControl;
            if (ctrl != null) ctrl.OwnerCell = this;
        }


    }
    [ToolboxItem(false)]
    public class DataGridViewMultiColumnComboEditingControl : DataGridViewComboBoxEditingControl
    {

        const int FixedAlignColumnSize = 100;
        const int LineWidth = 1;
        public DataGridViewMultiColumnComboCell OwnerCell = null;

        public DataGridViewMultiColumnComboEditingControl()
        {
            this.DrawMode = DrawMode.OwnerDrawFixed;
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        /**************************************************************************************************/
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Rectangle rec = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            DataGridViewMultiColumnComboColumn column = OwnerCell.OwningColumn as DataGridViewMultiColumnComboColumn;
            if (column == null) return;
            //if (column.DataSource is IList)
            //{
            //    column.FilterItemOnProperty(item, field);
            //    return;
            //}
            if (column.DataSource is IListSource == false || column.DataSource is IList == false) return;
            //DataTable valuesTbl = column.DataSource as DataTable;
            //string joinByField = column.ValueMember;
            SolidBrush normalText = new SolidBrush(SystemColors.ControlText);


            if (e.Index <= -1) return;
            DataRowView currentRow = Items[e.Index] as DataRowView;
            if (currentRow == null) return;
            DataRow row = currentRow.Row;

            string currentText = GetItemText(Items[e.Index]);


            SolidBrush normalBack = new SolidBrush(Color.White);

            e.Graphics.FillRectangle(normalBack, rec);
            if (DroppedDown && Margin.Top != rec.Top)
            {
                int currentOffset = rec.Left;

                SolidBrush hightlightedBack = new SolidBrush(SystemColors.Highlight);
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(hightlightedBack, rec);
                }

                bool addBorder = false;

                //    object valueItem;
                object[] itemarrary;
                if (column.DisplayNames.HasRecord())
                {
                    var columns = row.Table.Columns;
                    itemarrary = new object[column.DisplayNames.Length];
                    for (int i = 0; i < itemarrary.Length; i++)
                    {
                        if (columns.Contains(column.DisplayNames[i])) ;
                        {
                            itemarrary[i] = row[i];
                        }
                    }
                }
                else
                {
                    itemarrary = row.ItemArray;
                }
                foreach (object dataRowItem in itemarrary)
                {
                    //valueItem = dataRowItem;
                    string value = dataRowItem != null ? dataRowItem.ToString() : null;

                    if (addBorder)
                    {
                        SolidBrush gridBrush = new SolidBrush(Color.Gray);
                        long linesNum = LineWidth;
                        while (linesNum > 0)
                        {
                            linesNum--;
                            Point first = new Point(rec.Left + currentOffset, rec.Top);
                            Point last = new Point(rec.Left + currentOffset, rec.Bottom);
                            e.Graphics.DrawLine(new Pen(gridBrush), first, last);
                            currentOffset++;
                        }
                        gridBrush.Dispose();
                    }
                    else
                        addBorder = true;

                    SizeF extent = e.Graphics.MeasureString(value, e.Font);
                    decimal width = (decimal) extent.Width;

                    Rectangle textRec = new Rectangle(currentOffset, rec.Y, (int) decimal.Ceiling(width), rec.Height);


                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {

                        SolidBrush hightlightedText = new SolidBrush(SystemColors.HighlightText);

                        e.Graphics.FillRectangle(hightlightedBack, currentOffset, rec.Y, FixedAlignColumnSize,
                            extent.Height);

                        e.Graphics.DrawString(value, e.Font, hightlightedText, textRec);
                        hightlightedText.Dispose();
                    }
                    else
                    {

                        e.Graphics.FillRectangle(normalBack, currentOffset, rec.Y, FixedAlignColumnSize, extent.Height);

                        e.Graphics.DrawString(value, e.Font, normalText, textRec);
                    }

                    currentOffset += FixedAlignColumnSize;
                }
            }
            else
            {
                e.Graphics.DrawString(currentText, e.Font, normalText, rec);
            }
        }


    }
}