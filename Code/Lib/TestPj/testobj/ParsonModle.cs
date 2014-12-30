using System;
using Library;

namespace TestPj
{
    public class ParsonModle : RevertibleChangeModel
    {
        private string _userName;
        private string _account;
        private int _age;
        private string _proxy;
     

        public string UserName
        {
            get { return _userName; }
            set
            {
                if (value == _userName) return;
                OnSaveBaseValue("UserName", _userName);
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        public string Account
        {
            get { return _account; }
            set
            {
                if (value == _account) return;
                OnSaveBaseValue("Account", _account);
                _account = value;
                OnPropertyChanged("Account");
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value == _age) return;
                OnSaveBaseValue("Age", _age);
                _age = value;
                OnPropertyChanged("Age");
            }
        }

        public string Proxy
        {
            get { return _proxy; }
            set
            {
                if (value == _proxy) return;
                _proxy = value;
                OnPropertyChanged("Proxy");
            }
        }
    }

    [Flags]
    public enum ParsonType
    {

        None,
        Amdin = 1,
        A1 = 2,
        A2 = 4
    }
}