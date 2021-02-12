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

        Update updateHandler;
        Add addhandler;
        //Update handler;

        List<PointType> pointTypes;
        public PointsPage(MainWindow mainWindow)
        {
            InitializeComponent();
            //points.AddIndexes();
            PointsList.ItemsSource = DatabaseHandler.GetPointsList();
            this.mainWindow = mainWindow;

            updateHandler = UpdatePoint;
            addhandler = AddPoint;

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
            /*for (int i=0;i< points.Count; i++)
            {
                if (points[i].typeID == pointTypeID)
                {
                    points[i].typeID = pointTypeID;
                    points[i].typeID = pointTypeID;
                }
            }*/
        }

        private void OnPointSelected(object sender, MouseButtonEventArgs e)
        {
            if(PointsList.SelectedIndex == -1)
            {
                return;
            }
            e.Handled = true;
            int pointId = DatabaseHandler.GetPointsList()[PointsList.SelectedIndex].id;
            if (clickedPoints.ContainsKey(pointId))
            {
                int index = orderedTabs.FindIndex(a => a == clickedPoints[pointId]);
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
            string pointName = DatabaseHandler.GetPointsList()[PointsList.SelectedIndex].name;

            TabItem newTabItem = new TabItem
            {
                Name = "Point" + pointId.ToString()
            };

            Frame frame = new Frame();
            newTabItem.Content = frame;
            PointView newtab = new PointView(pointId, updateHandler);
            frame.Content = newtab;

            newTabItem.Header = CreateTabHeader(pointName, pointId, newtab);
            clickedPoints.Add(pointId, newTabItem);
            orderedTabs.Add(newTabItem);

            PointsList.SelectedIndex = -1;
            mainWindow.AddTab(newTabItem, newtab);
            //TODO Tell main window to select add and select tab

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
            closeButton.Click += (object sender, RoutedEventArgs e) => { OnTabClosing(id, view); };


            panel.Children.Add(label);
            panel.Children.Add(closeButton);

            return panel;
        }

        public void OnTabClosing(int index, PointView view)
        {
            //TODO: CHECK IF SAVED
            if (!clickedPoints.ContainsKey(index))
            {
                return;
            }
            mainWindow.OnTabClosing(orderedTabs.FindIndex(a => a == clickedPoints[index]),clickedPoints[index], view);
            orderedTabs.Remove(clickedPoints[index]);
            clickedPoints.Remove(index);
            view.Dispose();
        }

        public void UpdatePoint(int id, string name, int newType)
        {
            var point = DatabaseHandler.GetPointsList().Find(a => a.id == id);
            point.name = name;
            point.type = DatabaseHandler.GetPointType(newType);

            PointsList.Items.Refresh();
        }

        public void AddPoint()
        {
            DatabaseHandler.GetPointsList(true);
            //points.Add(DatabaseHandler.GetNewPoint());
            PointsList.Items.Refresh();
        }

        private void CreatePointType_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreateNewPointType(NewPointTypeName.Text);
        }

        private void CreateDocType_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreateNewDocType(NewDocTypeName.Text);
        }

        private void CreatePoint_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreatePoint(NewPointName.Text, (int)NewPointTypeCombo.SelectedValue);
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
