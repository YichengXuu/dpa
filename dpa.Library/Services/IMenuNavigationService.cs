namespace dpa.Library.Services;

public interface IMenuNavigationService {
    void NavigateTo(string view);
}

public static class MenuNavigationConstant {
    public const string TodayView = nameof(TodayView);

    public const string QueryView = nameof(QueryView);

    public const string FavoriteView = nameof(FavoriteView);
}