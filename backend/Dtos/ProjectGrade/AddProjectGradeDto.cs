using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using backend.Dtos.GradeBase;
using Microsoft.AspNetCore.Http;
 
namespace backend.Dtos.ProjectGrade
{
    public class AddProjectGradeDto : AddGradeBaseDto
    {
        public int GradedProjectGroupID { get; set; }
        public IFormFile File { get; set; }
    }
}