using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
using Microsoft.AspNetCore.Http;
 
namespace backend.Dtos.ProjectGrade
{
    public class GetProjectGradeDto : GetGradeBaseDto
    {
        public int GradingUserId { get; set; }
        public int GradedProjectGroupID { get; set; }
    }
}