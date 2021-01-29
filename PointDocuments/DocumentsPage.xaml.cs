using System.Collections.Generic;
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

        HashSet<int> changesToConnections;
        public DocumentsPage(int id)
        {
            InitializeComponent();
            this.id = id;

            sources = new Dictionary<DataGrid, List<DocTable>>();
            changesToConnections = new HashSet<int>();
            DatabaseHandler.Initialize();
            var categories = DatabaseHandler.GetSortedCategories();//TestData.docTypes.Select(a => a.id).ToList();
            foreach (var category in categories)
            {
                CreateCategory(category);
            }
        }

        void CreateCategory(DocumentType docType)
        {
            Expander expander = new Expander();
            expander.Header = docType.Name;
            expander.Margin = new Thickness(0, 0, 0, 10);
            expander.Expanded += (object sender, RoutedEventArgs e) => PopulateTable(docType.id, expander);

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
            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(id, docType);
            if (docReal.Count > 0) {
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
            createWindow.ShowDialog();
        }
    }
}


