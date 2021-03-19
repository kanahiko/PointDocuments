using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
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
    /// Логика взаимодействия для DocumentCreateWindow.xaml
    /// </summary>
    public partial class DocumentCreateWindow : Window
    {
        string filePath = string.Empty;
        List<DocumentType> doctypes { get; set; }

        public DocumentCreateWindow()
        {
            InitializeComponent();
            doctypes = DatabaseHandler.GetDocumentTypes();
            DocTypeCombo.ItemsSource = doctypes;
            DocTypeCombo.SelectedIndex = 0;

            SaveDocument.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                string path = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                FileNameLabel.Text = path;
                FileNameLabel.Foreground = Brushes.Black;
                DocName.Text = path.Substring(0, path.LastIndexOf("."));

                SaveDocument.IsEnabled = true;
            }
            else
            {
                if (filePath == string.Empty)
                {
                    SaveDocument.IsEnabled = false;
                    FileNameLabel.Foreground = Brushes.Red;
                }
            }
        }

        private void SaveDocument_Click(object sender, RoutedEventArgs e)
        {
            if (filePath == string.Empty)
            {
                FileNameLabel.Foreground = Brushes.Red;
            }
            else
            {
                //ADD FILE TO DATABASE
                string name = DocName.Text;
                if (name == "")
                {
                    name = FileNameLabel.Text;
                    name = name.Substring(0, name.LastIndexOf("."));
                }
                SavedDocument.IsChecked = true;
                DatabaseHandler.CreateDocument(filePath, (int)DocTypeCombo.SelectedValue, name);
                this.Close();
            }

        }

        private void DocName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DocName.Text.Length > 0)
            {
                DocNamePlaceholder.Visibility = Visibility.Hidden;
            }
            else
            {
                DocNamePlaceholder.Visibility = Visibility.Visible;
            }
        }
    }
}
