using AutoMapper;
using BusinessLogic.DTOs.User;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessLogic.Mappers
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<RegistedDto, UserEntity>()
           .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
           .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));

            //UserEntity -> LoginDto
            CreateMap<UserEntity, LoginDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => GetRoleFromUserEntity(src)))
                .ForMember(dest => dest.Password, opt => opt.Ignore()); // Пароль мепетись не має

            //LoginDto -> UserEntity
            CreateMap<LoginDto, UserEntity>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Роли не будуть переводитися
        }

        private string GetRoleFromUserEntity(UserEntity userEntity)
        {
            // Отримання ролі з UserEntity         
            return userEntity.UserRoles?.FirstOrDefault()?.Role?.Name;
        }       
        
    }
}
