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

        List<string> points = new List<string>();

        public MainWindow()
        {
            InitializeComponent();

            DocumentsPage page = new DocumentsPage(-1);
            DocumentsFrame.Content = page;

            for (int i=0;i< TestData.points.Length; i++)
            {
                points.Add(TestData.points[i].name);
            }
            PointsList.ItemsSource = points;
            //AddNewPointGrid.Visibility = Visibility.Collapsed;

        }

        private void OnPointSelected(object sender, MouseButtonEventArgs e)
        {
            if (clickedPoints.ContainsKey(PointsList.SelectedIndex) || Tabs.Items.Count >= Util.maxTabs)
            {
                return;
            }
            TabItem newTabItem = new TabItem
            {
                Name = TestData.points[PointsList.SelectedIndex].name+TestData.points[PointsList.SelectedIndex].id.ToString()
            };
           
            newTabItem.Header = CreateTabHeader(TestData.points[PointsList.SelectedIndex].name, PointsList.SelectedIndex);
            clickedPoints.Add(PointsList.SelectedIndex, newTabItem);

            Frame frame = new Frame();
            newTabItem.Content = frame;
            PointView newtab = new PointView(TestData.points[PointsList.SelectedIndex].id);
            frame.Content = newtab;
            
            Tabs.Items.Add(newTabItem);

            Tabs.SelectedIndex = Tabs.Items.Count - 1;
        }

        StackPanel CreateTabHeader(string name, int index)
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
            closeButton.Click += (object sender, RoutedEventArgs e) => { OnTabClosing(index); };


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
