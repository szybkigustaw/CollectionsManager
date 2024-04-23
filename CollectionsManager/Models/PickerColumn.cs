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
    public class PickerColumn : BaseItemColumn<string>, INotifyPropertyChanged 
    {
        private ObservableCollection<PickerColumnOption> options;
        private PickerColumnOption value;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if(Object.Equals(property, value)) return false;
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public ObservableCollection<PickerColumnOption> Options { get { return options; } set { SetProperty(ref options, value); } }
        public PickerColumnOption Value { get { return value; } set { SetProperty(ref this.value, value); } }

        public PickerColumn(string name, List<string> options) : base(name) 
        {
            Name = name;
            Options = new ObservableCollection<PickerColumnOption>();
            foreach (var item in options)
            {
                Options.Add(new PickerColumnOption(item));
            }    
        }
        public PickerColumn(Guid id, string name, List<PickerColumnOption> options, PickerColumnOption value) : base(id, name) 
        {
            Id = id;
            Name = name;
            Value = value;
            Options = new ObservableCollection<PickerColumnOption>(options);
        }
    }

    public class PickerColumnOption
    {
        private Guid id;
        private string option;

        public Guid Id { get { return id; } private set { id = value; } }
        public string Option { get { return option; } set { option = value;} }

        public PickerColumnOption(string option)
        {
            Id = Guid.NewGuid();
            Option = option;
        }

        public PickerColumnOption(Guid id, string option)
        {
            Id = id;
            Option = option;
        }
    }
}
