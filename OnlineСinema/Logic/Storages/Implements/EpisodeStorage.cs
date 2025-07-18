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
        public async Task ChangeOrderIndexesByIds(params (Guid id, int index)[] values)
        {
            var models = await GetQueryable().Where(x => values.Select(x => x.id).Contains(x.Id)).ToListAsync();

            var targetValues = values.ToDictionary(x => x.id, x => x.index);

            foreach (var item in models)
            {
                item.Orderindex = targetValues[item.Id];
            }

            await SaveChangesAsync();
        }

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

        public async Task<Episode> GetEpisodeWithTitleAndSeen(Guid id, Guid? userId)
        {
            var res = await GetQueryable()
                .Include(x => x.UserSeens.Where(x => x.Userid == userId.ToString()))
                .FirstOrDefaultAsync(x=>x.Id == id);

            return res;
        }

        public async Task UpdateIsSceen(Episode episode, Guid? userId = null, bool isSceen = true)
        {
            if (userId == null)
                return;

            if (isSceen && !episode.UserSeens.Any())
            {
                episode.UserSeens.Add(new()
                {
                    Id = Guid.NewGuid(),
                    Episodeid = episode.Id,
                    Userid = userId.ToString()
                });

                await SaveChangesAsync();
            }
            else if(!isSceen && episode.UserSeens.Any())
            {
                _dbContext.Remove(episode.UserSeens.First());

                await SaveChangesAsync();
            }
        }

        protected override Task<Episode> UpdateItem(Episode source, object target)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Episode> AddIncludes(IQueryable<Episode> query)
            => query
                .Include(x => x.Seasone)
                    .ThenInclude(x => x.Title).ThenInclude(x => x.Seasones).ThenInclude(x => x.Episodes)
            ;

        public async Task<Episode?> GetEpisodeWithSceenById(Guid id, Guid? userId = null) 
            => await GetQueryable()
                .Include(x => x.UserSeens.Where(y => userId == null || y.Userid == userId.ToString()))
                .FirstOrDefaultAsync(x => x.Id == id);
    }
}
