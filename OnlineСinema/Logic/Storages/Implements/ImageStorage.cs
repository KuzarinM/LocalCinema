using AdstractHelpers.Storage.Abstraction;
using AdstractHelpers.Storage.Abstraction.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineСinema.Database;
using OnlineСinema.Logic.Storages.Interfases;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Images;
using OnlineСinema.Models.Dtos.Titles;

namespace OnlineСinema.Logic.Storages.Implements
{
    public class ImageStorage(
        ILogger<ImageStorage> logger, 
        IMapper mapper, 
        CinemaDbContext dbContext
        ) : AbstractDbStorage<Image, Guid>(logger, mapper, dbContext), IImageStorage
    {
        public async Task<Guid> AddOrUpdateImage(string name, MemoryStream memoryStream, string extention, bool isCover)
        {
            var img = await GetQueryable().FirstOrDefaultAsync(x => x.Name == name && x.IsCover == isCover);

            if (img == null) 
            {
                img = new Image()
                {
                    Id = Guid.NewGuid(),
                    Data = memoryStream.ToArray(),
                    Name = name,
                    fileExtention = extention,
                    IsCover = isCover
                };

                await AddItem(img);
            }
            else
            {
                img.Data = memoryStream.ToArray();
                img.fileExtention = extention;
            }

            return img.Id;
        }

        public async Task<PaginationModel<ImageDto>> GetImages(string search, int pageNumber, int pageSize)
        {
            var items = GetQueryable().Where(x =>
            (string.IsNullOrEmpty(search) || x.Name.Contains(search)));

            var itemsPaged = await items
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize)
                    .ToListAsync();

            var totalCount = await items.CountAsync();

            return new PaginationModel<ImageDto>()
            {
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)(totalCount * 1f / pageSize + 0.99f),
                Items = _mapper.Map<List<ImageDto>>(itemsPaged)
            };
        }

        public Task<Image?> GetImageByName(string name)
        {
            return GetQueryable().FirstOrDefaultAsync(x=>x.Name == name);
        }

        public Task<List<Image>> GetImagesVariants(List<string> contains, bool isCover)
        {
            return GetQueryable().Where(x => x.IsCover == !isCover && contains.Any(y => x.Name.Contains(y))).ToListAsync();
        }

        protected override Task<Image> UpdateItem(Image source, object target)
        {
            throw new NotImplementedException();
        }
    }
}
