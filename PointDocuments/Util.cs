using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PointDocuments
{
    public static class Util
    {
        public static int Height = 450;
        public static int Width = 800;


        public static int maxTabs = 10;
        public static int panelHeight = 18;
        public static string x = "❌";
        public static Thickness zeroThickness = new Thickness(0, 0, 0, 0);
        public static Thickness rightThickness = new Thickness(0, 0, 5, 0);
        public static Thickness borderThickness = new Thickness(10, 10, 10, 10);
        public static int buttonSize = 15;
        public static int fontSize = 9;

        public static int startingTabsCount = 0;


        public static readonly HashSet<string> permissions = new HashSet<string>
        {
            "SELECT","UPDATE","INSERT","DELETE"
        };

        public static void AddIndexes(this List<DocTable> table)
        {
            for (int i = 0; i < table.Count; i++)
            {
                int index = i + 1;
                table[i].number = index;
            }
        }
        public static void AddIndexes(this List<PointTable> table)
        {
            for (int i = 0; i < table.Count; i++)
            {
                int index = i + 1;
                table[i].number = index;
            }
        }
        public static void AddIndexes(this List<TypeTable> table)
        {
            for (int i = 0; i < table.Count; i++)
            {
                int index = i + 1;
                table[i].number = index;
            }
        }

        public static void ShowErrorMessage(OkErrorDelegate onOk)
        {
            if (MessageBox.Show("Ошибка подключения к базе данных.\n" +
                "Обратитесь к администратору.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error) == MessageBoxResult.OK)
            {
                onOk();
            }
        }

        public static DataGrid CreateDatagrid(int id = -1)
        {
            DataGrid table = new DataGrid();
            table.AutoGenerateColumns = false;
            table.CanUserAddRows = false;
            table.CanUserDeleteRows = false;
            table.CanUserSortColumns = true;
            table.CanUserReorderColumns = false;
            table.SelectionMode = DataGridSelectionMode.Single;
            table.SelectionUnit = DataGridSelectionUnit.FullRow;
            /*table.LoadingRow += (object sender, DataGridRowEventArgs e) =>
            {
                System.Diagnostics.Debug.WriteLine("over here");
                ((DocTable)e.Row.Item).number = e.Row.GetIndex() + 1;
            };*/
            table.MaxHeight = 500;
            table.IsReadOnly = true;
            /*table.LostFocus += (object sender, RoutedEventArgs e) =>
            {
                table.UnselectAll();
            };*/

            DataGridTextColumn textColumn = new DataGridTextColumn();
            textColumn.Header = "#";

            Binding binding = new Binding();
            /* binding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1);
             binding.Converter = new RowToIndexConverter();*/
            textColumn.Binding = new Binding("number");
            table.Columns.Add(textColumn);

            //< DataGridTextColumn Header = "#" Binding = "{Binding RelativeSource={RelativeSource AncestorType=DataGridRow}, Converter={local:RowToIndexConverter}}" />

            textColumn = new DataGridTextColumn();
            textColumn.Header = "Название файла";
            textColumn.Binding = new Binding("name");
            table.Columns.Add(textColumn);


            textColumn = new DataGridTextColumn();
            textColumn.Header = "Дата изменения";
            binding = new Binding("date");
            binding.StringFormat = "dd.MM.yy HH:mm:ss";
            textColumn.Binding = binding;
            table.Columns.Add(textColumn);


            textColumn = new DataGridTextColumn();
            textColumn.Header = "Пользователь";
            textColumn.Binding = new Binding("username");
            table.Columns.Add(textColumn);

            if (id != -1)
            {
                CustomDataGridCheckBoxColumn checkBoxColumn = new CustomDataGridCheckBoxColumn();
                checkBoxColumn.Header = "Отностится к точке";
                binding = new Binding("isConnected");
                checkBoxColumn.Binding = binding;
                binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                binding.Mode = BindingMode.TwoWay;
                checkBoxColumn.IsReadOnly = true;
                table.Columns.Add(checkBoxColumn);
            }

            return table;
        }
    }

    public delegate void OkErrorDelegate();
    public delegate void UpdateTypes();

/*    public class RowToIndexConverter : System.Windows.Markup.MarkupExtension, IValueConverter
    {
        static RowToIndexConverter converter;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            if (row != null)
                return row.GetIndex() + 1;
            else
                return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (converter == null) converter = new RowToIndexConverter();
            return converter;
        }

        public RowToIndexConverter()
        {
        }
    }*/

    public class CustomDataGridCheckBoxColumn : DataGridCheckBoxColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            CheckBox checkBox = base.GenerateEditingElement(cell, dataItem) as CheckBox;
            return checkBox;
        }
    }

    [Flags]
    public enum Permissions
    {
        None = 0,
        SELECT = 1,
        UPDATE = 2,
        INSERT = 4,
        DELETE =8
    }

    public class UserRole
    {
        public Permissions DocumentHistory;
        public Permissions Documents;
        public Permissions DocumentType;
        public Permissions PointDocConnections;
        public Permissions Points;
        public Permissions PointTypes;

        public void AddPermissions(string table, List<string> permissions)
        {
            Permissions permision = 0;
            for (int i=0;i< permissions.Count; i++)
            {
                switch ((permissions[i][0]))
                {
                    case 'S':
                        permision |= Permissions.SELECT;
                        break;
                    case 'U':
                        permision |= Permissions.UPDATE;
                        break;
                    case 'I':
                        permision |= Permissions.INSERT;
                        break;
                    case 'D':
                        permision |= Permissions.DELETE;
                        break;
                }
            }

            switch (table)
            {
                case "DocumentHistory":
                    DocumentHistory = permision;
                    break;
                case "Documents":
                    Documents = permision;
                    break;
                case "DocumentType":
                    DocumentType = permision;
                    break;
                case "PointDocConnections":
                    PointDocConnections = permision;
                    break;
                case "Points":
                    Points = permision;
                    break;
                case "PointTypes":
                    PointTypes = permision;
                    break;
            }
        }
    }
}
