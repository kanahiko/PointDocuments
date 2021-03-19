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
        PointsPage pointsPage;
        DocumentsPage page;

        bool wasInitialized;
        TypesPage typesPage;

        List<TabItem> tabs;

        public MainWindow()
        {
            InitializeComponent();
            wasInitialized = false;
            //TabCheckBox.IsChecked = Properties.Settings.Default.isUsingTabs;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (!wasInitialized)
            {
                //Height="675" Width="1200"
                Visibility = Visibility.Hidden;
                wasInitialized = true;
                ConnectionCheckWindow checkWindow = new ConnectionCheckWindow();
                checkWindow.Owner = this;
                checkWindow.Closing += CheckWindow_Closing;
                checkWindow.Show();
                WindowState = WindowState.Normal;
                checkWindow.WindowState = WindowState.Normal;
                checkWindow.Focus();
            }
        }

        private void CheckWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!DatabaseHandler.isTested || !DatabaseHandler.canConnect)
            {
                Close();
            }
            else
            {
                //puts window into full screen
                Height = Util.Height;
                Width = Util.Width;
                Top = (SystemParameters.PrimaryScreenHeight - Height) / 2;
                Left = (SystemParameters.PrimaryScreenWidth - Width) / 2;
                Visibility = Visibility.Visible;
                WindowState = WindowState.Normal;
                Focus();
                InitializeWindow();
            }
            e.Cancel = false;
        }

        void InitializeWindow()
        {
            if (DatabaseHandler.canConnect)
            {
                DatabaseHandler.Initialize();
                Util.startingTabsCount = Tabs.Items.Count;

                typesPage = new TypesPage();
                TypesFrame.Content = typesPage;

                page = new DocumentsPage();
                typesPage.updateDocumentTypesHandler += page.UpdateDocumentType;
                DocumentsFrame.Content = page;
                

                pointsPage = new PointsPage(this);
                typesPage.updatePointTypesHandler += pointsPage.UpdatePointTypes;
                PointsFrame.Content = pointsPage;

            }
            else
            {
                IsEnabled = false;
            }
        }

        public void AddTab(TabItem newTab,PointView pointView){
            Tabs.Items.Add(newTab);
            Tabs.SelectedIndex = Tabs.Items.Count - 1;
            typesPage.updatePointTypesHandler += pointView.UpdatePointTypes;
            page.updaterOfPoints += pointView.UpdateCategories;
        }


        public void OnTabClosing(int index, TabItem removeTab, PointView pointView)
        {
            if (Tabs.SelectedIndex == index + Util.startingTabsCount)
            {
                Tabs.SelectedIndex = 0;
            }
            //TODO: CHECK IF SAVED
            Tabs.Items.Remove(removeTab);
            typesPage.updatePointTypesHandler += pointView.UpdatePointTypes;
            page.updaterOfPoints -= pointView.UpdateCategories;
        }

        public void OpenTab(int index)
        {
            Tabs.SelectedIndex = index + Util.startingTabsCount;
        }

        public bool CheckCanOpenTab()
        {
            return Tabs.Items.Count >= Util.maxTabs + Util.startingTabsCount;
        }

        private void TabCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.isUsingTabs = ((CheckBox)sender).IsChecked == true;

            //TRANSFORM
        }
    }
}
