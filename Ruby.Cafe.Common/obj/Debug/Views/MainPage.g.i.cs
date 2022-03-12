﻿#pragma checksum "..\..\..\Views\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "487718599C3E3D6E2BFA26B22C3F46397739863CC9E799298198C0245280A618"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Ruby.Cafe.Common.Screens;
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


namespace Ruby.Cafe.Common.Screens {
    
    
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 9 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 14 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid UpperPanel;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle AdminPanelBtn;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle PrevScenceBtn;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ScenceNameText;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle NextScenceBtn;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle CloseAppBtn;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\Views\MainPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas TablePanel;
        
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
            System.Uri resourceLocater = new System.Uri("/Ruby.Cafe.Common;component/views/mainpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\MainPage.xaml"
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
            
            #line 8 "..\..\..\Views\MainPage.xaml"
            ((Ruby.Cafe.Common.Screens.MainPage)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.Page_Unloaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.UpperPanel = ((System.Windows.Controls.Grid)(target));
            
            #line 14 "..\..\..\Views\MainPage.xaml"
            this.UpperPanel.Loaded += new System.Windows.RoutedEventHandler(this.CreatedEvent);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AdminPanelBtn = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 29 "..\..\..\Views\MainPage.xaml"
            this.AdminPanelBtn.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.EnterPanel);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PrevScenceBtn = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 34 "..\..\..\Views\MainPage.xaml"
            this.PrevScenceBtn.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.PrevScenceBtn_MouseDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.ScenceNameText = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.NextScenceBtn = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 41 "..\..\..\Views\MainPage.xaml"
            this.NextScenceBtn.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.NextScenceBtn_MouseDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.CloseAppBtn = ((System.Windows.Shapes.Rectangle)(target));
            
            #line 46 "..\..\..\Views\MainPage.xaml"
            this.CloseAppBtn.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.SendQuitMessage);
            
            #line default
            #line hidden
            return;
            case 9:
            this.TablePanel = ((System.Windows.Controls.Canvas)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}
