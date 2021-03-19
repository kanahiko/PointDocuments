using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointDocuments
{
    public class TabLabel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _tabName;
        public string tabName
        {
            get { return _tabName; }
            set
            {
                _tabName = value;
                this.NotifyPropertyChanged("tabName");
            }
        }
        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public class DocTable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private int _number;
        private string _name;
        private System.DateTime _date;
        private string _username;
        private bool _isConnected;

        public string downloadButtonName
        {   
            get { return _id != -1 ? "Скачать документ" : "Открыть в проводнике"; }
        }

        public System.Windows.Visibility restoreButtonVisibility
        {
            get { return _id != -1 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed; }
        }
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public int number
        {
            get { return _number; }
            set
            {
                _number = value;
                this.NotifyPropertyChanged("number");
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
            }
        }
        public System.DateTime date
        {
            get { return _date; }
            set
            {
                _date = value;
                this.NotifyPropertyChanged("date");
            }
        }
        public string username
        {
            get { return _username; }
            set
            {
                _username = value;
                this.NotifyPropertyChanged("username");
            }
        }
        public bool isConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                this.NotifyPropertyChanged("isConnected");
            }
        }
        public DocTable()
        {

        }
        public DocTable(int i, string a, System.DateTime b, string c)
        {
            id = i;
            name = a;
            date = b;
            username = c;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    public class PointTable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private int _number;
        private string _name;
        private string _type;
        private int _typeID;
        private bool _isConnected;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public int number
        {
            get { return _number; }
            set
            {
                _number = value;
                this.NotifyPropertyChanged("number");
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
            }
        }
        public string type
        {
            get { return _type; }
            set
            {
                _type = value;
                this.NotifyPropertyChanged("type");
            }
        }
        public bool isConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                this.NotifyPropertyChanged("isConnected");
            }
        }
        public int typeID
        {
            get { return _typeID; }
            set
            {
                _typeID = value;
            }
        }
        public PointTable()
        {

        }
        public PointTable(int i, string a, string b, int j)
        {
            id = i;
            name = a;
            type = b;
            typeID = j;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
    public class TypeTable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private int _number;
        private string _name;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }
        public int number
        {
            get { return _number; }
            set
            {
                _number = value;
                this.NotifyPropertyChanged("number");
            }
        }
        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.NotifyPropertyChanged("name");
            }
        }
        public TypeTable()
        {

        }
        public TypeTable(int i, string a)
        {
            id = i;
            name = a;
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

}
