using System;
using iTextSharp.text;
using Library.Draw;

namespace Library.FileExtension
{
    public static class FileBuilderHelper
    {
        public static object GetFileObject()
        {
            throw new NotImplementedException();
        }

        public static BaseColor ToBaseColor(this IToRGBColor color)
        {
            var reg = color.ToRGB();
            return new BaseColor(reg.R, reg.G, reg.B, reg.A);
        }
    }
}