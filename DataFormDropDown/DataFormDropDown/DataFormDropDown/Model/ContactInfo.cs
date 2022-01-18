using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataFormDropDown
{
    public class ContactInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String Name)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }
        public ContactInfo()
        {

        }

        public string Name { get; set; }

        private string country;
        public string Country
        {
            get { return this.country; }
            set
            {
                this.country = value;
                this.RaisePropertyChanged("Country");
            }
        }
    }
}
