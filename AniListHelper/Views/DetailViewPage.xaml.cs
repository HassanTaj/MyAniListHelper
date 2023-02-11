using AniListHelper.Models;

namespace AniListHelper.Views;

public partial class DetailViewPage : ContentPage {
    public DetailViewPage(MediaEntryModel selectedItem) {
        InitializeComponent();
        LblName.Text = selectedItem.Name;
        LblStatus.Text = selectedItem.Status;
        LblOtherNames.Text = selectedItem.OtherNames;
        LblDescription.Text = selectedItem.Description;
        //LblStartDate.Text = selectedItem.StartDate;
    }
}