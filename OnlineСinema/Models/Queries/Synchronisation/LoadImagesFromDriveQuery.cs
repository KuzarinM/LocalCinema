using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Internal.Synchonisation;

namespace OnlineСinema.Models.Queries.Synchronisation
{
    public class LoadImagesFromDriveQuery: IRequestModel<List<ImageModel>>
    {
        public string BaseDirPath { get; set; }
    }
}
