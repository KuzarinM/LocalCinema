using AdstractHelpers.Storage.Abstraction.Interfases;
using OnlineСinema.Models;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface ISeasonStorage : IDbStorage<Seasone, Guid>
    {
        public Task DeleteExcept(Guid titleId, List<string>? names, List<string>? paths);

        public Task UpdateNameAndPath(Seasone seasone, string Name, string Path);
    }
}
