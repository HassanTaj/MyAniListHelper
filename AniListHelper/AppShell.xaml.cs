namespace AniListHelper;

public partial class AppShell : Shell {
    public AppShell() {
        InitializeComponent();
    }
    protected override bool OnBackButtonPressed() {
        // true or false to disable or enable the action
        //TODO: 
        return base.OnBackButtonPressed();
    }
}
