namespace OnlineСinema.Models.Dtos.Users
{
    public class UserUpdateDto
    {
        public string? Login { get; set; }

        public string? Email { get; set; }

        public List<string>? Roles { get; set; }

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }   

        public bool? Enabled { get; set; }
    }
}
