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
    /// Interaction logic for Page_plan.xaml
    /// </summary>
    public partial class Page_plan : Page
    {
        public Page_plan()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            skp_possword.Visibility = Visibility.Visible;
            skp_content.Visibility = Visibility.Collapsed;
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
    }
}
