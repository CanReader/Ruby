using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Ruby.Cafe.Common.Controls
{
    public enum MB_ButtonTypes
    {
        OK = 0,
        Yes = 1,
        No = 2
    };

    public partial class MB_MessageBox : Window
    {
        public MB_ButtonTypes ButtonType;

        private bool Clicked = false;

        public MB_MessageBox(string Caption, string Message, System.Windows.MessageBoxButton type,System.Windows.MessageBoxImage icon)
        {
            InitializeComponent();

            this.MouseDown += (ssender,ee) => this.DragMove();

            OkayButton.Content = Ruby.Resources.Localization.MB_OKAY;
            YesButton.Content = Ruby.Resources.Localization.MB_YES;
            NoButton.Content = Ruby.Resources.Localization.MB_NO;

            switch (type)
            {
                case MessageBoxButton.OK:
                    OkayButton.Visibility = Visibility.Visible;
                    YesButton.Visibility = Visibility.Collapsed;
                    NoButton.Visibility = Visibility.Collapsed;
                    break;
                case MessageBoxButton.YesNo:
                    OkayButton.Visibility = Visibility.Collapsed;
                    YesButton.Visibility  = Visibility.Visible;
                    NoButton.Visibility   = Visibility.Visible;
                    break;
                default:
                    OkayButton.Visibility = Visibility.Collapsed;
                    YesButton.Visibility = Visibility.Collapsed;
                    NoButton.Visibility = Visibility.Collapsed;
                    break;
            }

            if(icon == MessageBoxImage.Error || icon == MessageBoxImage.Warning || icon == MessageBoxImage.Stop)
                Image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Ruby.Resources;Component/IMG/IMG_UI_ErrorException.png"));
            else if(icon == MessageBoxImage.Asterisk)
            Image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Ruby.Resources;Component/IMG/IMG_UI_Info.png"));
            else if(icon == MessageBoxImage.Question)
            Image.Source = new BitmapImage(new Uri(@"pack://application:,,,/Ruby.Resources;Component/IMG/IMG_UI_Question.png"));
            else
                TitlePanel.HorizontalAlignment = HorizontalAlignment.Center;

            this.Title.Content = Caption;
            this.Message.Text = Message;

        }

        private void OkayButton_Click(object sender, RoutedEventArgs e) { ButtonType = MB_ButtonTypes.OK; this.DialogResult = true; Clicked = true; this.Close();}

        private void YesButton_Click(object sender, RoutedEventArgs e)  { ButtonType = MB_ButtonTypes.Yes; this.DialogResult = true; Clicked = true; this.Close(); }

        private void NoButton_Click(object sender, RoutedEventArgs e)   { ButtonType = MB_ButtonTypes.No; this.DialogResult = true; Clicked = true; this.Close(); }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!Clicked) e.Cancel = true;
        }
    }
}
