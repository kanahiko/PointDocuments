using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PointsPage.xaml
    /// </summary>
    public partial class PointsPage : Page
    {
        MainWindow mainWindow;

        Dictionary<int, TabItem> clickedPoints = new Dictionary<int, TabItem>();
        List<TabItem> orderedTabs = new List<TabItem>();

        bool isUsingTabs = false;

        //Update updateHandler;
        //Add addhandler;
        //Update handler;

        List<PointType> pointTypes;
        public PointsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            //points.AddIndexes();
            PointsList.ItemsSource = DatabaseHandler.GetPointsList();
            this.mainWindow = mainWindow;

            //addhandler = AddPoint;

            NewPointTypeCombo.ItemsSource = DatabaseHandler.GetPointTypes();
            NewPointTypeCombo.SelectedIndex = 0;

            if (!DatabaseHandler.userRole.Points.HasFlag(Permissions.INSERT))
            {
                AddNewPointGrid.Visibility = Visibility.Collapsed;
            }

        }

        public void UpdatePointTypes()
        {
            int selectedIndex = NewPointTypeCombo.SelectedIndex;
            NewPointTypeCombo.Items.Refresh();
            NewPointTypeCombo.SelectedIndex = -1;
            NewPointTypeCombo.SelectedIndex = selectedIndex;

            DatabaseHandler.GetPointsList(true);
            PointsList.Items.Refresh();
        }

        private void OnPointSelected(object sender, MouseButtonEventArgs e)
        {
            if(PointsList.SelectedIndex == -1)
            {
                return;
            }
            e.Handled = true;
            int pointID = DatabaseHandler.GetPointsList()[PointsList.SelectedIndex].id;            
            string pointName = DatabaseHandler.GetPointsList()[PointsList.SelectedIndex].name;

            

            Frame frame = new Frame();
            PointView view = new PointView(pointID);
            view.updater += UpdatePoint;
            frame.Content = view;

            if (isUsingTabs)
            {
                CreatePointTab(pointID, pointName, view, frame);
            }
            else
            {
                CreatePointWindow(pointID, pointName, view, frame);
            }


            PointsList.SelectedIndex = -1;
            //TODO Tell main window to select add and select tab
        }

        void CreatePointWindow(int pointID,string pointName, PointView view, Frame frame)
        {
            Window window = new Window();
            window.Owner = Window.GetWindow(this);
            window.Title = pointName;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Height = Util.BigHeight;
            window.Width = Util.BigWidth;


            frame.Margin = Util.borderThickness;
            window.Content = frame;
            window.Owner = Window.GetWindow(this);
            window.Closing += (sender, e) =>
            {
                e.Cancel = OnTabClosing(pointID, view);
            };
            window.ShowInTaskbar = false;
            window.ShowDialog();
        }

        void CreatePointTab(int pointID, string pointName, PointView view, Frame frame)
        {
            if (clickedPoints.ContainsKey(pointID))
            {
                int index = orderedTabs.FindIndex(a => a == clickedPoints[pointID]);
                if (index == -1)
                {
                    return;
                }
                mainWindow.OpenTab(index);
                PointsList.SelectedIndex = -1;
                return;
            }
            //TODO Checks if can add more tabs
            if (mainWindow.CheckCanOpenTab())
            {
                return;
            }

            TabItem newTabItem = new TabItem
            {
                Name = "Point" + pointID.ToString()
            };
            newTabItem.Content = frame;
            newTabItem.Header = CreateTabHeader(pointName, pointID, view);
            clickedPoints.Add(pointID, newTabItem);
            orderedTabs.Add(newTabItem);
            mainWindow.AddTab(newTabItem, view);
        }

        StackPanel CreateTabHeader(string name, int id, PointView view)
        {
            StackPanel panel = new StackPanel();
            panel.Height = Util.panelHeight;
            panel.Orientation = Orientation.Horizontal;

            TextBlock label = new TextBlock();
            label.DataContext = view.tabLabel;
            label.SetBinding(TextBlock.TextProperty, "tabName");

            label.Height = Util.panelHeight;
            label.Padding = Util.rightThickness;
            label.MouseDown += (object sender, MouseButtonEventArgs e) => {
                if (e.ChangedButton == MouseButton.Middle)
                    OnTabClosing(id, view);
            };

            Button closeButton = new Button();
            closeButton.Content = Util.x;
            closeButton.Height = Util.buttonSize;
            closeButton.Width = Util.buttonSize;
            closeButton.Background = Brushes.Transparent;
            closeButton.FontSize = Util.fontSize;
            closeButton.Click += (object sender, RoutedEventArgs e) => 
            {
                 OnTabClosing(id, view);
            };


            panel.Children.Add(label);
            panel.Children.Add(closeButton);

            return panel;
        }

        public bool OnTabClosing(int index, PointView view)
        {
            if (view.CloseTab())
            {
                //TODO: CHECK IF SAVED
                if (isUsingTabs)
                {
                    if (!clickedPoints.ContainsKey(index))
                    {
                        return true;
                    }
                    mainWindow.OnTabClosing(orderedTabs.FindIndex(a => a == clickedPoints[index]), clickedPoints[index], view);
                    orderedTabs.Remove(clickedPoints[index]);
                    clickedPoints.Remove(index);
                    view.Dispose();
                }
                return false;
            }
            return true;
        }

        public void UpdatePoint(int id, string name, int newType)
        {
            var point = DatabaseHandler.GetPointsList().Find(a => a.id == id);
            point.name = name;
            point.type = DatabaseHandler.GetPointType(newType);

            PointsList.Items.Refresh();
        }

        public void UpdatePoints()
        {
            DatabaseHandler.GetPointsList(true);
            //points.Add(DatabaseHandler.GetNewPoint());
            PointsList.Items.Refresh();
        }


        private void CreatePoint_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreatePoint(NewPointName.Text, (int)NewPointTypeCombo.SelectedValue);
            UpdatePoints();
            NewPointName.Text = "";
        }

        private void NewPointName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewPointName.Text.Length > 0)
            {
                NewPointNamePlaceholder.Visibility = Visibility.Hidden;
                CreatePoint.IsEnabled = true;
            }
            else
            {
                NewPointNamePlaceholder.Visibility = Visibility.Visible;
                CreatePoint.IsEnabled = false;
            }
        }

        private void DeletePointButton_Click(object sender, RoutedEventArgs e)
        {
            if (PointsList.SelectedItem != null) 
            {
                if (DatabaseHandler.GetDocumentPointCount(((PointTable)PointsList.SelectedItem).id) == 0)
                { if (MessageBox.Show($"Вы точно хотите удалить точку \"{((PointTable)PointsList.SelectedItem).name}\"?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DatabaseHandler.DeletePoint(((PointTable)PointsList.SelectedItem).id);
                        UpdatePoints();
                    }
                }
                else
                {
                    MessageBox.Show($"Невозможно удалить \"{((PointTable)PointsList.SelectedItem).name}\". Привязано к документам.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

    }
    public delegate void Update(int id, string name, int newType);
    public delegate void Add();

    public class UpdateEventArgs
    {
        public UpdateEventArgs(int id,string name, int newType) { ID = id; Name = name; NewType = newType; }
        public int ID { get; }
        public string Name { get; }
        public int NewType { get; }
    }
}
