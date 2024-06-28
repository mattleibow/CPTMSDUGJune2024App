using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Exciting.TeamClient;
using CommunityToolkit.Maui;

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

		builder.Configuration.AddAspireApp(AspireAppSettings.Settings, "exciting-tunnel");

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
