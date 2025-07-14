namespace OnlineСinema.Models.Dtos.Titles
{
    public class TitleVideoInformaionDto
    {
        public Guid TitleId { get; set; }
        
        public string TitleName { get; set; }

        public string? EpisodeName { get; set; }

        public Guid? NextId { get; set; }

        public Guid? PreveousId { get; set; }

        public bool IsSceen { get; set; }
    }
}
