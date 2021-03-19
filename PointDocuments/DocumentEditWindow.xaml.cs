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
        //HashSet<int> addedIds;
        List<DocHistoryChange> docHistoryChanges;

        bool isDirty;
        public DocumentEditWindow(int id)
        {
            InitializeComponent();
            Height = Util.BigHeight;
            Width = Util.BigWidth;
            this.id = id;
            InitializeEditWindow();
            isDirty = false;
        }

        void InitializeEditWindow()
        {
            doctypes = DatabaseHandler.GetDocumentTypes();
            DocTypeCombo.ItemsSource = doctypes;

            //addedIds = new HashSet<int>();
            docHistoryChanges = new List<DocHistoryChange>();
            Tuple<int, string> doc = DatabaseHandler.GetDocument(id);
            originalDocType = doc.Item1;
            originalName = doc.Item2;
            DocumentName.Text = originalName;
            DocTypeCombo.SelectedValue = originalDocType;
            PointsCountLabel.Content = $"Связано с {DatabaseHandler.GetPointDocumentCount(id)} точками";
            PopulateHistoryTable();

            if (!DatabaseHandler.userRole.Documents.HasFlag(Permissions.UPDATE))
            {
                DocumentName.IsEnabled = false;
                DocTypeCombo.IsEnabled = false;
                if (!(DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.INSERT) ||
                    DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.UPDATE)))
                {
                    SaveButton.Visibility = Visibility.Collapsed;
                    CancelButton.Visibility = Visibility.Collapsed;                   
                }
            }

            if (!DatabaseHandler.userRole.DocumentHistory.HasFlag(Permissions.INSERT))
            {
                ChangeDocument.Visibility = Visibility.Collapsed; 
                CancelChange.Visibility = Visibility.Collapsed;
                SaveHistoryChange.Visibility = Visibility.Collapsed;
            }

            if (!(DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.DELETE) &&
                DatabaseHandler.userRole.PointDocConnections.HasFlag(Permissions.INSERT)))
            {
                AddPoints.Visibility = Visibility.Collapsed;
            }
        }

        void PopulateHistoryTable()
        {
            history = DatabaseHandler.GetDocumentHistory(id);
            DocumentHistory.ItemsSource = history;
        }
        void UpdateHistorytable(DocHistoryChange change)
        {
            DocTable doc;
            if (change.restoreID == null)
            {
                doc = new DocTable
                {
                    name = change.newDocument.Substring(change.newDocument.LastIndexOf("\\") + 1),
                    date = change.newTime,
                    username = System.Environment.UserName,
                    id = -1
                };
            }
            else
            {
                doc = new DocTable
                {
                    name = change.restoreID.name,
                    date = change.newTime,
                    username = System.Environment.UserName,
                    id = -1
                };
            }
            history.Insert(0, doc);
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
                docHistoryChanges.Add(new DocHistoryChange(filePath));
                UpdateHistorytable(docHistoryChanges[docHistoryChanges.Count - 1]);
            }
            SaveHistoryChange.IsEnabled = true;
            CancelChange.IsEnabled = true;
        }

        private void CancelChange_Click(object sender, RoutedEventArgs e)
        {
            if (docHistoryChanges.Count != 0)
            {
                for (int i = 0; i < docHistoryChanges.Count; i++)
                {
                    history.RemoveAt(0);
                }
                DocumentHistory.Items.Refresh();
                docHistoryChanges.Clear();


                SaveHistoryChange.IsEnabled = false;
                CancelChange.IsEnabled = false;
            }
        }

        private void SaveHistoryChange_Click(object sender, RoutedEventArgs e)
        {
            if (docHistoryChanges.Count != 0)
            {
                SaveDocumentHistory();
                PopulateHistoryTable();

                SaveHistoryChange.IsEnabled = false;
                CancelChange.IsEnabled = false;
            }
        }

        void SaveDocumentHistory()
        {
            for (int i = 0; i < docHistoryChanges.Count; i++)
            {
                if (docHistoryChanges[i].restoreID != null)
                {
                    DatabaseHandler.RestoreDocumentHistory(docHistoryChanges[i].restoreID.id, docHistoryChanges[i].newTime);
                }
                else
                {
                    DatabaseHandler.CreateDocumentHistrory(id, docHistoryChanges[i].newDocument, docHistoryChanges[i].newTime);
                }
            }
            docHistoryChanges.Clear();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isDirty || docHistoryChanges.Count != 0)
            {
                MessageBoxResult result;
                if (DocumentName.Text.Length > 0)
                {
                    result = MessageBox.Show("Сохранить все изменения документа?", "Внимание", MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation);
                }
                else
                {
                    result = MessageBox.Show("Невозможно сохранить изменения документа. Продолжить?", "Внимание", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);
                }
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        SaveDocument();
                        SaveDocumentHistory();
                        break;
                    case MessageBoxResult.OK:
                    case MessageBoxResult.No:
                        DocumentName.Text = originalName;
                        DocTypeCombo.SelectedValue = originalDocType;
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        return;
                        break;
                }
            }
            e.Cancel = false;
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentHistory.SelectedIndex > 0)
            {
                if (history[DocumentHistory.SelectedIndex].id != -1)
                {
                    docHistoryChanges.Add(new DocHistoryChange(history[DocumentHistory.SelectedIndex]));
                    UpdateHistorytable(docHistoryChanges[docHistoryChanges.Count-1]);
                }
                else
                {
                    DocTable tempTable = history[DocumentHistory.SelectedIndex];

                    DocHistoryChange tempChange = docHistoryChanges[docHistoryChanges.Count - DocumentHistory.SelectedIndex - 1];
                    docHistoryChanges.Remove(tempChange);
                    docHistoryChanges.Add(tempChange);


                    history.RemoveAt(DocumentHistory.SelectedIndex);
                    history.Insert(0, tempTable);

                    DocumentHistory.Items.Refresh();
                }
                //DatabaseHandler.RestoreDocumentHistory(history[DocumentHistory.SelectedIndex].id);

                SaveHistoryChange.IsEnabled = true;
                CancelChange.IsEnabled = true;
            }
        }
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentHistory.SelectedIndex != -1)
            {
                if (history[DocumentHistory.SelectedIndex].id != -1)
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
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{filePath}\"");
                    }
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{docHistoryChanges[docHistoryChanges.Count - DocumentHistory.SelectedIndex -1].newDocument}\"");
                }
            }
        }

        //===============================CHANGING DOC
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                SaveDocument();
            }
        }

        void SaveDocument()
        {
            isDirty = false;
            CancelButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            DatabaseHandler.ChangeDocument(id, DocumentName.Text, (int)DocTypeCombo.SelectedValue);
            originalName = DocumentName.Text;
            originalDocType = (int)DocTypeCombo.SelectedValue;
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDirty)
            {
                isDirty = false;
                CancelButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                DocumentName.Text = originalName;
                DocTypeCombo.SelectedValue = originalDocType;
            }
        }
        private void DocumentName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DocumentName.Text == originalName)
            {
                isDirty = (int)DocTypeCombo.SelectedValue != originalDocType;
                if (!isDirty)
                {
                    CancelButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                }
                return;
            }

            if (DocumentName.Text.Length > 0)
            {
                ErrorDocumentName.Visibility = Visibility.Hidden;
                SaveButton.IsEnabled = true;
            }
            else
            {
                ErrorDocumentName.Visibility = Visibility.Visible;
                SaveButton.IsEnabled = false;
            }

            isDirty = true;
            CancelButton.IsEnabled = true;
        }

        private void DocumentName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                Keyboard.ClearFocus(); 
                if (DocumentName.Text == originalName)
                {
                    isDirty = (int)DocTypeCombo.SelectedValue != originalDocType;
                    if (!isDirty)
                    {
                        CancelButton.IsEnabled = false;
                        SaveButton.IsEnabled = false;
                    }
                    return;
                }
                if (DocumentName.Text.Length > 0)
                {
                    ErrorDocumentName.Visibility = Visibility.Hidden;
                    SaveButton.IsEnabled = true;
                }
                else
                {
                    ErrorDocumentName.Visibility = Visibility.Visible;
                    SaveButton.IsEnabled = false;
                }
            }
            isDirty = true;
            CancelButton.IsEnabled = true;
            SaveButton.IsEnabled = true;
        }
        private void DocTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((int)DocTypeCombo.SelectedValue == originalDocType)
            {
                isDirty = DocumentName.Text != originalName;
                if (!isDirty)
                {
                    CancelButton.IsEnabled = false;
                    SaveButton.IsEnabled = false;
                }
                return;
            }
            isDirty = true;
            CancelButton.IsEnabled = true;
            SaveButton.IsEnabled = DocumentName.Text.Length > 0;
        }


        //================================
        private void AddPoints_Click(object sender, RoutedEventArgs e)
        {
            ConnectPointWindow connectPointWindow = new ConnectPointWindow(id);

            connectPointWindow.Closing += ConnectPointWindow_Closing;
            connectPointWindow.Owner = this;

            connectPointWindow.ShowDialog();
        }

        private void ConnectPointWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //throw new NotImplementedException();
            PointsCountLabel.Content = $"Связано с {DatabaseHandler.GetPointDocumentCount(id)} точками";
            e.Cancel = false;
        }

    }

    public class DocHistoryChange
    {
        public DocTable restoreID;
        public string newDocument;
        public DateTime newTime;

        public DocHistoryChange(string newDoc)
        {
            newDocument = newDoc;
            newTime = DateTime.Now;
        }
        public DocHistoryChange(DocTable restoreDocID)
        {
            restoreID = restoreDocID;
            newTime = DateTime.Now;
        }
    }
}
