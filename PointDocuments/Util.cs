using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        public static DataGrid CreateDatagrid(int id)
        {
            DataGrid table = new DataGrid();
            table.AutoGenerateColumns = false;
            table.CanUserAddRows = false;
            table.CanUserDeleteRows = false;
            table.CanUserSortColumns = true;
            table.SelectionMode = DataGridSelectionMode.Single;
            table.SelectionUnit = DataGridSelectionUnit.FullRow;
            table.MaxHeight = 500;
            table.IsReadOnly = true;

            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "Название файла";
            textColumn.Binding = new Binding("name");
            table.Columns.Add(textColumn);


            textColumn = new DataGridTextColumn();
            textColumn.Header = "Дата изменения";
            textColumn.Binding = new Binding("date");
            table.Columns.Add(textColumn);


            textColumn = new DataGridTextColumn();
            textColumn.Header = "Пользователь";
            textColumn.Binding = new Binding("username");
            table.Columns.Add(textColumn);

            if (id != -1)
            {
                CustomDataGridCheckBoxColumn checkBoxColumn = new CustomDataGridCheckBoxColumn();
                checkBoxColumn.Header = "Отностится к точке";

                Binding binding = new Binding("isConnected");
                checkBoxColumn.Binding = binding;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                binding.Mode = BindingMode.TwoWay;
                checkBoxColumn.IsReadOnly = true;
                table.Columns.Add(checkBoxColumn);
            }

            return table;
        }
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

    public partial class Document
    {
        public Document(string name, int doctype) 
        {
            Name = name;
            DocType = doctype;
        }
    }
    public partial class DocumentHistory
    {
        public DocumentHistory(int docID, byte[] file, System.DateTime date, string userName)
        {
            DocumentID = docID;
            DocumentBinary = file;
            Date = date;
            UserName = userName;
        }
    }
    public partial class Point
    {
        public Point(string name, int type)
        {
            Name = name;
            CategoryID = type;
        }
    }
    public partial class PointType
    {
        public PointType(string name)
        {
            Name = name;
        }
    }
    public partial class DocumentType
    {
        public DocumentType(string name)
        {
            Name = name;
        }
    }

    public partial class PointDocConnection
    {
        public PointDocConnection(int pointID, int docID)
        {
            PointID = pointID;
            DocumentID = docID;
        }
    }
}
