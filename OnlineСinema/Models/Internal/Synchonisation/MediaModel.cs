namespace OnlineСinema.Models.Internal.Synchonisation
{
    public class MediaModel
    {
        public string Name {  get; set; }

        public string Path { get; set; }

        public bool IsFilm {  get; set; }

        public bool IsSeason { get; set; }

        public bool IsEpisode { get; set; }

        public List<MediaModel> Children { get; set; } = [];
    }
}
