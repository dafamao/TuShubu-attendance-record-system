using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuShubu_attendance_record_system
{
    public class ShowTreeViewData
    {
        public ShowTreeViewData()
        {
            this.ChildNodes = new List<ShowTreeViewData>();
            this.ParentID = -1;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public List<ShowTreeViewData> ChildNodes { get; set; }
    }
}

