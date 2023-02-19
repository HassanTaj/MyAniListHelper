using AniListHelper.Infrastructure;
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

		builder.Services
		   .AddDbContext<AppDbContext>();
#if ANDROID
		MainActivity.SetStatusBarLight(false);
#endif

		return builder.Build();
	}
}
