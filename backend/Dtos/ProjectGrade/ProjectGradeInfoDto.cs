using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
using Microsoft.AspNetCore.Http;

namespace backend.Dtos.ProjectGrade
{
    public class ProjectGradeInfoDto : GradeBaseInfoDto
    {
        public UserInProjectGradeDto userInProjectGradeDto { get; set; }
        public int GradingUserId { get; set; }
        public ProjectGroupInProjectGradeDto projectGroupInProjectGradeDto { get; set; }
        public int GradedProjectGroupID { get; set; }
        public string FileEndpoint { get; set; }
        public bool HasFile { get; set; }

    }
}