using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для DocumentsWindow.xaml
    /// </summary>
    public partial class DocumentsPage : Page
    {
        int id;

        Dictionary<DataGrid, List<DocTable>> sources;
        Dictionary<int, DataGrid> typeToTable;

        HashSet<int> changesToConnections;
        public DocumentsPage(int id)
        {
            InitializeComponent();
            this.id = id;

            sources = new Dictionary<DataGrid, List<DocTable>>();
            typeToTable = new Dictionary<int, DataGrid>();
            changesToConnections = new HashSet<int>();
            DatabaseHandler.Initialize();
            var categories = DatabaseHandler.GetSortedCategories();
            foreach (var category in categories)
            {
                CreateCategory(category);
            }
        }

        /// <summary>
        /// Creating empty expanders for all categories
        /// </summary>
        /// <param name="docType"></param>
        void CreateCategory(DocumentType docType)
        {
            Expander expander = new Expander();
            expander.Header = docType.Name;
            expander.Margin = new Thickness(0, 0, 0, 10);
            expander.Expanded += (object sender, RoutedEventArgs e) => PopulateTable(docType.id, expander);

            DocumentsPanel.Children.Add(expander);
        }

        void PopulateTable(int docType, Expander expander)
        {
            if (expander.Content != null)
            {
                return;
            }

            expander.Expanded -= (object sender, RoutedEventArgs e) => PopulateTable(docType, expander);            
            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(id, docType);

            DataGrid table = Util.CreateDatagrid(id);
            if (id != -1)
            {
                table.PreviewMouseUp += Table_MouseDown;
                table.MouseDown += Table_MouseDown;
            }
            table.MouseDoubleClick += Table_MouseDoubleClick;

            sources.Add(table, docReal);

            table.ItemsSource = docReal;
            expander.Content = table;
            typeToTable.Add(docType, table);
        }

        void UpdateTable(int docType)
        {
            if (typeToTable.ContainsKey(docType))
            {
                sources[typeToTable[docType]].Add(DatabaseHandler.GetLatestDocument(docType, id));
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
            if (table.CurrentCell.Column != null && table.CurrentCell.Column.DisplayIndex != 3)
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
            createWindow.Closing += CreateWindow_Closing; 
            createWindow.ShowDialog();
        }

        private void CreateWindow_Closing(object sender, CancelEventArgs e)
        {
            int type = (int)((DocumentCreateWindow)sender).DocTypeCombo.SelectedValue;
            UpdateTable(type);
            e.Cancel = false;
        }

        
    }
}


