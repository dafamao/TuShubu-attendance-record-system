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
using System.IO;

namespace TuShubu_attendance_record_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SolidColorBrush my_Brush1 = new SolidColorBrush();
        SolidColorBrush my_Brush2 = new SolidColorBrush();
        SolidColorBrush my_Brush3 = new SolidColorBrush();

        public delegate void my_delegate();

        public static event my_delegate blogtempSave;
        public static event my_delegate mailtempSave;
        public static event my_delegate advicetempSave;

        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            my_time.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            my_time.AutoReset = false;

            my_prompt.Visibility = Visibility.Collapsed;

            DatabaseOperate.bindTreeViewData();
            DatabaseOperate.UpdateConfig();

            string datapath = App.pathDesktop + @"\考勤记录";

            if (!(Directory.Exists(datapath)))
            {
                Directory.CreateDirectory(datapath);
            }

            string starTime = DateTime.Now.ToString(DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) + "     " + DateTime.Now.ToString("yyyy.MM.dd"));
            ShowListViewData timeListViewItem = new ShowListViewData("启动时间：", DateTime.Now.ToString("HH:mm:ss"), starTime);
            App.ListViewData.Add(timeListViewItem);
            ShowListViewData splitLines = new ShowListViewData("——————————", "——————————", "——————————————————————————");
            App.ListViewData.Add(splitLines);

            page_Register_Loaded();
        }

        private void page_Register_Loaded()
        {
            this.my_frame.Navigate(new Uri("Page_register.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_register0.Fill = my_Brush1;
            rtg_mail0.Fill = my_Brush2;
            rtg_set0.Fill = my_Brush2;
            rtg_manage0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.my_prompt.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     my_prompt.Visibility = Visibility.Collapsed;
                 }
                ));
        }

        private void Title_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogSaveClick == false && App.IsblogTextHave == true)
            {
                my_prompt.Visibility = Visibility.Visible;
                my_time.Start();
                this.my_frame.Navigate(new Uri("Page_blog.xaml", UriKind.Relative));
            }
            else if (App.IsadviceSaveClick == false && App.IsadviceTextHave == true)
            {
                my_prompt.Visibility = Visibility.Visible;
                my_time.Start();
                this.my_frame.Navigate(new Uri("Page_advice.xaml", UriKind.Relative));
            }
            else
            {
                if ((App.ListViewData.Count()) > 2)
                {
                    registerfinalSave();
                }

                try
                {
                    string pathTemp_advice = App.pathDesktop + @"\考勤记录\temp_advice.txt";
                    string pathTemp_blog = App.pathDesktop + @"\考勤记录\temp_blog.txt";
                    string pathTemp_mail = App.pathDesktop + @"\考勤记录\temp_mail.txt";

                    File.Delete(pathTemp_advice);
                    File.Delete(pathTemp_blog);
                    File.Delete(pathTemp_mail);
                }
                catch
                {

                }

                Environment.Exit(0);
            }
        }

        private void my_frame_MouseEnter(object sender, MouseEventArgs e)
        {
            my_Brush3.Color = Color.FromArgb(255, 63, 113, 157);

            stp_hamburg.Width = 50;
            my_rtg.Width = 50;
            my_rtgs.Fill = my_Brush3;
            my_line.X2 = 40;
        }

        private void stp_hamburg_MouseEnter(object sender, MouseEventArgs e)
        {
            my_Brush3.Color = Color.FromArgb(0, 255, 255, 255);

            stp_hamburg.Width = 200;
            my_rtg.Width = 200;
            my_rtgs.Fill = my_Brush3;
            my_line.X2 = 160;
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            page_Register_Loaded();
        }

        private void btn_blog_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_blog.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_blog0.Fill = my_Brush1;
            rtg_mail0.Fill = my_Brush2;
            rtg_set0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_manage0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void btn_mail_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_mail.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_mail0.Fill = my_Brush1;
            rtg_manage0.Fill = my_Brush2;
            rtg_set0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void btn_advice_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_advice.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_set0.Fill = my_Brush2;
            rtg_mail0.Fill = my_Brush2;
            rtg_manage0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush1;
        }

        private void btn_manage_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_manage.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_manage0.Fill = my_Brush1;
            rtg_mail0.Fill = my_Brush2;
            rtg_set0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void btn_plan_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_plan.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_plan0.Fill = my_Brush1;
            rtg_manage0.Fill = my_Brush2;
            rtg_mail0.Fill = my_Brush2;
            rtg_set0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void btn_set_Click(object sender, RoutedEventArgs e)
        {
            if (App.IsblogLoaded)
            {
                blogtempSave();
                App.IsblogLoaded = false;
            }
            if (App.IsadviceLoaded)
            {
                advicetempSave();
                App.IsadviceLoaded = false;
            }
            if (App.IsmailLoaded)
            {
                mailtempSave();
                App.IsmailLoaded = false;
            }

            this.my_frame.Navigate(new Uri("Page_set.xaml", UriKind.Relative));

            my_Brush1.Color = Color.FromArgb(255, 223, 125, 127);
            my_Brush2.Color = Color.FromArgb(0, 255, 255, 255);

            rtg_set0.Fill = my_Brush1;
            rtg_mail0.Fill = my_Brush2;
            rtg_manage0.Fill = my_Brush2;
            rtg_register0.Fill = my_Brush2;
            //rtg_view0.Fill = my_Brush2;
            rtg_blog0.Fill = my_Brush2;
            rtg_plan0.Fill = my_Brush2;
            rtg_advice0.Fill = my_Brush2;
        }

        private void registerfinalSave()
        {
            string pathSave = App.pathDesktop + @"\考勤记录\签到记录.txt";

            StreamWriter stw_register = new StreamWriter(pathSave, true);

            int length = App.ListViewData.Count();

            ShowListViewData[] dataRegister = new ShowListViewData[length];
            App.ListViewData.CopyTo(dataRegister, 0);

            for (int i = 0; i < length; i++)
            {
                stw_register.WriteLine(dataRegister[i].Name + "     " + dataRegister[i].Time + "     " + dataRegister[i].Remarks);
                stw_register.WriteLine("");
            }

            stw_register.WriteLine("");
            stw_register.WriteLine("");
            stw_register.WriteLine("");
            stw_register.WriteLine("");
            stw_register.WriteLine("");
            stw_register.Close();
        }
    }
}
