using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.User;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.ColorSpaces.Companding;
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
        public readonly IImageWorker _imageWorker;

        public AppProfile(IImageWorker imageWorker)
        {
            _imageWorker = imageWorker;
        }


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


            CreateMap<BuildingDto, BuildingEntity>()
            .ForMember(dest => dest.ViewOfTheHouseId, opt => opt.MapFrom(src => src.ViewOfTheHouse.Id))
            .ForMember(dest => dest.TypeOfSaleId, opt => opt.MapFrom(src => src.TypeOfSale.Id))
            .ForMember(dest => dest.UserEntityId, opt => opt.MapFrom(src => src.UserEntity.Id))
            .ForMember(dest => dest.ImagesBulding, opt => opt.Ignore());

            CreateMap<BuildingEntity, BuildingDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.NumberOfRooms, opt => opt.MapFrom(src => src.NumberOfRooms))
            .ForMember(dest => dest.ViewOfTheHouse, opt => opt.MapFrom(src => new ViewOfTheHouse { Id = src.ViewOfTheHouse.Id, Name = src.ViewOfTheHouse.Name }))
            .ForMember(dest => dest.TypeOfSale, opt => opt.MapFrom(src => new TypeOfSale { Id = src.TypeOfSale.Id, Name = src.TypeOfSale.Name }))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.UserEntity, opt => opt.MapFrom(src => new UserEntity { Id = src.Id, FirstName = src.UserEntity.FirstName, LastName = src.UserEntity.LastName, PhoneNumber = src.UserEntity.PhoneNumber, Email = src.UserEntity.Email, Image = src.UserEntity.Image }))
            .ForMember(dest => dest.Image, opt => opt.Ignore())
            .ForMember(dest => dest.ImagesBulding, opt => opt.MapFrom(src => src.ImagesBulding.Select(image => new ImagesBuldingDto { BuildingEntityId = src.Id, Path = image.Path }).ToList()));
    

            CreateMap<ImagesBuldingDto, ImagesBulding>()
            .ForMember(dest => dest.BuildingEntityId, opt => opt.Ignore())
            .ForMember(dest => dest.Buildings, opt => opt.Ignore());

        CreateMap<ViewOfTheHouseDto, ViewOfTheHouse>().ReverseMap();

        CreateMap<TypeOfSaleDto, TypeOfSale>().ReverseMap();
    }

        //public string GetNameBuilding(BuildingEntity buildingEntity)
        //{
        //    string result = buildingEntity.ViewOfTheHouse.Name;            

        //    return result;
        //}

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
