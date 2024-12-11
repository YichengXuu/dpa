using System;
using dpa.Library.Services;
using dpa.Library.ViewModels;

namespace dpa.Services;

public class MenuNavigationService : IMenuNavigationService {
    public void NavigateTo(string view) {
        ViewModelBase viewModel = view switch {
            MenuNavigationConstant.TodayView => ServiceLocator.Current
                .TodayViewModel,
            MenuNavigationConstant.QueryView => ServiceLocator.Current
                .QueryViewModel,
            MenuNavigationConstant.FavoriteView => ServiceLocator.Current
                .FavoriteViewModel,
            _ => throw new Exception("未知的视图。")
        };

        ServiceLocator.Current.MainViewModel.SetMenuAndContent(view, viewModel);
    }
}