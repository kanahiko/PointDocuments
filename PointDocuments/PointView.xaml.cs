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
    public partial class PointView : Page, IDisposable
    {

        int id;
        HashSet<int> docPoint;
        Dictionary<int, DataGrid> typeToTable;
        Dictionary<int,Expander> categories;

        public List<PointType> pointTypes;
        public Update updater;

        public TabLabel tabLabel;

        string origName;
        int origType;

        bool isDirty;
        public PointView(int id)
        {
            InitializeComponent();

            this.id = id;


            typeToTable = new Dictionary<int, DataGrid>();
            categories = new Dictionary<int, Expander>();

            pointTypes = DatabaseHandler.GetPointTypes();
            CategoryCombo.ItemsSource = pointTypes;

            Point point = DatabaseHandler.GetPoint(id);
            origName = point.Name;
            origType = point.CategoryID;
            PointName.Text = point.Name;
            tabLabel = new TabLabel();
            tabLabel.tabName = point.Name;
            CategoryCombo.SelectedValue = point.CategoryID;
            //CategoryName.Content = DatabaseHandler.GetPointCategory(point.id);

            docPoint = DatabaseHandler.GetDocumentsID(id);
            List<DocumentType> sortedCategories = DatabaseHandler.GetSortedCategories(id, docPoint);

            foreach (var category in sortedCategories)
            {
                CreateCategory(category.id, category.Name);
            }

            if (!DatabaseHandler.userRole.Points.HasFlag(Permissions.UPDATE))
            {
                CategoryCombo.IsEditable = false;
                CategoryCombo.IsEnabled = false;
                PointName.IsEnabled = false;
                SavePointButton.Visibility = Visibility.Collapsed;
                CancelPointButton.Visibility = Visibility.Collapsed;

            }

            if (!DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.INSERT) &&
                !DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.DELETE))
            {
                AddDocumentButton.Visibility = Visibility.Collapsed;
            }

            isDirty = false;
        }

        public void UpdatePointTypes()
        {
            int selectedIndex = CategoryCombo.SelectedIndex;
            CategoryCombo.Items.Refresh();
            CategoryCombo.SelectedIndex = -1;
            CategoryCombo.SelectedIndex = selectedIndex;
        }

        void CreateCategory(int docType, string name)
        {
            Expander expander = new Expander();
            expander.Header = name;
            expander.Margin = new Thickness(0, 0, 0, 10);
            expander.Expanded += (object sender, RoutedEventArgs e) =>PopulateTable(docType,expander);
            //categoriesDictionary.Add(docType, expander);
            categories.Add(docType,expander);
            DocumentsPanel.Children.Add(expander);
        }

        void PopulateTable(int docType, Expander expander)
        {
            if (expander.Content != null)
            {
                return;
            }

            expander.Expanded -= (object sender, RoutedEventArgs e) => PopulateTable(docType,expander);

            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(docPoint, docType);
            DataGrid table = Util.CreateDatagrid();

            docReal.AddIndexes();

            table.ItemsSource = docReal;
            table.MouseDoubleClick += (object sender, MouseButtonEventArgs e) => Table_MouseDoubleClick(docType, (DocTable)table.CurrentItem);

            expander.Content = table;
            typeToTable.Add(docType, table);
        }

        void UpdateTable(int docType)
        {
            List<DocTable> docReal = DatabaseHandler.GetDocTableDocuments(docPoint, docType);
            docReal.AddIndexes();
            typeToTable[docType].ItemsSource = docReal;
        }

        private void Table_MouseDoubleClick(int docType, DocTable item)
        {
            if (item == null)
            {
                return;
            }
            DocumentEditWindow editWindow = new DocumentEditWindow(item.id);
            editWindow.Owner = Window.GetWindow(this);
            editWindow.ShowInTaskbar = false;
            editWindow.Closing += (sender, e) => { if (!e.Cancel) { UpdateCategories(); } };
            editWindow.ShowDialog();
        }

        private void AddDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.Owner = Window.GetWindow(this);
            window.Title = "Добавить документы";
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Height = Util.Height;
            window.Width = Util.Width;


            Frame frame = new Frame();
            frame.Margin = Util.borderThickness;
            var page = new DocumentsPage(id, window);
            frame.Content = page;
            window.Content = frame;
            window.Owner = Window.GetWindow(this);
            window.Closing += AddDocumentWindow_Closing;
            window.ShowInTaskbar = false;
            window.ShowDialog();

        }

        private void AddDocumentWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            UpdateCategories();
            e.Cancel = false;
        }

        public void UpdateCategories()
        {
            System.Diagnostics.Debug.WriteLine("ddddasdasdasddd");
            docPoint = DatabaseHandler.GetDocumentsID(id);
            List<DocumentType> sortedCategories = DatabaseHandler.GetSortedCategories(id, docPoint);
            foreach (var category in sortedCategories)
            {
                if (categories.ContainsKey(category.id))
                {
                    if (typeToTable.ContainsKey(category.id))
                    {
                        UpdateTable(category.id);
                    }
                }
                else
                {
                    CreateCategory(category.id, category.Name);
                }
            }
            SortCategories(sortedCategories);
        }

        //REMOVING UNUSED CATEGORIES AND SORTING THEM IN ALPHABET ORDER
        void SortCategories(List<DocumentType> sortedCategories)
        {
            HashSet<int> usedCategories = new HashSet<int>();
            HashSet<int> unusedCategories = new HashSet<int>();
            foreach (var category in sortedCategories)
            {
                usedCategories.Add(category.id);
                DocumentsPanel.Children.Remove(categories[category.id]);
                DocumentsPanel.Children.Add(categories[category.id]);
            }

            foreach(var category in categories)
            {
                if (!usedCategories.Contains(category.Key))
                {
                    DocumentsPanel.Children.Remove(category.Value);
                    unusedCategories.Add(category.Key);
                }
            }

            foreach(var unused in unusedCategories)
            {
                if (typeToTable.ContainsKey(unused))
                {
                    typeToTable.Remove(unused);
                }
                categories.Remove(unused);
            }
        }
        //POINT EDITING
        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryCombo.SelectedIndex != -1)
            {
                if ((int)CategoryCombo.SelectedValue == origType)
                {
                    isDirty = PointName.Text != origName;
                    if (!isDirty)
                    {
                        SavePointButton.IsEnabled = false;
                        CancelPointButton.IsEnabled = false;
                    }
                    return;
                }
                isDirty = true;
                CancelPointButton.IsEnabled = true;
                SavePointButton.IsEnabled = PointName.Text.Length > 0;
            }
        }

        private void PointName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
                if (PointName.Text == origName)
                {
                    isDirty = (int)CategoryCombo.SelectedValue != origType;
                    if (!isDirty)
                    {
                        SavePointButton.IsEnabled = false;
                        CancelPointButton.IsEnabled = false;
                    }
                    return;
                }
                isDirty = true;
                CancelPointButton.IsEnabled = true;
                if (PointName.Text.Length > 0)
                {
                    NameError.Visibility = Visibility.Hidden;
                    SavePointButton.IsEnabled = true;
                }
                else
                {
                    NameError.Visibility = Visibility.Visible;
                    SavePointButton.IsEnabled = false;
                }
            }
        }

        private void PointName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PointName.Text == origName)
            {
                isDirty = (int)CategoryCombo.SelectedValue != origType;
                if (!isDirty)
                {
                    SavePointButton.IsEnabled = false;
                    CancelPointButton.IsEnabled = false;
                }
                return;
            }
            isDirty = true;
            CancelPointButton.IsEnabled = true;
            if (PointName.Text.Length > 0)
            {
                NameError.Visibility = Visibility.Hidden;
                SavePointButton.IsEnabled = true;
            }
            else
            {
                NameError.Visibility = Visibility.Visible;
                SavePointButton.IsEnabled = false;
            }
        }
        private void SavePointButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                SavePoint();
                SavePointButton.IsEnabled = false;
                CancelPointButton.IsEnabled = false;
                isDirty = false;
            }
        }

        void SavePoint()
        {
            DatabaseHandler.ChangePoint(id, PointName.Text, (int)CategoryCombo.SelectedValue);
            tabLabel.tabName = PointName.Text;

            updater(id, PointName.Text, (int)CategoryCombo.SelectedValue);

            origName = PointName.Text;
            origType = (int)CategoryCombo.SelectedValue;
        }

        private void CancelPointButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                PointName.Text = origName;
                CategoryCombo.SelectedValue = origType;
                SavePointButton.IsEnabled = false;
                CancelPointButton.IsEnabled = false;
                isDirty = false;
            }
        }

        //trying to dispose and free memory
        bool Disposed = false;
        ~PointView()
        {
            DisposePage();
        }
        public void Dispose()
        {
            DisposePage();
            GC.SuppressFinalize(this);
        }

        void DisposePage()
        {
            if (!Disposed)
            {
               /* docPoint.Clear();
                foreach (var table in typeToTable)
                {
                    table.Value.ItemsSource = null;
                }
                typeToTable.Clear();
                foreach (var expander in categories)
                {
                    expander.Value.Content = null;
                }
                categories.Clear();

                pointTypes.Clear();
                updater = null;
                Content = null;*/
            }
            Disposed = true;
        }

        public bool CloseTab()
        {
            if (!isDirty)
            {
                return true;
            }
            MessageBoxResult result;
            if (PointName.Text.Length > 0)
            {
                result = MessageBox.Show("Сохранить все изменения точки?", "Внимание", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
            }
            else
            {
                result = MessageBox.Show("Невозможно сохранить изменения точки. Продолжить?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
            }

            switch (result)
            {
                case MessageBoxResult.Cancel:
                    return false;
                    break;
                case MessageBoxResult.Yes:
                    SavePoint();
                    break;
            }
            return true;
        }
    }
}
