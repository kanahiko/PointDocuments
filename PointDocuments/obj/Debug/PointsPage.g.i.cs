﻿#pragma checksum "..\..\PointsPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C251F7DA0E4D394743BE368D07B781FA5940D11FE8F81C5DBE2E832FDC1855E4"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using PointDocuments;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace PointDocuments {
    
    
    /// <summary>
    /// PointsPage
    /// </summary>
    public partial class PointsPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid PointsList;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel AddNewPointGrid;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock NewPointNamePlaceholder;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewPointName;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox NewPointTypeCombo;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreatePoint;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid AddNewPointTypeGrid;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewPointTypeName;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreatePointType;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid AddNewDocTypeGrid;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox NewDocTypeName;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\PointsPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateDocType;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PointDocuments;component/pointspage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\PointsPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.PointsList = ((System.Windows.Controls.DataGrid)(target));
            
            #line 19 "..\..\PointsPage.xaml"
            this.PointsList.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.OnPointSelected);
            
            #line default
            #line hidden
            return;
            case 2:
            this.AddNewPointGrid = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.NewPointNamePlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.NewPointName = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\PointsPage.xaml"
            this.NewPointName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.NewPointName_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.NewPointTypeCombo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.CreatePoint = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\PointsPage.xaml"
            this.CreatePoint.Click += new System.Windows.RoutedEventHandler(this.CreatePoint_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AddNewPointTypeGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.NewPointTypeName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.CreatePointType = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\PointsPage.xaml"
            this.CreatePointType.Click += new System.Windows.RoutedEventHandler(this.CreatePointType_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.AddNewDocTypeGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            this.NewDocTypeName = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.CreateDocType = ((System.Windows.Controls.Button)(target));
            
            #line 55 "..\..\PointsPage.xaml"
            this.CreateDocType.Click += new System.Windows.RoutedEventHandler(this.CreateDocType_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
