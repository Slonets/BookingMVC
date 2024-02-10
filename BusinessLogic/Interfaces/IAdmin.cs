using BusinessLogic.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAdmin
    {
        Task <UserDto> GetId(int id); 
        Task<IEnumerable<UserDto>> GetAll();
        Task Edit(UserDto userDto);
        Task Delete(int id);
    }
}
