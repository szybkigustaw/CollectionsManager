using CollectionsManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CollectionsManager.Views;

public partial class ItemsCollectionView : ContentView
{
	public static BindableProperty NameProperty = BindableProperty.Create(
			nameof(Name),
			typeof(string), 
			typeof(ItemsCollectionView)
		);

	public static BindableProperty CollectionIdProperty = BindableProperty.Create(
			nameof(CollectionId),
			typeof(Guid),
			typeof(ItemsCollectionView)
		);
	public static BindableProperty ItemsProperty = BindableProperty.Create(
			nameof(Items),
			typeof(ObservableCollection<Item>),
			typeof(ItemsCollectionView)
		);

	public static BindableProperty ModificationDateProperty = BindableProperty.Create(
			nameof(ModificationDate),
			typeof(DateTime),
			typeof(ItemsCollectionView)
		);

	public static BindableProperty CreationDateProperty = BindableProperty.Create(
			nameof(CreationDate),
			typeof(DateTime),
			typeof(ItemsCollectionView)
		);

	public static BindableProperty DeleteItemCommandProperty = BindableProperty.Create(
			nameof(DeleteItemCommand),
			typeof(ICommand),
			typeof(ItemsCollectionView)
		);

	public static BindableProperty EditItemCommandProperty = BindableProperty.Create(
			nameof(EditItemCommand),
			typeof(ICommand),
			typeof(ItemsCollectionView)
		);

	public string Name
	{
		get => (string)GetValue(NameProperty); 
		set => SetValue(NameProperty, value);
	}

	public Guid CollectionId
	{
		get => (Guid)GetValue(CollectionIdProperty);
		set => SetValue(CollectionIdProperty, value);
	}

	public ObservableCollection<Item> Items
	{
		get => (ObservableCollection<Item>)GetValue(ItemsProperty);
		set => SetValue(ItemsProperty, value);
	}

	public DateTime ModificationDate
	{
		get => (DateTime)GetValue(ModificationDateProperty);
		set => SetValue(ModificationDateProperty, value);
	}

	public DateTime CreationDate
	{
		get => (DateTime)(GetValue(CreationDateProperty));
		set => SetValue(CreationDateProperty, value);
	}
	public ICommand EditItemCommand
	{
		get => (ICommand)(GetValue(EditItemCommandProperty));
		set => SetValue(EditItemCommandProperty, value);
	}
	public ICommand DeleteItemCommand
	{
		get => (ICommand)(GetValue(DeleteItemCommandProperty));
		set => SetValue(DeleteItemCommandProperty, value);
	}

	public ItemsCollectionView()
	{
		InitializeComponent();
	}

    private async void AddItem(object sender, EventArgs e)
    {
		var query = new ShellNavigationQueryParameters
		{
			{"editing", false },
			{"CollectionId", CollectionId }
		};

		await Shell.Current.GoToAsync("///AddItem", query);
    }

    private async void DisplaySummary(object sender, EventArgs e)
    {
		var query = new ShellNavigationQueryParameters
		{
			{"collection_id", CollectionId }
		};
		await Shell.Current.GoToAsync("///Summary", query);
    }
    private async void EditCollection(object sender, EventArgs e)
    {
		var query = new ShellNavigationQueryParameters
		{
			{"Id", CollectionId }
		};

		await Shell.Current.GoToAsync("///EditCollection", query);
    }
}