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

            Point point = TestData.points.Where(a => a.id == id).First();
            PointName.Content = point.name;
            CategoryName.Content = TestData.categories.Where(a => a.id == point.catId).Select(a=>a.name).First();

            docPoint = TestData.pointDoc.Where(a => a.pointid == id).Select(a=>a.docid).ToHashSet();
            HashSet<int> categories = TestData.docs.Where(a => docPoint.Contains(a.id)).Select(a => a.doctypeid).ToHashSet();
            List<DocType> sortedCategories = TestData.docTypes.Where(a => categories.Contains(a.id)).OrderBy(a=>a.name).ToList();
            categoriesDictionary = new Dictionary<int, Expander>();

            foreach (var category in sortedCategories)
            {
                CreateCategory(category.id);
            }
        }

        void CreateCategory(int docType)
        {
            Expander expander = new Expander();
            expander.Header = TestData.docTypes.Where(a => a.id == docType).Select(a => a.name).First();
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

            List<Doc> docs = TestData.docs.Where(a => docPoint.Contains(a.id) && a.doctypeid == docType).ToList();
            //TODO only select nid, name, date and username!!!! no binaries
            List<DocTable> docReal = new List<DocTable>();
            for (int i = 0; i < docs.Count; i++)
            {
                var entry = TestData.docHistory
                           .Select((a) => new DocTable(a.docid, a.name, a.date, a.username))
                           .Where(a => a.id == docs[i].id)
                           .OrderByDescending(a => a.date)
                           .First();
                docReal.Add(entry);
                //docReal.Add(TestData.docHistory.Where(a => a.docid == docs[i].id).OrderByDescending(a => a.date).First());
            }

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
