using AniListHelper.Infrastructure;
using AniListHelper.Models;
using AniListNet;
using AniListNet.Objects;
using AniListNet.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using SQLite;
using System.Collections.ObjectModel;
using System.Linq;
using User = AniListNet.Objects.User;

namespace AniListHelper.Views;

public partial class SearchPage : ContentPage
{
    private readonly SecureStorageProcessor _secureStorage;
    private readonly AniClient _aniclient;
    //private readonly SQLiteAsyncConnection _db;
    private readonly AppDbContext _db;
    private List<MediaEntryModel> _data = new List<MediaEntryModel>();
    private User _user;
    private const uint AnimationDuration = 100u;
    public ObservableCollection<MediaEntryModel> MediaEntries { get; set; }

    public SearchPage(SecureStorageProcessor secureStorage, AniClient aniclient, AppDbContext conn)
    {
        InitializeComponent();
        _secureStorage = secureStorage;
        _aniclient = aniclient;
        _db = conn;
        Init();
        MediaEntries = new ObservableCollection<MediaEntryModel>(_data);
        BindingContext = this;
    }

    private async void Init()
    {
        var userstr = Preferences.Get(Constants.USER, null);
        _user = null;
        int userId = 0;
        if (!string.IsNullOrEmpty(userstr))
        {
            _user = JsonConvert.DeserializeObject<User>(userstr);
            userId = _user.Id;
            progressLabel.Text = $"Hey {_user.Name}\n" +
            $"Initializing take some time..";
        }
        var userEntry = await _aniclient.GetUserEntriesAsync(userId, new MediaEntryFilter
        {
            Type = MediaType.Anime
        }, new AniPaginationOptions(1, 1));

        // TODO: Update this when new Method Arrives that gets all the collections. 
        var mediaListCollection = await _aniclient.GetUserListCollectionAsync(userId, MediaType.Anime);
        var rowCount = await _db.MediaEntries.CountAsync();
        if (rowCount == 0)
        {

            progressLabel.Text = "Getting Data..";
            var userEntries = await _aniclient.GetUserEntriesAsync(userId, new MediaEntryFilter
            {
                Type = MediaType.Anime
            }, new AniPaginationOptions(1, 1));

            var mediaEntries = new List<MediaEntry>();
            var pageindex = 1;
            var size = 50;
            while (mediaEntries.Count != userEntries.TotalCount)
            {
                var alluserEntries = await _aniclient.GetUserEntriesAsync(userId, new MediaEntryFilter
                {
                    Type = MediaType.Anime
                }, new AniPaginationOptions(pageindex, size));

                mediaEntries.AddRange(alluserEntries.Data);
                if (pageindex != userEntries.LastPageIndex && alluserEntries.HasNextPage == true)
                {
                    pageindex++;
                }
                else
                {
                    break;
                }
            }
            progressLabel.Text = "Syncing Data...";
            if (mediaEntries != null && mediaEntries.Count > 0)
            {
                var mediaList = mediaEntries.Select(x => new MediaEntryModel
                {
                    Name = $"{x.Media.Title.PreferredTitle}",
                    //ImageUrl = x.Media.Cover.MediumImageUrl.ToString(),
                    OtherNames = string.Join(",", new string[] { x.Media.Title.EnglishTitle, x.Media.Title.RomajiTitle, x.Media.Title.NativeTitle }.Where(x => !string.IsNullOrEmpty(x)).ToList()),
                    Status = x.Status.ToString(),
                    //startDate = x.StartDate.ToString(),
                    //endDate = x.Progress.ToString(),
                });
                await _db.MediaEntries.AddRangeAsync(mediaList);
                await _db.SaveChangesAsync();
            }
        }

        lodingView.IsVisible = false;
        itemsView.IsVisible = true;

        //animeListview.ItemsSource = MediaEntries;
        //_data = await _db.MediaEntries.ToListAsync();
        //animeListview.SetBinding(ItemsView.HeaderProperty, ".");
    }

    protected override bool OnBackButtonPressed()
    {
        //if (!_aniclient.IsAuthenticated) {

        //    return base.OnBackButtonPressed();
        //}
        return _aniclient.IsAuthenticated ? false : true;
    }

    private async void searchBtn_Clicked(object sender, EventArgs e)
    {
        var searchTerm = searchEntry.Text;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var queryText = $"%{searchTerm}%";
            MediaEntries.Clear();
            itemsView.IsVisible = false;
            progressLabel.Text = "Loading..";
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            lodingView.IsVisible = true;

            var query = _db.MediaEntries
                .Where(x => EF.Functions.Like(x.Name, queryText) || EF.Functions.Like(x.OtherNames, queryText))
                .AsQueryable();
            var data = await query.ToListAsync();
            _data = data;
            data.ForEach(i => MediaEntries.Add(i));
            //var data = _data.Where(x => x.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            //data.ForEach(i => data.Add(i));
            if (data.Count == 0)
            {
                progressLabel.Text = "No Records Found";
                activityIndicator.IsVisible = false;
                searchFromAniList.IsVisible = true;
            }
            else
            {
                itemsView.IsVisible = true;
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                lodingView.IsVisible = false;
                searchFromAniList.IsVisible = false;

            }
        }
        else
        {
            MediaEntries.Clear();
            progressLabel.Text = "No Records Found";
            lodingView.IsVisible = true;
            itemsView.IsVisible = false;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

        }
        //itemsView.IsVisible = true;
        //lodingView.IsVisible = false;
    }

    private async void searchFromAniList_Clicked(object sender, EventArgs e)
    {
        var searchResu = await _aniclient.SearchMediaAsync(new SearchMediaFilter
        {
            Query = searchEntry.Text,
            Type = MediaType.Anime,
            Sort = MediaSort.Popularity,
            Format = new Dictionary<MediaFormat, bool>
               {
                  { MediaFormat.TV, true }, // set to only search for TV shows and movies
                  { MediaFormat.Movie, true },
                  { MediaFormat.TVShort, false } // set to not show TV shorts
               }
        });
        if (searchResu != null)
        {
            MediaEntries.Clear();
            var data = searchResu.Data.Select(x => new MediaEntryModel
            {
                Name = $"{x.Title.PreferredTitle}",
                OtherNames = string.Join(",", new string[] { x.Title.EnglishTitle, x.Title.RomajiTitle, x.Title.NativeTitle }.Where(x => !string.IsNullOrEmpty(x)).ToList()),
                Status = "",
            }).ToList();
            _data = data;
            data.ForEach(i => MediaEntries.Add(i));

            itemsView.IsVisible = true;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            lodingView.IsVisible = false;
            searchFromAniList.IsVisible = false;
        }
    }

    private async void OnAddToListSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private async void AnimeSyncAsync(MediaEntryModel item)
    {
        var mediaId = 0;
        var name = item.Name;
        var otherNames = item.OtherNames;
        var status = item.Status;
        var alertDisplayResult = "Anime added to you Anilist Anime list";

        var result = await _aniclient.SearchMediaAsync(item.Name);
        if (result != null)
        {
            mediaId = result.Data[0].Id;
        }

        var res = await _aniclient.SaveMediaEntryAsync(mediaId, new MediaEntryMutation
        {
            Status = MediaEntryStatus.Planning,
            Progress = 0,
            Score = 0,
        });

        var isAlreadyInDB = _db.MediaEntries.FirstOrDefault(x => x.Name == name) == null ? false : true;
        if (!isAlreadyInDB)
        {
            await _db.MediaEntries.AddAsync(new MediaEntryModel
            {
                Name = name,
                OtherNames = otherNames,
                Status = status,
            });
            await _db.SaveChangesAsync();
            alertDisplayResult = "Anime added to you Anilist Anime list & AnilistHelper";
        }
        await DisplayAlert("AnimeAddAlert", alertDisplayResult, "Close");
    }

    private async void AnimeSelectionEvent(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection.FirstOrDefault() as MediaEntryModel;
        if (selectedItem == null) return;
        //await Navigation.PushAsync(new DetailPage(selectedItem));
        //((CollectionView)sender).SelectedItem = null;

        AnimeSyncAsync(selectedItem);
    }

    async void GridArea_Tapped(System.Object sender, System.EventArgs e)
    {
        await CloseMenu();
    }

    private async Task CloseMenu()
    {
        //Close the menu and bring back back the main content
        _ = SearchContent.FadeTo(1, AnimationDuration);
        _ = SearchContent.ScaleTo(1, AnimationDuration);
        await SearchContent.TranslateTo(0, 0, AnimationDuration, Easing.CubicIn);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Reveal our menu and move the main content out of the view
        _ = SearchContent.TranslateTo(this.Width * 0.5, this.Height * 0, AnimationDuration, Easing.Default);
        await SearchContent.ScaleTo(1, AnimationDuration);
        _ = SearchContent.FadeTo(0.5, AnimationDuration);
    }
}