using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Draw;
using Library.FileExtension;
using Library.IO;
using NUnit.Framework;
namespace TestPj.Test
{
    public class PdfTest
    {
        class MyClass
        {
            public string str1 { get; set; }
            public string str2{ get; set; }
            public string str3 { get; set; }

        }
        [Test]
        public void WrapperTest()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("str1");
            dt.Columns.Add("str2");
            dt.Columns.Add("str3");
            for (int i = 0; i < 14; i++)
            {
                dt.Rows.Add(new[] { null, "2在", "ff五" });
                dt.Rows.Add(new[] { "11", "22", "33" });
                dt.Rows.Add(new[] { "13", "23" });
                dt.Rows.Add(new[] { "14", "24大", "ff地" });
                dt.Rows.Add(new[] { null, "25" });
                dt.Rows.Add(new[] { "15", "25" });
                dt.Rows.Add(new[] { "15", "" });
            }
            //List<MyClass> list=new List<MyClass>();
            //for (int i = 0; i < 14; i++)
            //{
            //    list.Add(new MyClass { str1 = null, str2 = "2在", str3 = "ff五" });
            //    list.Add(new MyClass { str1 = "11", str2 = "22", str3 = "33" });
            //    list.Add(new MyClass { str1 = "13", str2 = "23" });
            //    list.Add(new MyClass { str1 = "14", str2 = "24大", str3 = "ff地" });
            //    list.Add(new MyClass { str1 = null, str2 = "25" });
            //    list.Add(new MyClass { str1 = "15", str2 = "25" });
            //    list.Add(new MyClass { str1 = "15", str2 = "" });
            //}
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = Path.Combine(desktop, DateTime.Now.ToString("yyyyMMdd") + ".pdf");
            var tmppath = Path.Combine(desktop, "tmp.pdf");
            FileStream fs = new FileStream(path, FileMode.Create);
            PDFBuilder pdfBuilder = new PDFBuilder(fs);
            pdfBuilder.Failure += (x, y) => { Console.WriteLine(y.Error); };
            pdfBuilder.DocumentInfo = new DocumentInfo() { Author = "dason", Title = "title" };
            if (File.Exists(tmppath))
            {
                pdfBuilder.FileTemplate = tmppath;
            }
        //    pdfBuilder.Add(new LineElement(new ElementPosition(20, 50), 400, 5) { Color = new RGBColor(5, 99, 10) });
            //    pdfBuilder.Add(new RectangleElement(new ElementPosition(100, 90), new ElementSize(100, 200)) { Border = 10, Color = new RGBColor(205, 199, 100) });
            //    pdfBuilder.Add(new LineElement(new ElementPosition(103, 90), 100, 2));
            pdfBuilder.Add(new TableElement(new ElementPosition(20, 150), dt)
            {
                Heads = new[]
               {
                   new TableHeadElement(){BindName = "str1",DisplayName = "中文【str】",Width = 120},
                   new TableHeadElement(){BindName = "str2",DisplayName = "中文【str2】",Width = 150},
               },
                FillRows = true,
              //  FillRowCounts = 100,
                FillPage = true,
                NextPageMarginPosition = true,
            });
            pdfBuilder.Start();
        }
    }
}
