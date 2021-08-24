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

namespace Ruby.Cafe.Common.Controls
{
    /// <summary>
    /// Interaction logic for HistoryCard.xaml
    /// </summary>
    public partial class HistoryCard : UserControl
    {
            public static readonly DependencyProperty ProperyNameDP = DependencyProperty.Register("PropertyDependency",typeof(String),typeof(HistoryCard), new PropertyMetadata("Property"));

        public static readonly DependencyProperty PropertyValueDP = DependencyProperty.Register("ValueDependency",typeof(String),typeof(HistoryCard),new PropertyMetadata("0"));

                public static readonly DependencyProperty ThemeDP = DependencyProperty.Register("CardThemeDependency", typeof(int), typeof(HistoryCard), new PropertyMetadata(0));

        public String PropertyName
        {

            get
            {
                return (String)GetValue(ProperyNameDP);
            }
            set
            {
                SetValue(ProperyNameDP, value);
                this.PropertyNameTextBlock.Text = value;
            }
        }

        public String PropertyValue
        {
            get
            {
                return (String)GetValue(PropertyValueDP);
            }

            set
            {
                SetValue(PropertyValueDP,value);
                this.PropertyValueTextBlock.Text = value;
            }
        }

        public int CardTheme
        {
            get
            {
                return (int)GetValue(ThemeDP);
            }

            set
            {
                /*
                <LinearGradientBrush StartPoint="1,0" EndPoint="0,1">
                   <GradientStop Color="#9753ef" Offset="0.6"/>
                   <GradientStop Color="#FFC453EF"/>
               </LinearGradientBrush>
           */

                SetValue(ThemeDP,value);

                if (value == 0)
                {
                    List<GradientStop> gs = new List<GradientStop>();
                    gs.Add(new GradientStop(Color.FromRgb(235, 104, 255), 0));
                    gs.Add(new GradientStop(Color.FromRgb(120, 98, 181), 1.3));

                    Card.Background = new LinearGradientBrush(new GradientStopCollection(gs),
                        new Point(0, 1),
                        new Point(1, 0)
                    );
                }
                else if (value == 1)
                {
                    List<GradientStop> gs = new List<GradientStop>();
                    gs.Add(new GradientStop(Color.FromRgb(235, 134, 233), 0));
                    gs.Add(new GradientStop(Color.FromRgb(249, 98, 181), 1.3));

                    Card.Background = new LinearGradientBrush(new GradientStopCollection(gs),
                        new Point(0, 1),
                        new Point(1, 0)
                    );
                }
                else if (value == 2)
                {
                    List<GradientStop> gs = new List<GradientStop>();
                    gs.Add(new GradientStop(Color.FromRgb(0, 104, 203), 0));
                    gs.Add(new GradientStop(Color.FromRgb(249, 255, 181), 1.3));

                    Card.Background = new LinearGradientBrush(new GradientStopCollection(gs),
                        new Point(0, 1),
                        new Point(1, 0)
                    );
                }
                else if (value == 3)
                {
                    List<GradientStop> gs = new List<GradientStop>();
                    gs.Add(new GradientStop(Color.FromRgb(62, 224, 153), 0));
                    gs.Add(new GradientStop(Color.FromRgb(36, 190, 183), 1.3));

                    Card.Background = new LinearGradientBrush(new GradientStopCollection(gs),
                        new Point(1, 0),
                        new Point(0, 1)
                    );
                }
            }
        }
        
        public HistoryCard()
        {
            InitializeComponent();
        }
    }
}
