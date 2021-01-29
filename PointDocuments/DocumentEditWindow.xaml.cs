using System.IO;
using System.Windows;
using Microsoft.Win32;
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
    /// Логика взаимодействия для DocumentEditWindow.xaml
    /// </summary>
    public partial class DocumentEditWindow : Window
    {
        List<DocumentType> doctypes;

        int id;
        List<DocTable> history;
        public DocumentEditWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            PopulateHistoryTable();
        }

        void PopulateHistoryTable()
        {
            //TODO
            doctypes = DatabaseHandler.GetDocumentTypes();

            DocTypeCombo.ItemsSource = doctypes;

            if (id == -1)
            {
                DocTypeCombo.SelectedIndex = 0;

                DocumentNameLabel.Content = "Новый";
            }
            else
            {
                Tuple<int, string> doc = DatabaseHandler.GetDocument(id);
                DocTypeCombo.SelectedValue = doc.Item1;
                history = DatabaseHandler.GetDocumentHistory(id);
                PointsCountLabel.Content = $"Связано с {DatabaseHandler.GetPointDocumentCount(id)} точками";
                DocumentHistory.ItemsSource = history;
                DocumentNameLabel.Content = doc.Item2;
            }

        }
        private void ChangeDocument_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
            }
            //TODO send stuff to database and refresh table
            PopulateHistoryTable();
        }

        private void DocTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO send stuff to database
        }
    }
}
