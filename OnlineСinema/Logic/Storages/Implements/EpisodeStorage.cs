using AdstractHelpers.Storage.Abstraction;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;
using System.IO;
using System.Xml.Linq;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class EpisodeStorage(
        ILogger<EpisodeStorage> logger, 
        IMapper mapper, 
        CinemaDbContext dbContext
    ) : AbstractDbStorage<Episode, Guid>(logger, mapper, dbContext), IEpisodeStorage
    {
        public async Task DeleteExcept(Guid seasonId, List<string>? names, List<string>? paths)
        {
            names = names ?? [];
            paths = paths ?? [];

            await GetQueryable()
                .Where(x => x.Seasoneid == seasonId && !names.Contains(x.Name) && !paths.Contains(x.Path))
                .ExecuteDeleteAsync();
        }

        public async Task UpdateNameAndPath(Episode episode, string name, string path)
        {
            if (episode.Name != name)
                episode.Name = name;

            if (episode.Path != path)
                episode.Path = path;

            await _dbContext.SaveChangesAsync();
        }

        protected override Task<Episode> UpdateItem(Episode source, object target)
        {
            throw new NotImplementedException();
        }
    }
}
