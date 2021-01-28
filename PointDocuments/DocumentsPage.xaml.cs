using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для DocumentsWindow.xaml
    /// </summary>
    public partial class DocumentsPage : Page
    {
        int id;

        Dictionary<DataGrid, List<DocTable>> sources;

        HashSet<int> changesToConnections;
        public DocumentsPage(int id)
        {
            InitializeComponent();
            this.id = id;

            sources = new Dictionary<DataGrid, List<DocTable>>();
            changesToConnections = new HashSet<int>();
            var categories = TestData.docTypes.Select(a => a.id).ToList();
            foreach (var category in categories)
            {
                CreateCategory(category);
            }
        }

        void CreateCategory(int docType)
        {
            Expander expander = new Expander();
            expander.Header = TestData.docTypes.Where(a => a.id == docType).Select(a => a.name).First();
            expander.Margin = new Thickness(0, 0, 0, 10);
            expander.Expanded += (object sender, RoutedEventArgs e) => PopulateTable(docType, expander);

            //PopulateTable(docType, expander);
            DocumentsPanel.Children.Add(expander);

            //TestDataGrid.ItemsSource = docReal;
        }

        void PopulateTable(int docType, Expander expander)
        {
            if (expander.Content != null)
            {
                return;
            }

            expander.Expanded -= (object sender, RoutedEventArgs e) => PopulateTable(docType, expander);

            List<Doc> docs = TestData.docs.Where(a => a.doctypeid == docType).ToList();
            if (docs.Count > 0)
            {
                //TODO only select nid, name, date and username!!!! no binaries
                List<DocTable> docReal = new List<DocTable>();
                for (int i = 0; i < docs.Count; i++)
                {
                   var entry =TestData.docHistory
                        .Select((a) =>  new DocTable(a.docid,a.name, a.date, a.username))
                        .Where(a => a.id == docs[i].id)
                        .OrderByDescending(a => a.date)
                        .First();

                    if (id != -1)
                    {
                        entry.isConnected = TestData.pointDoc.Where(a => a.pointid == id && a.docid == entry.id).Any();
                    }
                    docReal.Add(entry);
                }

                DataGrid table = new DataGrid();
                table.AutoGenerateColumns = false;
                table.CanUserAddRows = false;
                table.CanUserDeleteRows = false;
                table.CanUserSortColumns = true;                
                table.SelectionMode = DataGridSelectionMode.Single;
                table.SelectionUnit = DataGridSelectionUnit.FullRow;
                table.MaxHeight = 500;
                table.IsReadOnly = true;
                if (id != -1)
                {
                    table.PreviewMouseUp += Table_MouseDown;
                    table.MouseDown += Table_MouseDown;
                }
                table.MouseDoubleClick += Table_MouseDoubleClick;

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

                sources.Add(table, docReal);

                table.ItemsSource = docReal;
                expander.Content = table;
            }

        }


        private void Table_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid table = (DataGrid)sender;
            if (table.CurrentColumn != null && table.CurrentColumn.DisplayIndex == 3)
            {
                sources[table][table.SelectedIndex].isConnected = !sources[table][table.SelectedIndex].isConnected;

                if (changesToConnections.Contains(sources[table][table.SelectedIndex].id))
                {
                    changesToConnections.Remove(sources[table][table.SelectedIndex].id);
                }
                else
                {
                    changesToConnections.Add(sources[table][table.SelectedIndex].id);
                }
            }
        }

        private void Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid table = (DataGrid)sender;
            if (table.CurrentCell.Column.DisplayIndex != 3)
            {
                DocumentEditWindow editWindow = new DocumentEditWindow(sources[table][table.SelectedIndex].id);
                editWindow.Owner = Window.GetWindow(this);
                editWindow.ShowDialog();
                //throw new NotImplementedException();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DocumentCreateWindow createWindow = new DocumentCreateWindow(); 
            createWindow.Owner = Window.GetWindow(this);
            createWindow.ShowDialog();
        }
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

    public int _id;
    public string _name;
    public string _date;
    public string _username;
    public bool _isConnected;
    public int id { get { return _id; }
        set
        {
            _id = value;
            this.NotifyPropertyChanged("Enabled");
        }
    }
    public string name { get { return _name; }
        set
        {
            _name = value;
            this.NotifyPropertyChanged("name");
        }
    }
    public string date { get { return _date; }
        set
        {
            _date = value;
            this.NotifyPropertyChanged("date");
        }
    }
    public string username { get { return _username; }
        set
        {
            _username = value;
            this.NotifyPropertyChanged("username");
        }
    }
    public bool isConnected { get { return _isConnected; }
        set
        {
            _isConnected = value;
            this.NotifyPropertyChanged("isCOnnected");
        }
    }

    public DocTable(int i,string a,string b,string c)
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
