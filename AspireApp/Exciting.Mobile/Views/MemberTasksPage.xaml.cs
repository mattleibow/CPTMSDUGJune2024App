namespace Exciting.Mobile;

public partial class MemberTasksPage : ContentPage
{
	public MemberTasksPage(MemberTasksViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	public async void OnEditMemberClicked(object? sender, EventArgs e)
	{
		if (BindingContext is not MemberTasksViewModel vm || vm.Member is not {} member)
			return;

		await Shell.Current.GoToAsync("//members/edit", new ShellNavigationQueryParameters
		{
			["member"] = member
		});
	}

	public void OnAddTaskClicked(object? sender, EventArgs e)
	{
	}

	public async void OnTaskSelected(object? sender, SelectionChangedEventArgs e)
	{
		if (e.CurrentSelection is null || e.CurrentSelection.Count == 0)
			return;

		if (e.CurrentSelection[0] is TaskItemViewModel task)
		{
			await Shell.Current.GoToAsync("//members/tasks/edit", new ShellNavigationQueryParameters
			{
				["task"] = task
			});
		}

		if (sender is CollectionView collectionView)
			collectionView.SelectedItem = null;
	}
}
