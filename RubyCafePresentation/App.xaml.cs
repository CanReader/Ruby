using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
namespace RubyCafePresentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void CatchUnhandledExceptions(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Ruby.Cafe.Common.Controls.MessageBox.ShowExceptionBox("Unhandled exception"," ",e.Exception);
            return;
        }
    }
}
