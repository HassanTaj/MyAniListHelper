using AniListHelper.Pages;

namespace AniListHelper;

public partial class App : Application {
    public App(LoginPage loginpage) {
        InitializeComponent();

        MainPage = loginpage;
    }
}
