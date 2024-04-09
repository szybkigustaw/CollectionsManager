using CollectionsManager.Models;
using CollectionsManager.Pages;
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
    public class AddItemViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private DataService _dataService;

        private bool editing;
        private Guid item_id;
        private CollectionsModel _model;
        private Guid _collection_id;
        private string name;
        private string image;
        private DateTime creation_date;
        private ObservableCollection<TextColumn> text_columns = new ObservableCollection<TextColumn>();
        private ObservableCollection<NumberColumn> number_columns = new ObservableCollection<NumberColumn>();
        private ObservableCollection<PickerColumn> picker_columns = new ObservableCollection<PickerColumn>();

        private string imagePath;

        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand SaveItemCommand { get; set; }
        public ICommand ResetFieldsCommand { get; set; }
        public ICommand CancelCommand {  get; set; }

        public ICommand SelectImageFromFileCommand {  get; set; }

        public ICommand AddTextColumnCommand { get; set; }
        public ICommand RemoveTextColumnCommand { get; set; }

        public ICommand AddNumberColumnCommand { get; set; }
        public ICommand RemoveNumberColumnCommand { get; set; }

        public ICommand AddPickerColumnCommand { get; set; }
        public ICommand RemovePickerColumnCommand { get; set; }

        public ICommand AddPickerColumnOptionCommand { get; set; }
        public ICommand RemovePickerColumnOptionCommand { get; set; }

        public Guid CollectionId { get => _collection_id; set { SetProperty(ref _collection_id, value); } }
        public String Name { get => name; set { SetProperty(ref name, value); } }
        public String Image { get => image; set { SetProperty(ref image, value); } }
        public ObservableCollection<TextColumn> TextColumns { get => text_columns; set { SetProperty(ref text_columns, value); } }
        public ObservableCollection<NumberColumn> NumberColumns { get => number_columns; set { SetProperty(ref number_columns, value); } }
        public ObservableCollection<PickerColumn> PickerColumns { get => picker_columns; set { SetProperty(ref picker_columns, value);  } }

        public string ImagePath { get => imagePath; set { SetProperty(ref imagePath, value); } }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        private bool SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if(Object.Equals(property,value)) 
            {
                return false;
            }
            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            this.editing = (bool)query["editing"];
            if (editing)
            {
                item_id = Guid.Parse(query["item_id"].ToString());
                foreach(var collection in _model.Collections)
                {
                    if(collection.Items.Where(c => c.Id == item_id).Any())
                    {
                        _collection_id = collection.Id;
                        break;
                    }
                }

                Item edited_item = _model.Collections.First(c => c.Id == _collection_id)
                                            .Items.First(i => i.Id == item_id);

                Name = edited_item.Name;
                Image = edited_item.Image;
                creation_date = edited_item.AddDate;
                TextColumns = new ObservableCollection<TextColumn>(edited_item.TextColumns);
                NumberColumns = new ObservableCollection<NumberColumn>(edited_item.NumberColumns);
                PickerColumns = new ObservableCollection<PickerColumn>(edited_item.PickerColumns);

                ImagePath = "Photo already chosen";
            }
            else
            {
                _collection_id = Guid.Parse(query["CollectionId"].ToString());
                BootstrapItem();
            }
        }

        private async void SelectImageFromFile()
        {
            var file_types = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                {DevicePlatform.WinUI, new[] { ".jpg", ".png" } }
            });

            var picker_options = new PickOptions
            {
                PickerTitle = "Select an image",
                FileTypes = file_types
            };

            try
            {
                var result = await FilePicker.Default.PickAsync(picker_options);
                if(result != null)
                {
                    if(result.FullPath.EndsWith("png", StringComparison.OrdinalIgnoreCase) || 
                        result.FullPath.EndsWith("jpg", StringComparison.OrdinalIgnoreCase))
                    {
                        ImagePath = result.FileName;
                        byte[] imageArray = File.ReadAllBytes(result.FullPath);
                        Image = Convert.ToBase64String(imageArray);
                    }
                }
            }
            catch(Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Select image error", $"Error occured during image selection. \n {ex.Message}", "OK");
            }
        }

       private void AddTextColumn()
        {
            TextColumns.Add(new TextColumn($"New column {TextColumns.Count()}"));
        }

        private void RemoveTextColumn(Guid id)
        {
            var text_column = TextColumns.First(x => x.Id == id);
            TextColumns.Remove(text_column);
        }

       private void AddNumberColumn()
        {
            NumberColumns.Add(new NumberColumn($"New column {NumberColumns.Count()}"));
        }
        private void RemoveNumberColumn(Guid id)
        {
            var number_column = NumberColumns.First(x => x.Id == id);
            NumberColumns.Remove(number_column);
        }

       private void AddPickerColumn()
        {
            PickerColumns.Add(new PickerColumn($"New column {PickerColumns.Count()}", new List<string>()));
        }
        private void RemovePickerColumn(Guid id)
        {
            var picker_column = PickerColumns.First(x => x.Id == id);
            PickerColumns.Remove(picker_column);
        }

        private void AddPickerColumnOption(Guid id)
        {
            var column = PickerColumns.First(c => c.Id == id);
            int index = PickerColumns.IndexOf(column);
            column.Options.Add(new PickerColumnOption($"New option {column.Options.Count()}"));
            PickerColumns.RemoveAt(index);
            PickerColumns.Insert(index, column);
        }

        private async void RemovePickerColumnOption(Guid option_id)
        {
            PickerColumn column = null;
            foreach(var picker_column in PickerColumns)
            {
                if(picker_column.Options.Where(o => o.Id == option_id).Any())
                {
                    column = picker_column; break;
                }
            }
            if (column == null)
            {
                await Application.Current.MainPage.DisplayAlert("Remove picker option", "Internal error", "OK");
                return;
            }

            PickerColumnOption option = column.Options.First(o => o.Id == option_id);
            int index = PickerColumns.IndexOf(column);
            column.Options.Remove(option);
            PickerColumns.RemoveAt(index);
            PickerColumns.Insert(index, column);
        }

        private async void SaveItem()
        {

            if (editing)
            {
                Item new_item = new Item(item_id, Name, Image, TextColumns.ToList(), NumberColumns.ToList(), PickerColumns.ToList(), creation_date);
                var collection = _model.Collections.First(x => x.Id == _collection_id);
                Item item = collection.Items.First(x => x.Id == item_id);
                var new_collection = _model.Collections.First(x => x.Id == _collection_id);

                var index = new_collection.Items.IndexOf(item);
                new_collection.Items.RemoveAt(index);
                new_collection.Items.Insert(index, new_item);
                new_collection = _dataService.NormalizeColumns(new_collection);
                new_collection = _dataService.MoveSoldToEnd(new_collection);
                _model.ReplaceCollection(collection, new_collection);

                Debug.WriteLine($"Collections count: {_model.CollectionsCount}");
                Debug.WriteLine($"Items count: {_model.Collections.First(x => x.Id == _collection_id).Items.Count()}");
            }
            else
            {
                Item item = new Item(Name, Image, TextColumns.ToList(), NumberColumns.ToList(), PickerColumns.ToList());
                var collection = _model.Collections.First(x => x.Id == _collection_id);
                var new_collection = _model.Collections.First(x => x.Id == _collection_id);

                if(collection.Items.Where(i => 
                    i.Name == item.Name 
                ).Any())
                {
                    bool result = await Application.Current.MainPage.DisplayAlert(
                        "Save item",
                        "It looks like you already have this item in your collection. Proceed anyway?",
                        "Yes",
                        "No"
                        );
                    if(!result)
                    {
                        return;
                    }
                }

                new_collection.Items.Add(item);
                new_collection = _dataService.NormalizeColumns(new_collection);
                new_collection = _dataService.MoveSoldToEnd(new_collection);
                _model.ReplaceCollection(collection, new_collection);
#if DEBUG
                Debug.WriteLine($"Collections count: {_model.CollectionsCount}");
                Debug.WriteLine($"Items count: {_model.Collections.First(x => x.Id == _collection_id).Items.Count()}");
#endif
            }
            ResetFields();
            await Shell.Current.GoToAsync("///MainPage");
        }

        private void ResetFields()
        {
            Name = string.Empty;
            Image = string.Empty;
            ImagePath = string.Empty;
            TextColumns = new ObservableCollection<TextColumn>();
            NumberColumns = new ObservableCollection<NumberColumn>();
            PickerColumns = new ObservableCollection<PickerColumn>();
        }

        private async void Cancel()
        {
            ResetFields();
            await Shell.Current.GoToAsync("///MainPage");
        }

        private void BootstrapItem()
        {
            TextColumns.Add(new TextColumn("Comment"));

            NumberColumns.Add(new NumberColumn("Price"));

            PickerColumns.Add(new PickerColumn("Status", new List<string>
            {
                "New", "Used", "To sale", "Sold", "Want to buy"
            }));

            PickerColumns.Add(new PickerColumn("Rating", new List<string>
            {
                "1","2","3","4","5","6","7","8","9","10"
            }));
        }

        public AddItemViewModel(CollectionsModel model, DataService dataService) 
        {
            _model = model;
            _dataService = dataService;
            SelectImageFromFileCommand = new Command(SelectImageFromFile);

            AddTextColumnCommand = new Command(AddTextColumn);
            RemoveTextColumnCommand = new Command<Guid>(RemoveTextColumn);

            AddNumberColumnCommand = new Command(AddNumberColumn);
            RemoveNumberColumnCommand = new Command<Guid>(RemoveNumberColumn);

            AddPickerColumnCommand = new Command(AddPickerColumn);
            RemovePickerColumnCommand = new Command<Guid>(RemovePickerColumn);
            AddPickerColumnOptionCommand = new Command<Guid>(AddPickerColumnOption);

            SaveItemCommand = new Command(SaveItem);
            ResetFieldsCommand = new Command(ResetFields);
            CancelCommand = new Command(Cancel);
        }
    }
}
