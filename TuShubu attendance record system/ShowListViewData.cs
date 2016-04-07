using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TuShubu_attendance_record_system
{
    public class ShowListViewData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;

        private string time;

        private string remarks;

        /// <summary>
        /// ID
        /// </summary>
        /// 
        public ShowListViewData(string name, string time, string remarks)
        {
            this.Name = name;

            this.Time = time;

            this.Remarks = remarks;
        }

        public string Name
        {
            get { return this.name; }

            set
            {
                this.name = value;

                OnPropertyChanged("Name");
            }
        }

        public string Time
        {
            get { return this.time; }

            set
            {
                this.time = value;

                OnPropertyChanged("Time");
            }
        }


        public string Remarks
        {
            get { return this.remarks; }

            set
            {
                this.remarks = value;

                OnPropertyChanged("Remarks");
            }
        }

        public void OnPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
