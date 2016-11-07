using System;

namespace NLog.Revicer
{
    public sealed class HTime: PropertyChangeModel
    {
        private long _h0;
        private long _h1;
        private long _h2;
        private long _h3;
        private long _h4;
        private long _h5;
        private long _h6;
        private long _h7;
        private long _h8;
        private long _h9;
        private long _h10;
        private long _h11;
        private long _h12;
        private long _h13;
        private long _h14;
        private long _h15;
        private long _h16;
        private long _h17;
        private long _h18;
        private long _h19;
        private long _h20;
        private long _h21;
        private long _h22;
        private long _h23;

        public void AddTime(DateTime time)
        {
            switch (time.Hour)
            {
                case 0: H0++; break;
                case 1: H1++; break;
                case 2: H2++; break;
                case 3: H3++; break;
                case 4: H4++; break;
                case 5: H5++; break;
                case 6: H6++; break;
                case 7: H7++; break;
                case 8: H8++; break;
                case 9: H9++; break;
                case 10: H10++; break;
                case 11: H11++; break;
                case 12: H12++; break;
                case 13: H13++; break;
                case 14: H14++; break;
                case 15: H15++; break;
                case 16: H16++; break;
                case 17: H17++; break;
                case 18: H18++; break;
                case 19: H19++; break;
                case 20: H20++; break;
                case 21: H21++; break;
                case 22: H22++; break;
                case 23: H23++; break;
                default:
                    break;
            }
        }
        public long H0
        {
            get { return _h0; }
            set
            {
                if (_h0 == value) return;
                _h0 = value;
                this.OnPropertyChanged("H0");
            }
        }
        public long H1
        {
            get { return _h1; }
            set
            {
                if (_h1 == value) return;
                _h1 = value;
                this.OnPropertyChanged("H1");
            }
        }
        public long H2
        {
            get { return _h2; }
            set
            {
                if (_h2 == value) return;
                _h2 = value;
                this.OnPropertyChanged("H2");
            }
        }
        public long H3
        {
            get { return _h3; }
            set
            {
                if (_h3 == value) return;
                _h3 = value;
                this.OnPropertyChanged("H3");
            }
        }
        public long H4
        {
            get { return _h4; }
            set
            {
                if (_h4 == value) return;
                _h4 = value;
                this.OnPropertyChanged("H4");
            }
        }
        public long H5
        {
            get { return _h5; }
            set
            {
                if (_h5 == value) return;
                _h5 = value;
                this.OnPropertyChanged("H5");
            }
        }
        public long H6
        {
            get { return _h6; }
            set
            {
                if (_h6 == value) return;
                _h6 = value;
                this.OnPropertyChanged("H6");
            }
        }
        public long H7
        {
            get { return _h7; }
            set
            {
                if (_h7 == value) return;
                _h7 = value;
                this.OnPropertyChanged("H7");
            }
        }
        public long H8
        {
            get { return _h8; }
            set
            {
                if (_h8== value) return;
                _h8= value;
                this.OnPropertyChanged("H8");
            }
        }
        public long H9
        {
            get { return _h9; }
            set
            {
                if (_h9 == value) return;
                _h9 = value;
                this.OnPropertyChanged("H9");
            }
        }
        public long H10
        {
            get { return _h10; }
            set
            {
                if (_h10 == value) return;
                _h10 = value;
                this.OnPropertyChanged("H10");
            }
        }
        public long H11
        {
            get { return _h11; }
            set
            {
                if (_h11 == value) return;
                _h11 = value;
                this.OnPropertyChanged("H11");
            }
        }
        public long H12
        {
            get { return _h12; }
            set
            {
                if (_h12 == value) return;
                _h12 = value;
                this.OnPropertyChanged("H12");
            }
        }
        public long H13
        {
            get { return _h13; }
            set
            {
                if (_h13 == value) return;
                _h13 = value;
                this.OnPropertyChanged("H13");
            }
        }
        public long H14
        {
            get { return _h14; }
            set
            {
                if (_h14 == value) return;
                _h14 = value;
                this.OnPropertyChanged("H14");
            }
        }
        public long H15
        {
            get { return _h15; }
            set
            {
                if (_h15 == value) return;
                _h15= value;
                this.OnPropertyChanged("H15");
            }
        }
        public long H16
        {
            get { return _h16; }
            set
            {
                if (_h16 == value) return;
                _h16 = value;
                this.OnPropertyChanged("H16");
            }
        }
        public long H17
        {
            get { return _h17; }
            set
            {
                if (_h17== value) return;
                _h17 = value;
                this.OnPropertyChanged("H17");
            }
        }
        public long H18
        {
            get { return _h18; }
            set
            {
                if (_h18 == value) return;
                _h18 = value;
                this.OnPropertyChanged("H18");
            }
        }
        public long H19
        {
            get { return _h19; }
            set
            {
                if (_h19 == value) return;
                _h19= value;
                this.OnPropertyChanged("H19");
            }
        }
        public long H20
        {
            get { return _h20; }
            set
            {
                if (_h20 == value) return;
                _h20 = value;
                this.OnPropertyChanged("H20");
            }
        }
        public long H21
        {
            get { return _h21; }
            set
            {
                if (_h21 == value) return;
                _h21 = value;
                this.OnPropertyChanged("H21");
            }
        }
        public long H22
        {
            get { return _h22; }
            set
            {
                if (_h22 == value) return;
                _h22 = value;
                this.OnPropertyChanged("H22");
            }
        }
        public long H23
        {
            get { return _h23; }
            set
            {
                if (_h23 == value) return;
                _h23 = value;
                this.OnPropertyChanged("H23");
            }
        }
    }
}