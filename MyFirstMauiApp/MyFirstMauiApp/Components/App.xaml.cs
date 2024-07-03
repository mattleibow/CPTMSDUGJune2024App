namespace MyFirstMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new MainWindow
        {
            Page = new AppShell()
        };
        return window;
    }
}
