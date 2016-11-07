namespace NLog.Revicer
{

    public class LogViewConfig : PropertyChangeModel
    {

        private int _port= 4001;
        private int _buffSize=2048;

        public int Port
        {
            get { return _port; }
            set
            {
                if (_port == value) return;
                _port = value;
                this.OnPropertyChanged("Port");
            }
        }

        public string Logger { get; set; }
        public int Livel { get; set; }

        public bool IsFilterLivel { get; set; }

        public int BuffSize
        {
            get { return _buffSize; }
            set
            {
                if (_buffSize == value) return;
                _buffSize = value;
                this.OnPropertyChanged("BuffSize");
            }
        }
    }
}