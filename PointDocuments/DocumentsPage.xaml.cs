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
        int id = -1;

        Dictionary<DataGrid, List<DocTable>> sources;
        Dictionary<int, DataGrid> typeToTable;
        Dictionary<DataGrid,int> tableToType;

        HashSet<int> changesToConnections;
        Dictionary<int, Expander> typeToExpander;

        bool canDelete;
        bool canInsert;

        public Add updaterOfPoints;
        public DocumentsPage()
        {
            InitializeComponent();
            InitializeWindow();
        }

        public DocumentsPage(int id,Window owner)
        {
            InitializeComponent();
            this.id = id;
            owner.Closing += DocumentsPage_Closing;
            canDelete = DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.DELETE);
            canInsert = DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.INSERT);
            InitializeWindow();
        }

        void InitializeWindow()
        {
            sources = new Dictionary<DataGrid, List<DocTable>>();
            typeToTable = new Dictionary<int, DataGrid>();
            tableToType = new Dictionary<DataGrid, int>();
            changesToConnections = new HashSet<int>();
            typeToExpander = new Dictionary<int, Expander>();
            //TODO CHANGE SOMETHING
            DatabaseHandler.Initialize();
            //Getting sorted categories and then creating expanders for those categories
            var categories = DatabaseHandler.GetSortedCategories();
            foreach (var category in categories)
            {
                CreateCategory(category);
            }

            if (!DatabaseHandler.userRole.Documents.HasFlag(Permissions.INSERT))
            {
                AddDocument.Visibility = Visibility.Collapsed;
            }
        }

        public void UpdateDocumentType()
        {
            var categories = DatabaseHandler.GetSortedCategories();
            HashSet<int> cat = new HashSet<int>();
            for(int i = 0; i < categories.Count; i++)
            {
                if (typeToExpander.ContainsKey(categories[i].id))
                {
                    DocumentsPanel.Children.Remove(typeToExpander[categories[i].id]);
                    DocumentsPanel.Children.Add(typeToExpander[categories[i].id]);
                }
                else
                {
                    CreateCategory(categories[i]);
                }
                cat.Add(categories[i].id);
            }
            HashSet<int> removeKeys = new HashSet<int>();
            foreach(var expandery in typeToExpander)
            {
                if (!cat.Contains(expandery.Key))
                {
                    if (typeToTable.ContainsKey(expandery.Key)) 
                    {
                        var table = typeToTable[expandery.Key];
                        typeToTable.Remove(expandery.Key);
                        tableToType.Remove(table);
                        sources[table] = null;
                        sources.Remove(table);
                    }
                    DocumentsPanel.Children.Remove(typeToExpander[expandery.Key]);
                    removeKeys.Add(expandery.Key);
                }
            }

            foreach(var removeKey in removeKeys)
            {
                typeToExpander.Remove(removeKey);
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
            typeToExpander.Add(docType.id, expander);
        }

        void PopulateTable(int docType, Expander expander)
        {
            if (expander.Content != null)
            {
                return;
            }

            expander.Expanded -= (object sender, RoutedEventArgs e) => PopulateTable(docType, expander);            
            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(id, docType);

            docReal.AddIndexes();

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
            tableToType.Add(table, docType);
        }

        void UpdateTable(int docType)
        {
            if (typeToTable.ContainsKey(docType))
            {
                sources[typeToTable[docType]].Add(DatabaseHandler.GetLatestDocument(docType, id));
                sources[typeToTable[docType]].AddIndexes();
                typeToTable[docType].Items.Refresh();
            }
        }


        private void Table_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGrid table = (DataGrid)sender;
            if (table.SelectedIndex != -1 && table.CurrentColumn != null && table.CurrentColumn.DisplayIndex == 4)
            {
                sources[table][table.SelectedIndex].isConnected = !sources[table][table.SelectedIndex].isConnected;

                if (changesToConnections.Contains(sources[table][table.SelectedIndex].id))
                {
                    changesToConnections.Remove(sources[table][table.SelectedIndex].id);
                }
                else
                {
                    if (sources[table][table.SelectedIndex].isConnected && canDelete ||
                        !sources[table][table.SelectedIndex].isConnected && canInsert)
                    {
                        changesToConnections.Add(sources[table][table.SelectedIndex].id);
                    }
                }
            }
        }

        /// <summary>
        /// Editing table row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Table_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid table = (DataGrid)sender;
            if (table.SelectedIndex !=-1 && table.CurrentCell.Column != null && table.CurrentCell.Column.DisplayIndex != 3)
            {
                DocumentEditWindow editWindow = new DocumentEditWindow(sources[table][table.SelectedIndex].id);
                int originalType = tableToType[table];
                editWindow.Owner = Window.GetWindow(this);
                editWindow.Closing += (object ss, CancelEventArgs ex) =>
                {
                    if (!ex.Cancel)
                    {
                        EditWindow_Closing(ss, table, table.SelectedIndex, originalType);
                        updaterOfPoints?.Invoke();
                    }
                };
                editWindow.ShowInTaskbar = false;
                editWindow.ShowDialog();
            }
        }

        private void EditWindow_Closing(object sender, DataGrid originalTable, int selectedIndex, int originalType)
        {
            DocumentEditWindow editWindow = (DocumentEditWindow)sender;

            DocTable item = sources[originalTable][selectedIndex];
            int newType = (int)editWindow.DocTypeCombo.SelectedValue;
            if (newType != originalType)
            {
                sources[originalTable].RemoveAt(selectedIndex);
                sources[originalTable].AddIndexes();
                originalTable.Items.Refresh();
                if (typeToTable.ContainsKey(newType))
                {
                    sources[typeToTable[newType]].Add(item);
                    sources[typeToTable[newType]].AddIndexes();
                    typeToTable[newType].Items.Refresh();
                }
                else
                {
                    //if there's no table generated then there is no need to add somewhere
                    return;
                }
            }
            item.name = editWindow.DocumentName.Text;
            item.date = DatabaseHandler.GetDocumentDate(item.id);


            //TODO UPDATE POINTS
        }

        private void AddDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            DocumentCreateWindow createWindow = new DocumentCreateWindow();
            createWindow.Owner = Window.GetWindow(this);
            createWindow.Closing += CreateWindow_Closing;
            createWindow.ShowInTaskbar = false;
            createWindow.ShowDialog();
        }

        private void CreateWindow_Closing(object sender, CancelEventArgs e)
        {
            if (((DocumentCreateWindow)sender).SavedDocument.IsChecked == true)
            {
                int type = (int)((DocumentCreateWindow)sender).DocTypeCombo.SelectedValue;
                UpdateTable(type);
                e.Cancel = false;
            }
        }

        private void DocumentsPage_Closing(object sender, CancelEventArgs e)
        {
            DatabaseHandler.ChangePointDocConnection(id, changesToConnections);
            e.Cancel = false;

        }
    }
}