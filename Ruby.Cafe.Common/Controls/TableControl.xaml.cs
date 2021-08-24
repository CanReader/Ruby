using System;
using System.Windows.Controls;

using System.Windows.Media;

namespace Ruby.Cafe.Common.Controls
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public Ruby.Cafe.Model.Table table { get; set; }

        public String TableName {get{return TableNameText.Text;} set{TableNameText.Text=value;}}

        public String TableStatue {get{return StatueText.Text;} set{StatueText.Text = value;}}

        public Color TableColor {get {return ((SolidColorBrush)TableBody.Background).Color;} set {TableBody.Background = new SolidColorBrush(value);}}

        public TableControl(Model.Table table)
        {
            InitializeComponent();

            this.table = table;

            TableName = table.Name;

            var color = System.Drawing.ColorTranslator.FromHtml("#" + table.TableColor);
            TableColor = Color.FromRgb(color.R,color.G,color.B);

            int TableStatueID = (int)table.CurrentStatue;

            if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("tr") || System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("TR"))
                TableStatue = ((Model.TableStatueTR)TableStatueID).ToString();
            else if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("fr") || System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("FR"))
                TableStatue = ((Model.TableStatueFR)TableStatueID).ToString();
            else if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("ger") || System.Threading.Thread.CurrentThread.CurrentCulture.Name.Contains("GER"))
                TableStatue = ((Model.TableStatueGER)TableStatueID).ToString();
            else
                TableStatue = table.CurrentStatue.ToString();

            TableStatue.Replace('_',' ');

            this.Width = this.ActualWidth;
            this.Height = this.Width * 3 / 4;
        }
    }
}
