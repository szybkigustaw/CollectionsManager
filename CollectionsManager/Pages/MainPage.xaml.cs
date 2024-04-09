using CollectionsManager.ViewModels;

namespace CollectionsManager.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();
        }
    }

}
