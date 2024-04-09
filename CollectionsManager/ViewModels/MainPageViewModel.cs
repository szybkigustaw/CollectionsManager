using CollectionsManager.Models;
using CollectionsManager.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionsManager.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private DataService _dataService;
        private CollectionsModel _model;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand AddCollectionCommand { get; set; }

        public ICommand SaveDataCommand {  get; set; }
        public ICommand ImportDataCommand { get; set; }
        public ICommand ExportDataCommand { get; set; }

        public ICommand EditItemCommand { get; set; }
        public ICommand DeleteItemCommand { get; set; }

        public CollectionsModel Model { get => _model; set { SetProperty(ref _model, value); } }

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

        private async void AddCollection()
            => await Shell.Current.GoToAsync("///AddCollection");

        private async void SaveData()
        {
            _dataService.SaveData();
            await Application.Current.MainPage.DisplayAlert("Save data", "Data saved successfully", "OK");
        }

        private async void ExportData()
        {
            bool result = await _dataService.ExportData();
            if(result)
            {
                await Application.Current.MainPage.DisplayAlert("Export data", "Data exported successfully!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Export data", "Error occurred during data export!", "OK");
            }
        }

        private async void EditItem(Guid item_id)
        {
            var query = new ShellNavigationQueryParameters
            {
                { "editing", true },
                { "item_id", item_id }
            };

            await Shell.Current.GoToAsync("///AddItem", query);
        }

        private async void DeleteItem(Guid item_id)
        {
            bool result = await Application.Current.MainPage.DisplayAlert("Item remove", "Do you really want to delete this item?", "Yes", "No");
            if(result)
            {
                Guid collection_id = new Guid();
                foreach(var loop_collection in _model.Collections)
                {
                    if(loop_collection.Items.Where(i => i.Id == item_id).Any())
                    {
                        collection_id = loop_collection.Id;
                        break;
                    }
                }

                var collection = _model.Collections.First(x => x.Id == collection_id);
                _model.Collections.Remove(collection);
                 
            }
        }

        private async void ImportData()
        {
            _dataService.SuccesfulLoad += async (sender, e) =>
            {
                await Application.Current.MainPage.DisplayAlert("Import data", "Data imported successfully!", "OK");
            };

            _dataService.LoadError += async (sender, e) =>
            {
                await Application.Current.MainPage.DisplayAlert("Import data", $"Data import error! {e.Message}", "OK");
            };

            await _dataService.ImportData();
        }

        public MainPageViewModel(DataService dataService, CollectionsModel model)
        {
            _dataService = dataService;
            _model = model;

            AddCollectionCommand = new Command(AddCollection);
            SaveDataCommand = new Command(SaveData);
            ExportDataCommand = new Command(ExportData);
            ImportDataCommand = new Command(ImportData);

            EditItemCommand = new Command<Guid>(EditItem);
            DeleteItemCommand = new Command<Guid>(DeleteItem);
        }
    }
}
