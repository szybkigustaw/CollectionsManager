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
    public class ItemsCollection : INotifyPropertyChanged
    {
        private Guid id;
        private string name;
        private ObservableCollection<Item> items;
        private DateTime creation_date;
        private DateTime modification_date;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(property, value)) return false;
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public Guid Id { get { return id; } private set { SetProperty(ref id, value); SetProperty(ref modification_date, DateTime.Now);  } }
        public string Name { get { return name; } set { SetProperty(ref name, value); SetProperty(ref modification_date, DateTime.Now);  } }
        public ObservableCollection<Item> Items { get {  return items; } set { SetProperty(ref items, value); SetProperty(ref modification_date, DateTime.Now); } }
        public DateTime CreationDate { get { return creation_date; } private set { SetProperty(ref creation_date, value); } }
        public DateTime ModificationDate { get { return modification_date; } set { SetProperty(ref modification_date, value); } }

        public ItemsCollection(string name, List<Item> items)
        {
            Id = Guid.NewGuid();
            Name = name;
            Items = new ObservableCollection<Item>(items);
            CreationDate = DateTime.Now;
            ModificationDate = DateTime.Now;
        }

        public ItemsCollection(Guid id, string name, List<Item> items, DateTime creation_date) : this(name, items) 
        {
            Id = id;
            Name = name;
            Items = new ObservableCollection<Item>(items);
            CreationDate = creation_date;
            ModificationDate = DateTime.Now;
        }
    }
}
