using CollectionsManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollectionsManager.ViewModels
{
    public class CollectionSummaryViewModel : INotifyPropertyChanged, IQueryAttributable
    {
        private CollectionsModel _model;
        private ItemsCollection summaryCollection;
        private int items_owned = 0;
        private int items_sold = 0;
        private int items_toSell = 0;

        public ItemsCollection SummaryCollection { get { return summaryCollection; } set { SetProperty(ref summaryCollection, value); } }
        public int ItemsOwned { get => items_owned; set => SetProperty(ref items_owned, value); }
        public int ItemsSold { get => items_sold; set => SetProperty(ref items_sold, value); }
        public int ItemsToSell { get => items_toSell; set => SetProperty(ref items_toSell, value); }

        public ICommand GoBackCommand { get; set; }

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
            Guid collectionId = Guid.Parse(query["collection_id"].ToString());
            SummaryCollection = _model.Collections.First(c => c.Id == collectionId);
            CalculateItemCounts();
#if DEBUG
            Debug.WriteLine($"Items count: {SummaryCollection.Items.Count()}");
            Debug.WriteLine($"Items owned: {ItemsOwned}");
            Debug.WriteLine($"Items sold: {ItemsSold}");
            Debug.WriteLine($"Items to sell: {ItemsToSell}");
#endif
        }

        private void CalculateItemCounts()
        {
            ItemsOwned = 0;
            ItemsSold = 0;
            ItemsToSell = 0;

            foreach(var item in SummaryCollection.Items)
            {
                if(item.PickerColumns.Where(c => c.Name == "Status").Any())
                {
                    string status = item.PickerColumns.FirstOrDefault(c => c.Name == "Status").Value.Option;
                    switch(status)
                    {
                        case "New":
                        case "Used":
                            {
                                ItemsOwned++;
                            }
                            break;
                        case "To sale": ItemsToSell++; break;
                        case "Sold": ItemsSold++; break;
                    }
                }
            }
        }

        private async void GoBack()
        {
            await Shell.Current.GoToAsync("///MainPage");
        }

        public CollectionSummaryViewModel(CollectionsModel model)
        {
            this._model = model;
            this.GoBackCommand = new Command(GoBack);
        }
    }
}
