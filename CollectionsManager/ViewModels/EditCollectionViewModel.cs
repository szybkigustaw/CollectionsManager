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
    public class EditCollectionViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private CollectionsModel _model;
        private ItemsCollection _baseCollection;
        private string name;

        public string Name { get { return name; } set { SetProperty(ref name, value); } }
        public ICommand SaveCollectionCommand { get; set; }
        public ICommand ResetFieldsCommand { get; set; }
        public ICommand DeleteCollectionCommand { get; set; }
        public ICommand CancelCommand { get; set; }

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var collection_id = Guid.Parse(query["Id"].ToString());
            _baseCollection = _model.Collections.First(c => c.Id == collection_id);
            Name = _baseCollection.Name;
        }

        private async void SaveCollection()
        {
            var new_collection = new ItemsCollection(_baseCollection.Id, Name, _baseCollection.Items.ToList(), _baseCollection.CreationDate);
            _model.ReplaceCollection(_baseCollection, new_collection);
            Name = string.Empty;
            await Shell.Current.GoToAsync("///MainPage");
        }

        private void ResetFields()
            => Name = String.Empty;

        private async void DeleteCollection()
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Delete collection", "Do you really want to delete this collection?", "Yes", "No");
            if(answer)
            {
                _model.Collections.Remove(_baseCollection);
                await Shell.Current.GoToAsync("///MainPage");
            }
            else
            {
                return;
            }
        }

        private async void Cancel()
            => await Shell.Current.GoToAsync("///MainPage");
        public EditCollectionViewModel(CollectionsModel model)
        {
            _model = model;
            SaveCollectionCommand = new Command(SaveCollection);
            DeleteCollectionCommand = new Command(DeleteCollection);
            ResetFieldsCommand = new Command(ResetFields);
            CancelCommand = new Command(Cancel);
        }
    }
}
