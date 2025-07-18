namespace OnlineСinema.Models.Dtos.Titles
{
    public class EpisodeSmallDto
    {
        public Guid SeasoneId { get; set; }

        public Guid EpisodeId { get; set; }

        public string SeasoneName { get; set; }

        public string EpisodeName { get; set;}

        public int SeasoneOrderIndex { get; set; }

        public int EpisodeOrderIndex { get; set; }
    }
}
