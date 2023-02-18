using AniListHelper.Models;

namespace AniListHelper.Views;

public partial class DetailPage : ContentPage {
    public DetailPage(MediaEntryModel selectedItem) {
        InitializeComponent();
        LblImage.Source = "https://picsum.photos/seed/picsum/200/300";
        LblName.Text = selectedItem.Name;
        LblStatus.Text = selectedItem.Status;
        LblOtherNames.Text = selectedItem.OtherNames;
    }
}