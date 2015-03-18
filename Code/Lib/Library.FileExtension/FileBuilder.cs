using System;
using System.IO;
using Library.Draw;
using Library.IO;
using Library.Logic;
using PageSize = Library.Draw.PageSize;

namespace Library.FileExtension
{
    public abstract class FileBuilder : BaseLogic, IFileBuilder
    {

        public string SaveFilePath { get; set; }
        public Stream BufferStream { get; protected set; }
        private PageSize _documentPageSize = PageSize.A4;
        private Margin _documentMargin = Margin.M5;
        public bool IsPageRotate { get; set; }
        public Margin DocumentMargin
        {
            get { return _documentMargin; }
            set { _documentMargin = value; }
        }

        public string FileTemplate { get; set; }
        public DocumentInfo DocumentInfo { get; set; }
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

        protected override void OnStart()
        {
            if (BufferStream == null) throw new Exception();

            OnMessge("生成文件");
            BuildFile();
            OnMessge("生成文件信息");
            BuilderFileInfo();
            OnMessge("生成文件模版");
            BuildFileTemplate();
            OnMessge("生成數據");
            BuildFileData();
            Save();
        }
    }
}