namespace OnlineСinema.Models.Dtos.Titles
{
    public class TitleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Isfilm { get; set; }

        public string Description { get; set; }

        public Guid Coverimageid { get; set; }

        public Guid Tileimageid { get; set; }

        public bool IsSeen { get; set; }

        public List<string> Tags { get; set; }

        public int SeasonesCount { get; set; }

        public int EpisodesCount { get; set; }
    }
}
