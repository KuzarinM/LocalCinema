using PIHelperSh.Configuration.Attributes;

namespace OnlineСinema.Core.Configurations
{
    [AutoConfiguration]
    public class OpenRouterConfiguration
    {
        public string Token { get; set; } = "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";

        public string TitleDescriptorModelName = "deepseek/deepseek-r1-0528:free";
    }
}
