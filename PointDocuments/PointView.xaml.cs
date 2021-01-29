using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для PointView.xaml
    /// </summary>
    public partial class PointView : Page
    {

        int id;
        HashSet<int> docPoint;
        Dictionary<int,Expander> categoriesDictionary;
        public PointView(int id)
        {
            InitializeComponent();

            this.id = id;

            Point point = DatabaseHandler.GetPoint(id);
            PointName.Content = point.Name;
            CategoryName.Content = DatabaseHandler.GetPointCategory(point.id);

            docPoint = DatabaseHandler.GetDocumentsID(id);
            List<DocumentType> sortedCategories = DatabaseHandler.GetSortedCategories(id, docPoint);
            categoriesDictionary = new Dictionary<int, Expander>();

            foreach (var category in sortedCategories)
            {
                CreateCategory(category.id, category.Name);
            }
        }

        void CreateCategory(int docType, string name)
        {
            Expander expander = new Expander();
            expander.Header = name;
            expander.Margin = new Thickness(0, 0, 0, 10);
            expander.Expanded += (object sender, RoutedEventArgs e) =>PopulateTable(docType,expander);
            categoriesDictionary.Add(docType, expander);
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

            expander.Expanded -= (object sender, RoutedEventArgs e) => PopulateTable(docType,expander);

            //TODO only select nid, name, date and username!!!! no binaries
            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(docPoint, docType);

            DataGrid table = new DataGrid();
            table.SelectionMode = DataGridSelectionMode.Single;
            table.SelectionUnit = DataGridSelectionUnit.FullRow;
            table.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Table_MouseDoubleClick(docType, docReal[table.SelectedIndex].id);
            table.AutoGenerateColumns = false;
            table.CanUserAddRows = false;
            table.CanUserDeleteRows = false;
            table.CanUserSortColumns = true;
            table.MaxHeight = 500;

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

            table.ItemsSource = docReal;

            expander.Content = table;
        }

        private void Table_MouseDoubleClick(int docType, int selectedIndex)
        {
            DocumentEditWindow editWindow = new DocumentEditWindow(selectedIndex);
            editWindow.Owner = Window.GetWindow(this);
            editWindow.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.Owner = Window.GetWindow(this);
            window.Title = "Добавить документы";
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Height = Util.Height;
            window.Width = Util.Width;


            Frame frame = new Frame();
            frame.Margin = Util.borderThickness;
            var page = new DocumentsPage(id);
            frame.Content = page;
            window.Content = frame;
            window.Owner = Window.GetWindow(this);
            window.ShowDialog();
        }

        void UpdateCategories()
        {
            //TODO SHOULD RECIEVE CHANGED CATEGORIES IDS
        }
    }
}
