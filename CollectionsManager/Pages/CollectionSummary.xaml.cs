using CollectionsManager.ViewModels;

namespace CollectionsManager.Pages;

public partial class CollectionSummary : ContentPage
{
	public CollectionSummary(CollectionSummaryViewModel vm)
	{
		this.BindingContext = vm;
		InitializeComponent();
	}
}