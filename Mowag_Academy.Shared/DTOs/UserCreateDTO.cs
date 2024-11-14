using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mowag_Academy.Shared.DTOs
{
    public class UserCreateDto
    {
        public string Username { get; set; }
        public string Password { get; set; } 
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime CourseDate { get; set; }
        public int PossibleStartingYear { get; set; }
        public string Role { get; set; } 
    }
}
