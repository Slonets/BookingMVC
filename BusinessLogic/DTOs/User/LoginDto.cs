using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.User
{
    public class LoginDto
    {
        public string Password { get; set; }        
        public string Email { get; set; }                   
    }
}
