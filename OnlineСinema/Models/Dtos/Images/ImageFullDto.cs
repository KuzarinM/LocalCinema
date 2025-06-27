namespace OnlineСinema.Models.Dtos.Images
{
    public class ImageFullDto: ImageDto
    {
        public MemoryStream Data {  get; set; }

        public string MediaType { get; set; }
    }
}
