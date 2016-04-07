using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace TuShubu_attendance_record_system
{
    class DatabaseOperate
    {
        private static string connection = @"Provider=Microsoft.Jet.OLEDB.4.0; Jet OLEDB:Database password=2015bookmanager; Data Source=..\..\Resource\memberManage.mdb";
        private static OleDbConnection my_connection = new OleDbConnection(connection);

        private static ShowComboBoxData singleData;

        public static void databaseOperate(string strSQL)
        {
            my_connection.Open();

            OleDbCommand my_command = new OleDbCommand();
            my_command.Connection = my_connection;
            my_command.CommandText = strSQL;
            my_command.ExecuteNonQuery();

            my_connection.Close();

            bindTreeViewData();
        }

        public static void bindTreeViewData()
        {
            my_connection.Open();

            OleDbDataAdapter memberData = new OleDbDataAdapter("select * from [member];", my_connection);
            DataSet my_memberSet = new DataSet();
            memberData.Fill(my_memberSet);
            DataTable memberTable = my_memberSet.Tables[0];

            App.TreeViewData.Clear();
            App.outputTreeView.Clear();
            App.GroupComboBoxData.Clear();

            for (int i = 0; i < memberTable.Rows.Count; i++)
            {
                DataRow tempRow = memberTable.Rows[i];

                if ((int)tempRow["parentID"] == -1)
                {
                    string groupSuffix = "";

                    int length = tempRow["name"].ToString().Length;

                    if (length <= 4)
                    {
                        for (int n = 0; n < 4 - length; n++)
                        {
                            groupSuffix += "    ";
                        }

                        groupSuffix += "组";
                    }

                    else
                    {
                        groupSuffix += "   组";
                    }

                    App.TreeViewData.Add(new ShowTreeViewData { ID = (int)tempRow["ID"], ParentID = (int)tempRow["parentID"], Name = tempRow["name"].ToString() + groupSuffix });
                    bindGroupComboBoxData((int)tempRow["ID"], tempRow["name"].ToString() + groupSuffix);
                }
                else
                {
                    App.TreeViewData.Add(new ShowTreeViewData { ID = (int)tempRow["ID"], ParentID = (int)tempRow["parentID"], Name = tempRow["name"].ToString() });
                }
            }

            my_connection.Close();

            App.outputTreeView = Bind(App.TreeViewData);
        }

        public static void bingMemberComboBoxData(int id)
        {
            my_connection.Open();

            OleDbDataAdapter memberData = new OleDbDataAdapter("select * from [member];", my_connection);
            DataSet my_memberSet = new DataSet();
            memberData.Fill(my_memberSet);
            DataTable memberTable = my_memberSet.Tables[0];

            App.MemberComboBoxData.Clear();

            for (int i = 0; i < memberTable.Rows.Count; i++)
            {
                DataRow tempRow = memberTable.Rows[i];

                if ((int)tempRow["parentID"] == id)
                {
                    singleData = new ShowComboBoxData((int)tempRow["ID"], tempRow["name"].ToString());
                    App.MemberComboBoxData.Add(singleData);
                }
            }

            my_connection.Close();
        }

        public static void UpdateConfig()
        {
            my_connection.Open();

            OleDbDataAdapter configurationData = new OleDbDataAdapter("select * from [configuration];", my_connection);
            DataSet my_configurationSet = new DataSet();
            configurationData.Fill(my_configurationSet);
            DataTable configurationTable = my_configurationSet.Tables[0];

            for (int i = 0; i < 13; i++)
            {
                if (i == 0)
                {
                    App.configuration.password = configurationTable.Rows[0][i + 1].ToString();
                }
                else if (i > 0 && i < 10)
                {
                    App.configuration.workTime[i - 1] = configurationTable.Rows[0][i + 1].ToString();
                }
                else
                {
                    App.configuration.mailServer[i - 10] = configurationTable.Rows[0][i + 1].ToString();
                }
            }

            my_connection.Close();
        }

        private static void bindGroupComboBoxData(int id, string name)
        {
            singleData = new ShowComboBoxData(id, name);
            App.GroupComboBoxData.Add(singleData);
        }

        /// <summary>
        /// 绑定树
        /// </summary>
        private static List<ShowTreeViewData> Bind(List<ShowTreeViewData> treeviewData)
        {
            List<ShowTreeViewData> outputList = new List<ShowTreeViewData>();

            for (int i = 0; i < treeviewData.Count; i++)
            {
                if (treeviewData[i].ParentID == -1)
                {
                    outputList.Add(treeviewData[i]);
                }
                else
                {
                    FindDownward(treeviewData, treeviewData[i].ParentID).ChildNodes.Add(treeviewData[i]);
                }
            }
            return outputList;
        }

        /// <summary>
        /// 递归向下查找
        /// </summary>
        private static ShowTreeViewData FindDownward(List<ShowTreeViewData> treeviewData, int id)
        {
            if (treeviewData == null) return null;

            for (int i = 0; i < treeviewData.Count; i++)
            {
                if (treeviewData[i].ID == id)
                {
                    return treeviewData[i];
                }

                ShowTreeViewData node = FindDownward(treeviewData[i].ChildNodes, id);

                if (node != null)
                {
                    return node;
                }
            }
            return null;
        }
    }
}
