using Library.Annotations;
using Library.ComponentModel.Logic;
using Library.Draw;
using Library.IO; 
using System;
using System.IO;
using PageSize = Library.Draw.PageSize;

namespace Library.FileExtension
{
    public abstract class FileBuilder :  IFileBuilder
    {
        public Stream BufferStream { get; protected set; }
        private PageSize _documentPageSize = PageSize.A4;
        private Margin _documentMargin = Margin.M5;
        public bool IsPageRotate { get; set; }

        public Margin DocumentMargin
        {
            get { return _documentMargin; }
            set { _documentMargin = value; }
        }

        protected Stream TemplateStream { get; private set; }

        public void SetTemplate(string path)
        {
            if (!File.Exists(path)) throw new LibException();
            TemplateStream = new MemoryStream(File.ReadAllBytes(path));
        }

        public void SetTemplate([NotNull] Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            TemplateStream = stream;
        }

        public IDocumentInfo DocumentInfo { get; set; }

        public PageSize DocumentPageSize
        {
            get { return _documentPageSize; }
            set { _documentPageSize = value; }
        }

        protected FileBuilder(Stream BufferStream)
        {
            this.BufferStream = BufferStream;
        }

        protected virtual void Save()
        {
        }

        protected virtual void BuilderFileInfo()
        {
        }

        protected virtual void BuildFile()
        {
        }

        protected virtual void BuildFileData()
        {
        }

        protected virtual void BuildFileTemplate()
        {
        }

        protected virtual void OnStart()
        {
            if (BufferStream == null) throw new Exception();

          //  OnMessge("生成文件");
            BuildFile();
          //  OnMessge("生成文件信息");
            BuilderFileInfo();
         //   OnMessge("生成文件模版");
            BuildFileTemplate();
         //   OnMessge("生成數據");
            BuildFileData();
            Save();
        }
    }
}