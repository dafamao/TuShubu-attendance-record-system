using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TuShubu_attendance_record_system
{
    public class ShowComboBoxData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string name;

        private int id;

        public ShowComboBoxData(int id, string name)
        {
            this.Name = name;

            this.Id = id;
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

        public int Id
        {
            get { return this.id; }

            set
            {
                this.id = value;

                OnPropertyChanged("Id");
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