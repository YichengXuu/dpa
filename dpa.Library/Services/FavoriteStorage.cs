using dpa.Library.Helpers;
using dpa.Library.Models;
using SQLite;

namespace dpa.Library.Services;

public class FavoriteStorage : IFavoriteStorage {
    public const string DbName = "favoritedb.sqlite3";

    public static readonly string PoetryDbPath =
        PathHelper.GetLocalFilePath(DbName);

    private SQLiteAsyncConnection _connection;

    private SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    private readonly IPreferenceStorage _preferenceStorage;

    public FavoriteStorage(IPreferenceStorage preferenceStorage) {
        _preferenceStorage = preferenceStorage;
    }

    public bool IsInitialized =>
        _preferenceStorage.Get(FavoriteStorageConstant.VersionKey,
            default(int)) == FavoriteStorageConstant.Version;

    public async Task InitializeAsync() {
        await Connection.CreateTableAsync<Favorite>();
        _preferenceStorage.Set(FavoriteStorageConstant.VersionKey,
            FavoriteStorageConstant.Version);
    }

    public async Task<Favorite> GetFavoriteAsync(int poetryId) =>
        await Connection.Table<Favorite>()
            .FirstOrDefaultAsync(p => p.PoetryId == poetryId);

    public async Task<IEnumerable<Favorite>> GetFavoritesAsync() =>
        await Connection.Table<Favorite>().Where(p => p.IsFavorite)
            .OrderByDescending(p => p.Timestamp).ToListAsync();

    public async Task SaveFavoriteAsync(Favorite favorite) {
        favorite.Timestamp = DateTime.Now.Ticks;
        await Connection.InsertOrReplaceAsync(favorite);
        Updated?.Invoke(this, favorite);
    }

    public event EventHandler<Favorite> Updated;

    public async Task CloseAsync() => await Connection.CloseAsync();
}

public static class FavoriteStorageConstant {
    public const string VersionKey =
        nameof(FavoriteStorageConstant) + "." + nameof(Version);

    public const int Version = 1;
}