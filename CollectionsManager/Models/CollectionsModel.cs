using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Models
{
    public class CollectionsModel : INotifyPropertyChanged
    {
        private ObservableCollection<ItemsCollection> _collections;

        public ObservableCollection<ItemsCollection> Collections { get { return _collections; } set { SetProperty(ref _collections, value); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if(Object.Equals(property, value))
            {
                return false;
            }

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
       
    }
}
