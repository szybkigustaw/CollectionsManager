using CollectionsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsManager.Services
{
    public class DataService
    {
        private FileService _fileService;
        private CollectionsModel _model;

        private bool _loading_collection = false;
        public bool LoadingCollection { get => _loading_collection; private set => _loading_collection = value; }

        private static FilePickerFileType FILE_TYPE = new FilePickerFileType(
                new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    {DevicePlatform.WinUI, new[] { ".txt" } }
                }
            );

        private static PickOptions PICK_OPTIONS = new PickOptions()
        {
            FileTypes = FILE_TYPE
        };

        public event EventHandler SuccesfulLoad;
        public event EventHandler<Exception> LoadError;

        private void OnSucessfulLoad()
            => SuccesfulLoad?.Invoke(this, EventArgs.Empty);

        private void OnLoadError(Exception ex)
            => LoadError?.Invoke(this, ex);


        public DataService(FileService fileService, CollectionsModel model)
        {
            _fileService = fileService;
            _model = model;

            _fileService.FilesLoaded += ApplyData;
            _fileService.LoadingError += HandleFileLoadingError;
        }

        public void SaveData()
            => _fileService.SaveData(_model.Collections.ToList());

        public void LoadData()
            => _fileService.LoadData();

        public async Task<bool> ExportData()
        {
            try 
            {
                var result = await FilePicker.Default.PickAsync(PICK_OPTIONS);
                if(result != null) 
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase) )
                    {
                        _fileService.SaveDataTo(_model.Collections.ToList(), result.FullPath);
                        return true;
                    }
                }

                return false;
            }

            catch 
            {
                return false;
            }
        }

        public async Task<bool> ExportCollection(ItemsCollection collection)
        {
            try 
            {
                var result = await FilePicker.Default.PickAsync(PICK_OPTIONS);
                if(result != null) 
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase) )
                    {
                        _fileService.SaveDataTo(new List<ItemsCollection> { collection }, result.FullPath);
                        return true;
                    }
                }

                return false;
            }

            catch 
            {
                return false;
            }
        }

        public async Task<bool> ImportData()
        {
            LoadingCollection = true;
            try 
            {
                var result = await FilePicker.Default.PickAsync(PICK_OPTIONS);
                if(result != null) 
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase) )
                    {
                        _fileService.LoadDataFrom(result.FullPath);
                        SaveData();
                        return true;
                    }
                }

                return false;
            }

            catch 
            {
                return false;
            }
        }

        public async Task<bool> ImportCollection()
        {
            
            try 
            {
                var result = await FilePicker.Default.PickAsync(PICK_OPTIONS);
                if(result != null) 
                {
                    if (result.FileName.EndsWith("txt", StringComparison.OrdinalIgnoreCase) )
                    {
                        _fileService.LoadDataFrom(result.FullPath);
                        SaveData();
                        return true;
                    }
                }

                return false;
            }

            catch 
            {
                return false;
            }
        }

        private void ApplyData(object sender, List<ItemsCollection> collections)
        {
            try
            {
                if (LoadingCollection)
                {
                    foreach (ItemsCollection collection in collections)
                    {
                        if (_model.Collections.Where(c => c.Id == collection.Id).Count() > 0)
                        {
                            ItemsCollection altered_collection = new ItemsCollection(Guid.NewGuid(), collection.Name, collection.Items.ToList(), collection.CreationDate);
                            _model.AddCollection(altered_collection);
                        }
                        else
                        {
                            _model.AddCollection(collection);
                        }
                    }

                    LoadingCollection = false;
                    OnSucessfulLoad();
                }
                else
                {
                    _model.Collections = new System.Collections.ObjectModel.ObservableCollection<ItemsCollection>(collections);
                }
            }
            catch (Exception ex)
            {
                OnLoadError(ex);
            }
        }

        private void HandleFileLoadingError(object sender, Exception ex)
            => OnLoadError(ex);

        public ItemsCollection MoveSoldToEnd(ItemsCollection collection)
        {
            List<Item> itemsList = collection.Items.ToList();

            List<Item> soldItems = itemsList.Where(i => i.PickerColumns.Where(c => c.Name == "Status" && c.Value.Option == "Sold").Any()).ToList();
            List<Item> unsoldItems = itemsList.Where(
                    i => i.PickerColumns.Where(
                            c => c.Name == "Status" && c.Value.Option == "Sold"
                        ).Count() == 0 ||
                        i.PickerColumns.Where(c => c.Name == "Status").Count() == 0
                ).ToList();

            List<Item> sortedList = new List<Item>();
            sortedList.AddRange(unsoldItems);
            sortedList.AddRange(soldItems);

            collection.Items = new System.Collections.ObjectModel.ObservableCollection<Item>(sortedList);
            return collection;
        }

        public ItemsCollection NormalizeColumns(ItemsCollection collection)
        {
            List<TextColumn> textColumns = collection.Items.First().TextColumns;
            List<NumberColumn> numberColumns = collection.Items.First().NumberColumns;
            List<PickerColumn> pickerColumns = collection.Items.First().PickerColumns;

            foreach(var item in collection.Items)
            {
                List<TextColumn> textColumnsMissingInItem = textColumns.Where(
                        t => item.TextColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();
                List<TextColumn> textColumnsMissingInList = item.TextColumns.Where(
                        t => textColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();

                foreach(var textColumn in textColumnsMissingInItem)
                {
                    item.TextColumns.Add(new TextColumn(textColumn.Name));
                }
                foreach(var textColumn in textColumnsMissingInList)
                {
                    textColumns.Add(new TextColumn(textColumn.Name));
                }

                List<NumberColumn> numberColumnsMissingInItem = numberColumns.Where(
                        t => item.NumberColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();
                List<NumberColumn> numberColumnsMissingInList = item.NumberColumns.Where(
                        t => numberColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();

                foreach(var numberColumn in numberColumnsMissingInItem)
                {
                    item.NumberColumns.Add(new NumberColumn(numberColumn.Name));
                }
                foreach(var numberColumn in numberColumnsMissingInList)
                {
                    numberColumns.Add(new NumberColumn(numberColumn.Name));
                }

                List<PickerColumn> pickerColumnsMissingInItem = pickerColumns.Where(
                        t => item.PickerColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();
                List<PickerColumn> pickerColumnsMissingInList = item.PickerColumns.Where(
                        t => pickerColumns.Where(T => T.Name == t.Name).Count() == 0
                        ).ToList();

                foreach(var pickerColumn in pickerColumnsMissingInItem)
                {
                    item.PickerColumns.Add(new PickerColumn(pickerColumn.Name, pickerColumn.Options.Select(o => o.Option).ToList())
                    {
                        Value = pickerColumn.Options.First()
                    });
                }
                foreach(var pickerColumn in pickerColumnsMissingInList)
                {
                    pickerColumns.Add(new PickerColumn(pickerColumn.Name, pickerColumn.Options.Select(o => o.Option).ToList()) 
                    {
                        Value = pickerColumn.Options.First()
                    });
                }
    
            }

            return collection;
        }

    }
}
