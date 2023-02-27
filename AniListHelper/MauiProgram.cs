using AniListHelper.Infrastructure;
using AniListHelper.Pages;
using AniListNet;
using CommunityToolkit.Maui;

namespace AniListHelper;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<App>();
		builder.Services.AddSingleton<AppShell>();
		builder.Services.AddDbContext<AppDbContext>();

		builder.Services.AddScoped<AppDbContext>();
		builder.Services.AddSingleton<AniClient>();
		builder.Services.AddSingleton<SecureStorageProcessor>();
	
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<SearchPage>();
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<MyAnimePage>();
		builder.Services.AddSingleton<SettingsPage>();
		builder.Services.AddSingleton<DetailPage>();
#if ANDROID
		MainActivity.SetStatusBarLight(false);
#endif

		return builder.Build();
	}
}
