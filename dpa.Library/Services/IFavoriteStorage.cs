using dpa.Library.Models;

namespace dpa.Library.Services;

public interface IFavoriteStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Favorite> GetFavoriteAsync(int poetryId);

    Task<IEnumerable<Favorite>> GetFavoritesAsync();

    Task SaveFavoriteAsync(Favorite favorite);

    event EventHandler<Favorite> Updated;
}