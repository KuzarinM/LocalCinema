namespace OnlineСinema.Models.Dtos.Users
{
    public class UserLoginDto
    {
        public string Login { get; set; } = null!;

        public List<string> UserRoles { get; set; } = [];

        public string AccessToken { get; set; } = null!;

        public string RefrachToken { get; set; } = null!;

        public int AccsessTokenLivetime { get; set; }
    }
}
