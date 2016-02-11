using iTextSharp.text;
using iTextSharp.text.pdf;
using Library.Draw;
using System;
using System.Collections.Generic;
using System.IO;
using IPageSize = iTextSharp.text.PageSize;

namespace Library.FileExtension
{
    public class PDFBuilder : FileBuilder
    {
        static PDFBuilder()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "simkai.ttf");
            var defaultBaseFont = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            DefaultFont = new iTextSharp.text.Font(defaultBaseFont, 18);
        }

        internal static readonly iTextSharp.text.Font DefaultFont;
        protected internal Document document { get; private set; }
        private PdfReader readerFileTemplate;
        protected internal PdfWriter writer { get; private set; }
        protected internal float Height { get; private set; }
        protected internal float Width { get; private set; }

        protected PdfTemplate Template;

        protected override void BuildFile()
        {
            if (TemplateStream != null)
            {
                readerFileTemplate = new PdfReader(TemplateStream);
                document = new Document(readerFileTemplate.GetPageSize(1));
                writer = PdfWriter.GetInstance(document, BufferStream);
                document.Open();

                var page = writer.GetImportedPage(readerFileTemplate, 1);
                Template = page;
                writer.DirectContent.AddTemplate(page, 0, 0);
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

        private readonly IList<DataElement> elements = new List<DataElement>();

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

                    case "RectangleElement":
                        DrawRectangle(element as RectangleElement);
                        break;

                    case "TableElement":
                        DrawTable(element as TableElement);
                        break;
                }
            }
        }

        private void Drawimage(ImageElement image)
        {
        }

        private void DrawTable(TableElement table)
        {
            new PDFTableBuilder(this, table).Builder();
        }

        protected internal void NewPage()
        {
            document.NewPage();
            if (Template != null)
            {
                writer.DirectContent.AddTemplate(Template, 0, 0);
            }
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

            if (BufferStream != null) BufferStream.Close();
            if (readerFileTemplate != null) readerFileTemplate.Close();
            if (TemplateStream != null) TemplateStream.Close();
        }

        public PDFBuilder(Stream BufferStream)
            : base(BufferStream)
        {
        }
    }
}