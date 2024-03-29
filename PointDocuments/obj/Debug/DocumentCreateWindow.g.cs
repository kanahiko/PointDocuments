﻿#pragma checksum "..\..\DocumentCreateWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "66B46F8DCDB41436E27A50AD3CC4AE273ED799FA240BEB6782D9EC857B9C1C93"
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
    /// DocumentCreateWindow
    /// </summary>
    public partial class DocumentCreateWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock FileNameLabel;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock DocNamePlaceholder;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DocName;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox DocTypeCombo;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SaveDocument;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\DocumentCreateWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox SavedDocument;
        
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
            System.Uri resourceLocater = new System.Uri("/PointDocuments;component/documentcreatewindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\DocumentCreateWindow.xaml"
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
            
            #line 16 "..\..\DocumentCreateWindow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.FileNameLabel = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.DocNamePlaceholder = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.DocName = ((System.Windows.Controls.TextBox)(target));
            
            #line 21 "..\..\DocumentCreateWindow.xaml"
            this.DocName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.DocName_TextChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.DocTypeCombo = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.SaveDocument = ((System.Windows.Controls.Button)(target));
            
            #line 28 "..\..\DocumentCreateWindow.xaml"
            this.SaveDocument.Click += new System.Windows.RoutedEventHandler(this.SaveDocument_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.SavedDocument = ((System.Windows.Controls.CheckBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

