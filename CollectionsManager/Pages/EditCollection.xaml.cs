using CollectionsManager.ViewModels;

namespace CollectionsManager.Pages;

public partial class EditCollection : ContentPage
{
	public EditCollection(EditCollectionViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}