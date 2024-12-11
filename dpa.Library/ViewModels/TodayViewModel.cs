using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using dpa.Library.Models;
using dpa.Library.Services;

namespace dpa.Library.ViewModels;

public class TodayViewModel : ViewModelBase {
    private readonly ITodayImageService _todayImageService;
    private readonly ITodayPoetryService _todayPoetryService;
    private readonly IContentNavigationService _contentNavigationService;

    public TodayViewModel(ITodayImageService todayImageService,
        ITodayPoetryService todayPoetryService,
        IContentNavigationService contentNavigationService) {
        _todayImageService = todayImageService;
        _todayPoetryService = todayPoetryService;
        _contentNavigationService = contentNavigationService;

        OnInitializedCommand = new RelayCommand(OnInitialized);
        ShowDetailCommand = new RelayCommand(ShowDetail);
    }

    private TodayPoetry _todayPoetry;

    public TodayPoetry TodayPoetry {
        get => _todayPoetry;
        set => SetProperty(ref _todayPoetry, value);
    }

    private TodayImage _todayImage;

    public TodayImage TodayImage {
        get => _todayImage;
        private set => SetProperty(ref _todayImage, value);
    }

    private bool _isLoading;

    public bool IsLoading {
        get => _isLoading;
        private set => SetProperty(ref _isLoading, value);
    }

    public ICommand OnInitializedCommand { get; }

    public void OnInitialized() {
        Task.Run(async () => {
            IsLoading = true;
            TodayPoetry = await _todayPoetryService.GetTodayPoetryAsync();
            IsLoading = false;
        });

        Task.Run(async () => {
            TodayImage = await _todayImageService.GetTodayImageAsync();
            
            var updateResult = await _todayImageService.CheckUpdateAsync();
            if (updateResult.HasUpdate) {
                TodayImage = updateResult.TodayImage;
            }
        });
    }
    
    public ICommand ShowDetailCommand { get; }

    public void ShowDetail() {
        _contentNavigationService.NavigateTo(
            ContentNavigationConstant.TodayDetailView, TodayPoetry);
    }
}