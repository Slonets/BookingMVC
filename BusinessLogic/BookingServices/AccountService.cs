using AutoMapper;
using BusinessLogic.DTOs.User;
using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BookingServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public AccountService(UserManager<UserEntity> userManager, IMapper mapper, IMailService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _mailService = mailService;
        }       

        public async Task Login(LoginDto loginDto)
        {
            var seach = _mapper.Map<UserEntity>(loginDto);

            UserEntity existUser = await _userManager.FindByEmailAsync(seach.Email);
            
            if (existUser== null)
            {
                throw new CustomHttpException("This castumer don't exsist!", HttpStatusCode.NotFound);                
            }
            
            IdentityResult res = await _userManager.CreateAsync(seach, loginDto.Password);
            await _userManager.AddToRoleAsync(seach, loginDto.Role);
           
        }
        public async Task Registration(RegistedDto dto)
        {
            UserEntity user = _mapper.Map<UserEntity>(dto);

            var resultCreated = await _userManager.CreateAsync(user, dto.Password);

            if (resultCreated.Succeeded)
            {
                try
                {
                    await _mailService.SendMailAsync(user.Email, "Реєстрація на сайті Booking.com", "\nКористувач " + user.FirstName + " " + user.LastName + " зареєстрований\nВаш email: " + user.Email + "\nВаш пароль: " + dto.Password);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }
            }  
            else
            {
                string erorMessage = string.Join("; ", resultCreated.Errors.Select(error => error.Description));
                throw new CustomHttpException(erorMessage, HttpStatusCode.BadRequest);
            }
        }
    }
}
