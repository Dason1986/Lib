#define DEBUGREADERS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.FileExtension.ExcelDataReader.Core.OpenXmlFormat;
using System.IO;
using Library.FileExtension.ExcelDataReader.Core;
using System.Data;
using System.Xml;
using System.Globalization;
using Library.ComponentModel;
using Library.HelperUtility;

namespace Library.FileExtension.ExcelDataReader
{

    public class ExcelOpenXmlReader : IExcelDataReader
    {
        #region Members

        private XlsxWorkbook _workbook;
        private bool _isValid;
        private bool _isClosed;
        private bool _isFirstRead;
        private string _exceptionMessage;
        private int _depth;
        private int _resultIndex;
        private int _emptyRowCount;
        private ZipWorker _zipWorker;
        private XmlReader _xmlReader;
        private Stream _sheetStream;
        private object[] _cellsValues;
        private object[] _savedCellsValues;

        private bool disposed;
        private bool _isFirstRowAsColumnNames = false;
        private const string COLUMN = "Column";
        private string instanceId = Guid.NewGuid().ToString();
        public int TotalRow
        {
            get
            {

                if (_workbook == null || _resultIndex < 0 || _resultIndex > _workbook.Sheets.Count) return 0;
                return _workbook.Sheets[_resultIndex].RowsCount;
            }
        }
        private List<int> _defaultDateTimeStyles;
        #endregion

        internal ExcelOpenXmlReader(bool isFirstRowAsColumnNames)
        {
            _isFirstRowAsColumnNames = isFirstRowAsColumnNames;
            _isValid = true;
            _isFirstRead = true;

            _defaultDateTimeStyles = new List<int>(new int[] 
			{
				14, 15, 16, 17, 18, 19, 20, 21, 22, 45, 46, 47
			});

        }

        private void ReadGlobals()
        {
            _workbook = new XlsxWorkbook(
                _zipWorker.GetWorkbookStream(),
                _zipWorker.GetWorkbookRelsStream(),
                _zipWorker.GetSharedStringsStream(),
                _zipWorker.GetStylesStream());

            CheckDateTimeNumFmts(_workbook.Styles.NumFmts);

        }

        private void CheckDateTimeNumFmts(List<XlsxNumFmt> list)
        {
            if (list.Count == 0) return;

            foreach (XlsxNumFmt numFmt in list)
            {
                if (string.IsNullOrEmpty(numFmt.FormatCode)) continue;
                string fc = numFmt.FormatCode.ToLower();

                int pos;
                while ((pos = fc.IndexOf('"')) > 0)
                {
                    int endPos = fc.IndexOf('"', pos + 1);

                    if (endPos > 0) fc = fc.Remove(pos, endPos - pos + 1);
                }

                //it should only detect it as a date if it contains
                //dd mm mmm yy yyyy
                //h hh ss
                //AM PM
                //and only if these appear as "words" so either contained in [ ]
                //or delimted in someway
                //updated to not detect as date if format contains a #
                var formatReader = new FormatReader() { FormatString = fc };
                if (formatReader.IsDateFormatString())
                {
                    _defaultDateTimeStyles.Add(numFmt.Id);
                }
            }
        }

        private void ReadSheetGlobals(XlsxWorksheet sheet)
        {
            if (_xmlReader != null) _xmlReader.Close();
            if (_sheetStream != null) _sheetStream.Close();

            _sheetStream = _zipWorker.GetWorksheetStream(sheet.Path);

            if (null == _sheetStream) return;

            _xmlReader = XmlReader.Create(_sheetStream);

            while (_xmlReader.Read())
            {
                if (_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Name == XlsxWorksheet.N_dimension)
                {
                    string dimValue = _xmlReader.GetAttribute(XlsxWorksheet.A_ref);

                    //					if (dimValue.IndexOf(':') > 0)
                    //					{
                    sheet.Dimension = new XlsxDimension(dimValue);
                    //					}
                    //					else
                    //					{
                    //						_xmlReader.Close();
                    //						_sheetStream.Close();
                    //					}

                    break;
                }
            }
        }

        private bool ReadSheetRow(XlsxWorksheet sheet)
        {
            if (null == _xmlReader) return false;

            if (_emptyRowCount != 0)
            {
                _cellsValues = new object[sheet.ColumnsCount];
                _emptyRowCount--;
                _depth++;

                return true;
            }

            if (_savedCellsValues != null)
            {
                _cellsValues = _savedCellsValues;
                _savedCellsValues = null;
                _depth++;

                return true;
            }

            if ((_xmlReader.NodeType == XmlNodeType.Element && _xmlReader.Name == XlsxWorksheet.N_row) ||
                _xmlReader.ReadToFollowing(XlsxWorksheet.N_row))
            {
                _cellsValues = new object[sheet.ColumnsCount];

                int rowIndex = int.Parse(_xmlReader.GetAttribute(XlsxWorksheet.A_r));
                if (rowIndex != (_depth + 1))
                {
                    _emptyRowCount = rowIndex - _depth - 1;
                }
                bool hasValue = false;
                string a_s = String.Empty;
                string a_t = String.Empty;
                string a_r = String.Empty;
                int col = 0;
                int row = 0;

                while (_xmlReader.Read())
                {
                    if (_xmlReader.Depth == 2) break;

                    if (_xmlReader.NodeType == XmlNodeType.Element)
                    {
                        hasValue = false;

                        if (_xmlReader.Name == XlsxWorksheet.N_c)
                        {
                            a_s = _xmlReader.GetAttribute(XlsxWorksheet.A_s);
                            a_t = _xmlReader.GetAttribute(XlsxWorksheet.A_t);
                            a_r = _xmlReader.GetAttribute(XlsxWorksheet.A_r);
                            XlsxDimension.XlsxDim(a_r, out col, out row);
                        }
                        else if (_xmlReader.Name == XlsxWorksheet.N_v)
                        {
                            hasValue = true;
                        }
                    }

                    if (_xmlReader.NodeType == XmlNodeType.Text && hasValue)
                    {
                        double number;
                        object o = _xmlReader.Value;

                        if (double.TryParse(o.ToString(), out number))
                            o = number;

                        if (null != a_t && a_t == XlsxWorksheet.A_s) //if string
                        {
                            o = Helpers.ConvertEscapeChars(_workbook.SST[int.Parse(o.ToString())]);
                        }
                        else if (a_t == "b") //boolean
                        {
                            o = _xmlReader.Value == "1";
                        }
                        else if (null != a_s) //if something else
                        {
                            XlsxXf xf = _workbook.Styles.CellXfs[int.Parse(a_s)];
                            if (xf.ApplyNumberFormat && o != null && o.ToString() != string.Empty && IsDateTimeStyle(xf.NumFmtId))
                                o = Helpers.ConvertFromOATime(number);
                            else if (xf.NumFmtId == 49)
                                o = o.ToString();
                        }



                        if (col - 1 < _cellsValues.Length)
                            _cellsValues[col - 1] = o;
                    }
                }

                if (_emptyRowCount > 0)
                {
                    _savedCellsValues = _cellsValues;
                    return ReadSheetRow(sheet);
                }
                _depth++;

                return true;
            }

            _xmlReader.Close();
            if (_sheetStream != null) _sheetStream.Close();

            return false;
        }

        private bool InitializeSheetRead()
        {
            if (ResultsCount <= 0) return false;
            _isFirstRead = false;
            ReadSheetGlobals(_workbook.Sheets[_resultIndex]);

            if (_workbook.Sheets[_resultIndex].Dimension == null) return false;



            _depth = 0;
            if (IsFirstRowAsColumnNames)
            {
                ReadSheetRow(_workbook.Sheets[_resultIndex]);
                currentCNames = new string[FieldCount];
                for (int i = 0; i < FieldCount; i++)
                {
                    currentCNames[i] = GetString(i);
                }
            }
            else
            {
                currentCNames = new string[FieldCount];
                for (int i = 0; i < FieldCount; i++)
                {
                    currentCNames[i] = string.Format("{0}{1}", COLUMN, i + 1);
                }
            }
            _emptyRowCount = 0;

            return true;
        }

        private bool IsDateTimeStyle(int styleId)
        {
            return _defaultDateTimeStyles.Contains(styleId);
        }


        #region IExcelDataReader Members

        public void Initialize(System.IO.Stream fileStream)
        {
            _zipWorker = new ZipWorker();
            _zipWorker.Extract(fileStream);

            if (!_zipWorker.IsValid)
            {
                _isValid = false;
                _exceptionMessage = _zipWorker.ExceptionMessage;

                Close();

                return;
            }

            ReadGlobals();
        }

        public System.Data.DataSet AsDataSet()
        {
            return AsDataSet(true);
        }

        public System.Data.DataSet AsDataSet(bool convertOADateTime)
        {
            if (!_isValid) return null;

            DataSet dataset = new DataSet();

            for (int ind = 0; ind < _workbook.Sheets.Count; ind++)
            {
               
                DataTable table=CreateTable(ind) ;
                if (table!=null)
                    dataset.Tables.Add(table);
            }
            dataset.AcceptChanges();
            Helpers.FixDataTypes(dataset);
            return dataset;
        }

        private DataTable CreateTable(int sheetindex, int top=0)
        {
            ReadSheetGlobals(_workbook.Sheets[sheetindex]);
            if (_workbook.Sheets[sheetindex].Dimension == null) return null;
            DataTable table = new DataTable(_workbook.Sheets[sheetindex].Name);



            _depth = 0;
            _emptyRowCount = 0;

            //DataTable columns
            if (!_isFirstRowAsColumnNames)
            {
                for (int i = 0; i < _workbook.Sheets[sheetindex].ColumnsCount; i++)
                {
                    table.Columns.Add(null, typeof(Object));
                }
            }
            else if (ReadSheetRow(_workbook.Sheets[sheetindex]))
            {
                for (int index = 0; index < _cellsValues.Length; index++)
                {
                    if (_cellsValues[index] != null && _cellsValues[index].ToString().Length > 0)
                        table.Columns.Add(_cellsValues[index].ToString(), typeof(Object));
                    else
                        table.Columns.Add(string.Concat(COLUMN, index), typeof(Object));
                }
            }
            else return table;

            table.BeginLoadData();
            int rows = 0;
            bool canread = true;
            while (canread&&ReadSheetRow(_workbook.Sheets[sheetindex]))
            {
                rows++;
                if (rows == top)
                    canread = false;
                table.Rows.Add(_cellsValues);
            }

            table.EndLoadData();
            return table;
        }

        public bool IsFirstRowAsColumnNames
        {
            get
            {
                return _isFirstRowAsColumnNames;
            }
        }

        public bool IsValid
        {
            get { return _isValid; }
        }

        public string ExceptionMessage
        {
            get { return _exceptionMessage; }
        }

        public string Name
        {
            get
            {
                return (_resultIndex >= 0 && _resultIndex < ResultsCount) ? _workbook.Sheets[_resultIndex].Name : null;
            }
        }

        public void Close()
        {
            _isClosed = true;

            if (_xmlReader != null) _xmlReader.Close();

            if (_sheetStream != null) _sheetStream.Close();

            if (_zipWorker != null) _zipWorker.Dispose();
        }

        public int Depth
        {
            get { return _depth; }
        }

        public int ResultsCount
        {
            get { return _workbook == null ? -1 : _workbook.Sheets.Count; }
        }

        public bool IsClosed
        {
            get { return _isClosed; }
        }

        public bool NextResult()
        {
            if (_resultIndex >= (this.ResultsCount - 1)) return false;

            _resultIndex++;

            _isFirstRead = true;
            InitializeSheetRead();
            return true;
        }

        public bool Read()
        {
            if (!_isValid) return false;

            if (_isFirstRead && !InitializeSheetRead())
            {
                return false;
            }

            return ReadSheetRow(_workbook.Sheets[_resultIndex]);
        }

        public int FieldCount
        {
            get { return (_resultIndex >= 0 && _resultIndex < ResultsCount) ? _workbook.Sheets[_resultIndex].ColumnsCount : -1; }
        }

        public bool GetBoolean(int i)
        {
            if (IsDBNull(i)) return false;

            return Boolean.Parse(_cellsValues[i].ToString());
        }

        public DateTime GetDateTime(int i)
        {
            if (IsDBNull(i)) return DateTime.MinValue;

            try
            {
                return (DateTime)_cellsValues[i];
            }
            catch (InvalidCastException)
            {
                return DateTime.MinValue;
            }

        }

        public decimal GetDecimal(int i)
        {
            if (IsDBNull(i)) return decimal.MinValue;

            return decimal.Parse(_cellsValues[i].ToString());
        }

        public double GetDouble(int i)
        {
            if (IsDBNull(i)) return double.MinValue;

            return double.Parse(_cellsValues[i].ToString());
        }

        public float GetFloat(int i)
        {
            if (IsDBNull(i)) return float.MinValue;

            return float.Parse(_cellsValues[i].ToString());
        }

        public short GetInt16(int i)
        {
            if (IsDBNull(i)) return short.MinValue;

            return short.Parse(_cellsValues[i].ToString());
        }

        public int GetInt32(int i)
        {
            if (IsDBNull(i)) return int.MinValue;

            return int.Parse(_cellsValues[i].ToString());
        }

        public long GetInt64(int i)
        {
            if (IsDBNull(i)) return long.MinValue;

            return long.Parse(_cellsValues[i].ToString());
        }

        public string GetString(int i)
        {
            if (IsDBNull(i)) return null;

            return _cellsValues[i].ToString();
        }

        public object GetValue(int i)
        {
            return _cellsValues[i];
        }

        public bool IsDBNull(int i)
        {
            return (null == _cellsValues[i]) || (DBNull.Value == _cellsValues[i]);
        }

        public object this[int i]
        {
            get { return _cellsValues[i]; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.

            if (!this.disposed)
            {
                if (disposing)
                {
                    if (_xmlReader != null) ((IDisposable)_xmlReader).Dispose();
                    if (_sheetStream != null) _sheetStream.Dispose();
                    if (_zipWorker != null) _zipWorker.Dispose();
                }

                _zipWorker = null;
                _xmlReader = null;
                _sheetStream = null;

                _workbook = null;
                _cellsValues = null;
                _savedCellsValues = null;

                disposed = true;
            }
        }

        ~ExcelOpenXmlReader()
        {
            Dispose(false);
        }

        #endregion

        #region  Not Supported IDataReader Members


        public DataTable GetSchemaTable()
        {
            throw new NotSupportedException();
        }

        public int RecordsAffected
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region Not Supported IDataRecord Members


        public byte GetByte(int i)
        {
            if (IsDBNull(i)) return byte.MinValue;

            return ObjectUtility.Cast<byte>(_cellsValues[i]);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public char GetChar(int i)
        {
            throw new NotSupportedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        public string GetDataTypeName(int i)
        {
            return "text";
        }

        public Type GetFieldType(int i)
        {
            return typeof(string);
        }

        public Guid GetGuid(int i)
        {
            if (IsDBNull(i)) return Guid.Empty;

            return ObjectUtility.Cast<Guid>(_cellsValues[i]);
        }

        public string GetName(int i)
        {
            return currentCNames[i];
        }

        private string[] currentCNames;
        public int GetOrdinal(string name)
        {
            for (int i = 0; i < currentCNames.Length; i++)
            {
                if (string.Equals(currentCNames[i], name, StringComparison.OrdinalIgnoreCase)) return i;
            }
            throw new Exception("列名不存在");
        }

        public int GetValues(object[] values)
        {
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = GetValue(i);
                }
                return values.Length;
            }
            return -1;
        }

        public object this[string name]
        {
            get { return this[GetOrdinal(name)]; }
        }

        #endregion




        public DataSet GetSchema()
        {
            DataSet excelSchemaDs = new DataSet();
            DataTable excelSchemaTable = new DataTable("TableNames");
            DataTable excelSchemaColumn = new DataTable("TableColumns");
            excelSchemaTable.Columns.Add("Table_Name", typeof(string));
            excelSchemaColumn.Columns.Add("Table_Name", typeof(string));
            excelSchemaColumn.Columns.Add("Column_Name", typeof(string));
            excelSchemaColumn.Columns.Add("DATA_TYPE", typeof(string));
            excelSchemaDs.Tables.Add(excelSchemaTable);
            excelSchemaDs.Tables.Add(excelSchemaColumn);


            for (int ind = 0; ind < _workbook.Sheets.Count; ind++)
            {


                ReadSheetGlobals(_workbook.Sheets[ind]);
                var sheet = _workbook.Sheets[ind];
                DataRow drRowTable = excelSchemaTable.NewRow();
                drRowTable["Table_Name"] = sheet.Name;
                excelSchemaTable.Rows.Add(drRowTable);

                if (_workbook.Sheets[ind].Dimension == null) continue;
                DataTable itemTable = new DataTable(string.Format("Schema [{0}]", sheet.Name));
                itemTable.Columns.Add("Column_Name", typeof(string));
                itemTable.Columns.Add("DATA_TYPE", typeof(string));
                excelSchemaDs.Tables.Add(itemTable);
                _depth = 0;
                _emptyRowCount = 0;

                //DataTable columns
                if (!_isFirstRowAsColumnNames)
                {

                    for (int i = 0; i < _workbook.Sheets[ind].ColumnsCount; i++)
                    {
                        var Column_Name = string.Format("Column{0}", i);
                        DataRow drRowColumn = excelSchemaColumn.NewRow();
                        drRowColumn["Table_Name"] = sheet.Name;
                        drRowColumn["Column_Name"] = Column_Name;
                        drRowColumn["DATA_TYPE"] = typeof(string).FullName;
                        excelSchemaColumn.Rows.Add(drRowColumn);

                        var itemtebledr = itemTable.NewRow();
                        itemTable.Rows.Add(itemtebledr);
                        itemtebledr[0] = Column_Name;
                        itemtebledr[1] = typeof(string).FullName;
                    }
                }
                else if (ReadSheetRow(sheet))
                {
                    for (int index = 0; index < _cellsValues.Length; index++)
                    {
                        var Column_Name = string.Empty;
                        if (_cellsValues[index] != null && _cellsValues[index].ToString().Length > 0)
                            Column_Name = _cellsValues[index].ToString();
                        else
                            Column_Name = string.Concat(COLUMN, index);
                        DataRow drRowColumn = excelSchemaColumn.NewRow();
                        excelSchemaColumn.Rows.Add(drRowColumn);
                        drRowColumn["Table_Name"] = sheet.Name;
                        drRowColumn["Column_Name"] = Column_Name;
                        drRowColumn["DATA_TYPE"] = typeof(string).FullName;

                        var itemtebledr = itemTable.NewRow();
                        itemTable.Rows.Add(itemtebledr);
                        itemtebledr[0] = Column_Name;
                        itemtebledr[1] = typeof(string).FullName;
                    }
                }


            } return excelSchemaDs;
        }


        public bool GoToResult(int index)
        {
            if (index >= (this.ResultsCount)) return false;

            _resultIndex = index;

            _isFirstRead = true;
            InitializeSheetRead();
            return true;
        }

        public bool GoToResult(string name)
        {
            var sheetindex = _workbook.Sheets.FindIndex(n => string.Equals(n.Name, name, StringComparison.CurrentCultureIgnoreCase));
            return GoToResult(sheetindex);
        }


        public DataTable AsTable(string name, int top)
        {
            if (top < 0) throw new Exception("记录数不能少于0");
            var sheetindex = _workbook.Sheets.FindIndex(n => string.Equals(n.Name, name, StringComparison.CurrentCultureIgnoreCase));
            return AsTable(sheetindex, top);
        }

        public DataTable AsTable(int index, int top)
        {
            if (top<0)throw new Exception("记录数不能少于0");
            return CreateTable(index, top);
        }
    }
}
