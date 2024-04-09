using CollectionsManager.ViewModels;

namespace CollectionsManager.Pages;

public partial class AddItem : ContentPage
{
	public AddItem(AddItemViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}