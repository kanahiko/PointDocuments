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
    public partial class MainWindow : Window
    {

        Dictionary<int,TabItem> clickedPoints = new Dictionary<int, TabItem>();

        //List<string> points = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            DocumentsPage page = new DocumentsPage(-1);
            DocumentsFrame.Content = page;

            /*for (int i=0;i < TestData.points.Length; i++)
            {
                points.Add(TestData.points[i].name);
            }*/
            DatabaseHandler.Initialize();
            PointsList.ItemsSource = DatabaseHandler.GetPointsList();

            //Test();
            //AddNewPointGrid.Visibility = Visibility.Collapsed;

        }

        void Test() {
            var t = DatabaseHandler.GetDocumentsID(14);
            var c = DatabaseHandler.GetDocTableDocuments(t, 2);
        }

        private void OnPointSelected(object sender, MouseButtonEventArgs e)
        {
            int pointId = DatabaseHandler.GetPointId(PointsList.SelectedIndex);
            if (clickedPoints.ContainsKey(pointId) || Tabs.Items.Count >= Util.maxTabs)
            {
                return;
            }
            string pointName = PointsList.Items[PointsList.SelectedIndex] as string;

            TabItem newTabItem = new TabItem
            {
                Name = pointName + pointId.ToString()
            };
           
            newTabItem.Header = CreateTabHeader(pointName, pointId);
            clickedPoints.Add(pointId, newTabItem);

            Frame frame = new Frame();
            newTabItem.Content = frame;
            PointView newtab = new PointView(pointId);
            frame.Content = newtab;
            
            Tabs.Items.Add(newTabItem);
            Tabs.SelectedIndex = Tabs.Items.Count - 1;
        }

        StackPanel CreateTabHeader(string name, int id)
        {
            StackPanel panel = new StackPanel();
            panel.Height = Util.panelHeight;
            panel.Orientation = Orientation.Horizontal;

            Label label = new Label();
            label.Height = Util.panelHeight;
            label.Padding = Util.rightThickness;
            label.Content = name;

            Button closeButton = new Button();
            closeButton.Content = Util.x;
            closeButton.Height = Util.buttonSize;
            closeButton.Width = Util.buttonSize;
            closeButton.Background = Brushes.Transparent;
            closeButton.FontSize = Util.fontSize;
            closeButton.Click += (object sender, RoutedEventArgs e) => { OnTabClosing(id); };


            panel.Children.Add(label);
            panel.Children.Add(closeButton);

            return panel;
        }

        void OnTabClosing(int index)
        {
            //TODO: CHECK IF SAVED
            if (!clickedPoints.ContainsKey(index))
            {
                return;
            }
            Tabs.Items.Remove(clickedPoints[index]);
            clickedPoints.Remove(index);
        }

        private void MainTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void MainTab_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("test");

        }
    }
}
