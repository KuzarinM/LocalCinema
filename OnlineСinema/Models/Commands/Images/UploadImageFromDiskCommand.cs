using AdstractHelpers.Mediator.Interfaces;

namespace OnlineСinema.Models.Commands.Images
{
    public class UploadImageFromDiskCommand: IRequestModel
    {
        public string DirName { get; set; }
    }
}
