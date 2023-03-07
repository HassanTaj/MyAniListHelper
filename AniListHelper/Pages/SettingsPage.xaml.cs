using AniListHelper.Infrastructure;
using AniListNet.Objects;
using Newtonsoft.Json;

namespace AniListHelper.Pages;

public partial class SettingsPage : ContentPage {
    private User _user;
    public string userImage;
    public string userBgImage;
    public SettingsPage() {

        InitializeComponent();
        _user = null;
        var userstr = Preferences.Get(Constants.USER, null);
        _user = null;
        int userId = 0;
        if (!string.IsNullOrEmpty(userstr)) {
            _user = JsonConvert.DeserializeObject<User>(userstr);
            userId = _user.Id;
        }
        //img.Source = _user?.Avatar?.MediumImageUrl?.AbsoluteUri?? string.Empty;
        //bgimg.Source = string.Empty; // _user.BannerImageUrl?.AbsoluteUri ?? string.Empty;
        //userBgImage = _user.BannerImageUrl?.AbsoluteUri?? string.Empty;
        //userName.Text = _user.Name;
    }
}