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
using System.Windows.Threading;

namespace TuShubu_attendance_record_system
{
    /// <summary>
    /// Interaction logic for Page_blog.xaml
    /// </summary>
    public partial class Page_blog : Page
    {
        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        public Page_blog()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_blog.txt";

                if (File.Exists(pathTemp))
                {
                    StreamReader str_tempblog = new StreamReader(pathTemp);
                    string line = str_tempblog.ReadLine();

                    if (!(line == null))
                    {
                        string[] strArray = line.Split('$');

                        txb_headman.Text = strArray[0];
                        txb_data.Text = strArray[1];
                        txb_leave.Text = strArray[2];
                        txb_cover.Text = strArray[3];
                        txb_late.Text = strArray[4];
                        txb_absent.Text = strArray[5];
                        txb_relay.Text = strArray[6];
                        txb_return.Text = strArray[7];
                        txb_praise1.Text = strArray[8];
                        txb_reason1.Text = strArray[9];
                        txb_praise2.Text = strArray[10];
                        txb_reason2.Text = strArray[11];
                        txb_blog.Text = strArray[12];
                    }
                }
                else
                {
                    txb_data.Text = DateTime.Now.ToString("yyyy.MM.dd") + "  " + DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
                }
            }
            catch
            {
                /// <summary>
                /// 捕捉到未知错误（这里一般是文件占用错误），跳过错误啥也不做
                /// </summary>
            }

            my_time.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            my_time.AutoReset = false;

            btn_save.Visibility = Visibility.Visible;
            btn_succeed.Visibility = Visibility.Collapsed;

            MainWindow.blogtempSave += new MainWindow.my_delegate(tempblog_save);

            App.IsblogLoaded = true;
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.btn_save.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     btn_save.Visibility = Visibility.Visible;
                     btn_succeed.Visibility = Visibility.Collapsed;
                 }
                ));
        }

        private void txb_blog_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txb_blog.Text == ""))
            {
                App.IsblogTextHave = true;
            }
            else
            {
                App.IsblogTextHave = false;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string pathSave = App.pathDesktop + @"\考勤记录\值班日志.txt";

            StreamWriter stw_blog = new StreamWriter(pathSave, true);

            string time_blog = DateTime.Now.ToString(DateTime.Now.ToString("HH:mm:ss") + "     " + DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) + "     " + DateTime.Now.ToString("yyyy.MM.dd"));

            stw_blog.WriteLine("记录时间：   " + time_blog);
            stw_blog.WriteLine("———————————————————————————————————");
            stw_blog.WriteLine("值班组长：   " + txb_headman.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("值班日期：   " + txb_data.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("请假人员：   " + txb_leave.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("补班人员：   " + txb_cover.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("迟到人员：   " + txb_late.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("旷岗人员：   " + txb_absent.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("替班人员：   " + txb_relay.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("还班人员：   " + txb_return.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("表扬人员：   " + txb_praise1.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("表扬理由：   " + txb_reason1.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("表扬人员：   " + txb_praise2.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("表扬理由：   " + txb_reason2.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("值班日志：");
            stw_blog.WriteLine("           " + txb_blog.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.Close();

            btn_save.Visibility = Visibility.Collapsed;
            btn_succeed.Visibility = Visibility.Visible;

            my_time.Start();

            App.IsblogSaveClick = true;
        }

        private void tempblog_save()
        {
            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_blog.txt";

                string tempblog;

                tempblog = txb_headman.Text;
                tempblog += "$" + txb_data.Text;
                tempblog += "$" + txb_leave.Text;
                tempblog += "$" + txb_cover.Text;
                tempblog += "$" + txb_late.Text;
                tempblog += "$" + txb_absent.Text;
                tempblog += "$" + txb_relay.Text;
                tempblog += "$" + txb_return.Text;
                tempblog += "$" + txb_praise1.Text;
                tempblog += "$" + txb_reason1.Text;
                tempblog += "$" + txb_praise2.Text;
                tempblog += "$" + txb_reason2.Text;
                tempblog += "$" + txb_blog.Text;

                StreamWriter stw_blog = new StreamWriter(pathTemp, false);
                stw_blog.WriteLine(tempblog);
                stw_blog.Close();
            }
            catch
            {
                /// <summary>
                /// 捕捉到未知错误（这里一般是文件占用错误），跳过错误啥也不做
                /// </summary>
            }
        }
    }
}
