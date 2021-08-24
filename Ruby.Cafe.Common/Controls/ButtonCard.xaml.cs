using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace Ruby.Cafe.Common.Controls
{
    public partial class ButtonCard : UserControl
    {
        public static readonly DependencyProperty TitleProp = DependencyProperty.Register("Title", typeof(String), typeof(ButtonCard), new PropertyMetadata("Title", new PropertyChangedCallback(OnTitleChanged)));
        public static readonly DependencyProperty DescProp = DependencyProperty.Register("Description", typeof(String), typeof(ButtonCard), new PropertyMetadata("Description", new PropertyChangedCallback(OnDescChanged)));
        public static readonly DependencyProperty ImageProp = DependencyProperty.Register("Image", typeof(String), typeof(ButtonCard), new PropertyMetadata("pack://application:,,,/Ruby.Cafe.Common;Component/IMG/ColorPicker.png", new PropertyChangedCallback(OnImageChanged)));

        public string TTitle
        {
            get
            {
                return (string)GetValue(TitleProp);
            }
            set
            {
                SetValue(TitleProp, value);
                Title.Content = value;
            }
        }

        public string DDescription
        {
            get
            {
                return (string)GetValue((DescProp));
            }
            set
            {
                SetValue(DescProp, value);
                Description.Content = value;
            }
        }

        public string ImagePath
        {
            get
            {
                return (string)GetValue(ImageProp);
            }
            set
            {

                SetValue(ImageProp,value);
                    CardImage.Source = new BitmapImage(new Uri(value,UriKind.RelativeOrAbsolute));
            }
        }

        private static void OnDescChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Do something 
        }

        private static void OnTitleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Do something
        }

        private static void OnImageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Do something
        }

        public ButtonCard()
        {
            InitializeComponent();
        }

    }
}
