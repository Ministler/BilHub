using backend.Models;

namespace backend.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string ProfileInfo { get; set; }
        public string Token { get; set; }
        public bool DarkModeStatus { get; set; }
        public UserTypeClass UserType { get; set; }
    }
}