using AdstractHelpers.Mediator.Interfaces;

namespace OnlineСinema.Models.Commands.Images
{
    public class UploadImageCommand: IRequestModel<Guid>
    {
        public string Name { get; set; }

        public MemoryStream Data { get; set; }

        public string Extention { get; set; }

        public bool IsCover { get; set; }
    }
}
