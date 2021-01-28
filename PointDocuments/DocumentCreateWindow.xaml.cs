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
        List<string> doctypes;
        List<int> docId;
        public DocumentCreateWindow()
        {
            InitializeComponent();

            doctypes = TestData.docTypes.Select(a => a.name).ToList();
            docId = TestData.docTypes.Select(a => a.id).ToList();

            DocTypeCombo.ItemsSource = doctypes;
            DocTypeCombo.SelectedIndex = 0;

            SaveDocument.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //filePath = string.Empty;
            System.Diagnostics.Debug.WriteLine(FileNameLabel.ActualWidth);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            //openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                string path = filePath.Substring(filePath.LastIndexOf("\\") + 1);
                /*if (path.Length > 10)
                {
                    path = path.Substring(0, 10) + "...";
                }*/
                FileNameLabel.Text = path;
                FileNameLabel.Foreground = Brushes.Black;

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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (filePath == string.Empty)
            {
                FileNameLabel.Foreground = Brushes.Red;
            }
            else
            {
                //ADD FILE TO DATABASE
                this.Close();
            }
        }

        private void DocTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //???
        }
    }
}
