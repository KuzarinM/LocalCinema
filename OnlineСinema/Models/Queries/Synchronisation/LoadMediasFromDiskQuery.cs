using AdstractHelpers.Mediator.Interfaces;
using OnlineСinema.Models.Internal.Synchonisation;
using System.ComponentModel.DataAnnotations;

namespace OnlineСinema.Models.Queries.Synchronisation
{
    public class LoadMediasFromDiskQuery: IRequestModel<List<MediaModel>>
    {
        [Required(ErrorMessage = "Путь до диска не указан")]
        public string DiskPath { get; set; } = null!;
    }
}
