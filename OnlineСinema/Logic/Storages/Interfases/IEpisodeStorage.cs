using AdstractHelpers.Storage.Abstraction.Interfases;
using OnlineСinema.Models;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface IEpisodeStorage : IDbStorage<Episode, Guid>
    {
        public Task DeleteExcept(Guid seasonId, List<string>? names, List<string>? paths);

        public Task UpdateNameAndPath(Episode episode, string Name, string Path);
    }
}
