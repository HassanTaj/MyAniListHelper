using AniListHelper.Models;

namespace AniListHelper.Views;

public partial class DetailPage : ContentPage {
    public DetailPage(MediaEntryModel selectedItem) {
        InitializeComponent();
        LblImage.Source = "https://s4.anilist.co/file/anilistcdn/media/anime/cover/large/bx21230-rfoUZud1Jn0L.png";
        LblName.Text = selectedItem.Name;
        LblHeading.Text = selectedItem.Name;
        LblStatus.Text = selectedItem.Status;
        LblOtherNames.Text = selectedItem.OtherNames;
    }
    private void Back_Tapped(object sender, EventArgs e) {
        Navigation.PopAsync();
    }
}