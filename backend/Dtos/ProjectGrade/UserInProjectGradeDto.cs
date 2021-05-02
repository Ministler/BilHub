using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.ProjectGrade
{
    public class UserInProjectGradeDto
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}