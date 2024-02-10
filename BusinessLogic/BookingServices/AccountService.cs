using AutoMapper;
using BusinessLogic.DTOs.User;
using BusinessLogic.Helpers;
using BusinessLogic.Interfaces;
using DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NETCore.MailKit.Core;
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
        private readonly ISmtpEmailService _emailService;
        private readonly IImageWorker _imageWorker;

        public AccountService(UserManager<UserEntity> userManager,
            SignInManager<UserEntity> signInManager,
            IMapper mapper, ISmtpEmailService emailService, IImageWorker imageWorker)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _imageWorker= imageWorker;
        }       

        public async Task<bool> Login(LoginDto loginDto)
        {    
            UserEntity existUser = await _userManager.FindByEmailAsync(loginDto.Email);
            
            if (existUser== null)
            {
                return false;                
            }
            var result = await _signInManager.PasswordSignInAsync(existUser, loginDto.Password, false, false);
            
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

            // Додайте обробку для збереження зображення
            var imageName = _imageWorker.ImageSave(dto.Image);
            user.Image = imageName;

            var resultCreated = await _userManager.CreateAsync(user, dto.Password);

            await _userManager.AddToRoleAsync(user, dto.Role);

            if (resultCreated.Succeeded)
            {
                try
                {
                    _emailService.SuccessfulLogin(dto.Email, dto.Password, dto.Email);
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
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();           
        }
    }
}
