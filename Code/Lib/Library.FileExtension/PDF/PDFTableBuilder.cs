using System;
using System.Collections;
using System.ComponentModel;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Library.Data;
using Library.HelperUtility;

namespace Library.FileExtension
{
    class PDFTableBuilder : PDFElementBuilder
    {
        private readonly PDFBuilder _pdfBuilder;
        private readonly TableElement table;

        public PDFTableBuilder(PDFBuilder pdfBuilder, TableElement table1)
        {
            _pdfBuilder = pdfBuilder;
            this.table = table1;
        }

        private float[] headwidth;
        public override void Builder()
        {
            if (table.Heads == null) throw new Exception();

            var columns = table.Heads.Length;
            headwidth = new float[columns];
            PdfPTable pdfPTable = new PdfPTable(columns);
            pdfPTable.LockedWidth = true;
            BuildHeader(columns, pdfPTable);
            pdfPTable.SkipFirstHeader = true;
            BuilBodyData(pdfPTable);
            pdfPTable.Complete = true;
            pdfPTable.SetTotalWidth(headwidth);
            var writer = _pdfBuilder.writer;

            var Height = _pdfBuilder.Height;
            if (pdfPTable.TotalHeight > Height)
            {
                Pager(pdfPTable);
            }
            else
            {
                pdfPTable.WriteSelectedRows(0, -1, 0, -1, table.Position.X, Math.Abs(table.Position.Y - Height), writer.DirectContent, true);
            }
            writer.DirectContent.Stroke();
        }

        private void BuilBodyData(PdfPTable pdfPTable)
        {
            DataManager manager = new DataManager(table.DataSource) { NameIgnoreCase = true };

            int columns = table.Heads.Length;


            var indexs = new int[columns];
            for (int i = 0; i < columns; i++)
            {
                var col = table.Heads[i];
                indexs[i] = manager.GetOrdinal(col.BindName);
            }

            for (int i = 0; i < manager.Count; i++)
            {
                manager.Position = i;
                PdfPCell[] rowCells = new PdfPCell[columns];
                int j = 0;
                foreach (var i1 in indexs)
                {
                    var cel = ObjectUtility.Cast<string>(manager.GetValue(i1));

                    rowCells[j] = new PdfPCell((new Phrase(string.IsNullOrEmpty(cel) ? " " : cel, PDFBuilder.DefaultFont)))
                    {
                        PaddingLeft = 4,
                        PaddingRight = 4,
                        UseBorderPadding = true
                    };
                    j++;
                }

                pdfPTable.Rows.Add(new PdfPRow(rowCells));
                pdfPTable.CompleteRow();
            }
            if (!table.FillRows) return;
            var count = table.FillRowCounts - manager.Count;
            if (count > 0)
            {
                FillTabel(count, pdfPTable);
            }
        }

        private void BuildHeader(int columns, PdfPTable pdfPTable)
        {
            PdfPCell[] headrowCells = new PdfPCell[columns];
            for (int j = 0; j < columns; j++)
            {
                var col = table.Heads[j];
                headrowCells[j] = new PdfPCell(new Phrase(col.DisplayName, PDFBuilder.DefaultFont))
                {
                    VerticalAlignment = col.VerticalAlignment,
                    HorizontalAlignment = col.HorizontalAlignment
                };
                headwidth[j] = col.Width;
            }
            pdfPTable.Rows.Add(new PdfPRow(headrowCells));

            pdfPTable.CompleteRow();
        }

        private void Pager(PdfPTable pdfPTable)
        {
            var writer = _pdfBuilder.writer;
            var Height = _pdfBuilder.Height;
            var document = _pdfBuilder.document;
            bool isfisrt = table.NextPageMarginPosition;
            var rowheight = pdfPTable.Rows[0].GetCells()[0].Height;
            var pageCount = (int)((Height - document.TopMargin - document.BottomMargin - table.Position.Y) / rowheight);
            float ylocal = Math.Abs(table.Position.Y - Height);
            var fillrow = pageCount + 1;
            while (true)
            {
                if (table.FillPage && pdfPTable.Rows.Count < fillrow)
                {
                    var count = fillrow - pdfPTable.Rows.Count;
                    if (count > 0)
                    {
                        FillTabel(count, pdfPTable);
                        pdfPTable.Complete = true;
                        pdfPTable.SetTotalWidth(headwidth);
                    }
                }
                pdfPTable.WriteSelectedRows(0, -1, 0, fillrow, table.Position.X, ylocal, writer.DirectContent, true);
                if (isfisrt)
                {
                    pageCount = (int)((Height - document.TopMargin - document.BottomMargin - 30) / rowheight);
                    fillrow = pageCount + 1;
                    var tmp = Height - document.TopMargin;
                    if (tmp > ylocal) ylocal = tmp;
                    isfisrt = false;
                }
                if (pdfPTable.Rows.Count > pageCount)
                {
                    pdfPTable.Rows.RemoveRange(1, pageCount);
                }
                else
                {
                    pdfPTable.Rows.RemoveRange(1, pdfPTable.Rows.Count - 1);
                }
                if (pdfPTable.Rows.Count <= 1) break;


                _pdfBuilder.NewPage();
            }
        }

        private void FillTabel(int count, PdfPTable pdfPTable)
        {
            for (int i = 0; i < count; i++)
            {
                AddEmptyRow(pdfPTable);
            }
        }

        private void AddEmptyRow(PdfPTable pdfPTable)
        {
            var columns = pdfPTable.NumberOfColumns;
            PdfPCell[] rowCells = new PdfPCell[columns];
            for (int j = 0; j < columns; j++)
            {
                rowCells[j] = new PdfPCell((new Phrase(" ", PDFBuilder.DefaultFont)))
                {
                    PaddingLeft = 4,
                    PaddingRight = 4,
                    UseBorderPadding = true
                };
            }
            pdfPTable.Rows.Add(new PdfPRow(rowCells));
            pdfPTable.CompleteRow();
        }
    }
}