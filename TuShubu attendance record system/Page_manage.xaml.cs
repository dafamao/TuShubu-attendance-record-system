using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.Windows.Markup;
using System.Data;
using System.Data.OleDb;

namespace TuShubu_attendance_record_system
{
    /// <summary>
    /// Interaction logic for Page_manage.xaml
    /// </summary>
    public partial class Page_manage : Page
    {
        private System.Timers.Timer my_time = new System.Timers.Timer(2000);

        private ShowTreeViewData selectDataStar;
        private ShowTreeViewData selectDataEnd;

        private ShowComboBoxData whichGroup;

        private int selectIndexStar = 0;
        private int selectIndexEnd = 0;

        public Page_manage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            my_treeview.Visibility = Visibility.Collapsed;
            RectangleA.Visibility = Visibility.Visible;
            RectangleB.Visibility = Visibility.Collapsed;
            RectangleC.Visibility = Visibility.Collapsed;
            skp_possword.Visibility = Visibility.Visible;
            skp_content.Visibility = Visibility.Collapsed;

            this.cmb_whichgroup.ItemsSource = App.GroupComboBoxData;
            this.my_treeview.ItemsSource = App.outputTreeView;

            my_time.Elapsed += new System.Timers.ElapsedEventHandler(Timer_TimesUp);
            my_time.AutoReset = false;

            if (!(cmb_whichgroup.Items.Count == 0))
            {
                cmb_whichgroup.SelectedIndex = 0;
            }
        }

        private void Timer_TimesUp(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.btn_addmember.Dispatcher.Invoke(
            new Action(
                 delegate
                 {
                     btn_addmember.Content = "添加";
                     btn_delete.Content = "删除所选";
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
                my_treeview.Visibility = Visibility.Visible;
                RectangleA.Visibility = Visibility.Collapsed;
                RectangleB.Visibility = Visibility.Visible;
                RectangleC.Visibility = Visibility.Visible;
                skp_possword.Visibility = Visibility.Collapsed;
                skp_content.Visibility = Visibility.Visible;
            }
            else
            {
                txbl_prompt.Text = "密码错误";
                pwb_password.Password = "";
            }
        }

        private void my_treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {
                selectDataStar = my_treeview.SelectedItem as ShowTreeViewData;
                txb_modify.Text = selectDataStar.Name;
            }

            catch
            { }
            
        }

        private void btn_addgroup_Click(object sender, RoutedEventArgs e)
        {
            txb_addgroup.Text = txb_addgroup.Text.Replace(" ", "");

            if (txb_addgroup.Text == "") return;

            if (txb_addgroup.Text[txb_addgroup.Text.Length - 1].ToString() == "组")
            {
                txb_addgroup.Text = txb_addgroup.Text.Replace(" ", "").TrimEnd('组');
            }

            string strSQL = "insert into [member](parentID,name) values('-1', '" + txb_addgroup.Text + "')";
            DatabaseOperate.databaseOperate(strSQL);

            this.cmb_whichgroup.ItemsSource = App.GroupComboBoxData;
            this.my_treeview.ItemsSource = App.outputTreeView;

            if (!(selectIndexEnd == -1))
            {
                cmb_whichgroup.SelectedIndex = selectIndexEnd;
            }
        }

        private void cmb_whichgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmb_whichgroup.SelectedItem == null) return;

            whichGroup = cmb_whichgroup.SelectedItem as ShowComboBoxData;
            selectIndexStar = cmb_whichgroup.SelectedIndex;
        }

        private void btn_addmember_Click(object sender, RoutedEventArgs e)
        {
            txb_addmember.Text = txb_addmember.Text.Replace(" ", "");

            if (txb_addmember.Text == "" || cmb_whichgroup.SelectedItem == null) return;

            selectIndexEnd = selectIndexStar;

            string strSQL = "insert into [member](parentID,name) values('" + whichGroup.Id + "', '" + txb_addmember.Text + "')";
            DatabaseOperate.databaseOperate(strSQL);

            btn_addmember.Content = "添加成功 !";
            my_time.Start();

            this.cmb_whichgroup.ItemsSource = App.GroupComboBoxData;
            this.my_treeview.ItemsSource = App.outputTreeView;

            if (!(selectIndexEnd == -1))
            {
                cmb_whichgroup.SelectedIndex = selectIndexEnd;
            }
        }

        private void btn_modify_Click(object sender, RoutedEventArgs e)
        {
            txb_modify.Text = txb_modify.Text.Replace(" ", "");

            if (!(txb_modify.Text == "") && !(selectDataStar == null) && !(selectDataStar.Name == txb_modify.Text))
            {
                selectDataEnd = selectDataStar;

                if (txb_modify.Text[txb_modify.Text.Length - 1].ToString() == "组")
                {
                    txb_modify.Text = txb_modify.Text.Replace(" ", "").TrimEnd('组');
                }

                string strSQL = " update [member] set [name] = '" + txb_modify.Text + "' where [ID] = " + selectDataEnd.ID + " ";
                DatabaseOperate.databaseOperate(strSQL);

                this.cmb_whichgroup.ItemsSource = App.GroupComboBoxData;
                this.my_treeview.ItemsSource = App.outputTreeView;
            }

            if (!(selectIndexEnd == -1))
            {
                cmb_whichgroup.SelectedIndex = selectIndexEnd;
            }
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            btn_delete.Content = "确认删除请连击两次 ！";
            my_time.Start();
        }

        private void btn_delete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(selectDataStar == null))
            {
                selectDataEnd = selectDataStar;

                if (selectDataEnd.ParentID == -1)
                {
                    string _strSQL = " delete from [member] where [parentID] = " + selectDataEnd.ID + " ";
                    DatabaseOperate.databaseOperate(_strSQL);
                }

                string strSQL = " delete from [member] where [ID] = " + selectDataEnd.ID + " ";
                DatabaseOperate.databaseOperate(strSQL);

                this.cmb_whichgroup.ItemsSource = App.GroupComboBoxData;
                this.my_treeview.ItemsSource = App.outputTreeView;
            }

            if (!(selectIndexEnd == -1))
            {
                cmb_whichgroup.SelectedIndex = selectIndexEnd;
            }
        }
    }
}
