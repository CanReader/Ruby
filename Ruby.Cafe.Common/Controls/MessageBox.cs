using System;

namespace Ruby.Cafe.Common.Controls
{
    public static class MessageBox
    {   
        public static MB_ButtonTypes ShowMessageBox(string Caption, string Message, System.Windows.MessageBoxButton type, System.Windows.MessageBoxImage image)
        {
            bool? dlgResult = null;
            MB_MessageBox ErrorBox = new MB_MessageBox(Caption, Message, type,image);
            dlgResult = ErrorBox.ShowDialog();

                return ErrorBox.ButtonType;
        }

        public static bool? ShowExceptionBox(string Caption, String Message, Exception e)
        {
            if (e == null) throw new Exception("Exception parameter cannot be null");

            bool? dlgResult = null;
            MB_ExceptionBox ErrorBox = new MB_ExceptionBox(Caption,Message,e);
            dlgResult = ErrorBox.ShowDialog();

            return dlgResult;
        }
    }
}
