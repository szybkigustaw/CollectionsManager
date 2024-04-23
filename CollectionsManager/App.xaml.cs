using CollectionsManager.Services;

namespace CollectionsManager
{
    public partial class App : Application
    {
        public App(DataService dataService)
        {
            InitializeComponent();

            dataService.LoadError += HandleLoadError;

            MainPage = new AppShell();

            dataService.LoadData();
        }

        private async void HandleLoadError(object sender, Exception ex)
        {
            await MainPage.DisplayAlert("Data load error", ex.Message, "OK");
        }
    }
}
