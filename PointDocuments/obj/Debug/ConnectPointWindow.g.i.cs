﻿#pragma checksum "..\..\ConnectPointWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "588398F5E509CABBC76C37E984ED9C9AAED5EF7630FDF205A0DE85D5AD1625BB"
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
    /// ConnectPointWindow
    /// </summary>
    public partial class ConnectPointWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 7 "..\..\ConnectPointWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal PointDocuments.ConnectPointWindow ConnectionWindow;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\ConnectPointWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ConnectedDataGrid;
        
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
            System.Uri resourceLocater = new System.Uri("/PointDocuments;component/connectpointwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ConnectPointWindow.xaml"
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
            this.ConnectionWindow = ((PointDocuments.ConnectPointWindow)(target));
            
            #line 8 "..\..\ConnectPointWindow.xaml"
            this.ConnectionWindow.Closing += new System.ComponentModel.CancelEventHandler(this.ConnectionWindow_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.ConnectedDataGrid = ((System.Windows.Controls.DataGrid)(target));
            
            #line 19 "..\..\ConnectPointWindow.xaml"
            this.ConnectedDataGrid.PreviewMouseUp += new System.Windows.Input.MouseButtonEventHandler(this.ConnectedDataGrid_Click);
            
            #line default
            #line hidden
            
            #line 19 "..\..\ConnectPointWindow.xaml"
            this.ConnectedDataGrid.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.ConnectedDataGrid_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
