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
    /// <summary>
    /// Логика взаимодействия для TypesPage.xaml
    /// </summary>
    public partial class TypesPage : Page
    {
        List<TypeTable> docTypes;
        List<TypeTable> pointTypes;
        public UpdateTypes updatePointTypesHandler;
        public UpdateTypes updateDocumentTypesHandler;
        public TypesPage()
        {
            InitializeComponent();

            docTypes = DatabaseHandler.GetDocumentTypesTable();
            docTypes.AddIndexes();
            DocumentTypeList.ItemsSource = docTypes;

            if (!DatabaseHandler.userRole.DocumentType.HasFlag(Permissions.DELETE))
            {
                DocumentTypeList.Columns[2].Visibility = Visibility.Hidden;
            }

            if (!DatabaseHandler.userRole.DocumentType.HasFlag(Permissions.INSERT))
            {
                NewDocTypePanel.Visibility = Visibility.Collapsed;
            }
            if (!DatabaseHandler.userRole.DocumentType.HasFlag(Permissions.UPDATE))
            {
                DocumentTypeList.Columns[1].IsReadOnly = true;
            }

            pointTypes = DatabaseHandler.GetPointTypesTable();
            pointTypes.AddIndexes();
            PointsTypeList.ItemsSource = pointTypes;

            if (!DatabaseHandler.userRole.PointTypes.HasFlag(Permissions.DELETE))
            {
                PointsTypeList.Columns[2].Visibility = Visibility.Hidden;
            }

            if (!DatabaseHandler.userRole.PointTypes.HasFlag(Permissions.INSERT))
            {
                NewPointTypePanel.Visibility = Visibility.Collapsed;
            }
            if (!DatabaseHandler.userRole.PointTypes.HasFlag(Permissions.UPDATE))
            {
                PointsTypeList.Columns[1].IsReadOnly = true;
            }
        }
        //=====================DOCUMENT TYPES
        private void DocumentTypeList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (DocumentTypeList.SelectedIndex != -1)
            {
                DatabaseHandler.ChangeDocType(docTypes[DocumentTypeList.SelectedIndex].id, docTypes[DocumentTypeList.SelectedIndex].name);
                //TODO UPDATE DOCUMENTS PAGE
                updateDocumentTypesHandler.Invoke();
            }
        }

        private void DeleteDocTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (DocumentTypeList.SelectedIndex != -1)
            {
                if (DatabaseHandler.CheckCanDeleteDocumentType(docTypes[DocumentTypeList.SelectedIndex].id)) 
                { 
                    if (MessageBox.Show($"Вы уверены, что хотите удалить \"{docTypes[DocumentTypeList.SelectedIndex].name}\"?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DatabaseHandler.DeleteDocType(docTypes[DocumentTypeList.SelectedIndex].id);
                        docTypes.RemoveAt(DocumentTypeList.SelectedIndex);
                        docTypes.AddIndexes();
                        DocumentTypeList.Items.Refresh();
                        //TODO UPDATE DOCUMENTS PAGE
                        updateDocumentTypesHandler.Invoke();
                    }
                }
                else
                {
                    MessageBox.Show($"Невозможно удалить \"{docTypes[DocumentTypeList.SelectedIndex].name}\". Привязано к документам.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
        }

        private void NewDocTypeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewDocTypeName.Text.Length > 0)
            {
                NewDocTypeNamePlaceholder.Visibility = Visibility.Hidden;
                AddNewDocTypeButton.IsEnabled = true;
            }
            else
            {
                NewDocTypeNamePlaceholder.Visibility = Visibility.Visible;
                AddNewDocTypeButton.IsEnabled = false;
            }
        }

        private void AddNewDocTypeButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreateNewDocType(NewDocTypeName.Text);
            NewDocTypeName.Text = "";
            TypeTable newType = DatabaseHandler.GetLatestDocumentType();
            docTypes.Add(newType);
            newType.number = docTypes.Count;
            DocumentTypeList.Items.Refresh();
            //TODO UPDATE DOCUMENTS PAGE
            updateDocumentTypesHandler.Invoke();
        }

        //=======================POINT TYPES
        private void PointsTypeList_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

            if (PointsTypeList.SelectedIndex != -1)
            {
                DatabaseHandler.ChangePointType(pointTypes[PointsTypeList.SelectedIndex].id, pointTypes[PointsTypeList.SelectedIndex].name);
                updatePointTypesHandler.Invoke();
                //TODO UPDATE MAIN PAGE AND ALL OPENED POINTS
            }
        }

        private void DeletePointTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (PointsTypeList.SelectedIndex != -1)
            {
                if (DatabaseHandler.CheckCanDeletePointType(pointTypes[PointsTypeList.SelectedIndex].id))
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить \"{pointTypes[PointsTypeList.SelectedIndex].name}\"?",
                        "Подтверждение",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        DatabaseHandler.DeletePointType(pointTypes[PointsTypeList.SelectedIndex].id);
                        pointTypes.RemoveAt(PointsTypeList.SelectedIndex);
                        pointTypes.AddIndexes();
                        PointsTypeList.Items.Refresh();
                        //TODO UPDATE MAIN PAGE AND ALL OPENED POINTS
                        updatePointTypesHandler.Invoke();
                    }
                }
                else
                {
                    MessageBox.Show($"Невозможно удалить \"{pointTypes[PointsTypeList.SelectedIndex].name}\". Привязано к точкам.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void NewPointTypeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NewPointTypeName.Text.Length > 0)
            {
                NewPointTypeNamePlaceholder.Visibility = Visibility.Hidden;
                AddNewPointTypeButton.IsEnabled = true;
            }
            else
            {
                NewPointTypeNamePlaceholder.Visibility = Visibility.Visible;
                AddNewPointTypeButton.IsEnabled = false;
            }

        }

        private void AddNewPointTypeButton_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.CreateNewPointType(NewPointTypeName.Text);
            NewPointTypeName.Text = "";
            TypeTable newType = DatabaseHandler.GetLatestPointType();
            pointTypes.Add(newType);
            newType.number = pointTypes.Count;
            PointsTypeList.Items.Refresh();
            updatePointTypesHandler.Invoke();
            //TODO UPDATE MAIN PAGE AND ALL OPENED POINTS
        }
    }
}
