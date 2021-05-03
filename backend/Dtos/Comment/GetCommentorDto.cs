using backend.Models;

namespace backend.Dtos.Comment
{
    public class GetCommentorDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserTypeClass UserType { get; set; }

    }
}