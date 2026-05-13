using AniListHelper.Infrastructure;
using AniListNet;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Identity.Client;
using Microsoft.Maui.Authentication;
using Newtonsoft.Json;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using Browser = AniListHelper.Infrastructure.Browser;

namespace AniListHelper.Pages;

public partial class LoginPage : ContentPage {
    private readonly SecureStorageProcessor _secureStorage;
    private readonly AniClient _aniclient;
    private readonly AppDbContext _db;
    private readonly AppShell _appShell;
    private readonly IPublicClientApplication pca;

    public LoginPage() {
        InitializeComponent();
    }
    public LoginPage(AppDbContext db, SecureStorageProcessor secureStorage, AniClient aniclient,
        AppShell appShell) {
        InitializeComponent();
        _aniclient = aniclient;
        _secureStorage = secureStorage;
        _db = db;
        _appShell = appShell;
        //var pcaOptions = new PublicClientApplicationOptions {
        //    ClientId = $"{Constants.App.CLIENT_ID}",
        //    RedirectUri = Constants.App.REDIRECT_URI
        //};
        //pca = PublicClientApplicationBuilder.CreateWithApplicationOptions(pcaOptions).Build();
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
            //await Navigation.PushAsync(new MainPage());
            //Navigation.RemovePage(this);
            Application.Current.MainPage = _appShell;

            //var authUri = pca.GetAuthorizationRequestUrl(new[] { Scope });
            //webView.Source = new Uri(Constants.App.AUTH_URI);

        }
        else {
            loginBtn.IsVisible = true;
            activityIndicator.IsVisible = false;
        }
    }

    private async void OnCounterClicked(object sender, EventArgs e) {
        try {
            WebAuthenticatorResult authResult = default;
            BrowserResult browserResult = default;
            OidcClient _oidcClient;
            LoginResult loginResult = default;



            if (DeviceInfo.Platform == DevicePlatform.Android) {
                authResult = await WebAuthenticator.Default.AuthenticateAsync(
                new WebAuthenticatorOptions() {
                    Url = new Uri(Constants.App.AUTH_URI),
                    CallbackUrl = new Uri(Constants.App.REDIRECT_URI),
                    PrefersEphemeralWebBrowserSession = true
                });
            }
            if (DeviceInfo.Platform == DevicePlatform.WinUI) {
                authResult = await WebAuthenticator.AuthenticateAsync(new Uri(Constants.App.DESK_AUTH_URI),new Uri(Constants.App.REDIRECT_URI));

                //browserResult = await new AuthBrowser().InvokeAsync(options: new IdentityModel.OidcClient.Browser.BrowserOptions(Constants.App.DESK_AUTH_URI, Constants.App.REDIRECT_URI));

                //var options = new OidcClientOptions {
                //    Authority = "https://anilist.co",
                //    ClientId = "10498",
                //    ClientSecret= Constants.App.SECRET,
                //    Scope = "openid profile email api offline_access",
                //    RedirectUri = "myapp://",
                //    Browser = new Browser()
                //};
                //_oidcClient = new OidcClient(options);
                //var result = await _oidcClient.LoginAsync(new LoginRequest());

                var pcaOptions = new PublicClientApplicationOptions {
                    ClientId = $"{Constants.App.CLIENT_ID}",
                    RedirectUri = Constants.App.REDIRECT_URI,
                };
                var pca = PublicClientApplicationBuilder.CreateWithApplicationOptions(pcaOptions).Build();
                // Inside a method or event handler
                var scopes = new[] { "" };
                var authResults = await pca.AcquireTokenInteractive(scopes)
                    .WithUseEmbeddedWebView(true) // Use embedded WebView control
                    .ExecuteAsync();

                if (authResults != null) {
                    // Access token acquired successfully
                    var accessToken = authResults.AccessToken;

                    // Use the access token to make API requests to AniList
                    // ...
                }


                var webView = new WebView();
                webView.Source = new UrlWebViewSource {
                    Url = Constants.App.AUTH_URI
                };
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
            else if (loginResult != null) {
                //string accessToken = authResult?.AccessToken;
                //var expiresIn = authResult?.ExpiresIn;
                //_secureStorage.SetAuthToken(accessToken, expiresIn);
            }
            else {
                await DisplayAlert("Alert", "Couldn't Authenticate", "OK");
            }
        }
        catch (TaskCanceledException ex) {
            Debug.Write(ex.ToString());
        }

    }

    private async void WebView_Navigated(object sender, WebNavigatedEventArgs e) {
        var url = e.Url;
        if (url.StartsWith(Constants.App.REDIRECT_URI)) {
            var authResult = await pca.AcquireTokenInteractive(new[] { "authorization_code" })
                .WithUseEmbeddedWebView(true)
                .WithParentActivityOrWindow(App.Current.MainPage)
                .ExecuteAsync();

            // Authentication completed, you can use the access token
            var accessToken = authResult?.AccessToken;

            // Store the access token or use it for API requests
            // ...

            // Navigate to the next page or perform necessary actions
            // ...
        }
    }
}

