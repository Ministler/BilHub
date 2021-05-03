namespace backend.Dtos.User
{
    public class UserUpdateDto
    {
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public string ProfileInfo { get; set; }
        public bool DarkModeStatus { get; set; }
    }
}