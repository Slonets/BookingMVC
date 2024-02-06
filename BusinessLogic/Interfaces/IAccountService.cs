using BusinessLogic.DTOs.User;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {       
        Task Login(LoginDto loginDto);
        Task Registration(RegistedDto registerDto);
    }
}
