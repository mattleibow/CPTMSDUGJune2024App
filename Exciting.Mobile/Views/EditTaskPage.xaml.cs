namespace Exciting.Mobile;

public partial class EditTaskPage : ContentPage
{
	public EditTaskPage(EditTaskViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

		notesEditor.JSInvokeTarget = viewModel;
	}
}
