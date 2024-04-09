using CollectionsManager.ViewModels;

namespace CollectionsManager.Pages;

public partial class AddCollection : ContentPage
{
	public AddCollection(AddCollectionViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}