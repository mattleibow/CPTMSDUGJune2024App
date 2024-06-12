using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Exciting.TeamClient;

namespace Exciting.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif

		builder.AddAppDefaults();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddHttpClient<TeamApiClient>(client =>
		{
			// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
			// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
			client.BaseAddress = new("https+http://teamapi");
		});

		builder.Services.AddTransient<MainPage>();

		return builder.Build();
	}
}
