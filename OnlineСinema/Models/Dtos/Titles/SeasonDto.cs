namespace OnlineСinema.Models.Dtos.Titles
{
    public class SeasonDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Orderindex { get; set; }

        public List<EpisodeDto> Episodes { get; set; } = [];
    }
}
