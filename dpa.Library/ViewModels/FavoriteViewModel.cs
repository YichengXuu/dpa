using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class FavoriteViewModel : ViewModelBase {
    private readonly IFavoriteStorage _favoriteStorage;

    private readonly IPoetryStorage _poetryStorage;

    private readonly IContentNavigationService _contentNavigationService;

    public FavoriteViewModel(IFavoriteStorage favoriteStorage,
        IPoetryStorage poetryStorage,
        IContentNavigationService contentNavigationService) {
        _favoriteStorage = favoriteStorage;
        _poetryStorage = poetryStorage;
        _contentNavigationService = contentNavigationService;

        _favoriteStorage.Updated += FavoriteStorageOnUpdated;
        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
        ShowPoetryCommand = new RelayCommand<Poetry>(ShowPoetry);
    }

    private async void FavoriteStorageOnUpdated(object sender, Favorite e) {
        PoetryFavoriteCollection.Remove(
            PoetryFavoriteCollection.FirstOrDefault(p =>
                p.Favorite.PoetryId == e.PoetryId));

        if (!e.IsFavorite) {
            return;
        }

        var poetryFavorite = new PoetryFavorite {
            Poetry = await _poetryStorage.GetPoetryAsync(e.PoetryId),
            Favorite = e
        };

        PoetryFavoriteCollection.Insert(0, poetryFavorite);
    }

    public ObservableCollection<PoetryFavorite> PoetryFavoriteCollection {
        get;
    } = new();

    public bool IsLoading {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private bool _isLoading;

    public ICommand OnInitializedCommand { get; }

    private bool _isLoaded;

    public async Task OnInitializedAsync() {
        if (_isLoaded) {
            return;
        }

        _isLoaded = true;

        IsLoading = true;

        PoetryFavoriteCollection.Clear();
        var favoriteList = await _favoriteStorage.GetFavoritesAsync();

        await Task.Delay(1000);
        var poetryFavorites = (await Task.WhenAll(favoriteList.Select(p =>
            Task.Run(async () => new PoetryFavorite {
                Poetry = await _poetryStorage.GetPoetryAsync(p.PoetryId),
                Favorite = p
            })))).ToList();
        foreach (var poetryFavorite in poetryFavorites) {
            PoetryFavoriteCollection.Add(poetryFavorite);
        }

        IsLoading = false;
    }

    public IRelayCommand<Poetry> ShowPoetryCommand { get; }

    public void ShowPoetry(Poetry poetry) =>
        _contentNavigationService.NavigateTo(
            ContentNavigationConstant.DetailView, poetry);
}

public class PoetryFavorite {
    public Poetry Poetry { get; set; }

    public Favorite Favorite { get; set; }
}