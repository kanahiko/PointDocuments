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
    /// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    ///
    /// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PointDocuments"
    ///
    ///
    /// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    /// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    /// будет использоваться:
    ///
    ///     xmlns:MyNamespace="clr-namespace:PointDocuments;assembly=PointDocuments"
    ///
    /// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    /// на данный проект и пересобрать во избежание ошибок компиляции:
    ///
    ///     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    ///     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    ///
    ///
    /// Шаг 2)
    /// Теперь можно использовать элемент управления в файле XAML.
    ///
    ///     <MyNamespace:DocumentCustomInfo/>
    ///
    /// </summary>
    public class DocumentCustomInfo : Control
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
    nameof(Value), typeof(string), typeof(DocumentCustomInfo),
    new FrameworkPropertyMetadata(
        "", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty DateValueProperty = DependencyProperty.Register(
    nameof(DateValue), typeof(string), typeof(DocumentCustomInfo),
    new FrameworkPropertyMetadata(
        "", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DateValue
        {
            get { return (string)GetValue(DateValueProperty); }
            set { SetValue(DateValueProperty, value); }
        }

        public static readonly DependencyProperty UserValueProperty = DependencyProperty.Register(
    nameof(UserValue), typeof(string), typeof(DocumentCustomInfo),
    new FrameworkPropertyMetadata(
        "", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string UserValue
        {
            get { return (string)GetValue(UserValueProperty); }
            set { SetValue(UserValueProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(DocumentCustomInfo), new FrameworkPropertyMetadata(new CornerRadius(0, 0, 0, 0)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        static DocumentCustomInfo()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DocumentCustomInfo), new FrameworkPropertyMetadata(typeof(DocumentCustomInfo)));
        }
    }
}
