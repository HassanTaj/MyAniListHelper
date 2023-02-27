using AniListHelper.Models;

namespace AniListHelper.Pages;

public partial class DetailPage : ContentPage {
    public DetailPage(MediaEntryModel selectedItem) {
        InitializeComponent();
        LblImage.Source = selectedItem?.ImageUrl ?? "";
        LblName.Text = selectedItem.Name;
        LblHeading.Text = selectedItem.Name;
        LblStatus.Text = selectedItem.Status;
        LblOtherNames.Text = selectedItem.OtherNames;
    }
    private void Back_Tapped(object sender, EventArgs e) {
        Navigation.PopAsync();
    }
}