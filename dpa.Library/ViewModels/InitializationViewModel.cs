using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class InitializationViewModel : ViewModelBase {
    private readonly IPoetryStorage _poetryStorage;
    private readonly IFavoriteStorage _favoriteStorage;
    private readonly IRootNavigationService _rootNavigationService;
    private readonly IMenuNavigationService _menuNavigationService;

    public InitializationViewModel(IPoetryStorage poetryStorage,
        IFavoriteStorage favoriteStorage,
        IRootNavigationService rootNavigationService,
        IMenuNavigationService menuNavigationService) {
        _poetryStorage = poetryStorage;
        _favoriteStorage = favoriteStorage;
        _rootNavigationService = rootNavigationService;
        _menuNavigationService = menuNavigationService;

        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }
    
    private ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync() {
        if (!_poetryStorage.IsInitialized) {
            await _poetryStorage.InitializeAsync();
        }

        if (!_favoriteStorage.IsInitialized) {
            await _favoriteStorage.InitializeAsync();
        }

        await Task.Delay(5000);

        _rootNavigationService.NavigateTo(RootNavigationConstant.MainView);
        _menuNavigationService.NavigateTo(MenuNavigationConstant.TodayView);
    }
}