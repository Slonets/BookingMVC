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

        public AccountService(UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IMapper mapper, IMailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _mailService = mailService;
        }       

        public async Task<bool> Login(LoginDto loginDto)
        {
            var seach = _mapper.Map<UserEntity>(loginDto);

            UserEntity existUser = await _userManager.FindByEmailAsync(seach.Email);
            
            if (existUser== null)
            {
                return false;                
            }
            var result = await _signInManager.PasswordSignInAsync(existUser, loginDto.Password, false, false);
            IdentityResult res = await _userManager.CreateAsync(seach, loginDto.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(existUser, isPersistent: false);
                return true;
            }
            return false;
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
