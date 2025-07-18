using PIHelperSh.Configuration.Attributes;

namespace OnlineСinema.Core.Configurations
{
    [AutoConfiguration]
    public class DisplayConfiguration
    {
        public string DisallowedTags { get; set; } = "Ролики|Вселенная стр|private";

        public List<string> DisallowedTagsList => DisallowedTags.Split("|").ToList();
    }
}
