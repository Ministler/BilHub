namespace backend.Dtos.User
{
    public class GetUserDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string Name { get; set; } = "";
        public bool VerifiedStatus { get; set; }
        public bool DarkModeStatus { get; set; }
    }
}