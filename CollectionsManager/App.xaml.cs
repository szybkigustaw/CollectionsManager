using CollectionsManager.Services;

namespace CollectionsManager
{
    public partial class App : Application
    {
        public App(DataService dataService)
        {
            InitializeComponent();

            dataService.LoadData();

            MainPage = new AppShell();
        }
    }
}
