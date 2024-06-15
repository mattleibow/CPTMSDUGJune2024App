namespace Exciting.Mobile;

public partial class EditTeamMemberPage : ContentPage
{
	public EditTeamMemberPage(EditTeamMemberViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}
