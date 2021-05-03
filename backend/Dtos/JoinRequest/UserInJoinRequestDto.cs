using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace backend.Dtos.JoinRequest
{
    public class UserInJoinRequestDto
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
    }
}