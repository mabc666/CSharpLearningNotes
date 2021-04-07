using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WpfDataBindingDemo
{
    public class MainViewModelByMVVM:ViewModelBase
    {
        public MainViewModelByMVVM()
        {
            age = 0;
            SaveCommand = new RelayCommand(()=> {
                Age = 888;
            });
            Task.Run(async () => {
                await Task.Delay(3000);
                Age = 9999;
            });
        }

        private int age;

        public int Age
        {
            get { return age; }
            set { 
                age = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; private set; }
    }
}
