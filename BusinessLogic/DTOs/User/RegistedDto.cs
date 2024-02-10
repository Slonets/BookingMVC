using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.User
{
    public class RegistedDto
    {
        public string FirstName { get; set; }        
        public string LastName { get; set; }
        public string Email { get; set; }     
        public string Password { get; set; }        
        public IFormFile Image { get; set; }
        public string Role { get; set; }
    }
}
