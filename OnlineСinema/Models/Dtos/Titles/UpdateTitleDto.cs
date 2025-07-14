namespace OnlineСinema.Models.Dtos.Titles
{
    public class UpdateTitleDto
    {
        public string? Name { get; set; }

        public bool? Isfilm { get; set; }

        public string? Description { get; set; }

        public List<string>? Tags { get; set; }

        public Guid? Coverimageid { get; set; }

        public Guid? Tileimageid { get; set; }
    }
}
