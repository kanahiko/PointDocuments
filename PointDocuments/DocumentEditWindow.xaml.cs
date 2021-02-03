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
        int originalDocType;
        string originalName;
        List<int> addedIds;
        public DocumentEditWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            PopulateHistoryTable();
        }

        void PopulateHistoryTable()
        {
            doctypes = DatabaseHandler.GetDocumentTypes();
            DocTypeCombo.ItemsSource = doctypes;

            Tuple<int, string> doc = DatabaseHandler.GetDocument(id);
            originalDocType = doc.Item1;
            originalName = doc.Item2;
            DocumentName.Text = originalName;
            DocTypeCombo.SelectedValue = originalDocType;

            history = DatabaseHandler.GetDocumentHistory(id);
            PointsCountLabel.Content = $"Связано с {DatabaseHandler.GetPointDocumentCount(id)} точками";
            DocumentHistory.ItemsSource = history;
        }

        void UpdateHistorytable()
        {
            history.Add(DatabaseHandler.GetLatestHistory(id));
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

            addedIds = new List<int>();
            DatabaseHandler.CreateDocumentHistrory(id, filePath);
            //TODO send stuff to database and refresh table
            UpdateHistorytable();
        }

        private void CancelChange_Click(object sender, RoutedEventArgs e)
        {
            DocumentName.Text = originalName;
            DocTypeCombo.SelectedValue = originalDocType;

            if (addedIds != null)
            {
                //TODO delete all added ids
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DatabaseHandler.ChangeDocument(id, DocumentName.Text, (int)DocTypeCombo.SelectedValue);
            e.Cancel = false;
        }
    }
}
