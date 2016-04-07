using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ObservableCollection<ShowListViewData> ListViewData = new ObservableCollection<ShowListViewData>();

        public static ObservableCollection<ShowComboBoxData> GroupComboBoxData = new ObservableCollection<ShowComboBoxData>();
        public static ObservableCollection<ShowComboBoxData> MemberComboBoxData = new ObservableCollection<ShowComboBoxData>();

        public static List<ShowTreeViewData> TreeViewData = new List<ShowTreeViewData>();
        public static List<ShowTreeViewData> outputTreeView = new List<ShowTreeViewData>();

        public static bool IsblogLoaded = false;
        public static bool IsmailLoaded = false;
        public static bool IsadviceLoaded = false;

        public static bool IsblogSaveClick = false;
        public static bool IsblogTextHave = false;

        public static bool IsadviceSaveClick = false;
        public static bool IsadviceTextHave = false;

        public static bool Isallout = false;

        public static string pathDesktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public struct configuration
        {
            public static string password;
            public static string[] workTime = new string[9];
            public static string[] mailServer = new string[3];
        }
    }
}
