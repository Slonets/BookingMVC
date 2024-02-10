using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string UserRoles { get; set; }
    }
}
