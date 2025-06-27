using AdstractHelpers.Storage.Abstraction.Interfases;
using AdstractHelpers.Storage.Abstraction.Models;
using OnlineСinema.Models;
using OnlineСinema.Models.Dtos.Images;

namespace OnlineСinema.Logic.Storages.Interfases
{
    public interface IImageStorage: IDbStorage<Image, Guid>
    {
        public Task<Guid> AddOrUpdateImage(string name, MemoryStream memoryStream, string extention, bool isCover);

        public Task<PaginationModel<ImageDto>> GetImages(string search, int pageNumber, int pageSize);

        public Task<List<Image>> GetImagesVariants(List<string> contains, bool isCover);

        public Task<Image?> GetImageByName(string name);
    }
}
