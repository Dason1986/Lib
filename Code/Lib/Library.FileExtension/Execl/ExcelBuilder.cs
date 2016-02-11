using System;
using System.IO;

namespace Library.FileExtension
{
    public class ExcelBuilder : FileBuilder
    {
        protected override void OnStart()
        {
            throw new NotImplementedException();
        }

        public ExcelBuilder(Stream BufferStream) : base(BufferStream)
        {
        }
    }
}