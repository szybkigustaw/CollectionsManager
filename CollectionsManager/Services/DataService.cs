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
                            throw new Exception("There is already the same collection in the app data!");
                        }
                        else
                        {
                            _model.Collections.Add(collection);
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
    }
}
