using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Library.Controls
{
    public class MultiColumnComboBox : ComboBox
    {
        private string _columnNameString = "";
        private int _columnWidthDefault = 75;
        private string _columnWidthString = "";
        private int _linkedColumnIndex;
        private TextBox _linkedTextBox;
        private int _ValueMemberColumnIndex = 0;
        private readonly Collection<string> _columnNames = new Collection<string>();
        private readonly Collection<int> _columnWidths = new Collection<int>();

        public MultiColumnComboBox()
        {
            BackColorOdd = Color.White;
            BackColorEven = ColorTable.QQGrayBackground;
            DrawMode = DrawMode.OwnerDrawVariable;

            ContextMenu = new ContextMenu();
        }

        public event System.EventHandler OpenSearchForm;

        public bool AutoComplete { get; set; }

        public bool AutoDropdown { get; set; }

        public Color BackColorEven { get; set; }

        public Color BackColorOdd { get; set; }

        public Collection<string> ColumnNameCollection
        {
            get
            {
                return _columnNames;
            }
        }

        public string ColumnNames
        {
            get
            {
                return _columnNameString;
            }

            set
            {

                if (!Convert.ToBoolean(value.Trim().Length))
                {
                    _columnNameString = "";
                }
                else if (value != null)
                {
                    char[] delimiterChars = { ',', ';', ':' };
                    string[] columnNames = value.Split(delimiterChars);

                    if (!DesignMode)
                    {
                        _columnNames.Clear();
                    }


                    foreach (string s in columnNames)
                    {

                        if (Convert.ToBoolean(s.Trim().Length))
                        {
                            if (!DesignMode)
                            {
                                _columnNames.Add(s.Trim());
                            }
                        }
                        else
                        {
                            throw new NotSupportedException("Column names can not be blank.");
                        }
                    }
                    _columnNameString = value;
                }
            }
        }

        public Collection<int> ColumnWidthCollection
        {
            get
            {
                return _columnWidths;
            }
        }

        public int ColumnWidthDefault
        {
            get
            {
                return _columnWidthDefault;
            }
            set
            {
                _columnWidthDefault = value;
            }
        }
        readonly char[] _delimiterChars = { ',', ';', ':' };
        public string ColumnWidths
        {
            get
            {
                return _columnWidthString;
            }

            set
            {
                if (value == null) return;
                if (!Convert.ToBoolean(value.Trim().Length))
                {
                    _columnWidthString = "";
                    return;
                }
                string[] columnWidths = value.Split(_delimiterChars);
                string invalidValue = "";
                int invalidIndex = -1;
                int idx = 1;

                foreach (string s in columnWidths)
                {

                    if (Convert.ToBoolean(s.Trim().Length))
                    {
                        int intValue;
                        if (!int.TryParse(s, out intValue))
                        {
                            invalidIndex = idx;
                            invalidValue = s;
                        }
                        else
                        {
                            idx++;
                        }
                    }
                    else
                    {
                        idx++;
                    }
                }


                if (invalidIndex > -1)
                {
                    string errMsg = "Invalid column width '" + invalidValue + "' located at column " + invalidIndex;
                    throw new ArgumentOutOfRangeException(errMsg);
                }
                _columnWidthString = value;


                if (!DesignMode)
                {
                    _columnWidths.Clear();
                    foreach (string s in columnWidths)
                    {
                        _columnWidths.Add(Convert.ToBoolean(s.Trim().Length) ? Convert.ToInt32(s) : _columnWidthDefault);
                    }


                    if (DataManager != null)
                    {
                        InitializeColumns();
                    }
                }
            }
        }

        public new DrawMode DrawMode
        {
            get
            {
                return base.DrawMode;
            }
            set
            {
                if (value != DrawMode.OwnerDrawVariable)
                {
                    throw new NotSupportedException("Needs to be DrawMode.OwnerDrawVariable");
                }
                base.DrawMode = value;
            }
        }

        public new ComboBoxStyle DropDownStyle
        {
            get
            {
                return base.DropDownStyle;
            }
            set
            {
                if (value != ComboBoxStyle.DropDown)
                {
                    return;
                }
                base.DropDownStyle = value;
            }
        }

        public int LinkedColumnIndex
        {
            get
            {
                return _linkedColumnIndex;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("A column index can not be negative");
                }
                _linkedColumnIndex = value;
            }
        }

        public TextBox LinkedTextBox
        {
            get
            {
                return _linkedTextBox;
            }
            set
            {
                _linkedTextBox = value;

                if (_linkedTextBox == null) return;
                _linkedTextBox.ReadOnly = true;
                _linkedTextBox.TabStop = false;
            }
        }

        public int TotalWidth { get; private set; }

        protected override void OnDataSourceChanged(EventArgs e)
        {
            base.OnDataSourceChanged(e);

            InitializeColumns();
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (DesignMode)
                return;



            Rectangle boundsRect = e.Bounds;
            int lastRight = 0;

            Color brushForeColor = Color.Black;
            if ((e.State & DrawItemState.Selected) == 0)
            {
                e.DrawBackground();
                Color backColor = e.Index % 2 == 0 ? BackColorOdd : BackColorEven;
                using (SolidBrush brushBackColor = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(brushBackColor, e.Bounds);
                }


            }
            else
            {

                using (Pen highLightPen = new Pen(ColorTable.QQHighLightColor))
                {
                    Rectangle drawRect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1);
                    using (SolidBrush brushBackColor = new SolidBrush(ColorTable.QQBlueBackground))
                        e.Graphics.FillRectangle(brushBackColor, e.Bounds);
                    e.Graphics.DrawRectangle(highLightPen, drawRect);

                    //InnerRect
                    drawRect.Inflate(-1, -1);
                    highLightPen.Color = ColorTable.QQHighLightInnerColor;
                    e.Graphics.DrawRectangle(highLightPen, drawRect);
                }
            }

            using (Pen linePen = new Pen(SystemColors.GrayText))
            {
                using (SolidBrush brush = new SolidBrush(brushForeColor))
                {
                    if (!Convert.ToBoolean(_columnNames.Count))
                    {
                        e.Graphics.DrawString(Convert.ToString(Items[e.Index]), Font, brush, boundsRect);
                    }
                    else
                    {

                        if (RightToLeft.Equals(RightToLeft.Yes))
                        {

                            StringFormat rtl = new StringFormat
                                               {
                                                   Alignment = StringAlignment.Near,
                                                   FormatFlags = StringFormatFlags.DirectionRightToLeft
                                               };


                            for (int colIndex = _columnNames.Count - 1; colIndex >= 0; colIndex--)
                            {
                                if (!Convert.ToBoolean(_columnWidths[colIndex])) continue;
                                string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], _columnNames[colIndex]));
                                boundsRect.X = lastRight;
                                boundsRect.Width = _columnWidths[colIndex];
                                lastRight = boundsRect.Right;

                                e.Graphics.DrawString(item, Font, brush, boundsRect, rtl);
                                if (colIndex > 0)
                                {
                                    e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);
                                }
                            }
                        }

                        else
                        {

                            for (int colIndex = 0; colIndex < _columnNames.Count; colIndex++)
                            {
                                if (!Convert.ToBoolean(_columnWidths[colIndex])) continue;
                                string item = Convert.ToString(FilterItemOnProperty(Items[e.Index], _columnNames[colIndex]));

                                boundsRect.X = lastRight;
                                boundsRect.Width = _columnWidths[colIndex];
                                lastRight = boundsRect.Right;
                                e.Graphics.DrawString(item, Font, brush, boundsRect);
                                if (colIndex < _columnNames.Count - 1)
                                {
                                    e.Graphics.DrawLine(linePen, boundsRect.Right, boundsRect.Top, boundsRect.Right, boundsRect.Bottom);
                                }
                            }
                        }
                    }
                }
            }

            e.DrawFocusRectangle();
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            if (TotalWidth <= 0) return;
            this.DropDownWidth = Items.Count > MaxDropDownItems
                ? TotalWidth + SystemInformation.VerticalScrollBarWidth
                : TotalWidth;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                case Keys.Delete:
                    SelectedIndex = -1;
                    Text = "";
                    if (_linkedTextBox != null)
                    {
                        _linkedTextBox.Text = "";
                    }
                    break;
                case Keys.F3:
                    if (OpenSearchForm != null)
                    {
                        OpenSearchForm(this, System.EventArgs.Empty);
                    }
                    break;
            }
        }

        // Some of the code for OnKeyPress was derived from some VB.NET code  
        // posted by Laurent Muller as a suggested improvement for another control.
        // http://www.codeproject.com/vb/net/autocomplete_combobox.asp?df=100&forumid=3716&select=579095#xx579095xx
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            int idx;
            string toFind;

            DroppedDown = AutoDropdown;
            if (!Char.IsControl(e.KeyChar))
            {
                if (AutoComplete)
                {
                    toFind = Text.Substring(0, SelectionStart) + e.KeyChar;
                    idx = FindStringExact(toFind);

                    if (idx == -1)
                    {

                        idx = FindString(toFind);
                    }
                    else
                    {

                        DroppedDown = false;
                    }

                    if (idx != -1)
                    {
                        SelectedIndex = idx;
                        SelectionStart = toFind.Length;
                        SelectionLength = Text.Length - SelectionStart;
                    }
                    else
                    {

                        e.KeyChar = (char)0;
                    }
                }
                else
                {
                    idx = FindString(e.KeyChar.ToString(), SelectedIndex);

                    if (idx != -1)
                    {
                        SelectedIndex = idx;
                    }
                }
            }


            if ((e.KeyChar == (char)(Keys.Back)) && AutoComplete && (Convert.ToBoolean(SelectionStart)))
            {

                toFind = Text.Substring(0, SelectionStart - 1);
                idx = FindString(toFind);

                if (idx != -1)
                {
                    SelectedIndex = idx;
                    SelectionStart = toFind.Length;
                    SelectionLength = Text.Length - SelectionStart;
                }
            }


            e.Handled = true;
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);

            if (_linkedTextBox == null) return;
            if (_linkedColumnIndex >= _columnNames.Count) return;
            _linkedTextBox.Text = Convert.ToString(FilterItemOnProperty(SelectedItem, _columnNames[_linkedColumnIndex]));
        }

        protected override void OnValueMemberChanged(EventArgs e)
        {
            base.OnValueMemberChanged(e);

            InitializeValueMemberColumn();
        }

        private void InitializeColumns()
        {
            if (Convert.ToBoolean(_columnNameString.Length))
            {
                TotalWidth = 0;

                for (int colIndex = 0; colIndex < _columnNames.Count; colIndex++)
                {
                    if (colIndex >= _columnWidths.Count)
                    {
                        _columnWidths.Add(_columnWidthDefault);
                    }
                    TotalWidth += _columnWidths[colIndex];
                }
            }
            else
            {
                if (DataManager != null)
                {
                    PropertyDescriptorCollection propertyDescriptorCollection = DataManager.GetItemProperties();

                    TotalWidth = 0;
                    _columnNames.Clear();

                    for (int colIndex = 0; colIndex < propertyDescriptorCollection.Count; colIndex++)
                    {
                        _columnNames.Add(propertyDescriptorCollection[colIndex].Name);

                        if (colIndex >= _columnWidths.Count)
                        {
                            _columnWidths.Add(_columnWidthDefault);
                        }
                        TotalWidth += _columnWidths[colIndex];
                    }
                }
            }


            if (_linkedColumnIndex >= _columnNames.Count)
            {
                _linkedColumnIndex = 0;
            }
        }

        private void InitializeValueMemberColumn()
        {
            int colIndex = 0;
            foreach (String columnName in _columnNames)
            {
                if (String.Compare(columnName, ValueMember, true, CultureInfo.CurrentUICulture) == 0)
                {
                    _ValueMemberColumnIndex = colIndex;
                    break;
                }
                colIndex++;
            }
        }
    }

}
