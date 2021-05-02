using System.Collections.Generic;

namespace backend.Dtos.ProjectGroup
{
    public class GetOzgurDto
    {
        public List<string> graders { get; set; }
        public List<OzgurStatDto> ozgurStatDtos { get; set; }
    }
}