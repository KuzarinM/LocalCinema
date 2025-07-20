namespace OnlineСinema.Models.Dtos.Users
{
    public class UserViewDto
    {
        public Guid Id { get; set; }

        public string Login {  get; set; }

        public string Email { get; set; }

        public List<string>? Roles { get; set; }

        public bool IsActive { get; set; } 
    }
}
