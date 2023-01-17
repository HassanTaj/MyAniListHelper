using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.View;

namespace AniListHelper;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity {
    private static MainActivity MainActivityInstance;
    protected override void OnCreate(Bundle savedInstanceState) {
        base.OnCreate(savedInstanceState);
        Platform.Init(this, savedInstanceState);

        var s = SystemUiFlags.LayoutFullscreen | SystemUiFlags.LayoutStable;
#pragma warning disable CS0618 // Type or member is obsolete
        FindViewById(Android.Resource.Id.Content).SystemUiVisibility =
        (StatusBarVisibility)s;
#pragma warning restore CS0618 // Type or member is obsolete
        Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
        WindowInsetsControllerCompat windowInsetsController = new
        WindowInsetsControllerCompat(Window, Window.DecorView);
        windowInsetsController.AppearanceLightStatusBars = false;

        MainActivityInstance = this;
    }

    public static void SetStatusBarLight(bool isLight) {
        if (MainActivityInstance != null) {
            WindowInsetsControllerCompat windowInsetsController =
            new WindowInsetsControllerCompat(MainActivityInstance.Window, MainActivityInstance.Window.DecorView);
            windowInsetsController.AppearanceLightStatusBars = isLight;
        }
    }
}
