using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using Library.Draw;
using Library.HelperUtility;
using IPageSize = iTextSharp.text.PageSize;

namespace Library.FileExtension
{

    public class PDFBuilder : FileBuilder
    {
        static PDFBuilder()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "simkai.ttf");
            var defaultBaseFont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            DefaultFont = new iTextSharp.text.Font(defaultBaseFont, 12);
        }

        private readonly static iTextSharp.text.Font DefaultFont;
        private Document document;

        private PdfWriter writer;
        protected float Height;
        protected float Width;
        protected PdfTemplate[] Templates;
        protected override void BuildFile()
        {
            if (!string.IsNullOrWhiteSpace(FileTemplate) && File.Exists(FileTemplate))
            {
                PdfReader reader = new PdfReader(FileTemplate);
                document = new Document(reader.GetPageSize(1));
                writer = PdfWriter.GetInstance(document, BufferStream);
                document.Open();
                Templates = new PdfTemplate[reader.NumberOfPages];
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    var page = writer.GetImportedPage(reader, 1);
                    Templates[i - 1] = page;
                    writer.DirectContent.AddTemplate(page, 0, 0);
                }
                reader.Close();
            }
            else
            {
                var size = IPageSize.GetRectangle(DocumentPageSize.ToString());
                if (IsPageRotate) size = size.Rotate();
                document = DocumentMargin == Margin.Empty ? new Document(size) :
                        new Document(size, DocumentMargin.Left, DocumentMargin.Right, DocumentMargin.Top, DocumentMargin.Bottom);
                writer = PdfWriter.GetInstance(document, BufferStream);
                document.Open();
                AddEmpty();
            }

            Height = document.PageSize.Height;
            Width = document.PageSize.Width;



        }
        readonly IList<DataElement> elements = new List<DataElement>();
        private void AddEmpty()
        {
            Paragraph paragraph = new Paragraph(" ");
            document.Add(paragraph);
        }
        public void Add(DataElement element)
        {
            elements.Add(element);
        }

        private void DrawLine(LineElement line)
        {
            var cb = writer.DirectContent;
            //  cb.SaveState();
            //  cb.Reset();
            var color = line.Color.ToBaseColor();
            cb.SetColorStroke(color);
            cb.SetLineWidth(line.LineWidth);

            var point = line.Position;
            var size = line.Size;

            cb.MoveTo(point.X, Math.Abs(point.Y - Height));
            cb.LineTo(size.Width + point.X, Math.Abs(size.Height - Height));
            cb.Stroke();

            Reset(cb);
            //cb.RestoreState();
        }

        protected override void BuildFileData()
        {
            foreach (var element in elements)
            {
                switch (element.GetType().Name)
                {
                    case "ImageElement":
                        Drawimage(element as ImageElement);
                        break;
                    case "LineElement":
                        DrawLine(element as LineElement);
                        break;
                    case "RectangleElement": DrawRectangle(element as RectangleElement);
                        break;
                    case "TableElement": DrawTable(element as TableElement);
                        break;

                }
            }
        }

        private void Drawimage(ImageElement image)
        {

        }
        private void DrawTable(TableElement table)
        {
            if (table.Heads == null) throw new Exception();

            var columns = table.Heads.Length;
            PdfPTable pdfPTable = new PdfPTable(columns);
            pdfPTable.LockedWidth = true;
            #region head

            var headwidth = new float[columns];


            PdfPCell[] headrowCells = new PdfPCell[columns];
            for (int j = 0; j < columns; j++)
            {
                var col = table.Heads[j];
                headrowCells[j] = new PdfPCell(new Phrase(col.DisplayName, DefaultFont))
                {
                    VerticalAlignment = col.VerticalAlignment,
                    HorizontalAlignment = col.HorizontalAlignment
                };
                headwidth[j] = col.Width;
            }
            pdfPTable.Rows.Add(new PdfPRow(headrowCells));

            pdfPTable.CompleteRow();

            #endregion

            pdfPTable.SkipFirstHeader = true;
            #region body data

            for (int i = 0; i < table.DataSource.Rows.Count; i++)
            {
                PdfPCell[] rowCells = new PdfPCell[columns];
                var row = table.DataSource.Rows[i];
                for (int j = 0; j < columns; j++)
                {
                    var col = table.Heads[j];
                    var cel = ObjectUtility.Cast<string>(row[col.BindName]);

                    rowCells[j] = new PdfPCell((new Phrase(string.IsNullOrEmpty(cel) ? " " : cel, DefaultFont)))
                    {
                        PaddingLeft = 4,
                        PaddingRight = 4,
                        UseBorderPadding = true
                    };

                }
                pdfPTable.Rows.Add(new PdfPRow(rowCells));
                pdfPTable.CompleteRow();
            }
            if (table.FillRows)
            {
                var count = table.FillRowCounts - table.DataSource.Rows.Count;
                if (count > 0)
                {
                    FillTabel(count, pdfPTable);
                }
            }
            #endregion

            pdfPTable.Complete = true;
            pdfPTable.SetTotalWidth(headwidth);
            if (pdfPTable.TotalHeight > Height)
            {
                bool isfisrt = table.NextPageMarginPosition;
                var rowheight = pdfPTable.TotalHeight / pdfPTable.Rows.Count;
                var pageCount = (int)((Height - document.TopMargin - document.BottomMargin) / rowheight);
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
                    document.NewPage();
                }
            }
            else
            {
                pdfPTable.WriteSelectedRows(0, -1, 0, -1, table.Position.X, Math.Abs(table.Position.Y - Height), writer.DirectContent, true);
            }


            writer.DirectContent.Stroke();
        }

        private static void FillTabel(int count, PdfPTable pdfPTable)
        {
            for (int i = 0; i < count; i++)
            {
                AddEmptyRow(pdfPTable);
            }
        }

        private static void AddEmptyRow(PdfPTable pdfPTable)
        {
            var columns = pdfPTable.NumberOfColumns;
            PdfPCell[] rowCells = new PdfPCell[columns];
            for (int j = 0; j < columns; j++)
            {
                rowCells[j] = new PdfPCell((new Phrase(" ", DefaultFont)))
                {
                    PaddingLeft = 4,
                    PaddingRight = 4,
                    UseBorderPadding = true
                };
            }
            pdfPTable.Rows.Add(new PdfPRow(rowCells));
            pdfPTable.CompleteRow();
        }

        private void DrawRectangle(RectangleElement box)
        {
            var cb = writer.DirectContent;
            var color = box.Color.ToBaseColor();
            cb.SetColorStroke(color);
            var lx = box.Position.X;
            var ly = Math.Abs(box.Position.Y - (Height - box.Size.Height));
            var ux = box.Size.Width;
            var uy = box.Size.Height;
            cb.SetLineWidth(box.Border);
            cb.Rectangle(lx, ly, ux, uy);
            cb.Stroke();
            Reset(cb);
        }

        private static void Reset(PdfContentByte cb)
        {
            cb.SetLineWidth(1);
            cb.ResetCMYKColorFill();
            cb.ResetCMYKColorStroke();
            cb.ResetGrayFill();
            cb.ResetGrayStroke();
            cb.ResetRGBColorFill();
            cb.ResetRGBColorStroke();
        }

        protected override void BuilderFileInfo()
        {
            if (DocumentInfo == null) return;
            document.AddAuthor(DocumentInfo.Author);
            document.AddKeywords(DocumentInfo.Keywords);
            document.AddSubject(DocumentInfo.Subject);
            document.AddTitle(DocumentInfo.Title);
        }

        protected override void BuildFileTemplate()
        {

        }

        protected override void Save()
        {
            //  writer.Close();
            document.Close();
            if (!string.IsNullOrEmpty(SaveFilePath))
            {
                var file = new FileStream(SaveFilePath, FileMode.Create);
                BufferStream.CopyTo(file);
                file.Close();
            }
            BufferStream.Close();
        }



        public PDFBuilder(Stream BufferStream)
            : base(BufferStream)
        {
        }
    }
}