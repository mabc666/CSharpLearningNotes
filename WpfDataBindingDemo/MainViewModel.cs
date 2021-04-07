using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace WpfDataBindingDemo
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public MainViewModel()
        {
            Name = "Hello World!";
            Task.Run(async()=> {
                await Task.Delay(3000);
                Name = "Changed Value";
            });
        }
        private String name;

        public String Name
        {
            get { return name; }
            set 
            { 
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string properName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(properName));
            }
        }
    }
}
