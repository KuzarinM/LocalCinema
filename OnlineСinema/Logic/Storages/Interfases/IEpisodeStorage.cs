using AdstractHelpers.Storage.Abstraction.Interfases;
using OnlineСinema.Models;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface IEpisodeStorage : IDbStorage<Episode, Guid>
    {
        public Task DeleteExcept(Guid seasonId, List<string>? names, List<string>? paths);

        public Task UpdateNameAndPath(Episode episode, string Name, string Path);

        public Task ChangeOrderIndexesByIds(params (Guid id, int index)[] values);

        public Task<Episode> GetEpisodeWithTitleAndSeen(Guid id, Guid? userId);
    }
}
