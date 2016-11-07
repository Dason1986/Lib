namespace NLog.Revicer
{
    public class ReportLogContent : ReportLog
    {
        private string _content;

        public string Message
        {
            get { return _content; }
            set
            {
                if (_content == value) return;
                _content = value;
                this.OnPropertyChanged("Content");
            }
        }
    }
}