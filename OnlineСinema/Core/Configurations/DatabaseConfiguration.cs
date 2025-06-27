using PIHelperSh.Configuration.Attributes;

namespace OnlineСinema.Core.Configurations
{
    [AutoConfiguration]
    public class DatabaseConfiguration
    {
        [FromEnvironment(VariableName = "DB_CONNECTION_STRING")]
        public string ConnectionString { get; set; } = "Password=postgres;Username=postgres;Database=cinema;Host=localhost";
    }
}
