using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace PointDocuments
{
    public static class Util
    {
        public static int Height = 450;
        public static int Width = 800;


        public static int maxTabs = 12;
        public static int panelHeight = 18;
        public static string x = "❌";
        public static Thickness zeroThickness = new Thickness(0, 0, 0, 0);
        public static Thickness rightThickness = new Thickness(0, 0, 5, 0);
        public static Thickness borderThickness = new Thickness(10, 10, 10, 10);
        public static int buttonSize = 15;
        public static int fontSize = 9;
    }

    public class CustomDataGridCheckBoxColumn : DataGridCheckBoxColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            CheckBox checkBox = base.GenerateEditingElement(cell, dataItem) as CheckBox;
            return checkBox;
        }
    }

    public class DocTable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _id;
        private string _name;
        private System.DateTime _date;
        private string _username;
        private bool _isConnected;
        public int id
        {
            get { return _id; }
            set
            {
                _id = value;
                this.NotifyPropertyChanged("Enabled");
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
                this.NotifyPropertyChanged("isCOnnected");
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
}
