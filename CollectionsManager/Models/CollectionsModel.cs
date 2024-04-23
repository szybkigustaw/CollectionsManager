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
        private ObservableCollection<ItemsCollection> _collections = new ObservableCollection<ItemsCollection>();
        private int _collectionsCount;

        public ObservableCollection<ItemsCollection> Collections { get { return _collections; } set { SetProperty(ref _collections, value); SetProperty(ref _collectionsCount, value.Count()); } }
        public int CollectionsCount { get => _collectionsCount; set { SetProperty(ref _collectionsCount, value); } }

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
       
        public void AddCollection(ItemsCollection collection)
        {
            Collections.Add(collection);
            CollectionsCount = Collections.Count();
        }

        public void ReplaceCollection(ItemsCollection old_collection, ItemsCollection new_collection)
        {
            int index = Collections.IndexOf(old_collection);
            Collections.RemoveAt(index);
            Collections.Insert(index, new_collection);
            CollectionsCount = Collections.Count();
        }

        public void AddItem(ItemsCollection collection, Item item)
        {
            ItemsCollection new_collection = collection;
            new_collection.Items.Add(item);
            ReplaceCollection(collection, new_collection);
        }
    }
}
