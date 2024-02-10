using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class UserRoleEntityDto
    {
        public virtual UserEntity User { get; set; }
        public virtual RoleEntity Role { get; set; }
    }
}
