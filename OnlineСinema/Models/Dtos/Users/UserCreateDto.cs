namespace OnlineСinema.Models.Dtos.Users
{
    public class UserCreateDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string Password { get; set; }
    }
}
