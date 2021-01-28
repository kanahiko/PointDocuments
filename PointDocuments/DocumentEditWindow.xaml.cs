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
        List<string> doctypes;
        List<int> docId;

        int id;
        public DocumentEditWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            PopulateHistoryTable();
        }

        void PopulateHistoryTable()
        {
            doctypes = TestData.docTypes.Select(a => a.name).ToList();
            docId = TestData.docTypes.Select(a => a.id).ToList();

            DocTypeCombo.ItemsSource = doctypes;

            if (id == -1)
            {
                DocTypeCombo.SelectedIndex = 0;

                DocumentNameLabel.Content = "Новый";
            }
            else
            {
                DocTypeCombo.SelectedIndex = docId.FindIndex(a => a == TestData.docs.Where(b => b.id == id).Select(b => b.doctypeid).First());
                List<DocumentHistory> history = TestData.docHistory.Where(a => a.docid == id).OrderByDescending(a => a.date).ToList();
                PointsCountLabel.Content = $"Связано с {TestData.pointDoc.Where(a => a.docid == id).Count()} точками";
                DocumentHistory.ItemsSource = history;
                DocumentNameLabel.Content = history[0].name;
            }

        }
        private void ChangeDocument_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();T 6
            openFileDialog.InitialDirectory = "c:\\";
            //openFileDialog.FilterIndex = 2;
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
