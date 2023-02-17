using AniListHelper.Infrastructure;
using AniListHelper.Views;
using AniListNet;
using Newtonsoft.Json;
using System.Diagnostics;

namespace AniListHelper;

public partial class MainPage : ContentPage {
    private readonly SecureStorageProcessor _secureStorage;
    private readonly AniClient _aniclient;
    private readonly AppDbContext _db;

    public MainPage() {
        InitializeComponent();
        _aniclient = new AniClient();
        _secureStorage = new SecureStorageProcessor();
        var path = Constants.Database.DatabasePath;
        //data/user/0/com.companyname.anilisthelper/files/anilisthelper.db3
        //_conn = new SQLiteAsyncConnection(Constants.Database.DatabasePath, Constants.Database.Flags);
        _db = new AppDbContext();
        Init();
    }
    private async void Init() {
        string token = await _secureStorage.GetAuthToken();
        if (token != null) {
            activityIndicator.IsVisible = true;
            loginBtn.IsVisible = false;
            var result = await _aniclient.TryAuthenticateAsync(token);
            // simple authentication with AniList
            var user = await _aniclient.GetAuthenticatedUserAsync();
            Preferences.Set(Constants.USER, JsonConvert.SerializeObject(user));
            await Navigation.PushAsync(new SearchPage(_secureStorage, _aniclient, _db));

        }
        else {
            loginBtn.IsVisible = true;
            activityIndicator.IsVisible = false;
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e) {
        try {
            WebAuthenticatorResult authResult = default;

            if (DeviceInfo.Platform == DevicePlatform.Android) {
                authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new WebAuthenticatorOptions() {
                    Url = new Uri(Constants.App.AUTH_URI),
                    CallbackUrl = new Uri(Constants.App.REDIRECT_URI),
                    PrefersEphemeralWebBrowserSession = true
                });
            }
            if (DeviceInfo.Platform == DevicePlatform.WinUI) {
                var result = await new AuthBrowser().InvokeAsync(options: new IdentityModel.OidcClient.Browser.BrowserOptions(Constants.App.AUTH_URI, Constants.App.REDIRECT_URI));

                await DisplayAlert("Alert", "This is windows", "OK");
            }

            if (DeviceInfo.Platform == DevicePlatform.iOS) { }
            //WebAuthenticatorResult result = await WinUIEx.WebAuthenticator.AuthenticateAsync(authorizeUrl, callbackUri);

            if (authResult != null) {
                string accessToken = authResult?.AccessToken;
                var expiresIn = authResult?.ExpiresIn;
                _secureStorage.SetAuthToken(accessToken, expiresIn);
                Init();
            }
            else {
                await DisplayAlert("Alert", "Couldn't Authenticate", "OK");
            }
        }
        catch (TaskCanceledException ex) {
            Debug.Write(ex.ToString());
        }

    }
}

