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
        HashSet<int> addedIds;
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

            addedIds = new HashSet<int>();
            Tuple<int, string> doc = DatabaseHandler.GetDocument(id);
            originalDocType = doc.Item1;
            originalName = doc.Item2;
            DocumentName.Text = originalName;
            DocTypeCombo.SelectedValue = originalDocType;

            history = DatabaseHandler.GetDocumentHistory(id);
            PointsCountLabel.Content = $"Связано с {DatabaseHandler.GetPointDocumentCount(id)} точками";
            DocumentHistory.ItemsSource = history;

            if (!DatabaseHandler.userRole.Documents.HasFlag(Permissions.UPDATE))
            {
                DocumentName.IsEnabled = false;
                DocTypeCombo.IsEnabled = false;
                if (!(DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.INSERT) ||
                    DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.UPDATE)))
                {
                    CancelChange.Visibility = Visibility.Collapsed;
                    CancelChange.IsEnabled = false;
                }
            }

            if (!DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.INSERT))
            {
                ChangeDocument.Visibility = Visibility.Collapsed;
            }
        }

        void UpdateHistorytable()
        {
            DocTable doc = DatabaseHandler.GetLatestHistory(id);
            addedIds.Add(doc.id);
            history.Insert(0,doc);
            DocumentHistory.Items.Refresh();
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

            DatabaseHandler.CreateDocumentHistrory(id, filePath);
            UpdateHistorytable();
        }

        private void CancelChange_Click(object sender, RoutedEventArgs e)
        {
            DocumentName.Text = originalName;
            DocTypeCombo.SelectedValue = originalDocType;

            if (addedIds != null)
            {
                for(int i = 0; i < addedIds.Count; i++)
                {
                    history.RemoveAt(0);
                }
                DocumentHistory.Items.Refresh();
                DatabaseHandler.DeleteDocumentHistory(addedIds);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DatabaseHandler.ChangeDocument(id, DocumentName.Text, (int)DocTypeCombo.SelectedValue);
            e.Cancel = false;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentHistory.SelectedIndex != -1)
            {
                DatabaseHandler.RestoreDocumentHistory(history[DocumentHistory.SelectedIndex].id);
                UpdateHistorytable();
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentHistory.SelectedIndex != -1)
            {
                int index = history[DocumentHistory.SelectedIndex].id;
                string fileName = DatabaseHandler.GetDocumentHistoryFileName(index);
                string fileType = fileName.Substring(fileName.LastIndexOf("."));
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = "c:\\";
                saveFileDialog.FileName = fileName;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == true)
                {
                    string filePath = saveFileDialog.FileName;
                    if (filePath.EndsWith(fileType))
                    {
                        filePath = filePath.Substring(0, filePath.Length - fileType.Length);
                    }
                    if (filePath.Length > 260 - fileType.Length)
                    {
                        filePath = filePath.Substring(0, 260 - fileType.Length);
                    }
                    filePath += fileType;
                    DatabaseHandler.DownloadDocument(index, filePath);
                }
            }
        }
    }
}
