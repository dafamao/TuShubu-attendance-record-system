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
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.IO;

namespace TuShubu_attendance_record_system
{
    /// <summary>
    /// Interaction logic for Page_register.xaml
    /// </summary>
    public partial class Page_register : Page
    {
        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        private DispatcherTimer ShowTimer;

        private static ShowListViewData singleData;

        private static ShowListViewData removeData;

        DateTime workTimeC1 = DateTime.Parse(App.configuration.workTime[0]);
        DateTime workTimeC2 = DateTime.Parse(App.configuration.workTime[1]);
        DateTime workTimeC3 = DateTime.Parse(App.configuration.workTime[2]);
        DateTime workTimeA1 = DateTime.Parse(App.configuration.workTime[3]);
        DateTime workTimeA2 = DateTime.Parse(App.configuration.workTime[4]);
        DateTime workTimeA3 = DateTime.Parse(App.configuration.workTime[5]);
        DateTime workTimeB1 = DateTime.Parse(App.configuration.workTime[6]);
        DateTime workTimeB2 = DateTime.Parse(App.configuration.workTime[7]);
        DateTime workTimeB3 = DateTime.Parse(App.configuration.workTime[8]);
        DateTime workTimeEnd = DateTime.Parse("23:59");

        public Page_register()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShowTimer = new System.Windows.Threading.DispatcherTimer();
            ShowTimer.Tick += new EventHandler(my_ShowTimer);
            ShowTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            ShowTimer.Start();

            this.my_ListView.ItemsSource = App.ListViewData;

            my_time.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            my_time.AutoReset = false;

            cmb_group.ItemsSource = App.GroupComboBoxData;

            if (!(cmb_group.Items.Count == 0))
            {
                cmb_group.SelectedIndex = 0;
            }
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.btn_allout.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     btn_allout.Content = "全部签退";
                 }
                ));
        }

        private void my_ShowTimer(object sender, EventArgs e)
        {
            this.txb_time1.Text = "";
            this.txb_time2.Text = "";
            this.txb_time3.Text = "";

            this.txb_time1.Text = DateTime.Now.ToString("HH:mm:ss");
            this.txb_time2.Text = DateTime.Now.ToString("yyyy.MM.dd");
            this.txb_time3.Text = DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
        }

        private void cmb_group_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_group.SelectedItem == null) return;

            var ComboBox = (ComboBox)sender;
            var item = (ShowComboBoxData)ComboBox.SelectedItem;

            DatabaseOperate.bingMemberComboBoxData(item.Id);

            cmb_name.ItemsSource = App.MemberComboBoxData;
        }

        private void cmb_name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_name.SelectedItem == null) return;

            var ComboBox = (ComboBox)sender;
            var item = (ShowComboBoxData)ComboBox.SelectedItem;

            bindListViewData(item.Name.ToString());
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            if ((!(removeData == null)) && (!(removeData.Name == "启动时间：")) && (!(removeData.Name == "——————————")))
            {
                App.ListViewData.Remove(removeData);
                this.my_ListView.ItemsSource = App.ListViewData;
            }
        }

        private void btn_special_Click(object sender, RoutedEventArgs e)
        {
            if (txb_special.Text == "")
                return;
            bindListViewData(this.txb_special.Text);
        }

        private void btn_allout_Click(object sender, RoutedEventArgs e)
        {
            if (App.Isallout)
            {
                btn_allout.Content = "不是已经签过退了吗 ？！";
                my_time.Start();
            }
            else
            {
                int timeJudgeA1 = DateTime.Now.CompareTo(workTimeA1);
                int timeJudgeA2 = DateTime.Now.CompareTo(workTimeA2);
                int timeJudgeA3 = DateTime.Now.CompareTo(workTimeA3);
                int timeJudgeB1 = DateTime.Now.CompareTo(workTimeB1);
                int timeJudgeB2 = DateTime.Now.CompareTo(workTimeB2);
                int timeJudgeB3 = DateTime.Now.CompareTo(workTimeB3);
                int timeJudgeC1 = DateTime.Now.CompareTo(workTimeC1);
                int timeJudgeC2 = DateTime.Now.CompareTo(workTimeC2);
                int timeJudgeC3 = DateTime.Now.CompareTo(workTimeC3);
                int timeJudgeEnd = DateTime.Now.CompareTo(workTimeEnd);

                if (((timeJudgeC3 == 1 || timeJudgeC3 == 0) && timeJudgeA1 == -1) || ((timeJudgeA3 == 1 || timeJudgeA3 == 0) && timeJudgeB1 == -1) || ((timeJudgeB3 == 1 || timeJudgeB3 == 0) && timeJudgeEnd == -1))
                {
                    int length = App.ListViewData.Count();

                    if (length > 2)
                    {
                        singleData = new ShowListViewData("——————————", "——————————", "——————————————————————————");

                        App.ListViewData.Add(singleData);
                        this.my_ListView.ItemsSource = App.ListViewData;

                        ShowListViewData[] temp = new ShowListViewData[length + 1];
                        App.ListViewData.CopyTo(temp, 0);

                        for (int i = 2; i < length; i++)
                        {
                            singleData = new ShowListViewData(temp[i].Name, DateTime.Now.ToString("HH:mm:ss"), "签退成功，工作辛苦了(^_^)");
                            App.ListViewData.Add(singleData);
                            this.my_ListView.ItemsSource = App.ListViewData;
                        }

                        App.Isallout = true;
                    }

                    btn_allout.Content = "全部签退成功 ！";

                    my_time.Start();
                }
                else
                {
                    btn_allout.Content = "并不是下班时间 !";

                    my_time.Start();
                }
            }
        }

        private void my_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeData = null;
            removeData = my_ListView.SelectedItem as ShowListViewData;
        }

        private void btn_developerLink_Click(object sender, RoutedEventArgs e)
        {
            developerLink developerLink = new developerLink();
            developerLink.Show();
        }

        private void bindListViewData(string content)
        {
            int timeJudgeA1 = DateTime.Now.CompareTo(workTimeA1);
            int timeJudgeA2 = DateTime.Now.CompareTo(workTimeA2);
            int timeJudgeA3 = DateTime.Now.CompareTo(workTimeA3);
            int timeJudgeB1 = DateTime.Now.CompareTo(workTimeB1);
            int timeJudgeB2 = DateTime.Now.CompareTo(workTimeB2);
            int timeJudgeB3 = DateTime.Now.CompareTo(workTimeB3);
            int timeJudgeC1 = DateTime.Now.CompareTo(workTimeC1);
            int timeJudgeC2 = DateTime.Now.CompareTo(workTimeC2);
            int timeJudgeC3 = DateTime.Now.CompareTo(workTimeC3);

            if ((DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) == "星期六" && (DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) == "星期日")))
            {
                if (((timeJudgeA1 == 1 || timeJudgeA1 == 0) && timeJudgeA2 == -1) || ((timeJudgeB1 == 1 || timeJudgeB1 == 0) && timeJudgeB2 == -1) || ((timeJudgeC1 == 1 || timeJudgeC1 == 0) && timeJudgeC2 == -1))
                {
                    //headItem1.Content += "欢迎提前上岗，祝你工作愉快(^_^)";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "欢迎提前上岗，祝你工作愉快(^_^)");
                }
                else if (((timeJudgeA2 == 1 || timeJudgeA2 == 0) && timeJudgeA3 == -1) || ((timeJudgeB2 == 1 || timeJudgeB2 == 0) && timeJudgeB3 == -1) || ((timeJudgeC2 == 1 || timeJudgeC2 == 0) && timeJudgeC3 == -1))
                {
                    //headItem1.Content += "迟到！严重警告一次，请在规定上岗时间前签到";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "迟到！严重警告一次，请在规定上岗时间前签到");
                }
                else
                {
                    //headItem1.Content += "非工作时间，请在规定上岗时间前三十分钟内签到，谢谢";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "非工作时间，请在规定上岗时间前三十分钟内签到，谢谢");
                }
            }
            else
            {
                if (((timeJudgeA1 == 1 || timeJudgeA1 == 0) && timeJudgeA2 == -1) || ((timeJudgeB1 == 1 || timeJudgeB1 == 0) && timeJudgeB2 == -1))
                {
                    //headItem1.Content += "欢迎提前上岗，祝你工作愉快(^_^)";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "欢迎提前上岗，祝你工作愉快(^_^)");
                }
                else if (((timeJudgeA2 == 1 || timeJudgeA2 == 0) && timeJudgeA3 == -1) || ((timeJudgeB2 == 1 || timeJudgeB2 == 0) && timeJudgeB3 == -1))
                {
                    //headItem1.Content += "迟到！严重警告一次，请在规定上岗时间前签到";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "迟到！严重警告一次，请在规定上岗时间前签到");
                }
                else
                {
                    //headItem1.Content += "非工作时间，请在规定上岗时间前三十分钟内签到，谢谢";
                    singleData = new ShowListViewData(content, DateTime.Now.ToString("HH:mm:ss"), "非工作时间，请在规定上岗时间前三十分钟内签到，谢谢");
                }
            }

            //my_ListView.Items.Add(headItem1);

            App.ListViewData.Add(singleData);

            this.my_ListView.ItemsSource = App.ListViewData;
        }
    }
}
