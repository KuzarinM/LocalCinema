namespace OnlineСinema.Models.Dtos.Users
{
    public class UserCreateDto
    {
        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string Password { get; set; }
    }
}
