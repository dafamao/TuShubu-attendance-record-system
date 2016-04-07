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
    /// Interaction logic for Page_mail.xaml
    /// </summary>
    public partial class Page_mail : Page
    {
        public Page_mail()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_mail.txt";

                if (File.Exists(pathTemp))
                {
                    StreamReader str_tempmail = new StreamReader(pathTemp);
                    string line = str_tempmail.ReadLine();

                    if (!(line == null))
                    {
                        string[] strArray = line.Split('$');

                        txb_mailer.Text = strArray[0];
                        txb_receiver.Text = strArray[1];
                        txb_feedback.Text = strArray[2];
                    }
                }
            }
            catch
            {
                /// <summary>
                /// 捕捉到未知错误（这里一般是文件占用错误），跳过错误啥也不做
                /// </summary>
            }

            MainWindow.mailtempSave += new MainWindow.my_delegate(tempmail_save);

            App.IsmailLoaded = true;
        }

        private void btn_deleteAdnexa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_mail_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tempmail_save()
        {
            try
            {
                string pathTemp = App.pathDesktop + @"\考勤记录\temp_mail.txt";

                string tempmail;

                tempmail = txb_mailer.Text;
                tempmail += "$" + txb_receiver.Text;
                tempmail += "$" + txb_feedback.Text;

                StreamWriter stw_mail = new StreamWriter(pathTemp, false);
                stw_mail.WriteLine(tempmail);
                stw_mail.Close();
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
