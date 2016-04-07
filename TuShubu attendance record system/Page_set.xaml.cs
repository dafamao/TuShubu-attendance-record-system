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

namespace TuShubu_attendance_record_system
{
    /// <summary>
    /// Interaction logic for Page_set.xaml
    /// </summary>
    public partial class Page_set : Page
    {
        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        public Page_set()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            skp_possword.Visibility = Visibility.Visible;
            skp_content.Visibility = Visibility.Collapsed;

            my_time.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            my_time.AutoReset = false;

            string[] strTemp = App.configuration.workTime[1].Split(':');
            txb_amWorkTimeA.Text = strTemp[0];
            txb_amWorkTimeB.Text = strTemp[1];
            strTemp = App.configuration.workTime[2].Split(':');
            txb_amWorkTimeC.Text = strTemp[0];
            txb_amWorkTimeD.Text = strTemp[1];
            strTemp = App.configuration.workTime[4].Split(':');
            txb_pmWorkTimeA.Text = strTemp[0];
            txb_pmWorkTimeB.Text = strTemp[1];
            strTemp = App.configuration.workTime[5].Split(':');
            txb_pmWorkTimeC.Text = strTemp[0];
            txb_pmWorkTimeD.Text = strTemp[1];
            strTemp = App.configuration.workTime[7].Split(':');
            txb_evenWorkTimeA.Text = strTemp[0];
            txb_evenWorkTimeB.Text = strTemp[1];
            strTemp = App.configuration.workTime[8].Split(':');
            txb_evenWorkTimeC.Text = strTemp[0];
            txb_evenWorkTimeD.Text = strTemp[1];

            txb_mailAccount.Text = App.configuration.mailServer[0];
            txbl_mailPassword.Password = App.configuration.mailServer[1];
            txb_mailServer.Text = App.configuration.mailServer[2];
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.btn_confirmModifyPassword.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     btn_confirmModifyPassword.Content = "确定";
                     btn_confirmModifyWorkTime.Content = "确定";
                     btn_confirmModifyMailServer.Content = "确定";
                 }
                ));
        }

        private void pwb_password_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txbl_prompt.Text == "密码错误")
            {
                txbl_prompt.Text = "请输入管理员密码";
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            if (pwb_password.Password == App.configuration.password)
            {
                skp_possword.Visibility = Visibility.Collapsed;
                skp_content.Visibility = Visibility.Visible;
            }
            else
            {
                txbl_prompt.Text = "密码错误";
                pwb_password.Password = "";
            }
        }

        private void btn_confirmModifyPassword_Click(object sender, RoutedEventArgs e)
        {
            if (txbl_newPassword.Password == "" || txbl_confirmPassword.Password == "")
            {
                return;
            }

            if (!(txbl_newPassword.Password == txbl_confirmPassword.Password))
            {
                txbl_newPassword.Password = "";
                txbl_confirmPassword.Password = "";

                btn_confirmModifyPassword.Content = "输入不一致 ！";
                my_time.Start();
            }
            else
            {
                string strSQL = " update [configuration] set [password] = '" + txbl_confirmPassword.Password + "'";
                DatabaseOperate.databaseOperate(strSQL);

                DatabaseOperate.UpdateConfig();

                btn_confirmModifyPassword.Content = "修改成功 ！";
                my_time.Start();
            }
        }

        private void btn_confirmModifyWorkTime_Click(object sender, RoutedEventArgs e)
        {
            if (txb_amWorkTimeA.Text == ""
                || txb_amWorkTimeB.Text == ""
                || txb_amWorkTimeC.Text == ""
                || txb_amWorkTimeD.Text == ""
                || txb_pmWorkTimeA.Text == ""
                || txb_pmWorkTimeB.Text == ""
                || txb_pmWorkTimeC.Text == ""
                || txb_pmWorkTimeD.Text == ""
                || txb_evenWorkTimeA.Text == ""
                || txb_evenWorkTimeB.Text == ""
                || txb_evenWorkTimeC.Text == ""
                || txb_evenWorkTimeD.Text == "")
            {
                btn_confirmModifyWorkTime.Content = "输入不能为空 ！";
                my_time.Start();
                return;
            }

            else
            {
                string[] WorkTime = new string[9];
                string[] WorkTimeTable = new string[] { "amWorkTime", "amWorkTimeStart", "amWorkTimeEnd", "pmWorkTime", "pmWorkTimeStart", "pmWorkTimeEnd", "evenWorkTime", "evenWorkTimeStart", "evenWorkTimeEnd" };

                if (Convert.ToInt16(txb_amWorkTimeB.Text) >= 30)
                {
                    if ((Convert.ToInt16(txb_amWorkTimeB.Text) - 30) < 10)
                    {
                        WorkTime[0] = txb_amWorkTimeA.Text + ":" + "0" + (Convert.ToInt16(txb_amWorkTimeB.Text) - 30).ToString();
                    }
                    else
                    {
                        WorkTime[0] = txb_amWorkTimeA.Text + ":" + (Convert.ToInt16(txb_amWorkTimeB.Text) - 30).ToString();
                    }
                }
                else
                {
                    WorkTime[0] = (Convert.ToUInt32(txb_amWorkTimeA.Text) - 1).ToString() + ":" + (Convert.ToInt16(txb_amWorkTimeB.Text) + 30).ToString();
                }
                WorkTime[1] = txb_amWorkTimeA.Text + ":" + txb_amWorkTimeB.Text;
                WorkTime[2] = txb_amWorkTimeC.Text + ":" + txb_amWorkTimeD.Text;

                if (Convert.ToInt16(txb_pmWorkTimeB.Text) >= 30)
                {
                    if ((Convert.ToInt16(txb_pmWorkTimeB.Text) - 30) < 10)
                    {
                        WorkTime[3] = txb_pmWorkTimeA.Text + ":" + "0" + (Convert.ToInt16(txb_pmWorkTimeB.Text) - 30).ToString();
                    }
                    else
                    {
                        WorkTime[3] = txb_pmWorkTimeA.Text + ":" + (Convert.ToInt16(txb_pmWorkTimeB.Text) - 30).ToString();
                    }
                }
                else
                {
                    WorkTime[3] = (Convert.ToUInt32(txb_pmWorkTimeA.Text) - 1).ToString() + ":" + (Convert.ToInt16(txb_pmWorkTimeB.Text) + 30).ToString();
                }
                WorkTime[4] = txb_pmWorkTimeA.Text + ":" + txb_pmWorkTimeB.Text;
                WorkTime[5] = txb_pmWorkTimeC.Text + ":" + txb_pmWorkTimeD.Text;

                if (Convert.ToInt16(txb_evenWorkTimeB.Text) >= 30)
                {
                    if (Convert.ToInt16(txb_evenWorkTimeB.Text) < 40)
                    {
                        WorkTime[6] = txb_evenWorkTimeA.Text + ":" + "0" + (Convert.ToInt16(txb_evenWorkTimeB.Text) - 30).ToString();
                    }
                    else
                    {
                        WorkTime[6] = txb_evenWorkTimeA.Text + ":" + (Convert.ToInt16(txb_evenWorkTimeB.Text) - 30).ToString();
                    }
                }
                else
                {
                    WorkTime[6] = (Convert.ToUInt32(txb_evenWorkTimeA.Text) - 1).ToString() + ":" + (Convert.ToInt16(txb_evenWorkTimeB.Text) + 30).ToString();
                }
                WorkTime[7] = txb_evenWorkTimeA.Text + ":" + txb_evenWorkTimeB.Text;
                WorkTime[8] = txb_evenWorkTimeC.Text + ":" + txb_evenWorkTimeD.Text;

                for (int i = 0; i < 9; i++)
                {
                    string strSQL = " update [configuration] set [" + WorkTimeTable[i] + "] = '" + WorkTime[i] + "'";
                    DatabaseOperate.databaseOperate(strSQL);
                }

                DatabaseOperate.UpdateConfig();

                btn_confirmModifyWorkTime.Content = "修改成功 ！";
                my_time.Start();
            }
        }

        private void btn_confirmModifyMailServer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
