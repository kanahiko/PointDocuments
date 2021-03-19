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
using System.Windows.Shapes;

namespace PointDocuments
{
    /// <summary>
    /// Логика взаимодействия для ConnectPointWindow.xaml
    /// </summary>
    public partial class ConnectPointWindow : Window
    {
        int docID;

        HashSet<int> changedIDs;
        List<PointTable> points;
        public ConnectPointWindow(int docID)
        {
            InitializeComponent();

            this.docID = docID;
            changedIDs = new HashSet<int>();
            AddCheckBoxColumn();

            points = DatabaseHandler.GetPointsList(docID);
            ConnectedDataGrid.ItemsSource = points;
        }

        void AddCheckBoxColumn()
        {
            CustomDataGridCheckBoxColumn checkBoxColumn = new CustomDataGridCheckBoxColumn();
            checkBoxColumn.Header = "Отностится к документу";
            Binding binding = new Binding("isConnected");
            checkBoxColumn.Binding = binding;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            binding.Mode = BindingMode.TwoWay;
            checkBoxColumn.IsReadOnly = true;
            ConnectedDataGrid.Columns.Add(checkBoxColumn);
        }


        private void CheckBoxButton_Click(object sender, RoutedEventArgs e)
        {
            if (changedIDs.Contains(points[ConnectedDataGrid.SelectedIndex].id))
            {
                changedIDs.Remove(points[ConnectedDataGrid.SelectedIndex].id);
            }
            else
            {
                changedIDs.Add(points[ConnectedDataGrid.SelectedIndex].id);
            }
        }

        private void ConnectionWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DatabaseHandler.ChangeDocPointConnection(docID, changedIDs);

            //TODO: UPDATE POINTS THAT ARE OPEN
            e.Cancel = false;
        }

        private void ConnectedDataGrid_Click(object sender, MouseButtonEventArgs e)
        {
            if (ConnectedDataGrid.SelectedIndex != -1 && ConnectedDataGrid.CurrentColumn != null && ConnectedDataGrid.CurrentColumn.DisplayIndex == 2)
            {
                points[ConnectedDataGrid.SelectedIndex].isConnected = !points[ConnectedDataGrid.SelectedIndex].isConnected;
                if (changedIDs.Contains(points[ConnectedDataGrid.SelectedIndex].id))
                {
                    changedIDs.Remove(points[ConnectedDataGrid.SelectedIndex].id);
                }
                else
                {
                    changedIDs.Add(points[ConnectedDataGrid.SelectedIndex].id);
                }
            }
        }
    }
}
