using Exciting.TeamClient;
using OpenTelemetry.Trace;
// using OpenTelemetry.Exporter;

namespace Exciting.Mobile;

public partial class MainPage : ContentPage
{
	private readonly TeamApiClient teamApi;
	
	int count = 0;

	public MainPage(TeamApiClient teamApiClient)
	{
		InitializeComponent();

		teamApi = teamApiClient;
	}

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		try
		{
			var members = await teamApi.GetMembersAsync();

			CounterBtn.Text += $" and {members.Length} members found.";
		}
		catch (Exception ex)
		{
			CounterBtn.Text += $" with error: {ex.Message}";
		}

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}

