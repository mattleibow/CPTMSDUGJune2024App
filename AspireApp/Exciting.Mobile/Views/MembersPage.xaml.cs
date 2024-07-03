namespace Exciting.Mobile;

public partial class MembersPage : ContentPage
{
	public MembersPage(MembersViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	public async void OnMemberSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection is null || e.CurrentSelection.Count == 0)
			return;

		if (e.CurrentSelection[0] is TeamMemberViewModel member)
		{
			await Shell.Current.GoToAsync("tasks", new ShellNavigationQueryParameters
			{
				["member"] = member
			});
		}

		if (sender is CollectionView collectionView)
			collectionView.SelectedItem = null;
	}
}
