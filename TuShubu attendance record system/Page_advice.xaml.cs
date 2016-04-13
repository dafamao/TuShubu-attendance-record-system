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
    /// Interaction logic for Page_advice.xaml
    /// </summary>
    public partial class Page_advice : Page
    {
        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        public Page_advice()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //skp_possword.Visibility = Visibility.Visible;
            //skp_content.Visibility = Visibility.Collapsed;

            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_advice.txt";

                if (File.Exists(pathTemp))
                {
                    StreamReader str_tempadvice = new StreamReader(pathTemp);
                    string line = str_tempadvice.ReadLine();

                    if (!(line == ""))
                    {
                        string[] strArray = line.Split('$');

                        txb_data.Text = strArray[0];
                        txb_advice.Text = strArray[1];
                    }

                    str_tempadvice.Close();
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

            MainWindow.advicetempSave += new MainWindow.my_delegate(tempadvice_save);

            App.IsadviceLoaded = true;
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.btn_save.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     btn_save.Content = "保存";
                 }
                ));
        }

        private void txb_advice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!(txb_advice.Text == ""))
            {
                App.IsadviceTextHave = true;
            }
            else
            {
                App.IsadviceTextHave = false;
            }
        }

        private void btn_save_Click(object sender, RoutedEventArgs e)
        {
            string pathSave = App.pathDesktop + @"\考勤记录\查岗意见.txt";

            StreamWriter stw_blog = new StreamWriter(pathSave, true);

            string time_advice = DateTime.Now.ToString(DateTime.Now.ToString("HH:mm:ss") + "     " + DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) + "     " + DateTime.Now.ToString("yyyy.MM.dd"));
            stw_blog.WriteLine("记录时间：   " + time_advice);
            stw_blog.WriteLine("———————————————————————————————————");
            stw_blog.WriteLine("查岗意见：");
            stw_blog.WriteLine("           " + txb_advice.Text);
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.WriteLine("");
            stw_blog.Close();

            btn_save.Content = "保存成功 ！";
            my_time.Start();

            App.IsadviceSaveClick = true;
        }

        private void tempadvice_save()
        {
            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_advice.txt";

                string tempadvice;

                tempadvice = txb_data.Text;
                tempadvice += "$" + txb_advice.Text;

                StreamWriter stw_advice = new StreamWriter(pathTemp, false);
                stw_advice.WriteLine(tempadvice);
                stw_advice.Close();
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
