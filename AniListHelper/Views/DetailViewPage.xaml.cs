using AniListHelper.Models;

namespace AniListHelper.Views;

public partial class DetailViewPage : ContentPage {
    public DetailViewPage(MediaEntryModel selectedItem) {
        InitializeComponent();
        //LblImage.Source = selectedItem.Image;
        LblImage.Source = "https://picsum.photos/seed/picsum/200/300";
        LblName.Text = selectedItem.Name;
        LblName.LineBreakMode = LineBreakMode.MiddleTruncation;
        LblStatus.Text = selectedItem.Status;
        LblOtherNames.Text = selectedItem.OtherNames;
        //LblDescription.Text = selectedItem.Description;
        //LblStartDate.Text = selectedItem.StartDate;
    }
}