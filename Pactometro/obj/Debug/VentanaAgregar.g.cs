﻿#pragma checksum "..\..\VentanaAgregar.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "52B4CB3264D8BCD7B0AEAEC12D22A294CA3445FD1C97F6F00B10630EC43A82EB"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using Pactometro;
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


namespace Pactometro {
    
    
    /// <summary>
    /// VentanaAgregar
    /// </summary>
    public partial class VentanaAgregar : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbTipoProceso;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dpFecha;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNumEscaños;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtPartido;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtEscaños;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtColor;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lvPartidos;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\VentanaAgregar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEliminar;
        
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
            System.Uri resourceLocater = new System.Uri("/Pactometro;component/ventanaagregar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\VentanaAgregar.xaml"
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
            this.cmbTipoProceso = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 2:
            this.dpFecha = ((System.Windows.Controls.DatePicker)(target));
            
            #line 38 "..\..\VentanaAgregar.xaml"
            this.dpFecha.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.dpFecha_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtNumEscaños = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\VentanaAgregar.xaml"
            this.txtNumEscaños.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtNumEscaños_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txtPartido = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtEscaños = ((System.Windows.Controls.TextBox)(target));
            
            #line 51 "..\..\VentanaAgregar.xaml"
            this.txtEscaños.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtEscaños_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtColor = ((System.Windows.Controls.TextBox)(target));
            
            #line 55 "..\..\VentanaAgregar.xaml"
            this.txtColor.PreviewTextInput += new System.Windows.Input.TextCompositionEventHandler(this.txtEscaños_PreviewTextInput);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 57 "..\..\VentanaAgregar.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnAñadirPartido_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 61 "..\..\VentanaAgregar.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.BtnAñadirProceso_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.lvPartidos = ((System.Windows.Controls.ListView)(target));
            
            #line 64 "..\..\VentanaAgregar.xaml"
            this.lvPartidos.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.lvPartidos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btnEliminar = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\VentanaAgregar.xaml"
            this.btnEliminar.Click += new System.Windows.RoutedEventHandler(this.BtnEliminarPartido_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

