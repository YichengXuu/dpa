using System.Linq.Expressions;
using dpa.Library.Models;

namespace dpa.Library.Services;

public interface IPoetryStorage {
    bool IsInitialized { get; }

    Task InitializeAsync();

    Task<Poetry> GetPoetryAsync(int id);

    Task<IList<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);
}