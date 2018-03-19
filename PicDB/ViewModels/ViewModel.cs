using System.ComponentModel;
using System.Windows;

namespace PicDB.ViewModels
{
    public abstract class ViewModel : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string prop)
        {
            var temp = PropertyChanged;
            temp?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}

