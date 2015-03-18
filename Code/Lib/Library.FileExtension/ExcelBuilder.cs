using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Library.IO;

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
