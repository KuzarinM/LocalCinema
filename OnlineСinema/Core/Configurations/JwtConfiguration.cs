using PIHelperSh.Configuration.Attributes;

namespace OnlineСinema.Core.Configurations
{
    [AutoConfiguration]
    public class JwtConfiguration
    {
        [FromEnvironment]
        public string TokenIssuer { get; set; } = "TestCinemaIssuer";

        [FromEnvironment]
        public string TokenAudience { get; set; } = "TestCinemaAudience";

        [FromEnvironment]
        public string AccessTokenSecretKey { get; set; } = "SyperSyperSecretJwtATokenKeyForCinema";

        [FromEnvironment]
        public string RefrashTokenSecretKey { get; set; } = "SyperSyperSecretJwtRTokenKeyForCinema";

        [FromEnvironment]
        public int LiveTimeMinutes { get; set; } = 20;
    }
}
