using Ruby.Setup.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace Ruby.Setup.Views
{
    /// <summary>
    /// Interaction logic for LastPage.xaml
    /// </summary>
    public partial class LastPage : Page
    {

        bool stop = false;

        public LastPage(SetupWindow MainWindow,List<Role> Roles,List<Employee> Employees)
        {
            InitializeComponent();

            MainWindow.WelcomeText = this.WelcomeText;

            this.Loaded += (ssender, ee)
                =>
            {

                new System.Threading.Thread(() =>
                {
                    Task t1 = new Task(() =>
                    {
                        System.Threading.Thread.Sleep(1000);
                        MainWindow.Dispatcher.Invoke(() => MainWindow.WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_CompletedMessage);
                        MainWindow.FadeOut();
                        System.Threading.Thread.Sleep(7000);
                        MainWindow.FadeIn();
                        System.Threading.Thread.Sleep(6000);
                    });

                    Task t2 = new Task(() =>
                    {
                        MainWindow.Dispatcher.Invoke(() => MainWindow.WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_SettingSettigsMessage);
                        MainWindow.FadeOut();
                        System.Threading.Thread.Sleep(7000);
                        MainWindow.FadeIn();
                        System.Threading.Thread.Sleep(6000);
                        MainWindow.Roles = Roles;
                        MainWindow.Employees = Employees;
                        if (!MainWindow.SaveSettings())
                            stop = true;
                    });


                    Task t3 = new Task(() =>
                    {
                        if (!stop)
                        {
                            MainWindow.Dispatcher.Invoke(() => MainWindow.WelcomeText.Text = Ruby.Resources.Localization.SetupScreen_EnjoyService);
                            MainWindow.FadeOut();
                            System.Threading.Thread.Sleep(8000);
                        }
                    });

                    Task t4 = new Task(() =>
                    {
                        if (!stop)
                        {
                            this.Dispatcher.Invoke(() => MainWindow.Close());
                            System.Diagnostics.Process.Start("RubyCafePresentation.exe");
                        }
                    });

                    t1.RunSynchronously();
                    t2.RunSynchronously();
                    t3.RunSynchronously();
                    t4.RunSynchronously();

                }).Start();

            };

        }
    }
}
