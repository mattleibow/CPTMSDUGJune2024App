namespace MyFirstMauiApp;

public partial class WowPage : ContentPage
{
    public WowPage()
    {
        InitializeComponent();
    }

    private async void OnNativeButtonClicked(object? sender, EventArgs e)
    {
        await DisplayAlert("Alert", "You have clicked the native button.", "OK");
    }
}
