using CollectionsManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionsManager.ViewModels
{
    public class AddCollectionViewModel : INotifyPropertyChanged
    {
        private string name;
        private CollectionsModel _model;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string Name { get { return name; } set { SetProperty(ref name, value); } }
        public ICommand AddCollectionCommand { get; set; }
        public ICommand ResetFieldsCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if(Object.Equals(property, value)) return false;
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private async void AddCollection()
        {
            if(_model.Collections.Count > 0 && _model.Collections.Where(c => c.Name == Name).Count() > 0)
            {
                await Application.Current.MainPage.DisplayAlert("Add collection error", "There is already a collection with the same name!", "OK");
                return;
            }
            ItemsCollection collection = new ItemsCollection(Name, []);
            _model.AddCollection(collection);
            Name = string.Empty;
            await Shell.Current.GoToAsync("///MainPage");
        }

        private void ResetFields()
            => Name = String.Empty;

        private async void Cancel()
            => await Shell.Current.GoToAsync("///MainPage");

        public AddCollectionViewModel(CollectionsModel model)
        {
            _model = model;
            AddCollectionCommand = new Command(AddCollection);
            ResetFieldsCommand = new Command(ResetFields);
            CancelCommand = new Command(Cancel);

        }
    }
}
