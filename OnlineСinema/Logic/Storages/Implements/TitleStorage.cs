using AdstractHelpers.Storage.Abstraction;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Titles;
using System.IO;
using System.Xml.Linq;
using tryAGI.OpenAI;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class TitleStorage(
        ILogger<TitleStorage> logger, 
        IMapper mapper, 
        CinemaDbContext dbContext
    ) : AbstractDbStorage<Title, Guid>(logger, mapper, dbContext), ITitleStorage
    {
        public async Task UpdateNameAndPath(Title source, string newName, string newPath)
        {
            if (source.Name != newName)
                source.Name = newName;

            if (source.Path != newPath)
                source.Path = newPath;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateTagList(Title source, List<Tag> tags)
        {
            source.Tags = tags;

            await _dbContext.SaveChangesAsync();
        }

        public Task<Title?> GetMedia(bool isFilm, string name, string path)
        {
            return GetQueryable().FirstOrDefaultAsync(x => x.Isfilm == isFilm && (x.Name == name || x.Path == path));
        }

        public Task<Title?> GetTitleById(Guid Id, Guid? userId = null)
        {
            return GetQueryable()
                .Include(x => x.UserSeens.Where(x => !userId.HasValue || x.Userid == userId.ToString()))
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<PaginationModel<TitleFullDto>> GetFullTitles(
            Guid? userId = null,
            string? search = null, 
            bool? isFilm = null, 
            List<string>? tags = null,
            int? pageSize = null, 
            int? pageNumber = null
        )
        {
            var items = GetQueryable()
                .Include(x=>x.UserSeens.Where(x=> !userId.HasValue || x.Userid == userId.ToString()))
                .Where(x =>
                    (string.IsNullOrEmpty(search) || x.Name.Contains(search)) &&
                    (isFilm == null || x.Isfilm == isFilm) &&
                    (tags == null || tags.All(y=>x.Tags.Select(x=>x.Name).Contains(y)))
                );

            if (pageSize != null && pageNumber != null)
            {
                var itemsPaged = await items
                    .Skip(pageSize.Value * pageNumber.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalCount = await items.CountAsync();

                return new PaginationModel<TitleFullDto>()
                {
                    PageSize = pageSize.Value,
                    CurrentPage = pageNumber.Value,
                    TotalPages = (int)(totalCount * 1f / pageSize + 0.99f),
                    Items = _mapper.Map<List<TitleFullDto>>(itemsPaged)
                };
            }

            var res = await items
                .ToListAsync();

            return new PaginationModel<TitleFullDto>()
            {
                PageSize = res.Count(),
                CurrentPage = 0,
                TotalPages = 1,
                Items = _mapper.Map<List<TitleFullDto>>(res)
            };
        }

        public async Task<PaginationModel<TitleDto>> GetTitles(
            Guid? userId = null,
            string? search = null,
            bool? isFilm = null,
            List<string>? tags = null,
            int? pageSize = null,
            int? pageNumber = null
        )
        {
            var items = GetQueryable()
                .Include(x => x.UserSeens.Where(x => !userId.HasValue || x.Userid == userId.ToString()))
                .Where(x =>
                    (string.IsNullOrEmpty(search) || x.Name.Contains(search)) &&
                    (isFilm == null || x.Isfilm == isFilm) &&
                    (tags == null || tags.All(y => x.Tags.Select(x => x.Name).Contains(y)))
                );

            if (pageSize != null && pageNumber != null)
            {
                var itemsPaged = await items
                    .Skip(pageSize.Value * pageNumber.Value)
                    .Take(pageSize.Value)
                    .ToListAsync();

                var totalCount = await items.CountAsync();

                return new PaginationModel<TitleDto>()
                {
                    PageSize = pageSize.Value,
                    CurrentPage = pageNumber.Value,
                    TotalPages = (int)(totalCount * 1f / pageSize + 0.99f),
                    Items = _mapper.Map<List<TitleDto>>(itemsPaged)
                };
            }

            var res = await items
                .ToListAsync();

            return new PaginationModel<TitleDto>()
            {
                PageSize = res.Count(),
                CurrentPage = 0,
                TotalPages = 1,
                Items = _mapper.Map<List<TitleDto>>(res)
            };
        }


        public async Task UpdateDescription(Title title, string description)
        {
            if(title.Description != description)
                title.Description = description;

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateImages(Title title, Guid? tileImageId = null, Guid? coverImageId = null)
        {
            if(tileImageId != null && title.Tileimageid != tileImageId)
                title.Tileimageid = tileImageId;

            if (coverImageId != null && title.Coverimageid != coverImageId)
                title.Coverimageid = coverImageId;

            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Title>> GetTitlesWithoutDescription()
        {
            return GetQueryable()
                .Where(x=>x.Description.ToLower() == "placeholder")
                .ToListAsync();
        }

        protected override IQueryable<Title> AddIncludes(IQueryable<Title> query) => query
            .Include(x => x.Tags)
            .Include(x => x.Seasones)
                .ThenInclude(x => x.Episodes);


        protected override Task<Title> UpdateItem(Title source, object target)
        {
            throw new NotImplementedException();
        }
    }
}
