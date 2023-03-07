using AniListHelper.Pages;

namespace AniListHelper;

public partial class AppShell : Shell {
    public AppShell() {
        InitializeComponent();
        //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

    }
    protected override bool OnBackButtonPressed() {
//        if (Navigation.NavigationStack.Count <= 2) {
//#if ANDROID
//            Application.Current.Quit();
//#endif
//#if WINDOWS
//            Application.Current?.CloseWindow(Application.Current.MainPage.Window);
//#endif
//}
        return base.OnBackButtonPressed();
    }
}
