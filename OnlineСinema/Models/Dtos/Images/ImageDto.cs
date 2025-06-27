namespace OnlineСinema.Models.Dtos.Images
{
    public class ImageDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Extention { get; set; }

        public bool IsCover { get; set; }
    }
}
