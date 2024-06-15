using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Exciting.TeamClient;
using CommunityToolkit.Maui;
using System.Text.RegularExpressions;

namespace Exciting.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
				fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
				fonts.AddFont("OpenSans-Italic.ttf", "OpenSansItalic");
				fonts.AddFont("OpenSans-BoldItalic.ttf", "OpenSansBoldItalic");
			});

#if DEBUG
		foreach (var setting in AspireAppSettings.Settings)
		{
			if (setting.Value.Contains("localhost"))
			{
				// source format is `http[s]://localhost:[port]`
				// tunnel format is `http[s]://exciting-tunnel-[port].devtunnels.ms`
				var newVal = Regex.Replace(
					setting.Value,
					@"://localhost\:(\d+)(.*)",
					"://exciting-tunnel-$1.devtunnels.ms$2");
				AspireAppSettings.Settings[setting.Key] = newVal;
			}
		}

		builder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif

		builder.AddAppDefaults();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		builder.Services.AddHybridWebView();

		builder.Services.AddHttpClient<TeamApiClient>(client =>
		{
			// This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
			// Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
			client.BaseAddress = new("https+http://teamapi");
		});

		builder.Services.AddTransient<MembersPage, MembersViewModel>();
		builder.Services.AddTransientWithShellRoute<MemberTasksPage, MemberTasksViewModel>("members/tasks");
		builder.Services.AddTransientWithShellRoute<EditTaskPage, EditTaskViewModel>("members/tasks/edit");
		builder.Services.AddTransientWithShellRoute<EditTeamMemberPage, EditTeamMemberViewModel>("members/edit");

		return builder.Build();
	}
}
