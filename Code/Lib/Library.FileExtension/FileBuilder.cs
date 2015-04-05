using System;
using System.IO;
using Library.Annotations;
using Library.Draw;
using Library.IO;
using Library.Logic;
using PageSize = Library.Draw.PageSize;

namespace Library.FileExtension
{
    public abstract class FileBuilder : BaseLogic, IFileBuilder
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

            OnMessge("�����ļ�");
            BuildFile();
            OnMessge("�����ļ���Ϣ");
            BuilderFileInfo();
            OnMessge("�����ļ�ģ��");
            BuildFileTemplate();
            OnMessge("���ɔ���");
            BuildFileData();
            Save();
        }
    }
}