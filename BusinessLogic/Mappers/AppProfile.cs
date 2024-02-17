using AutoMapper;
using BusinessLogic.DTOs;
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
             .ForMember(dest => dest.Image, opt => opt.Ignore()) //коли перетворюємо, то поле Image ігнорується    
             .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
             .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
             .ForMember(dest => dest.UserRoles, opt => opt.Ignore()); // Assuming UserRoles will be handled separately

            CreateMap<UserEntity, RegistedDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // Assuming password won't be mapped back
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => GetRoleFromUserEntity(src))); // Assuming a user has one role


            //UserEntity -> LoginDto
            CreateMap<UserEntity, LoginDto>().ReverseMap();

            CreateMap<UserRoleEntityDto, UserRoleEntity>().ReverseMap();

            CreateMap<UserEntity, UserDto>()
                
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => "300_"+src.Image))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => GetRoleFromUserEntity(src)));

            CreateMap<UserDto, UserEntity>()
            .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<BuildingEntity, BuildingDto>()
            .ForMember(dest => dest.ViewOfTheHouseId, opt => opt.MapFrom(src => src.ViewOfTheHouse))
            .ForMember(dest => dest.TypeOfSaleId, opt => opt.MapFrom(src => src.TypeOfSale))
            .ForMember(dest => dest.UserEntityId, opt => opt.MapFrom(src => src.UserEntity));

            CreateMap<BuildingDto, BuildingEntity>()
                .ForMember(dest => dest.ViewOfTheHouse, opt => opt.Ignore())
                .ForMember(dest => dest.TypeOfSale, opt => opt.Ignore())
                .ForMember(dest => dest.UserEntity, opt => opt.Ignore());

            CreateMap<ImagesBulding, ImagesBuildingDto>()
            .ForMember(dest => dest.BuildingEntityId, opt => opt.MapFrom(src => src.BuildingEntityId));

            CreateMap<ImagesBuildingDto, ImagesBulding>()
                .ForMember(dest => dest.BuildingEntityId, opt => opt.MapFrom(src => src.BuildingEntityId))
                .ForMember(dest => dest.Buildings, opt => opt.Ignore());

        }

        private string GetRoleFromUserEntity(UserEntity userEntity)
        {
            string result = "";
            foreach(var role in userEntity.UserRoles)
            {
                result += role.Role.Name+" ";
            }

            // Отримання ролі з UserEntity         
            return result;
        }       
        
    }
}
